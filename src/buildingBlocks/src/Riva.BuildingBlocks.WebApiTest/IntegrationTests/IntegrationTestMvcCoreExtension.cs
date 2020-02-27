using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.WebApi.Attributes.ActionFilters;
using Riva.BuildingBlocks.WebApi.Conventions;
using Riva.BuildingBlocks.WebApi.HostEnvironmentExtensions;
using Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions.MvcCoreBuilderExtensions;

namespace Riva.BuildingBlocks.WebApiTest.IntegrationTests
{
    public static class IntegrationTestMvcCoreExtension
    {
        public static IServiceCollection AddMvcCore(this IServiceCollection services, IWebHostEnvironment environment, Assembly assembly)
        {
            return NewtonsoftJsonExtension.AddNewtonsoftJson(services
                    .AddMvcCore(options =>
                    {
                        options.ReturnHttpNotAcceptable = true;
                        options.RequireHttpsPermanent = environment.IsNotLocalOrDocker();
                        options.RespectBrowserAcceptHeader = true;
                        options.OutputFormatters.RemoveType<StringOutputFormatter>();
                        options.ConfigureFilters();
                        options.Conventions.Add(new ApiExplorerGroupPerRouteConvention());
                    })
                    .AddApiExplorer()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                    .ConfigureApiBehaviorOptions(options =>
                    {
                        options.SuppressInferBindingSourcesForParameters = true;
                        options.SuppressModelStateInvalidFilter = true;
                    })
                    .AddDataAnnotations()
                    .AddJsonOptions())
                .AddApplicationPart(assembly)
                .AddApplicationPart(typeof(WebApi.Controllers.ErrorController).Assembly)
                .Services;
        }

        public static void ConfigureFilters(this MvcOptions options)
        {
            options.Filters.Add(new ApiModelStateValidatorFilter());
        }
    }
}