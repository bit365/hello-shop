// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using CommunityToolkit.Aspire.Hosting.Dapr;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloShop.AppHost.Extensions
{
    public static class DaprSidecarResourceBuilderExtensions
    {
        public static IResourceBuilder<IDaprSidecarResource> WithReference(this IResourceBuilder<IDaprSidecarResource> builder, IResourceBuilder<IResourceWithConnectionString> resourceBuilder, int waitInSeconds = 10)
        {
            builder.WithAnnotation(new EnvironmentCallbackAnnotation(async context =>
            {
                var notificationService = context.ExecutionContext.ServiceProvider.GetRequiredService<ResourceNotificationService>();
                await notificationService.WaitForResourceAsync(resourceBuilder.Resource.Name, KnownResourceStates.Running);
                await Task.Delay(TimeSpan.FromSeconds(waitInSeconds));
                var connectionStringName = resourceBuilder.Resource.ConnectionStringEnvironmentVariable ?? $"ConnectionStrings__{resourceBuilder.Resource.Name}";
                context.EnvironmentVariables[connectionStringName] = new ConnectionStringReference(resourceBuilder.Resource, false);
            }));

            return builder;
        }
    }
}
