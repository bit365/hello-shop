namespace MultiTenancySample.ServiceDefaults
{
    public interface ITenantIdProvider
    {
        Task<string?> GetTenantIdAsync();
    }
}
