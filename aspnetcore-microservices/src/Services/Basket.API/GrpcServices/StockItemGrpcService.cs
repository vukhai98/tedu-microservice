using Basket.API.Protos;
using Grpc.Core;
using Polly;
using Polly.Retry;

namespace Basket.API.GrpcServices
{
    public class StockItemGrpcService
    {
        private readonly StockProtoService.StockProtoServiceClient _stockProtoServiceClient;

        private readonly ILogger<StockItemGrpcService> _logger;
        private readonly AsyncRetryPolicy<StockRespone> _retryPolicy;


        public StockItemGrpcService(StockProtoService.StockProtoServiceClient stockProtoServiceClient, ILogger<StockItemGrpcService> logger)
        {
            _stockProtoServiceClient = stockProtoServiceClient ?? throw new ArgumentNullException(nameof(stockProtoServiceClient));
            _logger = logger;
            _retryPolicy = Policy<StockRespone>.Handle<RpcException>()
           .RetryAsync(3);
        }

        public async Task<StockRespone> GetStock(string itemNo)
        {
            try
            {
                _logger.LogInformation($"BEGIN Get stock ItemNo:{itemNo}");

                var stockRequest = new StockRequest { ItemNo = itemNo };

                return await _retryPolicy.ExecuteAsync(async () =>
                {
                    var result = await _stockProtoServiceClient.GetStockAsync(stockRequest);
                    if (result != null)
                        _logger.LogInformation($"END: Get Stock StockItemGrpcService Item No: {itemNo} - Stock value: {result.Quantity}");

                    return result;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when Get stock ItemNo:{itemNo} - {ex}");

                throw;
            }
        }

    }
}
