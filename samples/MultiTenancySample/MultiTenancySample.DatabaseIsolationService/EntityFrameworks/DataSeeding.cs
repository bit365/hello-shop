using Microsoft.EntityFrameworkCore;
using MultiTenancySample.DatabaseIsolationService.Entities;
using MultiTenancySample.ServiceDefaults;

namespace MultiTenancySample.DatabaseIsolationService.EntityFrameworks
{
    public class DataSeeding(IDbContextFactory<DatabaseIsolationServiceDbContext> dbContext, ICurrentTenant currentTenant)
    {
        public async Task SeedDataAsync()
        {
            currentTenant.SetTenant("Tenant1");

            DatabaseIsolationServiceDbContext dbContext1 = await dbContext.CreateDbContextAsync();

            if (await dbContext1.Database.EnsureCreatedAsync())
            {
                await dbContext1.AddRangeAsync(new List<Product> {
                    new() { Id = Guid.NewGuid(), Name = "Product1"},
                    new() { Id = Guid.NewGuid(), Name = "Product3"},
                    new() { Id = Guid.NewGuid(), Name = "Product5"}
                });

                await dbContext1.SaveChangesAsync();
            }

            currentTenant.SetTenant("Tenant2");

            DatabaseIsolationServiceDbContext dbContext2 = await dbContext.CreateDbContextAsync();

            if (await dbContext2.Database.EnsureCreatedAsync())
            {
                await dbContext2.AddRangeAsync(new List<Product>
                {
                    new() { Id = Guid.NewGuid(), Name = "Product2"},
                    new() { Id = Guid.NewGuid(), Name = "Product4"},
                    new() { Id = Guid.NewGuid(), Name = "Product6"}
                });

                await dbContext2.SaveChangesAsync();
            }
        }
    }
}
