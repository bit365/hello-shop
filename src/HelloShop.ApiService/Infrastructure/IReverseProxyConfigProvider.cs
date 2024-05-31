// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Yarp.ReverseProxy.Configuration;

namespace HelloShop.ApiService.Infrastructure;

public interface IReverseProxyConfigProvider
{
    Task<IReadOnlyList<RouteConfig>> GetRoutesAsync();

    Task<IReadOnlyList<ClusterConfig>> GetClustersAsync();
}
