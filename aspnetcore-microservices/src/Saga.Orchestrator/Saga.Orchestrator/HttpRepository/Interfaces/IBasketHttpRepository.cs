using Shared.DTOs.Baskets;

namespace Saga.Orchestrator.HttpRepository.Interfaces
{
    public interface IBasketHttpRepository
    {
        Task<CartDTO> GetBasket(string userName);

        Task<bool> DeleteBasket(string userName);
    }
}
