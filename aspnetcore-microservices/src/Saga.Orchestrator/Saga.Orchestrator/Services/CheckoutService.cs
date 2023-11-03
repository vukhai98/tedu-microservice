using AutoMapper;
using Basket.API.Entities;
using Saga.Orchestrator.HttpRepository.Interfaces;
using Saga.Orchestrator.Services.Interfaces;
using Shared.DTOs.Inventory;
using Shared.DTOs.Orders;
using ILogger = Serilog.ILogger;


namespace Saga.Orchestrator.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IBasketHttpRepository _basketHttpRepository;
        private readonly IInventoryHttpRepository _inventoryHttpRepository;
        private readonly IOrderHttpRepository _orderHttpRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CheckoutService(
            IBasketHttpRepository basketHttpRepository,
            IInventoryHttpRepository inventoryHttpRepository,
            IOrderHttpRepository orderHttpRepository,
            IMapper mapper,
            ILogger logger)
        {
            _basketHttpRepository = basketHttpRepository;
            _inventoryHttpRepository = inventoryHttpRepository;
            _orderHttpRepository = orderHttpRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<bool> CheckoutOrder(string userName, BasketCheckoutDto basketCheckoutDto)
        {
            #region Get cart from IBasketHttpRepository

            _logger.Information($"Start : get cart {userName}");

             var cart = await _basketHttpRepository.GetBasket(userName);

            if (cart == null)
                return false;

            _logger.Information($"End: get cart {userName} success");

            #endregion

            #region Create Order from IOrderHttpRepository

            _logger.Information($" Statrt creat order");

            var order = _mapper.Map<CreateOrderDto>(basketCheckoutDto);

            order.TotalPrice = cart.TotalPrice;

            var orderId = await _orderHttpRepository.CreateOrder(order);

            if (orderId < 0)
                return false;
            #endregion

            #region Get Order by order id

            var addOrder = await _orderHttpRepository.GetOrder(orderId);

            _logger.Information($" End: creat order success, OrderId:{orderId}");

            #endregion


            #region Sales Items from IInventoryHttpRepository

            var inventoryDocumentNoes = new List<string>();
            bool result;

            try
            {
                // Sales Items from IInventoryHttpRepository

                foreach (var item in cart.Items)
                {
                    _logger.Information($"Start: Sale ItemNo {item.ItemNo} - Quantity:{item.Quantity}");

                    var saleOrder = new SalesProductDto(addOrder.DocumentNo, item.Quantity);

                    saleOrder.SetItemNo(item.ItemNo);

                    var documentNo = await _inventoryHttpRepository.CreateSalesOrder(saleOrder);
                    inventoryDocumentNoes.Add(documentNo);

                    _logger.Information($"End: Sale Item No:{item.ItemNo}" + $"Quantity: {item.Quantity} - Document No: {documentNo}");

                }

                result = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                await RollbackCheckoutOrder(userName, orderId, inventoryDocumentNoes);
                return false;
            }
            return result;
            #endregion
            //Rollback checkout order
        }

        private async Task RollbackCheckoutOrder(string userName, long orderId, List<string> inventoryDocumentNos)
        {
            _logger.Information($"Start: Rollback checkoutOrder for userName: {userName} - orderId:{orderId} - inventory documentNos: {String.Join(",", inventoryDocumentNos)} ");

            var deletedDocumentNos = new List<string>();
            // delete order by order's id , order's documentno

            foreach (var documentNo in inventoryDocumentNos)
            {
                await _inventoryHttpRepository.DeleteOrderByDocumentNo(documentNo);
                deletedDocumentNos.Add(documentNo);
            }

            _logger.Information($"End: Deleted documentNos : {string.Join(",", deletedDocumentNos)}");
        }
    }
}