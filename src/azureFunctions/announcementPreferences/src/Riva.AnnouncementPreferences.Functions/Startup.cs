using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riva.AnnouncementPreferences.Core.Models;
using Riva.AnnouncementPreferences.Functions;
using Riva.AnnouncementPreferences.Core.ServiceCollectionExtensions;
using Riva.AnnouncementPreferences.Core.Services;
using Riva.BuildingBlocks.Core.Logger;
using Riva.BuildingBlocks.Infrastructure.Logger;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Riva.AnnouncementPreferences.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services
                .Configure<AppSettings>(config.GetSection("Values"))
                .AddCosmonaut(config)
                .AddLogging()
                .AddSingleton<ILogger, Logger>()
                .AddSingleton<IFlatForRentAnnouncementPreferenceService, FlatForRentAnnouncementPreferenceService>()
                .AddSingleton<IRoomForRentAnnouncementPreferenceService, RoomForRentAnnouncementPreferenceService>();
        }
    }
}