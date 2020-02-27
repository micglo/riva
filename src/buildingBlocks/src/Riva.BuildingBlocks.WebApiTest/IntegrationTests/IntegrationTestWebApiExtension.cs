using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Core.Logger;
using Riva.BuildingBlocks.Core.Services;
using Riva.BuildingBlocks.Infrastructure.Logger;
using Riva.BuildingBlocks.WebApi.Models.Configs;
using Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions;

namespace Riva.BuildingBlocks.WebApiTest.IntegrationTests
{
    public static class IntegrationTestWebApiExtension
    {
        public static IServiceCollection AddWebApiForIntegrationTest(this IServiceCollection services, WebApiExtensionConfig config)
        {
            ApiVersioningExtension.AddApiVersioning(services);
            VersionedApiExplorerExtension.AddVersionedApiExplorer(services);
            CorsExtension.AddCors(services);
            services
                .AddJwtAuthentication(config.AuthenticationExtensionConfig)
                .AddAuthorizationForIntegrationTest(config.AuthorizationExtensionConfig)
                .AddAutoMapper(config.AutoMapperAssemblies)
                .AddCommunicationBus(config.StartupAssembly)
                .AddMvcCore(config.Environment, config.StartupAssembly)
                .AddSwagger(config.SwaggerExtensionConfig);

            services.AddSingleton<Core.Mapper.IMapper, Infrastructure.Mapper.Mapper>();
            services.AddSingleton<ILogger, Logger>();
            services.AddSingleton(typeof(IOrderByExpressionCreator<>), typeof(OrderByExpressionCreator<>));

            return services;
        }
    }
}