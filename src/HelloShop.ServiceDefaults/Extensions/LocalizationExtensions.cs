// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HelloShop.ServiceDefaults.Extensions;

public static class LocalizationExtensions
{
    public static IServiceCollection AddCustomLocalization(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        return services;
    }

    public static IApplicationBuilder UseCustomLocalization(this IApplicationBuilder app)
    {
        var supportedCultures = new[] { "zh-CN", "en-US" };

        var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures.First())
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);

        app.UseRequestLocalization(localizationOptions);

        IStringLocalizerFactory localizerFactory = app.ApplicationServices.GetRequiredService<IStringLocalizerFactory>();

        ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, lambdaExpression) =>
        {
            string displayName = memberInfo.Name;

            DisplayAttribute? displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>(true);

            displayName = displayAttribute?.Name ?? displayName;

            DisplayNameAttribute? displayNameAttribute = memberInfo.GetCustomAttribute<DisplayNameAttribute>(true);

            displayName = displayNameAttribute?.DisplayName ?? displayName;

            var localizer = localizerFactory.Create(type);

            return localizer[displayName];
        };


        return app;
    }

}
