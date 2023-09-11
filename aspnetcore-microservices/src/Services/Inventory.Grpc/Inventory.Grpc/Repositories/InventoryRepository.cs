using Contracts.Domains;
using Inventory.Grpc.Entities;
using Inventory.Grpc.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using Shared.Configurations;

namespace Inventory.Grpc.Repositories
{
    public class InventoryRepository : MongoDbRepository<InventoryEntry>, IInventoryRepository
    {
        public InventoryRepository(IMongoClient client, MongoDBSettings settings) : base(client, settings)
        {
        }

        public async Task<int> GetStockQuantity(string itemNo)
        {
            var result = await Collection.AsQueryable().Where(x=> x.ItemNo == itemNo)
                                                 .SumAsync(x => x.Quantity);

            return result;
        }
    }
}
