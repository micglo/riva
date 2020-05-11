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
    public class RoomForRentAnnouncementPreferenceService : IRoomForRentAnnouncementPreferenceService
    {
        private readonly ICosmosStore<RoomForRentAnnouncementPreference> _cosmosStore;

        public RoomForRentAnnouncementPreferenceService(ICosmosStore<RoomForRentAnnouncementPreference> cosmosStore)
        {
            _cosmosStore = cosmosStore;
        }

        public async Task AddAsync(UserRoomForRentAnnouncementPreferenceCreatedIntegrationEvent integrationEvent)
        {
            var roomForRentAnnouncementPreference = new RoomForRentAnnouncementPreference
            {
                Id = integrationEvent.RoomForRentAnnouncementPreferenceId,
                UserId = integrationEvent.UserId,
                UserEmail = integrationEvent.UserEmail,
                CityId = integrationEvent.CityId,
                ServiceActive = integrationEvent.ServiceActive,
                AnnouncementSendingFrequency = integrationEvent.AnnouncementSendingFrequency,
                PriceMin = integrationEvent.PriceMin,
                PriceMax = integrationEvent.PriceMax,
                RoomType = integrationEvent.RoomType,
                CityDistricts = integrationEvent.CityDistricts.ToList(),
                CosmosEntityName = nameof(RoomForRentAnnouncementPreference)
            };
            var addResult = await _cosmosStore.AddAsync(roomForRentAnnouncementPreference);
            if (!addResult.IsSuccess)
                throw addResult.Exception;
        }

        public async Task UpdateAsync(UserRoomForRentAnnouncementPreferenceUpdatedIntegrationEvent integrationEvent)
        {
            var roomForRentAnnouncementPreference = await _cosmosStore.FindAsync(integrationEvent.RoomForRentAnnouncementPreferenceId.ToString());

            var sameCityDistricts = roomForRentAnnouncementPreference.CityDistricts.All(integrationEvent.CityDistricts.Contains) &&
                                    roomForRentAnnouncementPreference.CityDistricts.Count == integrationEvent.CityDistricts.Count;
            if (roomForRentAnnouncementPreference.CityId != integrationEvent.CityId ||
                roomForRentAnnouncementPreference.PriceMin != integrationEvent.PriceMin ||
                roomForRentAnnouncementPreference.PriceMax != integrationEvent.PriceMax ||
                roomForRentAnnouncementPreference.RoomType != integrationEvent.RoomType ||
                !sameCityDistricts)
                roomForRentAnnouncementPreference.AnnouncementUrlsToSend = new List<string>();

            roomForRentAnnouncementPreference.CityId = integrationEvent.CityId;
            roomForRentAnnouncementPreference.PriceMin = integrationEvent.PriceMin;
            roomForRentAnnouncementPreference.PriceMax = integrationEvent.PriceMax;
            roomForRentAnnouncementPreference.RoomType = integrationEvent.RoomType;
            roomForRentAnnouncementPreference.CityDistricts = integrationEvent.CityDistricts.ToList();

            var updateResult = await _cosmosStore.UpdateAsync(roomForRentAnnouncementPreference);
            if (!updateResult.IsSuccess)
                throw updateResult.Exception;
        }

        public async Task UpdateAsync(UserUpdatedIntegrationEvent integrationEvent)
        {
            var roomForRentAnnouncementPreferences =
                await _cosmosStore.Query().Where(x => x.UserId == integrationEvent.UserId).ToListAsync();

            foreach (var roomForRentAnnouncementPreference in roomForRentAnnouncementPreferences)
            {
                roomForRentAnnouncementPreference.ServiceActive = integrationEvent.ServiceActive;
                roomForRentAnnouncementPreference.AnnouncementSendingFrequency = integrationEvent.AnnouncementSendingFrequency;
            }

            var updateResult = await _cosmosStore.UpdateRangeAsync(roomForRentAnnouncementPreferences);
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