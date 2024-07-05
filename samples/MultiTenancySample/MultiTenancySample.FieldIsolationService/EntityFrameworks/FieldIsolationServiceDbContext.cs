// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MultiTenancySample.FieldIsolationService.Entities;
using MultiTenancySample.ServiceDefaults;

namespace MultiTenancySample.FieldIsolationService.EntityFrameworks
{
    public class FieldIsolationServiceDbContext(DbContextOptions<FieldIsolationServiceDbContext> options, ICurrentTenant currentTenant) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(32);

            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.IsAssignableTo(typeof(IMultiTenant)))
                {
                    modelBuilder.Entity(entityType.ClrType).AddQueryFilter<IMultiTenant>(e => e.TenantId == currentTenant.TenantId);
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new TenantSaveChangesInterceptor(currentTenant));

            base.OnConfiguring(optionsBuilder);
        }
    }
}
