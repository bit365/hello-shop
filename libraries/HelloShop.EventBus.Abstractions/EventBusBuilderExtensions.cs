// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;

namespace HelloShop.EventBus.Abstractions
{
    public static class EventBusBuilderExtensions
    {
        public static IEventBusBuilder ConfigureJsonOptions(this IEventBusBuilder eventBusBuilder, Action<JsonSerializerOptions> configure)
        {
            eventBusBuilder.Services.Configure<EventBusOptions>(o =>
            {
                configure(o.JsonSerializerOptions);
            });

            return eventBusBuilder;
        }

        public static IEventBusBuilder AddSubscription<TEvent, TEventHandler>(this IEventBusBuilder eventBusBuilder) where TEvent : DistributedEvent where TEventHandler : class, IDistributedEventHandler<TEvent>
        {
            eventBusBuilder.Services.AddKeyedTransient<IDistributedEventHandler, TEventHandler>(typeof(TEvent));

            eventBusBuilder.Services.Configure<EventBusOptions>(o =>
            {
                o.EventTypes[typeof(TEvent).Name] = typeof(TEvent);
            });

            return eventBusBuilder;
        }

        public static IEventBusBuilder AddSubscription(this IEventBusBuilder eventBusBuilder, Type eventType, Type eventHandlerType)
        {
            eventBusBuilder.Services.AddKeyedTransient(typeof(IDistributedEventHandler), eventType, eventHandlerType);

            eventBusBuilder.Services.Configure<EventBusOptions>(o =>
            {
                o.EventTypes[eventType.Name] = eventType;
            });

            return eventBusBuilder;
        }

        public static IEventBusBuilder AddSubscriptionFromAssembly(this IEventBusBuilder eventBusBuilder, Assembly? assembly = null)
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
