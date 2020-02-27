using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Riva.BuildingBlocks.WebApi.Models.Configs;
using Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions.Swagger.OperationFilters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, SwaggerExtensionConfig config)
        {
            return services.AddSwaggerGen(options =>
            {
                options.DescribeAllParametersInCamelCase();
                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out var methodInfo))
                        return false;

                    var versions = methodInfo.DeclaringType?.GetCustomAttributes(true)
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    return versions!.Any(v => $"v{v.MajorVersion}" == docName);
                });
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{config.IdentityUrl}/connect/authorize"),
                            TokenUrl = new Uri($"{config.IdentityUrl}/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                { config.ApiResourceName, "Access to API operations" }
                            }
                        }
                    }
                });
                options.EnableAnnotations();
                options.IgnoreObsoleteActions();
                options.IgnoreObsoleteProperties();
                options.OperationFilter<ApiVersionRequestParamOperationFilter>();
                options.OperationFilter<IfMatchRequestParamOperationFilter>();
                options.OperationFilter<UnauthorizedResponseOperationFilter>(config.ApiResourceName);
                options.OperationFilter<ForbiddenResponseOperationFilter>();
                options.OperationFilter<NotFoundResponseOperationFilter>();
                options.OperationFilter<PreconditionFailedResponseOperationFilter>();
                options.OperationFilter<UnprocessableEntityResponseOperationFilter>();
                options.OperationFilter<PreconditionRequiredResponseOperationFilter>();


                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc($"v{description.ApiVersion.MajorVersion}",
                        new OpenApiInfo
                        {
                            Title = $"{config.ApiResourceName} API V{description.ApiVersion.MajorVersion}",
                            Version = $"v{description.ApiVersion.MajorVersion}"
                        });
                }
            });
        }
    }
}