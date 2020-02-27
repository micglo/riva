using System.Collections.ObjectModel;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.WebApi.Enumerations.ErrorCodes;
using Riva.BuildingBlocks.WebApi.Models.Errors;

namespace Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions
{
    public static class ApiVersioningExtension
    {
        public const string ApiVersionHeaderName = "api-version";

        public static IServiceCollection AddApiVersioning(this IServiceCollection services)
        {
            return services.AddApiVersioning(ConfigureApiVersioningOptions);
        }

        private static void ConfigureApiVersioningOptions(ApiVersioningOptions options)
        {
            options.AssumeDefaultVersionWhenUnspecified = false;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new HeaderApiVersionReader(ApiVersionHeaderName);
            options.ErrorResponses = new ApiVersioningErrorResponseProvider();
        }

        private class ApiVersioningErrorResponseProvider : IErrorResponseProvider
        {
            public IActionResult CreateResponse(ErrorResponseContext context)
            {
                const string type = "https://tools.ietf.org/html/rfc4918#section-11.2";
                var instance = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}{context.Request.Path}";
                var details = new Collection<ErrorDetailsResponse>
                {
                    new ErrorDetailsResponse(context.ErrorCode, context.Message)
                };
                var errorResponse = new ErrorResponse(type, instance, HttpStatusCode.UnprocessableEntity,
                    RequestErrorCodeEnumeration.ValidationFailed.DisplayName, "Validation failed.", details);
                return new UnprocessableEntityObjectResult(errorResponse);
            }
        }
    }
}