using System.Threading.Tasks;
using Riva.AnnouncementPreferences.Core.IntegrationEvents.AnnouncementIntegrationEvents;

namespace Riva.AnnouncementPreferences.Core.Services
{
    public interface IRoomForRentAnnouncementPreferenceMatchService
    {
        Task MatchAnnouncementsToPreferencesAsync(RoomForRentAnnouncementsIntegrationEvent integrationEvent);
    }
}