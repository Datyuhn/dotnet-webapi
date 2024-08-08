using System.Runtime.CompilerServices;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(item.Id, item.ItemName, item.Description, item.Price, item.CreatedDate, item.UpdatedDate);
        }
    }
}