using System.Text.Json.Serialization;

namespace HelloShop.IdentityService;

public class AccountLoginRequest
{
  public required string UserName { get; init; }

  public required string Password { get; init; }

  [JsonIgnore]
  public string? TwoFactorCode { get; init; }

  [JsonIgnore]
  public string? TwoFactorRecoveryCode { get; init; }
}
