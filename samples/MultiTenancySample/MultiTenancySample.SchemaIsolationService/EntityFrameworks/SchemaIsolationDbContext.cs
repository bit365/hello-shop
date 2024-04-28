using Microsoft.EntityFrameworkCore;
using MultiTenancySample.SchemaIsolationService.Entities;
using MultiTenancySample.ServiceDefaults;

namespace MultiTenancySample.SchemaIsolationService.EntityFrameworks
{
    public class SchemaIsolationDbContext(DbContextOptions<SchemaIsolationDbContext> options, ICurrentTenant currentTenant) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(32);

            // This scenario is not directly supported by EF Core and is not a recommended solution.
            // https://learn.microsoft.com/en-us/ef/core/miscellaneous/multitenancy

            modelBuilder.HasDefaultSchema(currentTenant.TenantId);

            base.OnModelCreating(modelBuilder);
        }
    }
}