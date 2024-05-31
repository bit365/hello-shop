// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ProductService.Entities.Products;

namespace HelloShop.ProductService.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default);

        ValueTask<Product?> FindAsync(int id, CancellationToken cancellationToken = default);

        Task CreateAsync(Product product, CancellationToken cancellationToken = default);

        Task UpdateAsyc(Product product, CancellationToken cancellationToken = default);

        Task DeleteAsync(Product product, CancellationToken cancellationToken = default);
    }
}
