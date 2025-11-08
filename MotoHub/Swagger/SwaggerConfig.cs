using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace MotoHub.Swagger
{
    public class SwaggerConfig : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public SwaggerConfig(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var desc in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(desc.GroupName, new OpenApiInfo
                {
                    Title = "MotoHub API",
                    Version = desc.ApiVersion.ToString()
                });
            }

            var apiKeyScheme = new OpenApiSecurityScheme
            {
                Description = "Informe a API Key no header: X-API-KEY",
                Name = "X-API-KEY",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "ApiKeyScheme"
            };

            options.AddSecurityDefinition("ApiKey", apiKeyScheme);

            options.OperationFilter<AuthorizeCheckOperationFilter>();

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { apiKeyScheme, Array.Empty<string>() }
            });
        }
    }
}
