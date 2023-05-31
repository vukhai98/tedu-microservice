﻿using Contracts.Domains.Interfaces;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;

namespace Product.API.Persistence
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {

        }
        public DbSet<CatalogProduct> Products { set; get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Set ProductNo is Unquie
            modelBuilder.Entity<CatalogProduct>().HasIndex(x => x.No).IsUnique();
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var modified = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified ||
                e.State == EntityState.Added ||
                e.State == EntityState.Deleted);

            foreach (var item in modified)
            {
                switch (item.State)
                {
                    case EntityState.Modified:
                        Entry(item.Entity).Property("Id").IsModified = false;
                        if (item.Entity is IDateTracking modifiedEntity)
                        {
                            modifiedEntity.LastModifiedDate = DateTime.UtcNow;
                            item.State = EntityState.Modified;
                        }
                        break;
                    case EntityState.Added:
                        if (item.Entity is IDateTracking addEntity)
                        {
                            addEntity.CreatedDate = DateTime.UtcNow;
                        }
                        break;
                    default:
                        break;
                }
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
