// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace MultiTenancySample.ServiceDefaults
{
    public interface ITenantIdProvider
    {
        Task<string?> GetTenantIdAsync();
    }
}
