using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.LoadBalancing;

namespace HelloShop.ApiService.Infrastructure;

public class CustomReverseProxyConfigProvider(IConfiguredServiceEndPointResolver configuredServiceEndPointResolver) : IReverseProxyConfigProvider
{
    public async Task<IReadOnlyList<RouteConfig>> GetRoutesAsync()
    {
        List<RouteConfig> routeConfigs = [];

        foreach (var serviceEndPoint in await configuredServiceEndPointResolver.GetConfiguredServiceEndpointsAsync())
        {
            string serviceName = serviceEndPoint.ServiceName;

            routeConfigs.Add(new RouteConfig()
            {
                // Forces a new route id each time GetRoutes is called.
                RouteId = serviceName,
                ClusterId = serviceName,
                Match = new RouteMatch
                {
                    // This catch-all pattern matches all request paths.
                    Path = $"{serviceEndPoint.ServiceName}/{{**remainder}}"
                },
                Transforms = [new Dictionary<string, string> { { "PathPattern", "{**remainder}" } }]
            });
        }

        return routeConfigs;
    }

    public async Task<IReadOnlyList<ClusterConfig>> GetClustersAsync()
    {
        List<ClusterConfig> clusterConfigs = [];

        foreach (var serviceEndPoint in await configuredServiceEndPointResolver.GetConfiguredServiceEndpointsAsync())
        {
            string serviceName = serviceEndPoint.ServiceName;

            string? address = serviceEndPoint.Endpoints?.OrderBy(x => x)?.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(address))
            {
                UriBuilder uriBuilder = new(address) { Host = serviceName };

                clusterConfigs.Add(new ClusterConfig()
                {
                    ClusterId = serviceName,
                    LoadBalancingPolicy = LoadBalancingPolicies.FirstAlphabetical,
                    Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
                        {
                            { "destination1", new DestinationConfig() { Address = uriBuilder.ToString() } }
                        },
                });
            }
        }

        return clusterConfigs;
    }
}
