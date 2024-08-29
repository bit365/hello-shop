// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ProductService.Entities.Products;

// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ProductService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HelloShop.ProductService.Services
{
    public class ProductService(ProductServiceDbContext dbContext) : IProductService
    {
        public async Task CreateAsync(Product product, CancellationToken cancellationToken = default)
        {
            await dbContext.AddAsync(product, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Product product, CancellationToken cancellationToken = default)
        {
            dbContext.Remove(product);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async ValueTask<Product?> FindAsync(int id, CancellationToken cancellationToken = default) => await dbContext.FindAsync<Product>(id, cancellationToken);

        public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default) => await dbContext.Set<Product>().ToListAsync(cancellationToken);

        public async Task UpdateAsyc(Product product, CancellationToken cancellationToken = default)
        {
            dbContext.Update(product);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
