using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace Riva.BuildingBlocks.WebApi.ApplicationPipelines
{
    public static class SwaggerPipelines
    {
        private const string RouteTemplate = "swagger/{documentName}/swagger.json";
        private const string RoutePrefix = "swagger";
        private const string RivaIdentityApplicationName = "Riva Identity";

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder builder, string apiResourceName, Guid clientId)
        {
            builder
                .UseSwagger(options => options.RouteTemplate = RouteTemplate)
                .UseSwaggerUI(options =>
                {
                    options.RoutePrefix = RoutePrefix;
                    options.DocumentTitle = $"{apiResourceName} API docs";
                    options.OAuthClientId(clientId.ToString());
                    options.OAuthAppName(RivaIdentityApplicationName);
                    options.OAuthScopeSeparator(" ");
                    var provider = builder.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
                    var descriptions = provider.ApiVersionDescriptions.OrderByDescending(x => x.ApiVersion);
                    foreach (var description in descriptions)
                    {
                        var versionNumber = description.ApiVersion.MajorVersion;
                        options.SwaggerEndpoint(
                            $"/swagger/v{versionNumber}/swagger.json",
                            $"{apiResourceName} API V{versionNumber}");
                    }
                });

            return builder;
        }
    }
}