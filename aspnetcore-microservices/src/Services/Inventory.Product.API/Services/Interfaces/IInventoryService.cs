using Inventory.Product.API.Entities;
using Inventory.Product.API.Repositories.Interfaces;
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
