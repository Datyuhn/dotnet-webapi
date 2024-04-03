using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.DTOs;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemController : ControllerBase
    {
        private static readonly List<ItemDto> items = new() {
            new ItemDto(Guid.NewGuid(), "Potion", "Restore a small health", 5, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Antodote", "Cure poison", 7, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Sword", "Deal a small amount of damage", 20, DateTimeOffset.UtcNow),
        };

        [HttpGet]
        public IEnumerable<ItemDto> Get()
        {
            return items;
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetById(Guid id)
        {
            var item = items.Where(item => item.Id == id).SingleOrDefault();
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public ActionResult Post(CreateItemDto itemDto)
        {
            var item = new ItemDto(Guid.NewGuid(), itemDto.ItemName, itemDto.Description, itemDto.Price, DateTimeOffset.UtcNow);
            items.Add(item);

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, UpdateItemDto itemDto)
        {
            var existedItem = items.Where(item => item.Id == id).SingleOrDefault();
            if (existedItem == null)
            {
                return NotFound();
            }
            var updatedItem = existedItem with
            {
                ItemName = itemDto.ItemName,
                Description = itemDto.Description,
                Price = itemDto.Price,
            };
            var index = items.FindIndex(existedItem => existedItem.Id == id);
            items[index] = updatedItem;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var item = items.Where(e => e.Id == id).SingleOrDefault();
            if (item == null)
            {
                return NotFound();
            }
            items.Remove(item);

            return NoContent();
        }
    }
}