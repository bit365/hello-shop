// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.EventBus.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace HelloShop.EventBus.Dapr
{
    public static class DaprEventBusExtensions
    {
        private const string DefaultSectionName = "DaprEventBus";

        public static IEventBusBuilder AddDaprEventBus(this IHostApplicationBuilder builder, string sectionName = DefaultSectionName)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.Services.AddDaprClient();

            DaprEventBusOptions daprOptions = new();

            builder.Configuration.GetSection(sectionName).Bind(daprOptions);

            builder.Services.AddSingleton(Options.Create(daprOptions));

            builder.Services.AddSingleton<IEventBus, DaprEventBus>();

            if (daprOptions.RequireAuthenticatedDaprApiToken)
            {
                builder.Services.AddAuthentication().AddDapr();
                builder.Services.Configure<AuthorizationOptions>(options => options.AddDapr());
            }

            return new DistributedEventBusBuilder(builder.Services);
        }

        private class DistributedEventBusBuilder(IServiceCollection services) : IEventBusBuilder
        {
            public IServiceCollection Services => services;
        }

        public static WebApplication MapDaprEventBus(this WebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);

            var eventBusOptions = app.Services.GetRequiredService<IOptions<EventBusOptions>>().Value;

            var daprEventBusOptions = app.Services.GetRequiredService<IOptions<DaprEventBusOptions>>().Value;

            RouteHandlerBuilder routeHandler = app.MapPost($"/api/DistributedEvents", async (DaprCloudEvent<JsonElement> cloudEvent, IHttpContextAccessor contextAccessor) =>
            {
                var httpContext = contextAccessor.HttpContext ?? throw new InvalidOperationException("HTTP context not available.");

                if (string.IsNullOrWhiteSpace(cloudEvent.Topic) || !eventBusOptions.EventTypes.TryGetValue(cloudEvent.Topic, out Type? eventType) || eventType is null)
                {
                    return Results.NotFound();
                }

                var jsonOptions = httpContext.RequestServices.GetRequiredService<IOptions<Microsoft.AspNetCore.Mvc.JsonOptions>>().Value;

                if (JsonSerializer.Deserialize(cloudEvent.Data.GetRawText(), eventType, jsonOptions.JsonSerializerOptions) is not DistributedEvent @event)
                {
                    return Results.BadRequest();
                }

                foreach (var handler in httpContext.RequestServices.GetKeyedServices<IDistributedEventHandler>(eventType))
                {
                    await handler.HandleAsync(@event);
                }

                return Results.Ok();

            }).WithTags(nameof(DistributedEvent));

            app.MapSubscribeHandler();

            foreach (var subscription in eventBusOptions.EventTypes)
            {
                routeHandler.WithTopic(daprEventBusOptions.PubSubName, subscription.Key, enableRawPayload: false);
            }

            return app;
        }
    }
}
