using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions.Swagger.OperationFilters
{
    public class UnauthorizedResponseOperationFilter : IOperationFilter
    {
        private readonly string _apiResourceName;

        public UnauthorizedResponseOperationFilter(string apiResourceName)
        {
            _apiResourceName = apiResourceName;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authorizeAttribute =
                context.MethodInfo.DeclaringType?.CustomAttributes.SingleOrDefault(x =>
                    x.AttributeType == typeof(AuthorizeAttribute));
            if (authorizeAttribute is null)
            {
                authorizeAttribute =
                    context.MethodInfo.CustomAttributes.SingleOrDefault(x =>
                        x.AttributeType == typeof(AuthorizeAttribute));
                if (authorizeAttribute is null)
                    return;
            }

            var allowAnonymousAttribute =
                context.MethodInfo.CustomAttributes.SingleOrDefault(x =>
                    x.AttributeType == typeof(AllowAnonymousAttribute));
            if(allowAnonymousAttribute != null)
                return;

            const string key = "401";
            if (operation.Responses.ContainsKey(key))
                return;

            var openApiResponse = new OpenApiResponse
            {
                Description = "Unauthorized.",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    {
                        "application/json", new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.Schema,
                                    Id = "ErrorResponse"
                                }
                            }
                        }
                    },
                    {
                        "text/json", new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.Schema,
                                    Id = "ErrorResponse"
                                }
                            }
                        }
                    }
                }
            };
            operation.Responses.Add(key, openApiResponse);

            var oAuthScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            };
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    [ oAuthScheme ] = new [] { _apiResourceName }
                }
            };
        }
    }
}