using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Core.Queries;
using Riva.Users.Core.Queries;
using Riva.Users.Core.Queries.Handlers;

namespace Riva.Users.Web.Api.DependencyInjection
{
    public static class QueryHandlersRegistrar
    {
        public static IServiceCollection RegisterQueryHandlers(this IServiceCollection services)
        {
            return services
                .AddScoped<IQueryHandler<GetUsersInputQuery, CollectionOutputQuery<UserOutputQuery>>, GetUsersQueryHandler>()
                .AddScoped<IQueryHandler<GetUserInputQuery, UserOutputQuery>, GetUserQueryHandler>()
                .AddScoped<IQueryHandler<GetFlatForRentAnnouncementPreferenceInputQuery, FlatForRentAnnouncementPreferenceOutputQuery>, GetFlatForRentAnnouncementPreferenceQueryHandler>()
                .AddScoped<IQueryHandler<GetRoomForRentAnnouncementPreferenceInputQuery, RoomForRentAnnouncementPreferenceOutputQuery>, GetRoomForRentAnnouncementPreferenceQueryHandler>();
        }
    }
}