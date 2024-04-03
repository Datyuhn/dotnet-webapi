using System;
using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service.DTOs
{
    public record ItemDto(Guid Id, string ItemName, string Description, decimal Price, DateTimeOffset CreatedDate);

    public record CreateItemDto([Required] string ItemName, string Description, [Range(0, 1000)] decimal Price);

    public record UpdateItemDto([Required] string ItemName, string Description, [Range(0, 1000)] decimal Price);
}