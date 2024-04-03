namespace HelloShop.ServiceDefaults.Authorization;

public record struct ResourceInfo(string ResourceType, string ResourceId) : IAuthorizationResource
{
    public override readonly string ToString() => $"{ResourceType}:{ResourceId}";
}

