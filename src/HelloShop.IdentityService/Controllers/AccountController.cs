// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.IdentityService.Entities;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HelloShop.IdentityService;

[Route("api/[controller]")]
[ApiController]
public class AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IOptionsMonitor<JwtBearerOptions> jwtBearerOptions) : ControllerBase
{
    [HttpPost(nameof(Login))]
    public async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>> Login([FromBody] AccountLoginRequest request)
    {
        signInManager.AuthenticationScheme = CustomJwtBearerDefaults.AuthenticationScheme;

        var result = await signInManager.PasswordSignInAsync(request.UserName, request.Password, false, lockoutOnFailure: true);

        if (result.RequiresTwoFactor)
        {
            if (!string.IsNullOrEmpty(request.TwoFactorCode))
            {
                result = await signInManager.TwoFactorAuthenticatorSignInAsync(request.TwoFactorCode, false, rememberClient: false);
            }
            else if (!string.IsNullOrEmpty(request.TwoFactorRecoveryCode))
            {
                result = await signInManager.TwoFactorRecoveryCodeSignInAsync(request.TwoFactorRecoveryCode);
            }
        }

        if (!result.Succeeded)
        {
            return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
        }

        // The signInManager already produced the needed response in the form of a cookie or bearer token.
        return TypedResults.Empty;
    }

    [HttpPost(nameof(Refresh))]
    public async Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult, SignInHttpResult, ChallengeHttpResult>> Refresh([FromBody] AccountRefreshRequest request)
    {
        var options = jwtBearerOptions.Get(JwtBearerDefaults.AuthenticationScheme);

        var handler = new JwtSecurityTokenHandler();

        var validationResult = await handler.ValidateTokenAsync(request.RefreshToken, options.TokenValidationParameters);

        // Reject the /refresh attempt with a 401 if the token expired or the security stamp validation fails
        if (!validationResult.IsValid || await signInManager.ValidateSecurityStampAsync(new ClaimsPrincipal(validationResult.ClaimsIdentity)) is not User user)
        {
            return TypedResults.Challenge();
        }

        var newPrincipal = await signInManager.CreateUserPrincipalAsync(user);

        return TypedResults.SignIn(newPrincipal, authenticationScheme: CustomJwtBearerDefaults.AuthenticationScheme);
    }

    [HttpPost(nameof(Register))]
    public async Task<Results<Ok, ValidationProblem>> Register([FromBody] AccountRegisterRequest request)
    {
        var user = new User { UserName = request.UserName, Email = request.Email };

        if (string.IsNullOrEmpty(request.Email) || !new EmailAddressAttribute().IsValid(request.Email))
        {
            return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(request.Email)));
        }

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return CreateValidationProblem(result);
        }

        return TypedResults.Ok();
    }

    private static ValidationProblem CreateValidationProblem(IdentityResult result)
    {
        // We expect a single error code and description in the normal case.
        // This could be golfed with GroupBy and ToDictionary, but perf! :P
        Debug.Assert(!result.Succeeded);

        var errorDictionary = new Dictionary<string, string[]>(1);

        foreach (var error in result.Errors)
        {
            string[] newDescriptions;

            if (errorDictionary.TryGetValue(error.Code, out var descriptions))
            {
                newDescriptions = new string[descriptions.Length + 1];
                Array.Copy(descriptions, newDescriptions, descriptions.Length);
                newDescriptions[descriptions.Length] = error.Description;
            }
            else
            {
                newDescriptions = [error.Description];
            }

            errorDictionary[error.Code] = newDescriptions;
        }

        return TypedResults.ValidationProblem(errorDictionary);
    }
}
