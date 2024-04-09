﻿using FluentValidation;
using HelloShop.IdentityService.Entities;
using HelloShop.IdentityService.EntityFrameworks;
using HelloShop.IdentityService.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace HelloShop.IdentityService.Validations.Users;

public class UserCreateRequestValidator : AbstractValidator<UserCreateRequest>
{
    public UserCreateRequestValidator(IdentityServiceDbContext dbContext)
    {
        RuleFor(m => m.UserName).NotNull().NotEmpty().Length(8, 16).Matches("^[a-zA-Z]+$");

        RuleFor(m => m.PhoneNumber).NotNull().NotEmpty().Length(11).Matches(@"^1\d{10}$").Must((phoneNumber) =>
        {
            return !dbContext.Set<User>().Any(e => e.PhoneNumber == phoneNumber);
        });

        RuleFor(m => m.Password).NotNull().NotEmpty().Length(8, 16);

        RuleFor(m => m.Email).EmailAddress().Length(8, 32);
    }
}