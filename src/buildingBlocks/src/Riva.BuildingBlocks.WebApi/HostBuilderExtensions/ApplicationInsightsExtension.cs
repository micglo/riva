using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Riva.BuildingBlocks.WebApi.ConfigurationExtensions;
using Riva.BuildingBlocks.WebApi.HostEnvironmentExtensions;
using Riva.BuildingBlocks.WebApi.Models.AppSettings;
using Riva.BuildingBlocks.WebApi.Models.Constants;

namespace Riva.BuildingBlocks.WebApi.HostBuilderExtensions
{
    public static class ApplicationInsightsExtension
    {
        public static IHostBuilder AddLogging(this IHostBuilder builder, Type programClassType, Type startupClassType)
        {
            return builder.ConfigureLogging((hostingContext, logging) =>
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                    .AddEnvironmentVariables()
                    .Build();

                if(hostingContext.HostingEnvironment.IsLocalOrDockerOrDevelopment())
                    logging.AddConsole();

                logging.AddApplicationInsights(config.GetSectionAppSettings<ApplicationInsightsAppSettings>(AppSettingsConstants.ApplicationInsights).InstrumentationKey);
                logging.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Warning);
                logging.AddFilter<ApplicationInsightsLoggerProvider>(programClassType.FullName, LogLevel.Warning);
                logging.AddFilter<ApplicationInsightsLoggerProvider>(startupClassType.FullName, LogLevel.Warning);
            });
        }
    }
}