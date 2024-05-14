using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using play.Catalog.Service.Extensions;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Common;
using Play.Catalog.Contracts;
using System.Threading;


namespace Play.Catalog.Service.Controllers
{
    //https://localhost:5001/items
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {

        private readonly IRepository<Item> itemRepository;
        private readonly IPublishEndpoint publishEndpoint;

        public ItemsController(IRepository<Item> itemRepository, IPublishEndpoint publishEndpoint)
        {
            this.itemRepository = itemRepository;
            this.publishEndpoint = publishEndpoint;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
        {
            var items = (await itemRepository.GetAllAsync()).Select(item => item.AsDto());
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await itemRepository.GetASync(id);
            if (item == null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createdItem)
        {
            var item = new Item
            {
                name = createdItem.name,
                description = createdItem.description,
                price = createdItem.price,
                createdDate = DateTimeOffset.UtcNow
            };

            await itemRepository.CreateAsync(item);

            await publishEndpoint.Publish(new CatalogItemCreated(item.id, item.name, item.description));



            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto newData)
        {
            var item = await itemRepository.GetASync(id);
            if (item == null)
            {
                return NotFound();
            }
            item.name = newData.name;
            item.description = newData.description;
            item.price = newData.price;

            await itemRepository.UpdateAsync(item);

            await publishEndpoint.Publish(new CatalogItemUpdated(item.id, item.name,item.description));

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var item = await itemRepository.GetASync(id);
            if (item == null)
            {
                return NotFound();
            }
            await itemRepository.RemoveAsync(item.id);
            await publishEndpoint.Publish(new CatalogItemDeleted(id));
            return NoContent();
        }
    }
}