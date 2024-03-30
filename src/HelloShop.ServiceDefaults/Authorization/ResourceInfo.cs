namespace HelloShop.ServiceDefaults.Authorization;

public class ResourceInfo : IAuthorizationResource
{
    public required string ResourceType { get; set; }

    public required string ResourceId { get; set; }

    public static implicit operator string(ResourceInfo resource) => resource.ToString();

    public static explicit operator ResourceInfo(string resourcePath)
    {
        string[] separators = resourcePath.Split(":");

        if (separators == null || separators.Length != 2)
        {
            throw new ArgumentException("Resource path must be in the format 'type:id'", nameof(resourcePath));
        }

        ResourceInfo resourceInfo = new() { ResourceType = separators.First(), ResourceId = separators.Last() };

        return resourceInfo;
    }


    public override string ToString() => $"{ResourceType}:{ResourceId}";
}
