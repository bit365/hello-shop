using Microsoft.EntityFrameworkCore;
using MultiTenancySample.SchemaIsolationService.Entities;
using MultiTenancySample.ServiceDefaults;

namespace MultiTenancySample.SchemaIsolationService.EntityFrameworks
{
    public class DataSeeding(IDbContextFactory<SchemaIsolationDbContext> dbContext, ICurrentTenant currentTenant)
    {
        public async Task SeedDataAsync()
        {
            currentTenant.SetTenant("Tenant1");

            SchemaIsolationDbContext dbContext1 = await dbContext.CreateDbContextAsync();

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

            SchemaIsolationDbContext dbContext2 = dbContext.CreateDbContext();

            await dbContext2.Database.MigrateAsync();

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
