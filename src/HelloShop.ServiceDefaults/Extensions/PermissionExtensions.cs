using HelloShop.ServiceDefaults.Authorization;
using HelloShop.ServiceDefaults.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HelloShop.ServiceDefaults.Extensions;

public static class PermissionExtensions
{
    public static IServiceCollection AddPermissionDefinitions(this IServiceCollection services, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();

        var permissionDefinitionProviders = assembly.ExportedTypes.Where(t => t.IsAssignableTo(typeof(IPermissionDefinitionProvider)));

        permissionDefinitionProviders.ToList().ForEach(t => services.AddTransient(typeof(IPermissionDefinitionProvider), t));

        services.AddTransient<IPermissionDefinitionManager, PermissionDefinitionManager>();

        return services;
    }

    public static IEndpointRouteBuilder MapPermissionDefinitions(this IEndpointRouteBuilder endpoints, params string[] tags)
    {
        var routeGroup = endpoints.MapGroup(string.Empty);

        routeGroup.MapGet("PermissionDefinitions", async (IPermissionDefinitionManager permissionDefinitionManager) =>
        {
            List<PermissionGroupDefinitionResponse> result = [];

            var permissionGroups = await permissionDefinitionManager.GetGroupsAsync();

            foreach (var permissionGroup in permissionGroups)
            {
                PermissionGroupDefinitionResponse permissionGroupDefinition = new()
                {
                    Name = permissionGroup.Name,
                    DisplayName = permissionGroup.DisplayName,
                    Permissions = []
                };

                foreach (PermissionDefinition? permission in permissionGroup.GetPermissionsWithChildren())
                {
                    PermissionDefinitionResponse permissionDefinition = new()
                    {
                        Name = permission.Name,
                        DisplayName = permission.DisplayName,
                        ParentName = permission.Parent?.Name
                    };

                    permissionGroupDefinition.Permissions.Add(permissionDefinition);
                }

                result.Add(permissionGroupDefinition);
            }

            return result;

        }).WithTags(tags);

        return routeGroup;
    }

    public static IServiceCollection AddRemotePermissionChecker(this IServiceCollection services, Action<RemotePermissionCheckerOptions> configureOptions)
    {
        services.Configure(configureOptions);

        services.AddTransient<IPermissionChecker, RemotePermissionChecker>();

        return services;
    }

    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
        services.AddTransient<IAuthorizationHandler, PermissionRequirementHandler>();

        return services;
    }
}
