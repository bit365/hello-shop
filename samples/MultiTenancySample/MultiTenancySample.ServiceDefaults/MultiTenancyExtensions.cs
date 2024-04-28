using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MultiTenancySample.ServiceDefaults
{
    public static class MultiTenancyExtensions
    {
        public static IServiceCollection AddMultiTenancy(this IServiceCollection services)
        {
            services.AddScoped<ICurrentTenant, CurrentTenant>();
            services.AddScoped<ITenantIdProvider, TenantIdProvider>();

            return services.AddScoped<TenantMiddleware>();
        }

        public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app)
        {
            app.UseMiddleware<TenantMiddleware>();

            return app;
        }
    }
}
