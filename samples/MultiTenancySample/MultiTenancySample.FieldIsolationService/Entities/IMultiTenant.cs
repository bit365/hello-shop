namespace MultiTenancySample.FieldIsolationService.Entities
{
    public interface IMultiTenant
    {
        string? TenantId { get; set; }
    }
}
