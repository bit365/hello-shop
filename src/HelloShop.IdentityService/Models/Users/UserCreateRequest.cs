using System.ComponentModel.DataAnnotations;

namespace HelloShop.IdentityService.Models.Users;

public class UserCreateRequest
{
    public required string UserName { get; init; }

    [Length(8, 16)]
    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public required string Password { get; init; }
}
