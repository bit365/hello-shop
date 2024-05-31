// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.ServiceDefaults.Permissions;

public class PermissionGroupDefinitionResponse
{
    public required string GroupName { get; init; }

    public string? DisplayName { get; set; }

    public required List<PermissionDefinitionResponse> Permissions { get; init; }
}
