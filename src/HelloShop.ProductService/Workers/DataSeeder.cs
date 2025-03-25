// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ProductService.Entities.Products;
using HelloShop.ProductService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace HelloShop.ProductService.Workers
{
    public class DataSeeder(IServiceScopeFactory scopeFactory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = scopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ProductServiceDbContext>();
            var strategy = dbContext.Database.CreateExecutionStrategy();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            var timeProvider = scope.ServiceProvider.GetRequiredService<TimeProvider>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DataSeeder>>();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await dbContext.Database.BeginTransactionAsync(stoppingToken);

                if (!dbContext.Set<Product>().Any())
                {
                    string sourcePath = Path.Combine(env.ContentRootPath, "Workers", "SetupCatalogSamples.json");
                    using var stream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
                    var catalogItems = await JsonSerializer.DeserializeAsync<IEnumerable<CatalogSourceEntry>>(stream, cancellationToken: stoppingToken);

                    if (catalogItems != null && catalogItems.Any())
                    {
                        dbContext.RemoveRange(dbContext.Set<Brand>());
                        await dbContext.Set<Brand>().AddRangeAsync(catalogItems.DistinctBy(x => x.Type).Select(x => new Brand { Name = x.Type }), stoppingToken);
                        await dbContext.SaveChangesAsync(stoppingToken);
                        var brandIdsByName = await dbContext.Set<Brand>().ToDictionaryAsync(x => x.Name, x => x.Id, cancellationToken: stoppingToken);

                        logger.LogInformation("Seeded catalog with {NumBrands} brands.", brandIdsByName.Count);

                        await dbContext.Set<Product>().AddRangeAsync(catalogItems.Select(x => new Product
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Description = x.Description,
                            Price = x.Price,
                            BrandId = brandIdsByName[x.Type],
                            ImageUrl = $"https://oss.xcode.me/notes/helloshop/products/{x.Id}.webp",
                            AvailableStock = 100,
                            CreationTime = timeProvider.GetUtcNow()
                        }), stoppingToken);
                        int result = await dbContext.SaveChangesAsync(stoppingToken);

                        logger.LogInformation("Seeded catalog with {NumItems} items.", result);
                    }
                }

                await transaction.CommitAsync(stoppingToken);
            });
        }

        private record CatalogSourceEntry(int Id, string Name, string Type, string Brand, string Description, decimal Price);
    }
}
