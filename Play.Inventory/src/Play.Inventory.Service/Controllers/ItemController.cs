using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Entities;
using Play.Inventory.Service.Dtos;
using System.Linq;

namespace Play.Inventory.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemController : ControllerBase
    {
        private readonly IRepository<InventoryItem> itemRepository;

        public ItemController(IRepository<InventoryItem> itemRepository)
        {
            this.itemRepository = itemRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }

            var items = (await itemRepository.GetAllAsync(item => item.UserId == userId))
                        .Select(item => item.AsDto());
            
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemsDto grantItemDto)
        {
            var inventoryItem = 
                await itemRepository.GetAsync(item =>
                item.UserId == grantItemDto.UserId &&
                item.CatalogItemId == grantItemDto.CatologItemId);

            if (inventoryItem == null)
            {
                inventoryItem = new()
                {
                    CatalogItemId = grantItemDto.CatologItemId,
                    UserId = grantItemDto.UserId,
                    Quantity = grantItemDto.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };

                await itemRepository.CreateAsync(inventoryItem);
            }
            else 
            {
                inventoryItem.Quantity += grantItemDto.Quantity;
                await itemRepository.UpdateAsync(inventoryItem);
            }

            return Ok();
        }
    }
}