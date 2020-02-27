using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Riva.BuildingBlocks.WebApi.HostEnvironmentExtensions;
using Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions;

namespace Riva.BuildingBlocks.WebApi.ApplicationPipelines
{
    public static class WebApiPipeline
    {
        public const string ErrorHandlingPath = "/api/error";
        public const string StatusCodeErrorHandlingPath = "/api/error/{0}";

        public static IApplicationBuilder UseWebApi(this IApplicationBuilder builder, IWebHostEnvironment env, string apiResourceName, Guid swaggerClientId)
        {
            if (env.IsLocalOrDocker())
                builder.UseDeveloperExceptionPage();

            else
                builder
                    .UseExceptionHandler(ErrorHandlingPath)
                    .UseStatusCodePagesWithReExecute(StatusCodeErrorHandlingPath)
                    .UseHsts()
                    .UseHttpsRedirection();

            builder
                .UseRouting()
                .UseCors(CorsExtension.AllowAnyCorsPolicyName)
                .UseApiVersioning()
                .UseHealthChecks()
                .UseSwagger(apiResourceName, swaggerClientId)
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints => endpoints.MapControllers());

            return builder;
        }
    }
}