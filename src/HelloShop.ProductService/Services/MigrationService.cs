// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;

namespace HelloShop.ProductService.Services
{
    public class MigrationService<TDbContext>(IServiceScopeFactory scopeFactory) where TDbContext : DbContext
    {
        public const string ActivitySourceName = "Migrations";
        private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

        public async ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

            try
            {
                using var scope = scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
                await RunMigrationAsync(dbContext, cancellationToken);
            }
            catch (Exception ex)
            {
                activity?.AddException(ex);
            }
        }

        private static async ValueTask RunMigrationAsync(TDbContext dbContext, CancellationToken cancellationToken)
        {
            var strategy = dbContext.Database.CreateExecutionStrategy();
            var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();
            var historyRepository = dbContext.GetService<IHistoryRepository>();
            await strategy.ExecuteAsync(async () =>
            {
                if (!await dbCreator.ExistsAsync(cancellationToken))
                {
                    await dbCreator.CreateAsync(cancellationToken);
                }
                await historyRepository.CreateIfNotExistsAsync();
                await dbContext.Database.MigrateAsync(cancellationToken);
            });
        }
    }
}
