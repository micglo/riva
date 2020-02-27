using Microsoft.Extensions.DependencyInjection;
using Riva.Identity.Domain.PersistedGrants.Repositories;
using Riva.Identity.Domain.Roles.Repositories;
using Riva.Identity.Domain.Accounts.Repositories;
using Riva.Identity.Infrastructure.DataAccess.IdentityServerSqlServer.Repositories;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Repositories;

namespace Riva.Identity.Web.Api.DependencyInjection
{
    public static class RepositoryRegistrar
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped<IRoleRepository, RoleRepository>()
                .AddScoped<IAccountRepository, AccountRepository>()
                .AddScoped<IPersistedGrantRepository, PersistedGrantRepository>();
        }
    }
}