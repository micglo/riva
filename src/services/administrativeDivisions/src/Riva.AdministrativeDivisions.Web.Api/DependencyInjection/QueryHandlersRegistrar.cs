using Microsoft.Extensions.DependencyInjection;
using Riva.AdministrativeDivisions.Core.Queries;
using Riva.AdministrativeDivisions.Core.Queries.Handlers;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.AdministrativeDivisions.Web.Api.DependencyInjection
{
    public static class QueryHandlersRegistrar
    {
        public static IServiceCollection RegisterQueryHandlers(this IServiceCollection services)
        {
            return services
                .AddScoped<IQueryHandler<GetStatesInputQuery, CollectionOutputQuery<StateOutputQuery>>, GetStatesQueryHandler>()
                .AddScoped<IQueryHandler<GetStateInputQuery, StateOutputQuery>, GetStateQueryHandler>()
                .AddScoped<IQueryHandler<GetCitiesInputQuery, CollectionOutputQuery<CityOutputQuery>>, GetCitiesQueryHandler>()
                .AddScoped<IQueryHandler<GetCityInputQuery, CityOutputQuery>, GetCityQueryHandler>()
                .AddScoped<IQueryHandler<GetCityDistrictsInputQuery, CollectionOutputQuery<CityDistrictOutputQuery>>, GetCityDistrictsQueryHandler>()
                .AddScoped<IQueryHandler<GetCityDistrictInputQuery, CityDistrictOutputQuery>, GetCityDistrictQueryHandler>();
        }
    }
}