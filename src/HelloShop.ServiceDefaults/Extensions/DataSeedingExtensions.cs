// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ServiceDefaults.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HelloShop.ServiceDefaults.Extensions
{
    public static class DataSeedingExtensions
    {
        public static IServiceCollection AddDataSeedingProviders(this IServiceCollection services, Assembly? assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();

            var dataSeedProviders = assembly.ExportedTypes.Where(t => t.IsAssignableTo(typeof(IDataSeedingProvider)) && t.IsClass);

            dataSeedProviders.ToList().ForEach(t => services.AddTransient(typeof(IDataSeedingProvider), t));

            return services;
        }

        public static IApplicationBuilder UseDataSeedingProviders(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var dataSeedingProviders = serviceScope.ServiceProvider.GetServices<IDataSeedingProvider>().OrderBy(x => x.Order);

            foreach (var dataSeedingProvider in dataSeedingProviders)
            {
                dataSeedingProvider.SeedingAsync(serviceScope.ServiceProvider).Wait();
            }

            return app;
        }
    }
}
