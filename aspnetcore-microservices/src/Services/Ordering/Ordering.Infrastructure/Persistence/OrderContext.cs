using Contracts.Common.Events;
using Contracts.Common.Interfaces;
using Contracts.Domains.Interfaces;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContext : DbContext
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderContext> _logger;

        public OrderContext(DbContextOptions<OrderContext> options, IMediator mediator, ILogger<OrderContext> logger) : base(options)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public DbSet<Order> Orders { get; set; }

        private List<BaseEvent> _baseEvents;

        // Tự động đăng ký cho tất cả các  configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        private void SetBaseEventsBeforeSavesChanges()
        {
            // Lay cac entity co event 
            var domainEntities = base.ChangeTracker.Entries<IEventEntity>()
                                              .Select(e => e.Entity)
                                              .Where(x => x.DomainEvents().Any())
                                              .ToList();

            _baseEvents = domainEntities.SelectMany(x => x.DomainEvents()).ToList();

            domainEntities.ForEach(x => x.ClearDomainEvents()); 

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToke = new CancellationToken())
        {
            SetBaseEventsBeforeSavesChanges();

            var modified = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified
                                                          || e.State == EntityState.Added
                                                          || e.State == EntityState.Deleted);

            foreach (var item in modified)
            {
                switch (item.State)
                {
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    case EntityState.Deleted:
                        break;
                    case EntityState.Modified:
                        Entry(item.Entity).Property("Id").IsModified = false;
                        if (item.Entity is IDateTracking modifiedEntity)
                        {
                            modifiedEntity.LastModifiedDate = DateTime.UtcNow;
                            item.State = EntityState.Modified;
                        }
                        break;
                    case EntityState.Added:
                        if (item.Entity is IDateTracking addedEntity)
                        {
                            addedEntity.CreatedDate = DateTime.UtcNow;
                            item.State = EntityState.Added;
                        }
                        break;
                    default:
                        break;
                }
            }

            var result = base.SaveChangesAsync(cancellationToke);

            _mediator.DispatchDomainEventAsync(_baseEvents, _logger);

            return result;
        }
    }
}
