namespace MultiTenancySample.ServiceDefaults
{
    public class CurrentTenant : ICurrentTenant
    {
        public string? TenantId { get; private set; }

        public IDisposable SetTenant(string? tenantId)
        {
            string? parentTenantId = TenantId;

            TenantId = tenantId;

            return new DisposeAction(() =>
            {
                TenantId = parentTenantId;
            });
        }

        public class DisposeAction(Action action) : IDisposable
        {
            void IDisposable.Dispose()
            {
                action();
                GC.SuppressFinalize(this);
            }
        }
    }
}
