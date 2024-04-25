using HelloShop.ServiceDefaults.Permissions;

namespace HelloShop.ApiService.Services;

public interface IPermissionService
{
    Task<IReadOnlyList<PermissionGroupDefinitionResponse>> GetAllPermissionGorupDefinitionsAsync(CancellationToken cancellationToken = default);
}