using Grpc.Core;
using Inventory.Grpc.Repositories.Interfaces;

namespace Inventory.Grpc.Services
{
    public class InventoryService : StockProtoService.StockProtoServiceBase
    {
        private readonly IInventoryRepository _inventoryRepository;

        private readonly ILogger<InventoryService> _logger;

        public InventoryService(IInventoryRepository inventoryRepository, ILogger<InventoryService> logger)
        {
            _inventoryRepository = inventoryRepository;
            _logger = logger;
        }

        public override async Task<StockRespone> GetStock(StockRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"BEGIN Get stock of ItemNo:{request.ItemNo}");

            var quantity = await _inventoryRepository.GetStockQuantity(request.ItemNo);

            var stockRespone = new StockRespone()
            {
                Quantity = quantity,
            };

            _logger.LogInformation($"END Get stock of ItemNo:{request.ItemNo} - quantity:{stockRespone.Quantity}");

            return stockRespone;
        }
    }
}
