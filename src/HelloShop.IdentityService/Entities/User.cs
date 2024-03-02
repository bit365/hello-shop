namespace HelloShop.IdentityService.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; } = default!;

        public string PasswordHash { get; set; } = default!;

        public DateTimeOffset CreationTime { get; set; } = DateTimeOffset.UtcNow;
    }
}
