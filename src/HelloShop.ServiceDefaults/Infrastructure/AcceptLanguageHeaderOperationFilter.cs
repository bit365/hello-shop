using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HelloShop.ServiceDefaults.Infrastructure;

public class AcceptLanguageHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var parameter = new OpenApiParameter
        {
            Name = HeaderNames.AcceptLanguage,
            In = ParameterLocation.Header,
            Required = false,
            Schema = new OpenApiSchema
            {
                Default = new OpenApiString("zh-CN"),
                Type = "string",
                Enum = [new OpenApiString("zh-CN"), new OpenApiString("en-US")]
            }
        };

        operation.Parameters.Add(parameter);
    }
}
