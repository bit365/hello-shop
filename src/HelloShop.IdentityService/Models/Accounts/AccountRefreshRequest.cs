// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.IdentityService.Models.Accounts;

public class AccountRefreshRequest
{
    public required string RefreshToken { get; init; }

}