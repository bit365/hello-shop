namespace HelloShop.ApiService.Infrastructure
{
    public class ConfiguredServiceEndPoint
    {
        public required string ServiceName { get; set; }

        public IReadOnlyCollection<string>? Endpoints { get; set; }
    }
}