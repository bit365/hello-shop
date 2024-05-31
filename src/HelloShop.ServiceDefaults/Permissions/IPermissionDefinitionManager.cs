// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.ServiceDefaults.Permissions;

public interface IPermissionDefinitionManager
{
    Task<PermissionDefinition> GetAsync(string name);

    Task<PermissionDefinition?> GetOrNullAsync(string name);

    Task<IReadOnlyList<PermissionDefinition>> GetPermissionsAsync();

    Task<IReadOnlyList<PermissionGroupDefinition>> GetGroupsAsync();
}
