using System;

namespace Play.Inventory.Service.Dtos
{
    public record GrantItemsDto(Guid UserId, Guid CatalogItemId, int Quantity);
    public record InventoryItemDto(Guid CatalogItemId, string Name, string ImageUrl, int Quantity, DateTimeOffset AcquiredDate);
    // public record CatalogItemDto(Guid Id, string Name, decimal Price, DateTimeOffset createdDate, string ImageUrl);
    public record CatalogItemDto(Guid Id, string Name, string ImageUrl);

}
