using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Riva.BuildingBlocks.WebApi.Attributes.ActionFilters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions.Swagger.OperationFilters
{
    public class PreconditionRequiredResponseOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var requireIfMatchAttribute = context.MethodInfo.CustomAttributes.SingleOrDefault(x => x.AttributeType == typeof(RequireIfMatchAttribute));
            if (requireIfMatchAttribute is null)
                return;

            const string key = "428";
            if (operation.Responses.ContainsKey(key))
                return;

            var openApiResponse = new OpenApiResponse
            {
                Description = "Precondition is required.",
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
    }
}