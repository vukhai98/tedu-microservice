using Contracts.Common.Interfaces;
using Infrastructure.Common;

namespace Customer.API.Repositories.Interfaces
{
    public interface ICustomerRepository : IRepositoryQueryBase<Entities.Customer,int,CustomerContext>
    {
        Task<Entities.Customer> GetCustomerByUserName(string userName);
    }
}
