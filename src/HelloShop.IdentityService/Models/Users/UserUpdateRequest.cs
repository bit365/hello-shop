namespace HelloShop.IdentityService.Models.Users;

public class UserUpdateRequest
{
    public int Id { get; init; }

    public required string UserName { get; init; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }
}
