using Contracts.Domains;
using Inventory.Grpc.Entities;
using Inventory.Grpc.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using Shared.Configurations;
using ILogger = Serilog.ILogger;

namespace Inventory.Grpc.Repositories
{
    public class InventoryRepository : MongoDbRepository<InventoryEntry>, IInventoryRepository
    {
        private readonly ILogger _logger;

        public InventoryRepository(IMongoClient client, MongoDBSettings settings, ILogger logger) : base(client, settings)
        {
            _logger = logger;
        }

        public async Task<int> GetStockQuantity(string itemNo)
        {
            try
            {
                _logger.Information($"BEGIN GetStockQuantity: {itemNo}");
                var result = Collection.AsQueryable().Where(x => x.ItemNo.Equals(itemNo)).Sum(x => x.Quantity);
                _logger.Information($"END GetStockQuantity: {itemNo} - value:{result}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return 0;
            }

        }
    }
}
