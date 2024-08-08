using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Play.Catalog.Contracts;
using Play.Catalog.Service.Context;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service;
using Play.Common;
// using Play.Catalog.Service.Migrations;
using var catalogDB = new CatalogContext();

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private List<Item> itemList;
        private readonly IRepository<Item> itemsRepository;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly CatalogContext catalogContext;

        private readonly CatalogContext catalogContext;

        public ItemsController(IRepository<Item> itemsRepository, IPublishEndpoint publishEndpoint, CatalogContext catalogContext)
        {
            this.itemsRepository = itemsRepository;
            this.publishEndpoint = publishEndpoint;
            //this.catalogContext = catalogContext;
        }

        #region MongoDB
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
        {
            var items = (await itemsRepository.GetAllAsync())
                        .Select(item => item.AsDto());

            itemList = items.Select(itemDto => new Item
            {
                Id = itemDto.Id,
                ItemName = itemDto.Name,
                Description = itemDto.Description,
                Price = itemDto.Price,
                CreatedDate = itemDto.CreatedDate,
                UpdatedDate = itemDto.UpdatedDate
            }).ToList();

            foreach (var _item in itemList)
            {
                var item = catalogContext.Items.Where(e => e.Id == _item.Id).FirstOrDefault();
                if (item == null)
                {
                    catalogContext.Items.Add(_item);
                }
            }
            await catalogContext.SaveChangesAsync();
            
            return Ok(items);
        }

        // GET /items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await itemsRepository.GetAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            // var _item = await catalogContext.Items.FindAsync(id);
            // if (_item == null)
            // {
            //     return NotFound();
            // }

            return item.AsDto();
        }

        // POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
        {
            var item = new Item
            {
                ItemName = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow,
                UpdatedDate = DateTimeOffset.UtcNow
            };

            // catalogContext.Items.Add(item);
            // catalogContext.SaveChanges();

            await itemsRepository.CreateAsync(item);

            await publishEndpoint.Publish(new CatalogItemCreated(item.Id, item.ItemName, item.Description));

            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }

        // PUT /items/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = await itemsRepository.GetAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.ItemName = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;
            existingItem.UpdatedDate = DateTimeOffset.UtcNow;

            catalogContext.Entry(existingItem).State = EntityState.Modified;
            await catalogContext.SaveChangesAsync();

            await itemsRepository.UpdateAsync(existingItem);

            await publishEndpoint.Publish(new CatalogItemUpdated(existingItem.Id, existingItem.ItemName, existingItem.Description, existingItem.UpdatedDate));

            return NoContent();
        }

        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var item = await itemsRepository.GetAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            catalogContext.Items.Remove(item);
            await catalogContext.SaveChangesAsync();

            await itemsRepository.RemoveAsync(item.Id);

            await publishEndpoint.Publish(new CatalogItemDeleted(id));

            return NoContent();
        }
        #endregion
    }    
}