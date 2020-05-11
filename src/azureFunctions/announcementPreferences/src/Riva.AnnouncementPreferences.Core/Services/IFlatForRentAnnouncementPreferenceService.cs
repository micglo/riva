using System;
using System.Threading.Tasks;
using Riva.AnnouncementPreferences.Core.IntegrationEvents.UserIntegrationEvents;

namespace Riva.AnnouncementPreferences.Core.Services
{
    public interface IFlatForRentAnnouncementPreferenceService
    {
        Task AddAsync(UserFlatForRentAnnouncementPreferenceCreatedIntegrationEvent integrationEvent);
        Task UpdateAsync(UserFlatForRentAnnouncementPreferenceUpdatedIntegrationEvent integrationEvent);
        Task UpdateAsync(UserUpdatedIntegrationEvent integrationEvent);
        Task DeleteByIdAsync(Guid id);
        Task DeleteAllByUserIdAsync(Guid userId);
    }
}