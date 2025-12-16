// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ServiceDefaults.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace HelloShop.ServiceDefaults.Extensions
{
    public static class OpenApiExtensions
    {
        public static IServiceCollection AddOpenApiServices(this IServiceCollection services, Action<OpenApiOptions>? configureOptions = null)
        {
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                options.AddDocumentTransformer<AcceptLanguageHeaderDocumentTransformer>();
                configureOptions?.Invoke(options);
            });

            return services;
        }

        internal sealed class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
        {
            public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
            {
                var securityScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                };

                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
                {
                    ["bearerAuth"] = securityScheme
                };

                var securitySchemeReference = new OpenApiSecuritySchemeReference("bearerAuth", document);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    [securitySchemeReference] = []
                };

                foreach (var path in document.Paths.Values)
                {
                    if (path.Operations != null)
                    {
                        foreach (var operation in path.Operations.Values)
                        {
                            operation.Security ??= [];
                            operation.Security.Add(securityRequirement);
                        }
                    }
                }

                return Task.CompletedTask;
            }
        }

        public static WebApplication UseOpenApiWithUI(this WebApplication app, Action<SwaggerUIOptions>? uiConfigureOptions = null)
        {
            app.MapOpenApi();

            app.Map("/ServiceDefaults", appBuilder => appBuilder.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly())
            }));

            app.UseSwaggerUI(options => {
                options.DocumentTitle = Assembly.GetEntryAssembly()?.GetName().Name;
                options.InjectStylesheet("/ServiceDefaults/Resources/OpenApi/Custom.css");
                options.SwaggerEndpoint("/openapi/v1.json", "v1");
                uiConfigureOptions?.Invoke(options);
            });

            return app;
        }
    }
}
