// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;

namespace HelloShop.ServiceDefaults.Permissions;

public class PermissionDefinitionManager : IPermissionDefinitionManager
{
    private readonly Lazy<Dictionary<string, PermissionGroupDefinition>> _lazyPermissionGroupDefinitions;

    protected IDictionary<string, PermissionGroupDefinition> PermissionGroupDefinitions => _lazyPermissionGroupDefinitions.Value;

    private readonly Lazy<Dictionary<string, PermissionDefinition>> _lazyPermissionDefinitions;

    protected IDictionary<string, PermissionDefinition> PermissionDefinitions => _lazyPermissionDefinitions.Value;

    private readonly IServiceProvider _serviceProvider;

    public PermissionDefinitionManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _lazyPermissionGroupDefinitions = new Lazy<Dictionary<string, PermissionGroupDefinition>>(CreatePermissionGroupDefinitions, isThreadSafe: true);
        _lazyPermissionDefinitions = new Lazy<Dictionary<string, PermissionDefinition>>(CreatePermissionDefinitions, isThreadSafe: true);
    }

    protected virtual Dictionary<string, PermissionGroupDefinition> CreatePermissionGroupDefinitions()
    {
        using var scope = _serviceProvider.CreateScope();

        var context = new PermissionDefinitionContext(scope.ServiceProvider);

        var providers = _serviceProvider.GetServices<IPermissionDefinitionProvider>();

        foreach (IPermissionDefinitionProvider provider in providers)
        {
            provider.Define(context);
        }

        return context.Groups;
    }

    protected virtual Dictionary<string, PermissionDefinition> CreatePermissionDefinitions()
    {
        var permissions = new Dictionary<string, PermissionDefinition>();

        foreach (var groupDefinition in PermissionGroupDefinitions.Values)
        {
            foreach (var permission in groupDefinition.Permissions)
            {
                AddPermissionToDictionaryRecursively(permissions, permission);
            }
        }

        return permissions;
    }

    protected virtual void AddPermissionToDictionaryRecursively(Dictionary<string, PermissionDefinition> permissions, PermissionDefinition permission)
    {
        if (permissions.ContainsKey(permission.Name))
        {
            throw new InvalidOperationException($"Duplicate permission name {permission.Name}");
        }

        permissions[permission.Name] = permission;

        foreach (var child in permission.Children)
        {
            AddPermissionToDictionaryRecursively(permissions, child);
        }
    }

    public async Task<PermissionDefinition> GetAsync(string name)
    {
        var permission = await GetOrNullAsync(name);

        return permission ?? throw new InvalidOperationException($"Undefined permission {name}");
    }

    public Task<PermissionDefinition?> GetOrNullAsync(string name)
    {
        return Task.FromResult(PermissionDefinitions.TryGetValue(name, out var obj) ? obj : default);
    }

    public Task<IReadOnlyList<PermissionGroupDefinition>> GetGroupsAsync()
    {
        IReadOnlyList<PermissionGroupDefinition> permissionGroups = [.. PermissionGroupDefinitions.Values];

        return Task.FromResult(permissionGroups);
    }

    public Task<IReadOnlyList<PermissionDefinition>> GetPermissionsAsync()
    {
        IReadOnlyList<PermissionDefinition> permissions = [.. PermissionDefinitions.Values];

        return Task.FromResult(permissions);
    }
}
