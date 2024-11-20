// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using AutoMapper;
using HelloShop.OrderingService.Constants;
using HelloShop.OrderingService.Entities.Orders;
using HelloShop.OrderingService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HelloShop.OrderingService.Queries
{
    public class OrderQueries : IOrderQueries
    {
        private readonly OrderingServiceDbContext _dbContext;

        private readonly IMapper _mapper;

        public OrderQueries(OrderingServiceDbContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _dbContext = dbContext;
            // TODO：Provide the AddKeyedDbContext extension method on Services at https://github.com/dotnet/efcore/issues/34591
            dbContext.Database.SetConnectionString(configuration.GetConnectionString(DbConstants.SlaveConnectionStringName));
            _mapper = mapper;
        }

        public async Task<OrderDetails> GetOrderAsync(int id)
        {
            var order = await _dbContext.Set<Order>().AsNoTracking().Include(o => o.OrderItems).SingleOrDefaultAsync(o => o.Id == id);
            var orderDetails = _mapper.Map<OrderDetails>(order);

            return orderDetails;
        }

        public async Task<IEnumerable<OrderSummary>> GetOrdersFromUserAsync(int userId)
        {
            var orders = await _dbContext.Set<Order>().AsNoTracking().Where(o => o.BuyerId == userId).ToListAsync();
            var orderSummaries = _mapper.Map<IEnumerable<OrderSummary>>(orders);
            return orderSummaries;
        }
    }
}
