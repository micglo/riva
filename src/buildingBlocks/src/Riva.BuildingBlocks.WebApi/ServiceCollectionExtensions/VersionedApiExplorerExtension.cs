using Microsoft.Extensions.DependencyInjection;

namespace Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions
{
    public static class VersionedApiExplorerExtension
    {
        public static IServiceCollection AddVersionedApiExplorer(this IServiceCollection services)
        {
            return services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.AssumeDefaultVersionWhenUnspecified = false;
                options.DefaultApiVersionParameterDescription = ApiVersioningExtension.ApiVersionHeaderName;
            });
        }
    }
}