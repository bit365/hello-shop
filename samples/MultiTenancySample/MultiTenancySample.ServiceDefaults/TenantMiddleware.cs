// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.AspNetCore.Http;

namespace MultiTenancySample.ServiceDefaults
{
    public class TenantMiddleware(ICurrentTenant currentTenant, ITenantIdProvider tenantProvider) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string? tenantId = await tenantProvider.GetTenantIdAsync();

            using (currentTenant.SetTenant(tenantId))
            {
                await next(context);
            }
        }
    }
}
