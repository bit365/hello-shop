namespace HelloShop.ServiceDefaults.Authorization;

public class PermissionGrantedResponse
{
    public required string Name { get; set; }

    public string? ResourceType { get; set; }

    public string? ResourceId { get; set; }

    public bool IsGranted { get; set; }
}
