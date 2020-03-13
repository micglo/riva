using Microsoft.Extensions.DependencyInjection;
using Riva.AdministrativeDivisions.Domain.Cities.Repositories;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Repositories;
using Riva.AdministrativeDivisions.Domain.States.Repositories;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Repositories;

namespace Riva.AdministrativeDivisions.Web.Api.DependencyInjection
{
    public static class RepositoryRegistrar
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped<IStateRepository, StateRepository>()
                .AddScoped<ICityRepository, CityRepository>()
                .AddScoped<ICityDistrictRepository, CityDistrictRepository>();
        }
    }
}