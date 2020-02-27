using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Core.Queries;
using Riva.Identity.Core.Queries;
using Riva.Identity.Core.Queries.Handlers;

namespace Riva.Identity.Web.Api.DependencyInjection
{
    public static class QueryHandlersRegistrar
    {
        public static IServiceCollection RegisterQueryHandlers(this IServiceCollection services)
        {
            return services
                .AddScoped<IQueryHandler<GetAccountInputQuery, GetAccountOutputQuery>, GetAccountQueryHandler>()
                .AddScoped<IQueryHandler<GetAccountsInputQuery, CollectionOutputQuery<GetAccountsOutputQuery>>, GetAccountsQueryHandler>()
                .AddScoped<IQueryHandler<GetRoleInputQuery, RoleOutputQuery>, GetRoleQueryHandler>()
                .AddScoped<IQueryHandler<GetRolesInputQuery, CollectionOutputQuery<RoleOutputQuery>>, GetRolesQueryHandler>();
        }
    }
}