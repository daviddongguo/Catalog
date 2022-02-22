using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using David.Common;
using Play.Catalog.Contracts;

namespace Play.Catalog.Service.Controller
{
    public class ItemsController : BaseApiController
    {
        private readonly IRepository<Item> _rep;
        private readonly IPublishEndpoint _publishEndpoint;

        public ItemsController(IRepository<Item> rep, IPublishEndpoint publishEndpoint)
        {
            this._rep = rep;
            _publishEndpoint = publishEndpoint;
        }


        [HttpGet]
        public async Task<ActionResult<List<ItemDto>>> List()
        {
            var items = (await _rep.GetAllAsync()).Select(items => items.AsDto());
            return Ok(items);
        }

        [HttpGet("{id}")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ItemDto>> GetById(Guid id)
        {
            var item = await _rep.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item.AsDto());
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddItem([FromBody] CreateItemDto item)
        {
            var toAddItem = new Item()
            {
                Name = item.Name,
                Price = item.Price,
                CreatedDate = DateTimeOffset.UtcNow,
                ImageUrl = item.ImageUrl
            };

            await _rep.CreateAsync(toAddItem);
            await _publishEndpoint.Publish(new CatalogItemCreated(toAddItem.Id, toAddItem.Name, toAddItem.ImageUrl, toAddItem.Price));

            return CreatedAtAction(nameof(GetById), new { id = toAddItem.Id }, toAddItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, [FromBody] UpdateItemDto updateItem)
        {
            var existingItem = await _rep.GetAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            existingItem.Name = string.IsNullOrWhiteSpace(updateItem.Name) ? existingItem.Name : updateItem.Name;
            existingItem.Price = updateItem.Price < 0.0m ? existingItem.Price : updateItem.Price;
            existingItem.ImageUrl = string.IsNullOrWhiteSpace(updateItem.ImageUrl) ? existingItem.ImageUrl : updateItem.ImageUrl;

            await _rep.UpdateAsync(existingItem);
            await _publishEndpoint.Publish(new CatalogItemUpdated(existingItem.Id, existingItem.Name, existingItem.ImageUrl, existingItem.Price));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var existingItem = await _rep.GetAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }

            await _rep.RemoveAsync(id);
            await _publishEndpoint.Publish(new CatalogItemDeleted(id));

            return NoContent();
        }
    }
}
