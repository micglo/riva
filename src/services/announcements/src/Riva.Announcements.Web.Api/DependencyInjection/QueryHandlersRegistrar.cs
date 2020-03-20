using Microsoft.Extensions.DependencyInjection;
using Riva.Announcements.Core.Queries;
using Riva.Announcements.Core.Queries.Handlers;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.Announcements.Web.Api.DependencyInjection
{
    public static class QueryHandlersRegistrar
    {
        public static IServiceCollection RegisterQueryHandlers(this IServiceCollection services)
        {
            return services
                .AddSingleton<IQueryHandler<GetFlatForRentAnnouncementsInputQuery, CollectionOutputQuery<FlatForRentAnnouncementOutputQuery>>, GetFlatForRentAnnouncementsQueryHandler>()
                .AddSingleton<IQueryHandler<GetFlatForRentAnnouncementInputQuery, FlatForRentAnnouncementOutputQuery>, GetFlatForRentAnnouncementQueryHandler>()
                .AddSingleton<IQueryHandler<GetRoomForRentAnnouncementsInputQuery, CollectionOutputQuery<RoomForRentAnnouncementOutputQuery>>, GetRoomForRentAnnouncementsQueryHandler>()
                .AddSingleton<IQueryHandler<GetRoomForRentAnnouncementInputQuery, RoomForRentAnnouncementOutputQuery>, GetRoomForRentAnnouncementQueryHandler>();
        }
    }
}