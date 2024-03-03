// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HelloShop.ServiceDefaults.Extensions
{
    public static class OpenApiExtensions
    {
        public static IServiceCollection AddOpenApi(this IServiceCollection services, Action<SwaggerGenOptions>? configureOptions = null)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(configureOptions);

            return services;
        }

        public static IApplicationBuilder UseOpenApi(this IApplicationBuilder app, Action<SwaggerOptions>? apiConfigureOptions = null, Action<SwaggerUIOptions>? uiConfigureOptions = null)
        {
            app.UseSwagger(apiConfigureOptions);
            app.UseSwaggerUI(uiConfigureOptions);

            return app;
        }
    }
}
