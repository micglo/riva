using Microsoft.Extensions.DependencyInjection;
using Riva.Users.Domain.Cities.Repositories;
using Riva.Users.Domain.Users.Repositories;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Repositories;

namespace Riva.Users.Web.Api.DependencyInjection
{
    public static class RepositoryRegistrar
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<ICityRepository, CityRepository>();
        }
    }
}