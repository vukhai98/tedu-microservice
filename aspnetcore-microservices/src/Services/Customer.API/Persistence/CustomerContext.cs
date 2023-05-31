using Microsoft.EntityFrameworkCore;

namespace Customer.API.Persistence
{
    public class CustomerContext: DbContext
    {
        private readonly IConfiguration _configuration;
        public CustomerContext(DbContextOptions<CustomerContext> options, IConfiguration configuration) : base(options)
        {
            _configuration= configuration;
        }

        public DbSet<Entities.Customer> Customers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration["ConnectionStrings:DefaultConnectionString"]);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Entities.Customer>().HasIndex(x=> x.UserName).IsUnique();
            modelBuilder.Entity<Entities.Customer>().HasIndex(x=> x.EmailAddress).IsUnique();
        }
    }
}
