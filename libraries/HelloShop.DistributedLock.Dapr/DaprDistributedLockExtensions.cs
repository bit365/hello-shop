// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;

namespace HelloShop.DistributedLock.Dapr
{
    public static class DaprDistributedLockExtensions
    {
        public static IServiceCollection AddDaprDistributedLock(this IServiceCollection services)
        {
            services.AddSingleton<IDistributedLock, DaprDistributedLock>();

            return services;
        }
    }
}
