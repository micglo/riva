using System.Linq;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Riva.BuildingBlocks.WebApi.Attributes.ActionFilters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions.Swagger.OperationFilters
{
    public class IfMatchRequestParamOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var requireIfMatchAttribute =
                context.MethodInfo.CustomAttributes.SingleOrDefault(x =>
                    x.AttributeType == typeof(RequireIfMatchAttribute));
            if (requireIfMatchAttribute is null)
                return;

            var ifMatchParam =
                operation.Parameters.FirstOrDefault(x => x.Name.ToLower().Equals(HeaderNames.IfMatch.ToLower()));
            if (ifMatchParam is null)
                return;

            ifMatchParam.AllowEmptyValue = false;
            ifMatchParam.Description = "Row version in base64 string.";
            ifMatchParam.Example = new OpenApiString("\"AAAAAAAAZZI=\"");
            ifMatchParam.In = ParameterLocation.Header;
            ifMatchParam.Required = true;
        }
    }
}