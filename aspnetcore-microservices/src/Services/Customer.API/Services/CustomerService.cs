using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Interfaces;

namespace Customer.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository) 
        {
            _customerRepository = customerRepository;
        }    
        public async Task<IResult> GetAlls() 
        {
            var result =  _customerRepository.FindAll();
            return Results.Ok(result);
        }

        public async Task<IResult> GetUserByName(string userName)
        {
            var result = await _customerRepository.GetCustomerByUserName(userName);

            return Results.Ok(result);
        }
    }
}
