// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.ServiceDefaults.Permissions;

public interface IPermissionDefinitionProvider
{
    void Define(PermissionDefinitionContext context);
}
