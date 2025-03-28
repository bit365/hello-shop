// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.EventBus.Logging;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HelloShop.OrderingService.Infrastructure
{
    public partial class OrderingServiceDbContext(DbContextOptions<OrderingServiceDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.UseDistributedEventLogs();
        }
    }
}
