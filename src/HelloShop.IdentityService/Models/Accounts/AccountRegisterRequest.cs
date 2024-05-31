// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

namespace HelloShop.IdentityService;

public class AccountRegisterRequest
{
    public required string UserName { get; init; }

    public required string Password { get; init; }

    public string? Email { get; init; }
}
