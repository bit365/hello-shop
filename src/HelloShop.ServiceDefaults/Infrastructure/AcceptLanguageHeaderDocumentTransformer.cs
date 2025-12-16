// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.AspNetCore.OpenApi;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi;

namespace HelloShop.ServiceDefaults.Infrastructure
{
    public sealed class AcceptLanguageHeaderDocumentTransformer : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            var parameter = new OpenApiParameter
            {
                Name = HeaderNames.AcceptLanguage,
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = JsonSchemaType.String,
                    Default = "zh-CN",
                    Enum = ["zh-CN", "en-US"]
                }
            };

            foreach (var path in document.Paths.Values)
            {
                if (path.Operations != null)
                {
                    foreach (var operation in path.Operations.Values)
                    {
                        operation.Parameters ??= [];
                        operation.Parameters.Add(parameter);
                    }
                    continue;
                }
            }

            return Task.CompletedTask;
        }
    }
}
