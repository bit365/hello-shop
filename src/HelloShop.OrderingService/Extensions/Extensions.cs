// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.EventBus.Abstractions;
using HelloShop.EventBus.Dapr;
using HelloShop.OrderingService.Behaviors;
using HelloShop.OrderingService.Constants;
using HelloShop.OrderingService.Infrastructure;
using HelloShop.OrderingService.Queries;
using HelloShop.OrderingService.Services;
using HelloShop.OrderingService.Workers;
using HelloShop.ServiceDefaults.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
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

            builder.AddNpgsqlDbContext<OrderingServiceDbContext>(connectionName: DbConstants.MasterConnectionStringName, configureDbContextOptions: options =>
            {
                new NpgsqlDbContextOptionsBuilder(options).MigrationsHistoryTable(DbConstants.MigrationsHistoryTableName);
                options.UseSnakeCaseNamingConvention();
            });

            builder.Services.AddSingleton<MigrationService<OrderingServiceDbContext>>().AddHostedService<DataSeeder>();

            builder.Services.AddScoped<IClientRequestManager, ClientRequestManager>();

            builder.Services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                options.AddOpenBehavior(typeof(LoggingBehavior<,>));
                options.AddOpenBehavior(typeof(ValidatorBehavior<,>));
                options.AddOpenBehavior(typeof(TransactionBehavior<,>));
            });

            builder.Services.AddScoped<IOrderQueries, OrderQueries>();

            builder.Services.AddModelMapper().AddModelValidator();

            builder.Services.AddTransient<ISmsSender, MessageService>().AddTransient<IEmailSender, MessageService>();

            builder.AddDaprEventBus().AddSubscriptionFromAssembly();

            builder.Services.Configure<HostOptions>(hostOptions =>
            {
                hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
            });

            builder.Services.AddHostedService<GracePeriodWorker>();
            builder.Services.AddHostedService<PaymentWorker>();

            builder.Services.AddTransient<IDistributedEventService, DistributedEventService<OrderingServiceDbContext>>();

            builder.Services.AddOpenApi();

            builder.Services.AddSingleton(TimeProvider.System);
        }

        public static WebApplication MapApplicationEndpoints(this WebApplication app)
        {
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseOpenApi();
            app.MapDaprEventBus();

            return app;
        }
    }
}
