using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Core.Services;
using Riva.BuildingBlocks.Infrastructure.Services;
using Riva.BuildingBlocks.WebApi.ConfigurationExtensions;
using Riva.Users.Core.Services;
using Riva.Users.Infrastructure.Services;
using Riva.Users.Web.Api.Models.AppSettings;
using Riva.Users.Web.Api.Models.Constants;

namespace Riva.Users.Web.Api.DependencyInjection
{
    public static class ServicesRegistrar
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration config)
        {
            return services
                .AddScoped<IUserGetterService, UserGetterService>()
                .AddTransient<IHttpClientService, HttpClientService>()
                .AddScoped<IRivaIdentityApiClientService, RivaIdentityApiClientService>()
                .AddScoped<IUserVerificationService, UserVerificationService>()
                .AddScoped<IAccountVerificationService, AccountVerificationService>()
                .AddScoped<ICityVerificationService, CityVerificationService>()
                .AddScoped<IAuthorizationService, AuthorizationService>()
                .AddScoped<ICityGetterService, CityGetterService>()
                .AddSingleton<IFlatForRentAnnouncementPreferenceVerificationService, FlatForRentAnnouncementPreferenceVerificationService>()
                .AddScoped<IUserRevertService, UserRevertService>()
                .AddSingleton<IRoomForRentAnnouncementPreferenceVerificationService, RoomForRentAnnouncementPreferenceVerificationService>()
                .AddSingleton<IFlatForRentAnnouncementPreferenceGetterService, FlatForRentAnnouncementPreferenceGetterService>()
                .AddSingleton<IRoomForRentAnnouncementPreferenceGetterService, RoomForRentAnnouncementPreferenceGetterService>()
                .AddSingleton<IBlobContainerService, BlobContainerService>()
                .AddSingleton(x =>
                {
                    var connectionStringAppSettings =
                        config.GetSectionAppSettings<ConnectionStringsAppSettings>(BuildingBlocks.WebApi.Models
                            .Constants.AppSettingsConstants.ConnectionStrings);
                    var blobContainerAppSettings =
                        config.GetSectionAppSettings<BlobContainerAppSettings>(AppSettingsConstants.BlobContainer);
                    return new BlobContainerClient(connectionStringAppSettings.StorageAccountConnectionString,
                        blobContainerAppSettings.BlobContainerName);
                });
        }
    }
}