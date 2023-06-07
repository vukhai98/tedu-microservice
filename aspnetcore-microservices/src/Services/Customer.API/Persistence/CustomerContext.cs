using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Customer.API.Repositories
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {

        }
        public DbSet<Entities.Customer> Customers { set; get; }
    }
}