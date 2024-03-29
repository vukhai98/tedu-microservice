﻿using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Intrerfaces;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBaseAsync<Order, long, OrderContext>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext, IUnitOfWork<OrderContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            var result = await CreateAsync(order);

            return order;
        }

        public async Task<IEnumerable<Order>> GetOrderByUserName(string userName)
        {
            var result = await FindByCondition(x => x.UserName.ToLower().Equals(userName.ToLower())).ToListAsync();

            return result;
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
             await UpdateAsync(order);

            return order;
        }
    }
}
