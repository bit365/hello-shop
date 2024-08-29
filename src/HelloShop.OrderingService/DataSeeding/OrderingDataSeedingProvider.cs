// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.OrderingService.Infrastructure;
using HelloShop.ServiceDefaults.Infrastructure;

namespace HelloShop.OrderingService.DataSeeding
{
    public class OrderingDataSeedingProvider(OrderingServiceDbContext dbContext) : IDataSeedingProvider
    {
        public async Task SeedingAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
        {
            await dbContext.Database.EnsureCreatedAsync(cancellationToken);
        }
    }
}
