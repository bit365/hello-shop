// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ProductService.Entities.Products;
using HelloShop.ProductService.Infrastructure;
using HelloShop.ServiceDefaults.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace HelloShop.ProductService.DataSeeding
{
    public class ProductDataSeedingProvider(ProductServiceDbContext dbContext, IWebHostEnvironment env, ILogger<ProductDataSeedingProvider> logger) : IDataSeedingProvider
    {
        public record CatalogSourceEntry(int Id, string Name, string Type, string Brand, string Description, decimal Price);

        public async Task SeedingAsync(IServiceProvider ServiceProvider, CancellationToken cancellationToken = default)
        {
            await dbContext.Database.EnsureCreatedAsync(cancellationToken);

            if (!dbContext.Set<Product>().Any())
            {
                string sourcePath = Path.Combine(env.ContentRootPath, "DataSeeding", "SetupCatalogSamples.json");
                string sourceJson = await File.ReadAllTextAsync(sourcePath, cancellationToken);
                var catalogItems = JsonSerializer.Deserialize<IEnumerable<CatalogSourceEntry>>(sourceJson);

                if (catalogItems != null && catalogItems.Any())
                {
                    dbContext.RemoveRange(dbContext.Set<Brand>());

                    await dbContext.Set<Brand>().AddRangeAsync(catalogItems.DistinctBy(x => x.Type).Select(x => new Brand { Name = x.Type }), cancellationToken);

                    await dbContext.SaveChangesAsync(cancellationToken);

                    var brandIdsByName = await dbContext.Set<Brand>().ToDictionaryAsync(x => x.Name, x => x.Id, cancellationToken: cancellationToken);

                    logger.LogInformation("Seeded catalog with {NumBrands} brands", brandIdsByName.Count);

                    await dbContext.Set<Product>().AddRangeAsync(catalogItems.Select(x => new Product
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Price = x.Price,
                        BrandId = brandIdsByName[x.Type],
                        ImageUrl = $"https://oss.xcode.me/notes/helloshop/products/{x.Id}.webp",
                        AvailableStock = 100
                    }), cancellationToken);

                    int result = await dbContext.SaveChangesAsync(cancellationToken);

                    logger.LogInformation("Seeded catalog with {NumItems} items", result);
                }
            }
        }
    }
}