using System;
using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service.Dtos
{
    public record ItemDto(Guid Id, string Name, decimal Price, DateTimeOffset CreatedDate, string ImageUrl);
    public record CreateItemDto([Required] string Name, [Range(0, 99.99)] decimal Price, string ImageUrl);
    public record UpdateItemDto(string Name, [Range(0, 99.99)] decimal Price, string ImageUrl);

}

