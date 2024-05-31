// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ServiceDefaults.Permissions;
using Microsoft.Extensions.Localization;

namespace HelloShop.ProductService.PermissionProviders;

public class CatalogPermissionDefinitionProvider(IStringLocalizer<CatalogPermissionDefinitionProvider> localizer) : IPermissionDefinitionProvider
{
    public void Define(PermissionDefinitionContext context)
    {
        var identityGroup = context.AddGroup(CatalogPermissions.GroupName, localizer["CatalogManagement"]);

        var productsPermission = identityGroup.AddPermission(CatalogPermissions.Products.Default, localizer["ProductManagement"]);
        productsPermission.AddChild(CatalogPermissions.Products.Create, localizer["Create"]);
        productsPermission.AddChild(CatalogPermissions.Products.Details, localizer["Details"]);
        productsPermission.AddChild(CatalogPermissions.Products.Update, localizer["Edit"]);
        productsPermission.AddChild(CatalogPermissions.Products.Delete, localizer["Delete"]);

        var brandsPermission = identityGroup.AddPermission(CatalogPermissions.Brands.Default, localizer["BrandManagement"]);
        brandsPermission.AddChild(CatalogPermissions.Brands.Create, localizer["Create"]);
        brandsPermission.AddChild(CatalogPermissions.Brands.Details, localizer["Details"]);
        brandsPermission.AddChild(CatalogPermissions.Brands.Update, localizer["Edit"]);
        brandsPermission.AddChild(CatalogPermissions.Brands.Delete, localizer["Delete"]);
    }
}