using Microsoft.Extensions.DependencyInjection;
using Riva.Announcements.Core.Services;
using Riva.Announcements.Infrastructure.Services;
using Riva.BuildingBlocks.Core.Services;
using Riva.BuildingBlocks.Infrastructure.Services;

namespace Riva.Announcements.Web.Api.DependencyInjection
{
    public static class ServicesRegistrar
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IHttpClientService, HttpClientService>()
                .AddScoped<IRivaAdministrativeDivisionsApiClientService, RivaAdministrativeDivisionsApiClientService>()
                .AddScoped<ICityVerificationService, CityVerificationService>()
                .AddScoped<ICityDistrictVerificationService, CityDistrictVerificationService>()
                .AddSingleton<IRoomForRentAnnouncementGetterService, RoomForRentAnnouncementGetterService>()
                .AddSingleton<IFlatForRentAnnouncementGetterService, FlatForRentAnnouncementGetterService>();
        }
    }
}