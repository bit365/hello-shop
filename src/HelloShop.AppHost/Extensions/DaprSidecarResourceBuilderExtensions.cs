// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using CommunityToolkit.Aspire.Hosting.Dapr;
using Microsoft.Extensions.DependencyInjection;

namespace HelloShop.AppHost.Extensions
{
    public static class DaprSidecarResourceBuilderExtensions
    {
        public static IResourceBuilder<IDaprSidecarResource> WithReferenceAndWaitFor(this IResourceBuilder<IDaprSidecarResource> builder, IResourceBuilder<IResourceWithConnectionString> resourceBuilder)
        {
            builder.WithAnnotation(new EnvironmentCallbackAnnotation(async context =>
            {
                var notificationService = context.ExecutionContext.ServiceProvider.GetRequiredService<ResourceNotificationService>();
                await notificationService.WaitForResourceHealthyAsync(resourceBuilder.Resource.Name);
                var connectionStringName = resourceBuilder.Resource.ConnectionStringEnvironmentVariable ?? $"ConnectionStrings__{resourceBuilder.Resource.Name}";
                context.EnvironmentVariables[connectionStringName] = new ConnectionStringReference(resourceBuilder.Resource, false);
            }));

            return builder;
        }
    }
}
