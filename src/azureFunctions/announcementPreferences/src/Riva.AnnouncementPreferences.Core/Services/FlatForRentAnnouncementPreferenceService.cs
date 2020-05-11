using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Extensions;
using Riva.AnnouncementPreferences.Core.Entities;
using Riva.AnnouncementPreferences.Core.Exceptions;
using Riva.AnnouncementPreferences.Core.IntegrationEvents.UserIntegrationEvents;

namespace Riva.AnnouncementPreferences.Core.Services
{
    public class FlatForRentAnnouncementPreferenceService : IFlatForRentAnnouncementPreferenceService
    {
        private readonly ICosmosStore<FlatForRentAnnouncementPreference> _cosmosStore;

        public FlatForRentAnnouncementPreferenceService(ICosmosStore<FlatForRentAnnouncementPreference> cosmosStore)
        {
            _cosmosStore = cosmosStore;
        }

        public async Task AddAsync(UserFlatForRentAnnouncementPreferenceCreatedIntegrationEvent integrationEvent)
        {
            var flatForRentAnnouncementPreference = new FlatForRentAnnouncementPreference
            {
                Id = integrationEvent.FlatForRentAnnouncementPreferenceId,
                UserId = integrationEvent.UserId,
                UserEmail = integrationEvent.UserEmail,
                CityId = integrationEvent.CityId,
                ServiceActive = integrationEvent.ServiceActive,
                AnnouncementSendingFrequency = integrationEvent.AnnouncementSendingFrequency,
                PriceMin = integrationEvent.PriceMin,
                PriceMax = integrationEvent.PriceMax,
                RoomNumbersMin = integrationEvent.RoomNumbersMin,
                RoomNumbersMax = integrationEvent.RoomNumbersMax,
                CityDistricts = integrationEvent.CityDistricts.ToList(),
                CosmosEntityName = nameof(FlatForRentAnnouncementPreference)
            };
            var addResult = await _cosmosStore.AddAsync(flatForRentAnnouncementPreference);
            if (!addResult.IsSuccess)
                throw addResult.Exception;
        }

        public async Task UpdateAsync(UserFlatForRentAnnouncementPreferenceUpdatedIntegrationEvent integrationEvent)
        {
            var flatForRentAnnouncementPreference = await _cosmosStore.FindAsync(integrationEvent.FlatForRentAnnouncementPreferenceId.ToString());

            var sameCityDistricts = flatForRentAnnouncementPreference.CityDistricts.All(integrationEvent.CityDistricts.Contains) &&
                                    flatForRentAnnouncementPreference.CityDistricts.Count == integrationEvent.CityDistricts.Count;
            if (flatForRentAnnouncementPreference.CityId != integrationEvent.CityId ||
               flatForRentAnnouncementPreference.PriceMin != integrationEvent.PriceMin ||
               flatForRentAnnouncementPreference.PriceMax != integrationEvent.PriceMax ||
               flatForRentAnnouncementPreference.RoomNumbersMin != integrationEvent.RoomNumbersMin ||
               flatForRentAnnouncementPreference.RoomNumbersMax != integrationEvent.RoomNumbersMax ||
               !sameCityDistricts)
                flatForRentAnnouncementPreference.AnnouncementUrlsToSend = new List<string>();

            flatForRentAnnouncementPreference.CityId = integrationEvent.CityId;
            flatForRentAnnouncementPreference.PriceMin = integrationEvent.PriceMin;
            flatForRentAnnouncementPreference.PriceMax = integrationEvent.PriceMax;
            flatForRentAnnouncementPreference.RoomNumbersMin = integrationEvent.RoomNumbersMin;
            flatForRentAnnouncementPreference.RoomNumbersMax = integrationEvent.RoomNumbersMax;
            flatForRentAnnouncementPreference.CityDistricts = integrationEvent.CityDistricts.ToList();

            var updateResult = await _cosmosStore.UpdateAsync(flatForRentAnnouncementPreference);
            if (!updateResult.IsSuccess)
                throw updateResult.Exception;
        }

        public async Task UpdateAsync(UserUpdatedIntegrationEvent integrationEvent)
        {
            var flatForRentAnnouncementPreferences =
                await _cosmosStore.Query().Where(x => x.UserId == integrationEvent.UserId).ToListAsync();

            foreach (var flatForRentAnnouncementPreference in flatForRentAnnouncementPreferences)
            {
                flatForRentAnnouncementPreference.ServiceActive = integrationEvent.ServiceActive;
                flatForRentAnnouncementPreference.AnnouncementSendingFrequency = integrationEvent.AnnouncementSendingFrequency;
            }

            var updateResult = await _cosmosStore.UpdateRangeAsync(flatForRentAnnouncementPreferences);
            if (!updateResult.IsSuccess)
                throw new UpdateManyFailureException();
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var removeResult = await _cosmosStore.RemoveByIdAsync(id.ToString());
            if (!removeResult.IsSuccess)
                throw removeResult.Exception;
        }

        public async Task DeleteAllByUserIdAsync(Guid userId)
        {
            var flatForRentAnnouncementPreferences = await _cosmosStore.Query().Where(x => x.UserId == userId).ToListAsync();
            var removeResult = await _cosmosStore.RemoveRangeAsync(flatForRentAnnouncementPreferences);
            if (!removeResult.IsSuccess)
                throw new DeleteManyFailureException();
        }
    }
}