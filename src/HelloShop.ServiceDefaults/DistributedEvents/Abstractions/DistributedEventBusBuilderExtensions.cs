// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;

namespace HelloShop.ServiceDefaults.DistributedEvents.Abstractions
{
    public static class DistributedEventBusBuilderExtensions
    {
        public static IDistributedEventBusBuilder ConfigureJsonOptions(this IDistributedEventBusBuilder eventBusBuilder, Action<JsonSerializerOptions> configure)
        {
            eventBusBuilder.Services.Configure<DistributedEventBusOptions>(o =>
            {
                configure(o.JsonSerializerOptions);
            });

            return eventBusBuilder;
        }

        public static IDistributedEventBusBuilder AddSubscription<TEvent, TEventHandler>(this IDistributedEventBusBuilder eventBusBuilder) where TEvent : DistributedEvent where TEventHandler : class, IDistributedEventHandler<TEvent>
        {
            eventBusBuilder.Services.AddKeyedTransient<IDistributedEventHandler, TEventHandler>(typeof(TEvent));

            eventBusBuilder.Services.Configure<DistributedEventBusOptions>(o =>
            {
                o.EventTypes[typeof(TEvent).Name] = typeof(TEvent);
            });

            return eventBusBuilder;
        }

        public static IDistributedEventBusBuilder AddSubscription(this IDistributedEventBusBuilder eventBusBuilder, Type eventType, Type eventHandlerType)
        {
            eventBusBuilder.Services.AddKeyedTransient(typeof(IDistributedEventHandler), eventType, eventHandlerType);

            eventBusBuilder.Services.Configure<DistributedEventBusOptions>(o =>
            {
                o.EventTypes[eventType.Name] = eventType;
            });

            return eventBusBuilder;
        }

        public static IDistributedEventBusBuilder AddSubscriptionFromAssembly(this IDistributedEventBusBuilder eventBusBuilder, Assembly? assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();

            var handlers = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IDistributedEventHandler))).ToList();

            handlers.ForEach(handler =>
            {
                var eventType = handler.GetInterfaces().Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDistributedEventHandler<>)).GetGenericArguments().Single();
                eventBusBuilder.AddSubscription(eventType, handler);
            });

            return eventBusBuilder;
        }
    }
}
