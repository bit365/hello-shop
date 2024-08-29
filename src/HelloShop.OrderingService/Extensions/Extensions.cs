// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.OrderingService.Constants;
using HelloShop.OrderingService.Infrastructure;
using HelloShop.ServiceDefaults.Extensions;
using Microsoft.EntityFrameworkCore;

namespace HelloShop.OrderingService.Extensions
{
    public static class Extensions
    {
        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {
            builder.Services.AddDataSeedingProviders();

            builder.Services.AddDbContext<OrderingServiceDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString(DbConstants.MasterConnectionStringName));
            });
        }

        public static WebApplication MapApplicationEndpoints(this WebApplication app)
        {
            app.UseDataSeedingProviders();

            return app;
        }
    }
}
