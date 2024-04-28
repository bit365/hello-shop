using Microsoft.EntityFrameworkCore;
using MultiTenancySample.DatabaseIsolationService.Entities;
using MultiTenancySample.ServiceDefaults;


namespace MultiTenancySample.DatabaseIsolationService.EntityFrameworks
{
    public class DatabaseIsolationServiceDbContext(DbContextOptions<DatabaseIsolationServiceDbContext> options, ICurrentTenant currentTenant, IConfiguration configuration) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(32);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (currentTenant.TenantId != null)
            {
                string? connectionString = configuration.GetConnectionString(currentTenant.TenantId);

                optionsBuilder.UseNpgsql(connectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}
