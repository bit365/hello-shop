using Yarp.ReverseProxy.Configuration;

namespace HelloShop.ApiService.Infrastructure;

public interface IReverseProxyConfigProvider
{
    Task<IReadOnlyList<RouteConfig>> GetRoutesAsync();

    Task<IReadOnlyList<ClusterConfig>> GetClustersAsync();
}
