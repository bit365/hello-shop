// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.ApiService.Infrastructure;

public interface IConfiguredServiceEndPointResolver
{
    public Task<IReadOnlyCollection<ConfiguredServiceEndPoint>> GetConfiguredServiceEndpointsAsync(CancellationToken cancellationToken = default);
}