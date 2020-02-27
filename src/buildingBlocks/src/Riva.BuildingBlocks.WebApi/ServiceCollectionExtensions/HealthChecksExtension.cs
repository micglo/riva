using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions
{
    public static class HealthChecksExtension
    {
        public const string SelfName = "self";

        public static IHealthChecksBuilder AddHealthChecks(this IServiceCollection services)
        {
            return HealthCheckServiceCollectionExtensions.AddHealthChecks(services).AddCheck(SelfName, () => HealthCheckResult.Healthy());
        }
    }
}