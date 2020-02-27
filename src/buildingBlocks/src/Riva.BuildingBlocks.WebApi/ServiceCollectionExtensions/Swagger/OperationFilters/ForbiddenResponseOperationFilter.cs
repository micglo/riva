using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions.Swagger.OperationFilters
{
    public class ForbiddenResponseOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!HasAdministratorPolicy(context))
                return;

            const string key = "403";
            if (operation.Responses.ContainsKey(key))
                return;

            var openApiResponse = new OpenApiResponse
            {
                Description = "Forbidden.",
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
        }

        private static bool HasAdministratorPolicy(OperationFilterContext context)
        {
            var customAuthorizeAttributes = GetCustomAuthorizeAttributes(context);
            if (customAuthorizeAttributes is null || !customAuthorizeAttributes.Any())
                return false;

            return customAuthorizeAttributes.SelectMany(attribute => attribute.NamedArguments).Any(namedArgument =>
                namedArgument.MemberName.Equals($"{nameof(AuthorizeAttribute.Policy)}") &&
                ((string) namedArgument.TypedValue.Value == AuthorizationExtension.JwtBearerAdministratorPolicyName ||
                 (string) namedArgument.TypedValue.Value == AuthorizationExtension.JwtBearerAdministratorSystemPolicyName ||
                 (string) namedArgument.TypedValue.Value == AuthorizationExtension.JwtBearerSystemPolicyName));
        }

        private static ICollection<CustomAttributeData> GetCustomAuthorizeAttributes(OperationFilterContext context)
        {
            var customAuthorizeAttributes =
                context.MethodInfo.DeclaringType?.CustomAttributes.Where(x =>
                    x.AttributeType == typeof(AuthorizeAttribute)).ToList() ?? new List<CustomAttributeData>();

            customAuthorizeAttributes.AddRange(
                context.MethodInfo.CustomAttributes.Where(x => x.AttributeType == typeof(AuthorizeAttribute)));

            return customAuthorizeAttributes;
        }
    }
}