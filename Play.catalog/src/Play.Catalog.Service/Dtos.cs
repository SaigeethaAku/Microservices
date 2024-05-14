using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Play.Catalog.Service.Dtos
{
    public record ItemDto(Guid id, string name, string description, decimal price, DateTimeOffset createdDate);
    public record CreateItemDto([Required] string name, string description, [Range(0, 1000)] decimal price);
    public record UpdateItemDto([Required] string name, string description, [Range(0, 1000)] decimal price);

}