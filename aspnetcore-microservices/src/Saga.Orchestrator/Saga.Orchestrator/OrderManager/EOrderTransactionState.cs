namespace Saga.Orchestrator.OrderManager
{
    public enum EOrderTransactionState
    {
        NotStarted,
        BasketGot,
        BasketGetFailed,
        BasketDeleted,
        OrderCreated,
        OrderCreatFailed,
        OrderGot,
        OrderGetFailed,
        OrderDeleted,
        OrderDeletedFailed,
        InventoryUpdated,
        InventoryUpdatedFailed,
        RollbackInventory,
        InventoryRollback,
        InventoryRollbackFailed
    }
}
