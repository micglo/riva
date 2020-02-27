using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Core.Logger;
using Riva.BuildingBlocks.Core.Services;
using Riva.BuildingBlocks.Infrastructure.Logger;
using Riva.BuildingBlocks.WebApi.HostEnvironmentExtensions;
using Riva.BuildingBlocks.WebApi.Models.Configs;

namespace Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions
{
    public static class WebApiExtension
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services, WebApiExtensionConfig config)
        {
            services
                .AddApiVersioning()
                .AddVersionedApiExplorer()
                .AddJwtAuthentication(config.AuthenticationExtensionConfig)
                .AddAuthorization(config.AuthorizationExtensionConfig)
                .AddAutoMapper(config.AutoMapperAssemblies)
                .AddCommunicationBus(config.StartupAssembly)
                .AddCors()
                .AddMvcCore(config.Environment, config.StartupAssembly)
                .AddSwagger(config.SwaggerExtensionConfig)
                .AddApplicationInsightsTelemetry();

            if (config.Environment.IsNotLocalOrDocker())
                services.AddHsts();

            services.AddSingleton<Core.Mapper.IMapper, Infrastructure.Mapper.Mapper>();
            services.AddSingleton<ILogger, Logger>();
            services.AddSingleton(typeof(IOrderByExpressionCreator<>), typeof(OrderByExpressionCreator<>));

            return services;
        }
    }
}