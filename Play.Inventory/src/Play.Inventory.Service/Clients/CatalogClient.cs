using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Play.Inventory.Service.Dtos;

namespace Play.Inventory.Clients
{
    public class CatalogClient
    {
        private readonly HttpClient _client;
        public CatalogClient(HttpClient client)
        {
            _client = client;
        }

        // public async Task<CatalogItemDto> GetCatalogItemByIdAsync(Guid id)
        // {
        //     var item = await _client.GetFromJsonAsync<CatalogItemDto>($"/items/{id}");
        //     return item;
        // }

        public async Task<IEnumerable<CatalogItemDto>> GetCatalogItemsAsync()
        {
            var item = await _client.GetFromJsonAsync<IEnumerable<CatalogItemDto>>("api/Items");
            return item;
        }

    }
}
