// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MultiTenancySample.FieldIsolationService.Entities;
using MultiTenancySample.ServiceDefaults;

namespace MultiTenancySample.FieldIsolationService.EntityFrameworks
{
    public class TenantSaveChangesInterceptor(ICurrentTenant currentTenant) : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData.Context is not null)
            {
                MultiTenancyTracking(eventData.Context);
            }

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context is not null)
            {
                MultiTenancyTracking(eventData.Context);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void MultiTenancyTracking(DbContext dbContext)
        {
            IEnumerable<EntityEntry<IMultiTenant>> multiTenancyEntries = dbContext.ChangeTracker.Entries<IMultiTenant>().Where(entry => entry.State == EntityState.Added || entry.State == EntityState.Modified);

            multiTenancyEntries?.ToList().ForEach(entityEntry =>
            {
                entityEntry.Entity.TenantId ??= currentTenant.TenantId;
            });
        }
    }
}
