using Microsoft.Extensions.DependencyInjection;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Infrastructure.Services;

namespace Riva.AdministrativeDivisions.Web.Api.DependencyInjection
{
    public static class ServicesRegistrar
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IStateGetterService, StateGetterService>()
                .AddScoped<IStateVerificationService, StateVerificationService>()
                .AddScoped<ICityGetterService, CityGetterService>()
                .AddScoped<ICityVerificationService, CityVerificationService>()
                .AddScoped<ICityDistrictGetterService, CityDistrictGetterService>()
                .AddScoped<ICityDistrictVerificationService, CityDistrictVerificationService>();
        }
    }
}