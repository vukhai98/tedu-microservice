﻿using Contracts.Common.Interfaces;
using Customer.API.Persistence;
using Customer.API.Repositories.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Repositories
{
    public class CustomerRepository : RepositoryBaseAsync<Entities.Customer, int, CustomerContext>, ICustomerRepository
    {
        public CustomerRepository(CustomerContext dbContext, IUnitOfWork<CustomerContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public async Task<Entities.Customer> GetCustomerByUserName(string userName)
        {
            return await FindByCondition(x => x.UserName.Equals(userName)).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Entities.Customer>> GetCustomers()
        {
            return await FindAll().ToListAsync();
        }
    }
}
