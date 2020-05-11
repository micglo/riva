using System;
using System.Threading.Tasks;
using Riva.AnnouncementPreferences.Core.IntegrationEvents.UserIntegrationEvents;

namespace Riva.AnnouncementPreferences.Core.Services
{
    public interface IRoomForRentAnnouncementPreferenceService
    {
        Task AddAsync(UserRoomForRentAnnouncementPreferenceCreatedIntegrationEvent integrationEvent);
        Task UpdateAsync(UserRoomForRentAnnouncementPreferenceUpdatedIntegrationEvent integrationEvent);
        Task UpdateAsync(UserUpdatedIntegrationEvent integrationEvent);
        Task DeleteByIdAsync(Guid id);
        Task DeleteAllByUserIdAsync(Guid userId);
    }
}