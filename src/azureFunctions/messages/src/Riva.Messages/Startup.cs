using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Core.Logger;
using Riva.BuildingBlocks.Infrastructure.Logger;
using Riva.Messages;
using SendGrid;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Riva.Messages
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
                .AddLogging()
                .AddSingleton<ILogger, Logger>()
                .AddSingleton<ISendGridClient, SendGridClient>(x => new SendGridClient(config.GetValue<string>(ConstantVariables.SendGridApiKeyVariableName)));
        }
    }
}