using HelloShop.ServiceDefaults.Models.Paging;

namespace HelloShop.IdentityService.Models.Users;

public class UserListRequest : KeywordSearchRequest
{
    public string? PhoneNumber { get; init; }
}
