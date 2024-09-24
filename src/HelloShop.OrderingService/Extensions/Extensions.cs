// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.OrderingService.Behaviors;
using HelloShop.OrderingService.Constants;
using HelloShop.OrderingService.Infrastructure;
using HelloShop.OrderingService.Services;
using HelloShop.OrderingService.Workers;
using HelloShop.ServiceDefaults.DistributedEvents.Abstractions;
using HelloShop.ServiceDefaults.DistributedEvents.DaprBuildingBlocks;
using HelloShop.ServiceDefaults.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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

            builder.Services.AddScoped<IClientRequestManager, ClientRequestManager>();

            builder.Services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                options.AddBehavior(typeof(LoggingBehavior<,>));
                options.AddBehavior(typeof(ValidatorBehavior<,>));
                options.AddBehavior(typeof(TransactionBehavior<,>));
            });

            builder.Services.AddModelMapper().AddModelValidator();

            builder.Services.AddTransient<ISmsSender, MessageService>().AddTransient<IEmailSender, MessageService>();

            builder.AddDaprDistributedEventBus().AddSubscriptionFromAssembly();

            builder.Services.Configure<HostOptions>(hostOptions =>
            {
                hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
            });

            builder.Services.AddHostedService<GracePeriodWorker>();
        }

        public static WebApplication MapApplicationEndpoints(this WebApplication app)
        {
            app.UseDataSeedingProviders();
            app.MapDaprDistributedEventBus();

            return app;
        }
    }
}
