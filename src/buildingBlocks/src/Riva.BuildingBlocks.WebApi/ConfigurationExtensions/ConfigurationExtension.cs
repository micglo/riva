using Microsoft.Extensions.Configuration;

namespace Riva.BuildingBlocks.WebApi.ConfigurationExtensions
{
    public static class ConfigurationExtension
    {
        public static TAppSettings GetSectionAppSettings<TAppSettings>(this IConfiguration configuration, string section) where TAppSettings : new()
        {
            var model = new TAppSettings();
            configuration.GetSection(section).Bind(model);

            return model;
        }
    }
}