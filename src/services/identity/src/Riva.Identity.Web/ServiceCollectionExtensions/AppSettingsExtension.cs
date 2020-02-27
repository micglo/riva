using System.Collections.Generic;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Riva.Identity.Web.Models.AppSettings;
using Riva.Identity.Web.Models.Constants;

namespace Riva.Identity.Web.ServiceCollectionExtensions
{
    public static class AppSettingsExtension
    {
        public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.Configure<ApplicationUrlsAppSettings>(
                configuration.GetSection(AppSettingsConstants.ApplicationUrls));

            if (!env.IsProduction())
                services.Configure<List<Client>>(configuration.GetSection(AppSettingsConstants.Clients));

            return services;
        }
    }
}