// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace MultiTenancySample.FieldIsolationService.Entities
{
    public interface IMultiTenant
    {
        string? TenantId { get; set; }
    }
}
