using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions;

namespace Riva.BuildingBlocks.WebApi.ApplicationPipelines
{
    public static class HealthChecksPipeline
    {
        private const string HealthCheckPath = "/hc";
        private const string LivenessPath = "/liveness";

        public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder builder)
        {
            builder.UseHealthChecks(HealthCheckPath, new HealthCheckOptions
            {
                Predicate = _ => true,
                ResultStatusCodes = {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                },
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            builder.UseHealthChecks(LivenessPath, new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains(HealthChecksExtension.SelfName)
            });

            return builder;
        }
    }
}