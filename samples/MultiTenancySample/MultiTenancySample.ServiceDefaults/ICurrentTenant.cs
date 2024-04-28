namespace MultiTenancySample.ServiceDefaults
{
    public interface ICurrentTenant
    {
        string? TenantId { get; }

        IDisposable SetTenant(string? tenantId);
    }
}
