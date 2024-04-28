namespace MultiTenancySample.DatabaseIsolationService.Entities
{
    public class Product
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }
    }
}