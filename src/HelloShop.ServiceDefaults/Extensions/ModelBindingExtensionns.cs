// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using FluentValidation;
using FluentValidation.AspNetCore;
using HelloShop.ServiceDefaults.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HelloShop.ServiceDefaults.Extensions;

public static class ModelBindingExtensionns
{
    public static IServiceCollection AddModelMapper(this IServiceCollection services, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();

        services.AddAutoMapper(assembly);

        return services;
    }

    public static IServiceCollection AddModelValidator(this IServiceCollection services, Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();

        services.AddValidatorsFromAssembly(assembly).AddFluentValidationAutoValidation();

        ValidatorOptions.Global.LanguageManager = new CustomFluentValidationLanguageManager();

        return services;
    }
}
