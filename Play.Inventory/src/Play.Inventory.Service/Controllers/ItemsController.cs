using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using David.Common;
using David.Common.Controllers;
using Microsoft.AspNetCore.Mvc;
using Play.Inventory.Clients;
using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Controllers
{
    public class IntemsController : BaseApiController
    {
        private readonly IRepository<InventoryItem> _inventoryRep;
        private readonly IRepository<CatalogItem> _catalogRep;

        public IntemsController(IRepository<InventoryItem> inventoryRep, IRepository<CatalogItem> catalogRep)
        {
            _inventoryRep = inventoryRep;
            _catalogRep = catalogRep;

        }
        [HttpGet()]
        [Route("/all")]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> GetAllAsync()
        {
            var items = (await _inventoryRep.GetAllAsync());
            return Ok(items);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAllByUserIdAsync([FromQuery] Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }
            var inventoryItems = await _inventoryRep.GetAllAsync(inv => inv.UserId == userId);
            var catalogIds = inventoryItems.Select(inv => inv.CatalogItemId);
            var catalogItems = await _catalogRep.GetAllAsync(cat => catalogIds.Contains(cat.Id));

            var inventoryItemDtos = inventoryItems.Select(inv =>
            {
                var catalogItem = catalogItems.SingleOrDefault(cat => cat.Id == inv.CatalogItemId);
                if (catalogItem != null)
                {
                    return inv.AsDto(catalogItem.Name, catalogItem.ImageUrl);
                }
                return inv.AsDto("", "");
            });

            return Ok(inventoryItemDtos);
        }

        [HttpPost]
        public async Task<ActionResult> Post(GrantItemsDto grantInventory)
        {
            var existedInventory = await _inventoryRep.GetAsync(inv => inv.UserId == grantInventory.UserId && inv.CatalogItemId == grantInventory.CatalogItemId);
            if (existedInventory != null)
            {
                existedInventory.Quantity += grantInventory.Quantity;
                await _inventoryRep.UpdateAsync(existedInventory);
                return Ok();
            }

            var userId = grantInventory.UserId;
            var catalogItemId = grantInventory.CatalogItemId;
            if (await _catalogRep.GetAsync(catalogItemId) == null)
            {
                return BadRequest("catalog id is invalid");
            }


            var newInventory = new InventoryItem()
            {
                UserId = grantInventory.UserId,
                CatalogItemId = grantInventory.CatalogItemId,
                Quantity = grantInventory.Quantity,
                AcquiredData = System.DateTimeOffset.UtcNow

            };
            await _inventoryRep.CreateAsync(newInventory);
            return Ok();

        }

    }


}
