// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.ServiceDefaults.Permissions;

public class PermissionDefinition
{
    public string Name { get; } = default!;

    public string? DisplayName { get; set; }

    public PermissionDefinition? Parent { get; private set; }

    public bool IsEnabled { get; set; }

    private readonly List<PermissionDefinition> _children = [];

    public IReadOnlyList<PermissionDefinition> Children => [.. _children];

    protected internal PermissionDefinition(string name, string? displayName = null, bool isEnabled = true)
    {
        Name = name;
        DisplayName = displayName;
        IsEnabled = isEnabled;
    }

    public virtual PermissionDefinition AddChild(string name, string? displayName = null, bool isEnabled = true)
    {
        var child = new PermissionDefinition(name, displayName, isEnabled) { Parent = this };
        _children.Add(child);
        return child;
    }
}
