using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions.Swagger.OperationFilters
{
    public class NotFoundResponseOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters is null || !operation.Parameters.Any())
                return;

            var idParam = operation.Parameters.FirstOrDefault(x => x.Name.Equals("id"));
            if (idParam is null)
                return;

            const string key = "404";
            if (operation.Responses.ContainsKey(key))
                return;

            var openApiResponse = new OpenApiResponse
            {
                Description = "Resource not found.",
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