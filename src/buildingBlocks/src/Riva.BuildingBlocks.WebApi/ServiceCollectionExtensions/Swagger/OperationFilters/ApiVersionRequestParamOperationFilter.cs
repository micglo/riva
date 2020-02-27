using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions.Swagger.OperationFilters
{
    public class ApiVersionRequestParamOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters is null || !operation.Parameters.Any())
                return;

            var apiVersionParam = operation.Parameters.FirstOrDefault(x => x.Name.Equals(ApiVersioningExtension.ApiVersionHeaderName));
            if (apiVersionParam is null) 
                return;

            apiVersionParam.AllowEmptyValue = false;
            apiVersionParam.Description = "Numeric string of API version to be used.";
            apiVersionParam.Example = new OpenApiString(context.ApiDescription.GetApiVersion().MajorVersion.ToString());
            apiVersionParam.In = ParameterLocation.Header;
            apiVersionParam.Required = true;
        }
    }
}