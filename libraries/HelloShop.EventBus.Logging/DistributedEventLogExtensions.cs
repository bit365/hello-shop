// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace HelloShop.EventBus.Logging
{
    public static class DistributedEventLogExtensions
    {
        public static void UseDistributedEventLogs(this ModelBuilder builder) => builder.ApplyConfiguration(new EventLogEntityTypeConfiguration());

        public static IServiceCollection AddDistributedEventLogs<TContext>([NotNull] this IServiceCollection services) where TContext : DbContext
        {
            services.AddTransient<IDistributedEventLogService, DistributedEventLogService<TContext>>().AddHostedService<DistributedEventWorker>();
            return services;
        }
    }
}
