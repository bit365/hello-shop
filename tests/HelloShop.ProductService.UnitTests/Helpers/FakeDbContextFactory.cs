// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ProductService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HelloShop.ProductService.UnitTests.Helpers
{
    public class FakeDbContextFactory : IDbContextFactory<ProductServiceDbContext>
    {
        public ProductServiceDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ProductServiceDbContext>().UseInMemoryDatabase($"InMemoryTestDb-{DateTimeOffset.UtcNow.ToFileTime()}").Options;

            return new ProductServiceDbContext(options);
        }
    }
}