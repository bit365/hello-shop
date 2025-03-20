// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HelloShop.EventBus.RabbitMQ
{
    public static class RabbitMQEventBusExtensions
    {
        private const string DefaultSectionName = "RabbitMQEventBus";

        public static IEventBusBuilder AddRabbitMqEventBus(this IHostApplicationBuilder builder, string connectionName, string sectionName = DefaultSectionName)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.AddRabbitMQClient(connectionName, configureConnectionFactory: factory =>
            {
                factory.DispatchConsumersAsync = true;
            });

            builder.Services.Configure<EventBusOptions>(builder.Configuration.GetSection(sectionName));

            builder.Services.AddSingleton<IEventBus, RabbitMQEventBus>();
            builder.Services.AddSingleton<IHostedService>(sp => (RabbitMQEventBus)sp.GetRequiredService<IEventBus>());

            return new EventBusBuilder(builder.Services);
        }

        private class EventBusBuilder(IServiceCollection services) : IEventBusBuilder
        {
            public IServiceCollection Services => services;
        }
    }
}
