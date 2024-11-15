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
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace HelloShop.OrderingService.Extensions
{
    public static class Extensions
    {
        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {
            const string issuerSigningKey = ServiceDefaults.Constants.IdentityConstants.IssuerSigningKey;

            builder.Services.AddAuthentication().AddJwtBearer(options =>
            {
                options.TokenValidationParameters.ValidateIssuer = false;
                options.TokenValidationParameters.ValidateAudience = false;
                options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(issuerSigningKey));
            });

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddDataSeedingProviders();

            builder.Services.AddDbContext<OrderingServiceDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString(DbConstants.MasterConnectionStringName));
            });

            builder.Services.AddScoped<IClientRequestManager, ClientRequestManager>();

            builder.Services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                options.AddOpenBehavior(typeof(LoggingBehavior<,>));
                options.AddOpenBehavior(typeof(ValidatorBehavior<,>));
                options.AddOpenBehavior(typeof(TransactionBehavior<,>));
            });

            builder.Services.AddModelMapper().AddModelValidator();

            builder.Services.AddTransient<ISmsSender, MessageService>().AddTransient<IEmailSender, MessageService>();

            builder.AddDaprDistributedEventBus().AddSubscriptionFromAssembly();

            builder.Services.Configure<HostOptions>(hostOptions =>
            {
                hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
            });

            builder.Services.AddHostedService<GracePeriodWorker>();
            builder.Services.AddHostedService<PaymentWorker>();

            builder.Services.AddTransient<IDistributedEventService, DistributedEventService<OrderingServiceDbContext>>();

            builder.Services.AddOpenApi();
        }

        public static WebApplication MapApplicationEndpoints(this WebApplication app)
        {
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseOpenApi();
            app.UseDataSeedingProviders();
            app.MapDaprDistributedEventBus();

            return app;
        }
    }
}
