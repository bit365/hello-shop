// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ServiceDefaults.Permissions;

namespace HelloShop.ApiService.Services;

public interface IPermissionService
{
    Task<IReadOnlyList<PermissionGroupDefinitionResponse>> GetAllPermissionGorupDefinitionsAsync(CancellationToken cancellationToken = default);
}