using System;

namespace Play.Catalog.Contracts
{
    public record CatalogItemCreated(Guid Id, string Name, string ImageUrl, decimal Price);
    public record CatalogItemUpdated(Guid Id, string Name, string ImageUrl, decimal Price);
    public record CatalogItemDeleted(Guid Id);
}
