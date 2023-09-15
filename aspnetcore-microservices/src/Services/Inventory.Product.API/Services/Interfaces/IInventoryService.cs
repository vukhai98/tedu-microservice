using Contracts.Domains.Interfaces;
using Inventory.Product.API.Entities;
using MongoDB.Driver;
using Shared.DTOs.Inventory;
using Shared.SeedWork;

namespace Inventory.Product.API.Services.Interfaces
{
    public interface IInventoryService : IMogoDbRepositoryBase<InventoryEntry>
    {
        Task<IEnumerable<InventoryEntryDto>> GetAllByItemNoAsync(string itemNo);
        Task<PageList<InventoryEntryDto>> GetAllByItemNoPaggingAsync(GetInventoryPagingQuery query);

        Task<InventoryEntryDto> GetById(string id);

        Task<InventoryEntryDto> PurchaseItemAsync(string itemNo, PurchaseProductDto request);
    }
}
