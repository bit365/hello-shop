// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace MultiTenancySample.ServiceDefaults
{
    public interface ICurrentTenant
    {
        string? TenantId { get; }

        IDisposable SetTenant(string? tenantId);
    }
}
