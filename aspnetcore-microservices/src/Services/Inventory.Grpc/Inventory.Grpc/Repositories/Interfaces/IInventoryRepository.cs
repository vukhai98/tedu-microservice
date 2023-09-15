using Contracts.Domains.Interfaces;
using Inventory.Grpc.Entities;

namespace Inventory.Grpc.Repositories.Interfaces
{
    public interface IInventoryRepository : IMogoDbRepositoryBase<InventoryEntry>
    {
        Task<int> GetStockQuantity(string itemNo);

    }
}
