// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ServiceDefaults.Constants;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HelloShop.ProductService.FunctionalTests.Utilities
{
    public class FakeAccessTokenCreator
    {
        public static string Create()
        {
            DateTimeOffset utcNow = TimeProvider.System.GetUtcNow();

            SymmetricSecurityKey signingKey = new(Encoding.Default.GetBytes(IdentityConstants.IssuerSigningKey));

            var claimsIdentity = new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "1"), new Claim(ClaimTypes.Name, "admin"), new Claim(CustomClaimTypes.RoleIdentifier, "1")]);

            SecurityTokenDescriptor accessTokenDescriptor = new()
            {
                Subject = claimsIdentity,
                SigningCredentials = new(signingKey, SecurityAlgorithms.HmacSha256),
                Expires = utcNow.Add(TimeSpan.FromDays(byte.MaxValue)).LocalDateTime,
            };

            JwtSecurityTokenHandler tokenHandler = new();

            JwtSecurityToken jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(accessTokenDescriptor);

            return tokenHandler.WriteToken(jwtSecurityToken);
        }
    }
}