using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Riva.BuildingBlocks.WebApi.Models.Constants;

namespace Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions
{
    public static class CorsExtension
    {
        public const string AllowAnyCorsPolicyName = "AllowAnyCorsPolicy";

        public static IServiceCollection AddCors(this IServiceCollection services)
        {
            return services.AddCors(options =>
                {
                    options.AddPolicy(AllowAnyCorsPolicyName, config =>
                        {
                            config
                                .SetIsOriginAllowed(host => true)
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .WithExposedHeaders(HeaderNameConstants.XCorrelationId, HeaderNameConstants.ApiSupportedVersions, HeaderNames.Location)
                                .AllowCredentials();
                        }
                    );
                    options.DefaultPolicyName = AllowAnyCorsPolicyName;
                }
            );
        }
    }
}