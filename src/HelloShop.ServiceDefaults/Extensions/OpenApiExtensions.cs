// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ServiceDefaults.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace HelloShop.ServiceDefaults.Extensions
{
    public static class OpenApiExtensions
    {
        public static IServiceCollection AddOpenApi(this IServiceCollection services, Action<SwaggerGenOptions>? configureOptions = null)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(configureOptions);

            services.Configure<SwaggerUIOptions>(options =>
            {
                options.DocumentTitle = Assembly.GetEntryAssembly()?.GetName().Name;
                options.InjectStylesheet("/ServiceDefaults/Resources/OpenApi/Custom.css");
            });

            services.Configure<SwaggerGenOptions>(options =>
            {
                options.OperationFilter<AcceptLanguageHeaderOperationFilter>();

                options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }

        public static IApplicationBuilder UseOpenApi(this IApplicationBuilder app, Action<SwaggerOptions>? apiConfigureOptions = null, Action<SwaggerUIOptions>? uiConfigureOptions = null)
        {
            app.UseSwagger(apiConfigureOptions);
            app.UseSwaggerUI(uiConfigureOptions);

            app.Map("/ServiceDefaults", appBuilder => appBuilder.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly())
            }));

            return app;
        }
    }
}
