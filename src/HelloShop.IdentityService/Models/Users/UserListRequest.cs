// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ServiceDefaults.Models.Paging;

namespace HelloShop.IdentityService.Models.Users;

public class UserListRequest : KeywordSearchRequest
{
    public string? PhoneNumber { get; init; }
}
