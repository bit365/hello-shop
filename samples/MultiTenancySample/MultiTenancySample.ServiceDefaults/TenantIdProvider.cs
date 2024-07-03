using Microsoft.AspNetCore.Http;

namespace MultiTenancySample.ServiceDefaults
{
    public class TenantIdProvider(IHttpContextAccessor httpContextAccessor) : ITenantIdProvider
    {
        public async Task<string?> GetTenantIdAsync()
        {
            HttpContext httpContext = httpContextAccessor.HttpContext ?? new DefaultHttpContext();

            const string tenantKey = "tenant";

            if(httpContext.User.FindAll(tenantKey).Any())
            {
                return httpContext.User.FindFirst(tenantKey)?.Value;
            }

            if (httpContext.Request.Headers.TryGetValue(tenantKey, out var headerValues))
            {
                return headerValues.First();
            }

            if (httpContext.Request.Query.TryGetValue(tenantKey, out var queryValues))
            {
                return queryValues.First();
            }

            if (httpContext.Request.Cookies.TryGetValue(tenantKey, out var cookieValue))
            {
                return cookieValue;
            }

            if (httpContext.Request.RouteValues.TryGetValue(tenantKey, out var routeValue))
            {
                return routeValue?.ToString();
            }

            return await Task.FromResult<string?>(null);
        }
    }
}
