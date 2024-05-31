// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace HelloShop.IdentityService;

public class CustomJwtBearerHandler(IOptionsMonitor<CustomJwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder) : SignInAuthenticationHandler<CustomJwtBearerOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync() => throw new NotImplementedException();

    protected override async Task HandleSignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
    {
        var utcNow = TimeProvider.GetUtcNow();

        JwtSecurityTokenHandler tokenHandler = new();

        var signingKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(Options.IssuerSigningKey));

        var accessTokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = Options.Issuer,
            Audience = Options.Audience,
            Subject = user.Identity as ClaimsIdentity,
            SigningCredentials = new(signingKey, Options.SecurityAlgorithm),
            Expires = utcNow.Add(Options.AccessTokenExpiration).LocalDateTime,
        };

        var accessToken = tokenHandler.CreateJwtSecurityToken(accessTokenDescriptor);

        var refreshTokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = Options.Issuer,
            Audience = Options.Audience,
            Subject = user.Identity as ClaimsIdentity,
            SigningCredentials = new(signingKey, Options.SecurityAlgorithm),
            Expires = utcNow.Add(Options.RefreshTokenExpiration).LocalDateTime,
        };

        var refreshToken = tokenHandler.CreateJwtSecurityToken(refreshTokenDescriptor);

        AccessTokenResponse response = new()
        {
            AccessToken = tokenHandler.WriteToken(accessToken),
            ExpiresIn = (long)Options.AccessTokenExpiration.TotalSeconds,
            RefreshToken = tokenHandler.WriteToken(refreshToken)
        };

        await Context.Response.WriteAsJsonAsync(response);
    }

    protected override Task HandleSignOutAsync(AuthenticationProperties? properties) => throw new NotImplementedException();
}
