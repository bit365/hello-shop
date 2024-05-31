// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ProductService.Entities.Products;
using HelloShop.ProductService.EntityFrameworks;
using HelloShop.ServiceDefaults.Infrastructure;

namespace HelloShop.ProductService.DataSeeding
{
    public class ProductDataSeedingProvider(ProductServiceDbContext dbContext) : IDataSeedingProvider
    {
        public async Task SeedingAsync(IServiceProvider ServiceProvider)
        {
            if (!dbContext.Set<Product>().Any())
            {
                Dictionary<string, List<Product>> products = new()
                {
                    { "Google", new List<Product>{
                        new() { Name = "Android", Price = 156.99m },
                        new() { Name = "Gmail", Price = 135.82m },
                        new() { Name = "Google Drive", Price = 99.99m },
                        new() { Name = "Google Maps", Price = 199.99m },
                        new() { Name = "Google Photos", Price = 299.99m },
                        new() { Name = "Google Play", Price = 399.99m },
                        new() { Name = "Google Search", Price = 499.99m },
                        new() { Name = "Google Translate", Price = 599.99m },
                        new() { Name = "Google Chrome", Price = 699.99m },
                        new() { Name = "Google Earth", Price = 799.99m },
                    }},
                    { "Microsoft", new List<Product>{
                        new() { Name = "Windows", Price = 156.99m },
                        new() { Name = "Office", Price = 135.82m },
                        new() { Name = "Azure", Price = 99.99m },
                        new() { Name = "Xbox", Price = 199.99m },
                        new() { Name = "Skype", Price = 299.99m },
                        new() { Name = "LinkedIn", Price = 399.99m },
                        new() { Name = "GitHub", Price = 499.99m },
                        new() { Name = "Visual Studio", Price = 599.99m },
                        new() { Name = "Bing", Price = 699.99m },
                        new() { Name = "OneDrive", Price = 799.99m },
                    }},
                    { "Apple", new List<Product>{
                        new() { Name = "iPhone", Price = 156.99m },
                        new() { Name = "iPad", Price = 135.82m },
                        new() { Name = "Mac", Price = 99.99m },
                        new() { Name = "Apple Watch", Price = 199.99m },
                        new() { Name = "Apple TV", Price = 299.99m },
                        new() { Name = "AirPods", Price = 399.99m },
                        new() { Name = "HomePod", Price = 499.99m },
                        new() { Name = "iPod", Price = 599.99m },
                        new() { Name = "Apple Music", Price = 699.99m },
                        new() { Name = "Apple Pay", Price = 799.99m },
                    }},
                    { "Amazon", new List<Product>{
                        new() { Name = "Amazon Prime", Price = 156.99m },
                        new() { Name = "Kindle", Price = 135.82m },
                        new() { Name = "Fire TV", Price = 99.99m },
                        new() { Name = "Echo", Price = 199.99m },
                        new() { Name = "Ring", Price = 299.99m },
                        new() { Name = "Twitch", Price = 399.99m },
                        new() { Name = "Audible", Price = 499.99m },
                        new() { Name = "Goodreads", Price = 599.99m },
                        new() { Name = "IMDb", Price = 699.99m },
                        new() { Name = "Whole Foods", Price = 799.99m },
                    }},
                    { "Samsung", new List<Product>
                    {
                        new() { Name = "Galaxy", Price = 156.99m },
                        new() { Name = "SmartThings", Price = 135.82m },
                        new() { Name = "Samsung Pay", Price = 99.99m },
                        new() { Name = "Samsung Health", Price = 199.99m },
                        new() { Name = "Samsung DeX", Price = 299.99m },
                        new() { Name = "Samsung Knox", Price = 399.99m },
                        new() { Name = "Samsung Internet", Price = 499.99m },
                        new() { Name = "Samsung Cloud", Price = 599.99m },
                        new() { Name = "Samsung TV", Price = 699.99m },
                        new() { Name = "Samsung Galaxy Store", Price = 799.99m },
                    }}
                };

                foreach (var (brandName, productList) in products)
                {
                    var brand = new Brand { Name = brandName };

                    await dbContext.AddAsync(brand);

                    foreach (var product in productList)
                    {
                        product.Brand = brand;
                        await dbContext.AddAsync(product);
                    }
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}