namespace Saga.Orchestrator.OrderManager
{
    public enum EOrderAction
    {
        GetBasket,
        CreateOrder,
        GetOrder,
        DeleteOrder,
        UpdateInventory,
        RollbackInventory,
        DeleteBasket,
        DeleteInventory,
    }
}
