// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.EntityFrameworkCore;

namespace HelloShop.EventBus.Logging
{
    public class ResilientTransaction(DbContext dbContext)
    {
        public static ResilientTransaction New(DbContext context) => new(context);

        public async Task ExecuteAsync(Func<Task> action)
        {
            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await dbContext.Database.BeginTransactionAsync();
                await action();
                await transaction.CommitAsync();
            });
        }
    }
}
