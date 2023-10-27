using Basket.API.Entities;

namespace Saga.Orchestrator.Services.Interfaces
{
    public interface ICheckoutService
    {
        Task<bool> CheckoutOrder(string userName, BasketCheckoutDto basketCheckoutDto);
    }
}
