using AutoMapper;
using Basket.API.Entities;
using Contracts.Sages.SagaOrderManager;
using Saga.Orchestrator.HttpRepository;
using Saga.Orchestrator.HttpRepository.Interfaces;
using Saga.Orchestrator.Services;
using Shared.DTOs.Baskets;
using Shared.DTOs.Inventory;
using Shared.DTOs.Orders;
using ILogger = Serilog.ILogger;


namespace Saga.Orchestrator.OrderManager
{
    public class SagaOrderManager : ISagaOrderManager<BasketCheckoutDto, OrderRespone>
    {
        private readonly IBasketHttpRepository _basketHttpRepository;
        private readonly IInventoryHttpRepository _inventoryHttpRepository;
        private readonly IOrderHttpRepository _orderHttpRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public SagaOrderManager(
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

        public OrderRespone CreateOrder(BasketCheckoutDto input)
        {
            var orderStateMachine = new Stateless.StateMachine<EOrderTransactionState, EOrderAction>(EOrderTransactionState.NotStarted);

            long orderId = -1;
            CartDTO cart = new CartDTO();
            OrderDto addedOrder = new OrderDto();
            var inventoryDocumentNo = string.Empty;

            orderStateMachine.Configure(EOrderTransactionState.NotStarted)
                             .PermitDynamic(EOrderAction.GetBasket, () =>
                             {
                                 cart = _basketHttpRepository.GetBasket(input.UserName).Result;

                                 return cart != null ? EOrderTransactionState.BasketGot : EOrderTransactionState.BasketGetFailed;
                             });

            orderStateMachine.Configure(EOrderTransactionState.BasketGot)
                             .PermitDynamic(EOrderAction.CreateOrder, () =>
                             {
                                 var order = _mapper.Map<CreateOrderDto>(input);

                                 order.TotalPrice = cart.TotalPrice;
                                 orderId = _orderHttpRepository.CreateOrder(order).Result;

                                 return orderId > 0 ? EOrderTransactionState.OrderCreated : EOrderTransactionState.OrderCreatFailed;
                             }).OnEntry(() => orderStateMachine.Fire(EOrderAction.CreateOrder));


            orderStateMachine.Configure(EOrderTransactionState.OrderCreated)
                            .PermitDynamic(EOrderAction.GetOrder, () =>
                            {
                                addedOrder = _orderHttpRepository.GetOrder(orderId).Result;

                                return addedOrder != null ? EOrderTransactionState.OrderGot : EOrderTransactionState.OrderGetFailed;
                            }).OnEntry(() => orderStateMachine.Fire(EOrderAction.GetOrder));

            orderStateMachine.Configure(EOrderTransactionState.OrderGot)
                             .PermitDynamic(EOrderAction.UpdateInventory, () =>
                             {
                                 var saleOrder = new SalesOrderDto()
                                 {
                                     OrderDocumentNo = addedOrder.DocumentNo,
                                     SaleItems = _mapper.Map<List<SaleItemDto>>(cart.Items)
                                 };
                                 inventoryDocumentNo = _inventoryHttpRepository.CreateOrderSale(addedOrder.DocumentNo, saleOrder).Result;

                                 return !string.IsNullOrEmpty(inventoryDocumentNo) ? EOrderTransactionState.InventoryUpdated : EOrderTransactionState.InventoryUpdatedFailed;
                             }).OnEntry(() => orderStateMachine.Fire(EOrderAction.UpdateInventory));

            orderStateMachine.Configure(EOrderTransactionState.InventoryUpdated)
                             .PermitDynamic(EOrderAction.DeleteBasket, () =>
                             {
                                 var result = _basketHttpRepository.DeleteBasket(input.UserName).Result;

                                 return result ? EOrderTransactionState.BasketDeleted : EOrderTransactionState.InventoryUpdatedFailed;
                             }).OnEntry(() => orderStateMachine.Fire(EOrderAction.DeleteBasket));

            orderStateMachine.Configure(EOrderTransactionState.InventoryUpdatedFailed)
                             .PermitDynamic(EOrderAction.DeleteInventory, () =>
                             {
                                 RollbackOrder(input.UserName, inventoryDocumentNo, orderId);

                                 return EOrderTransactionState.InventoryRollback;
                             }).OnEntry(() => orderStateMachine.Fire(EOrderAction.DeleteInventory));

            orderStateMachine.Fire(EOrderAction.GetBasket);

            return new OrderRespone(orderStateMachine.State == EOrderTransactionState.InventoryUpdated);
        }

        public OrderRespone RollbackOrder(string userName, string documentNo, long orderId)
        {
            var orderStateMachine = new Stateless.StateMachine<EOrderTransactionState, EOrderAction>(EOrderTransactionState.RollbackInventory);

            orderStateMachine.Configure(EOrderTransactionState.RollbackInventory)
                             .PermitDynamic(EOrderAction.DeleteInventory, () =>
                             {
                                 var result = _inventoryHttpRepository.DeleteOrderByDocumentNo(documentNo).Result;

                                 return EOrderTransactionState.InventoryRollback;

                             });

            orderStateMachine.Configure(EOrderTransactionState.InventoryRollback)
                             .PermitDynamic(EOrderAction.DeleteOrder, () =>
                             {
                                 var result = _orderHttpRepository.DeleteOrder(orderId).Result;

                                 return result ? EOrderTransactionState.OrderDeleted : EOrderTransactionState.OrderDeletedFailed;

                             }).OnEntry(() => orderStateMachine.Fire(EOrderAction.DeleteOrder));

            orderStateMachine.Fire(EOrderAction.DeleteInventory);

            return new OrderRespone(orderStateMachine.State == EOrderTransactionState.InventoryRollback);
        }
    }
}
