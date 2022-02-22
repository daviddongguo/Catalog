using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service
{
    public static class Extensions
    {
        public static InventoryItemDto AsDto(this InventoryItem item, string name, string imageUrl)
        {
            return new InventoryItemDto(item.CatalogItemId, name, imageUrl, item.Quantity, item.AcquiredData);
        }
    }
}
