namespace HelloShop.ApiService.Infrastructure;

public interface IConfiguredServiceEndPointResolver
{
    public Task<IReadOnlyCollection<ConfiguredServiceEndPoint>> GetConfiguredServiceEndpointsAsync(CancellationToken cancellationToken = default);
}