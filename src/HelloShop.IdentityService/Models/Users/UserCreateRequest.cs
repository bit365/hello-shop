namespace HelloShop.IdentityService.Models.Users;

public class UserCreateRequest
{
    public required string UserName { get; init; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public required string Password { get; init; }
}
