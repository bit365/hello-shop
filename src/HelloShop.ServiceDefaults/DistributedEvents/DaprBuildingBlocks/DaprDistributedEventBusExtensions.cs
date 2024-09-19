// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using FluentValidation;
using FluentValidation.Results;
using HelloShop.ServiceDefaults.DistributedEvents.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace HelloShop.ServiceDefaults.DistributedEvents.DaprBuildingBlocks
{
    public static class DaprDistributedEventBusExtensions
    {
        private const string DefaultSectionName = "DaprDistributedEventBus";

        public static IDistributedEventBusBuilder AddDaprDistributedEventBus(this IHostApplicationBuilder builder, string sectionName = DefaultSectionName)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.Services.AddDaprClient();

            DaprDistributedEventBusOptions daprOptions = new();

            builder.Configuration.GetSection(sectionName).Bind(daprOptions);

            builder.Services.AddSingleton(Options.Create(daprOptions));

            builder.Services.AddSingleton<IDistributedEventBus, DaprDistributedEventBus>();

            if (daprOptions.RequireAuthenticatedDaprApiToken)
            {
                builder.Services.AddAuthentication().AddDapr();
                builder.Services.Configure<AuthorizationOptions>(options => options.AddDapr());
            }

            return new DistributedEventBusBuilder(builder.Services);
        }

        private class DistributedEventBusBuilder(IServiceCollection services) : IDistributedEventBusBuilder
        {
            public IServiceCollection Services => services;
        }

        public static WebApplication MapDaprDistributedEventBus(this WebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);

            var eventBusOptions = app.Services.GetRequiredService<IOptions<DistributedEventBusOptions>>().Value;

            var daprEventBusOptions = app.Services.GetRequiredService<IOptions<DaprDistributedEventBusOptions>>().Value;

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

                if (httpContext.RequestServices.GetService(typeof(IValidator<>).MakeGenericType(eventType)) is IValidator validator)
                {
                    ValidationResult validationResult = await validator.ValidateAsync(new ValidationContext<DistributedEvent>(@event));

                    if (!validationResult.IsValid)
                    {
                        return Results.ValidationProblem(validationResult.ToDictionary());
                    }
                }

                foreach (var handler in httpContext.RequestServices.GetKeyedServices<IDistributedEventHandler>(eventType))
                {
                    await handler.HandleAsync(@event);
                }

                return Results.Ok();

            }).WithTags(nameof(DistributedEvent));

            foreach (var subscription in eventBusOptions.EventTypes)
            {
                routeHandler.WithTopic(daprEventBusOptions.PubSubName, subscription.Key, enableRawPayload: false);
            }

            app.MapSubscribeHandler();

            return app;
        }
    }
}
