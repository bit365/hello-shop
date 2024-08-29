// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using System.Text.Json.Serialization;

namespace HelloShop.IdentityService.Models.Accounts;

public class AccountLoginRequest
{
    public required string UserName { get; init; }

    public required string Password { get; init; }

    [JsonIgnore]
    public string? TwoFactorCode { get; init; }

    [JsonIgnore]
    public string? TwoFactorRecoveryCode { get; init; }
}
