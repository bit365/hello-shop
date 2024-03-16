namespace HelloShop.ServiceDefaults.Permissions;

public class PermissionDefinitionResponse
{
    public required string Name { get; init; }

    public string? DisplayName { get; set; }

    public string? ParentName { get; set; }
}
