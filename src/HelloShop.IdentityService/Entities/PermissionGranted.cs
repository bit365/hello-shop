// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.IdentityService.Entities;

public class PermissionGranted
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public required string PermissionName { get; set; }

    public string? ResourceType { get; set; }

    public string? ResourceId { get; set; }
}
