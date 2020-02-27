using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace Riva.BuildingBlocks.WebApiTest.IntegrationTests
{
    public static class IntegrationTestWebHostBuilderExtension
    {
        public static IWebHostBuilder ConfigureWebHostBuilderForIntegrationTest(this IWebHostBuilder builder)
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.json");
            builder.UseSolutionRelativeContentRoot(projectDir);
            builder.ConfigureAppConfiguration((context, conf) => { conf.AddJsonFile(configPath); });
            return builder;
        }
    }
}