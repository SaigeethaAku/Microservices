using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;

namespace play.Catalog.Service.Extensions
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(item.id, item.name, item.description, item.price, item.createdDate);
        }
    }
}