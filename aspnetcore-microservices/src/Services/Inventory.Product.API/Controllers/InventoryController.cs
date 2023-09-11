using Inventory.Product.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Inventory;
using Shared.SeedWork;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Inventory.Product.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;

        }

        /// <summary>
        /// api/inventory/items/{itemNo}
        /// </summary>
        /// <param name="itemNo"></param>
        /// <returns></returns>
        [HttpGet("items/{itemNo}")]
        [ProducesResponseType(typeof(IEnumerable<InventoryEntryDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<InventoryEntryDto>>> GetAllByItemNo([Required] string itemNo)
        {
            var result = await _inventoryService.GetAllByItemNoAsync(itemNo);

            if (result == null || result.Count() == 0)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// api/inventory/items/{itemNo}/paging
        /// </summary>
        /// <param name="itemNo"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("items/{itemNo}/paging")]
        [ProducesResponseType(typeof(PageList<InventoryEntryDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PageList<InventoryEntryDto>>> GetAllByItemNoPaging([Required] string itemNo, [FromQuery] GetInventoryPagingQuery query)
        {
            query.SetItemNo(itemNo);

            var result = await _inventoryService.GetAllByItemNoPaggingAsync(query);

            if (result == null || result.Count() == 0)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// api/inventory/items/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("item/{id}")]
        [ProducesResponseType(typeof(InventoryEntryDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<InventoryEntryDto>> GetById([Required] string id)
        {

            var result = await _inventoryService.GetById(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// api/inventory/items/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("item/{id}")]
        [ProducesResponseType(typeof(InventoryEntryDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<InventoryEntryDto>> DeleteById([Required] string id)
        {
            var inventoryEntry = await _inventoryService.GetById(id);

            if (inventoryEntry == null) return NotFound();

            await _inventoryService.DeleteAsync(id);

            return NoContent();
        }

        /// <summary>
        /// api/inventory/items/{itemNo}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("item/{itemNo}")]
        [ProducesResponseType(typeof(InventoryEntryDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<InventoryEntryDto>> PurchaseOrder([Required] string itemNo, [FromBody] PurchaseProductDto dto)
        {
            dto.ItemNo = itemNo;    
            var result = await _inventoryService.PurchaseItemAsync(itemNo, dto);

            return Ok(result);
        }


    }
}
