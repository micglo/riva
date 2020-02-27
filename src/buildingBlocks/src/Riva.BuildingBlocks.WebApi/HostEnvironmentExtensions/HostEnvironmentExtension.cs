using Microsoft.Extensions.Hosting;
using Riva.BuildingBlocks.WebApi.Models.Environments;

namespace Riva.BuildingBlocks.WebApi.HostEnvironmentExtensions
{
    public static class HostEnvironmentExtension
    {
        public static bool IsLocal(this IHostEnvironment environment)
        {
            return environment.EnvironmentName.Equals(Environment.LocalEnvironment);
        }

        public static bool IsLocalOrDocker(this IHostEnvironment environment)
        {
            return environment.EnvironmentName.Equals(Environment.LocalEnvironment) ||
                   environment.EnvironmentName.Equals(Environment.DockerEnvironment);
        }

        public static bool IsNotLocalOrDocker(this IHostEnvironment environment)
        {
            return !environment.EnvironmentName.Equals(Environment.LocalEnvironment) && 
                   !environment.EnvironmentName.Equals(Environment.DockerEnvironment);
        }

        public static bool IsLocalOrDockerOrDevelopment(this IHostEnvironment environment)
        {
            return environment.EnvironmentName.Equals(Environment.LocalEnvironment) ||
                   environment.EnvironmentName.Equals(Environment.DockerEnvironment) ||
                   environment.IsDevelopment();
        }
    }
}