using Microsoft.Extensions.DependencyInjection;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Repositories;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Repositories;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Repositories;

namespace Riva.Announcements.Web.Api.DependencyInjection
{
    public static class RepositoryRegistrar
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IRoomForRentAnnouncementRepository, RoomForRentAnnouncementRepository>()
                .AddSingleton<IFlatForRentAnnouncementRepository, FlatForRentAnnouncementRepository>();
        }
    }
}