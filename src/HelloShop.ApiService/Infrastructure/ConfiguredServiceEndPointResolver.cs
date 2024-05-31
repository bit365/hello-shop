// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.Extensions.Options;
using Microsoft.Extensions.ServiceDiscovery;

namespace HelloShop.ApiService.Infrastructure;

public class ConfiguredServiceEndPointResolver(IConfiguration configuration, IOptions<ConfigurationServiceEndpointProviderOptions> resolverOptions) : IConfiguredServiceEndPointResolver
{
    private readonly Lazy<IReadOnlyCollection<ConfiguredServiceEndPoint>> _serviceEndPoints = new(() =>
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(resolverOptions.Value.SectionName, nameof(resolverOptions.Value.SectionName));

        IConfigurationSection section = configuration.GetRequiredSection(resolverOptions.Value.SectionName);

        List<ConfiguredServiceEndPoint> serviceEndPoints = [];

        foreach (IConfigurationSection serviceSection in section.GetChildren())
        {
            string serviceName = serviceSection.Key;

            serviceEndPoints.Add(new ConfiguredServiceEndPoint
            {
                ServiceName = serviceName,
                Endpoints = serviceSection.GetChildren().SelectMany(x => x.Get<List<string>>() ?? []).Distinct().ToList()
            });
        }

        return serviceEndPoints;
    });

    public async Task<IReadOnlyCollection<ConfiguredServiceEndPoint>> GetConfiguredServiceEndpointsAsync(CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(_serviceEndPoints.Value);
    }
}
