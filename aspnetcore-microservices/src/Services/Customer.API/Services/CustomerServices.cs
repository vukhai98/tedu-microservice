using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Interfaces;

namespace Customer.API.Services
{
    public class CustomerServices : ICustomerServices
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerServices(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<IResult> GetCustomerByUserName(string userName)
        {
            var result = await _customerRepository.GetCustomerByUserName(userName);

            return result != null ? Results.Ok(result) : Results.NotFound();
        }
    }
}