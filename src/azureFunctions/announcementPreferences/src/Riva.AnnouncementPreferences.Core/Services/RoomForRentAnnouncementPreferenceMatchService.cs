using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Extensions;
using Microsoft.Azure.CosmosDB.BulkExecutor;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Riva.AnnouncementPreferences.Core.Entities;
using Riva.AnnouncementPreferences.Core.IntegrationEvents.AnnouncementIntegrationEvents;
using Riva.AnnouncementPreferences.Core.Models;

namespace Riva.AnnouncementPreferences.Core.Services
{
    public class RoomForRentAnnouncementPreferenceMatchService : IRoomForRentAnnouncementPreferenceMatchService
    {
        private readonly ICosmosStore<RoomForRentAnnouncementPreference> _cosmosStore;
        private readonly IBulkExecutor _bulkExecutor;
        private readonly int _maxDegreeOfParallelism;
        private readonly ParallelOptions _parallelOptions;

        public RoomForRentAnnouncementPreferenceMatchService(ICosmosStore<RoomForRentAnnouncementPreference> cosmosStore,
            IBulkExecutorInitializer bulkExecutorInitializer, IOptions<AppSettings> options)
        {
            _cosmosStore = cosmosStore;
            _bulkExecutor = Task.Run(bulkExecutorInitializer.InitializeBulkExecutorAsync).Result;
            _maxDegreeOfParallelism = Convert.ToInt32(options.Value.MaxDegreeOfParallelism);
            _parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = _maxDegreeOfParallelism };
        }

        public async Task MatchAnnouncementsToPreferencesAsync(RoomForRentAnnouncementsIntegrationEvent integrationEvent)
        {
            var roomForRentAnnouncementPreferences =
                await _cosmosStore.Query().Where(x => x.CityId == integrationEvent.CityId && x.ServiceActive).ToListAsync();

            var processedAnnouncementPreferenceIds = new HashSet<Guid>();
            Parallel.ForEach(
                roomForRentAnnouncementPreferences,
                _parallelOptions,
                roomForRentAnnouncementPreference => ProcessRoomForRentAnnouncementPreference(roomForRentAnnouncementPreference, integrationEvent, processedAnnouncementPreferenceIds));

            var documents = roomForRentAnnouncementPreferences.Select(JsonConvert.SerializeObject);
            await _bulkExecutor.BulkImportAsync(documents, true, true, _maxDegreeOfParallelism);
        }

        private static void ProcessRoomForRentAnnouncementPreference(RoomForRentAnnouncementPreference roomForRentAnnouncementPreference,
            RoomForRentAnnouncementsIntegrationEvent integrationEvent, HashSet<Guid> processedAnnouncementPreferenceIds)
        {
            lock (processedAnnouncementPreferenceIds)
            {
                if (!processedAnnouncementPreferenceIds.Add(roomForRentAnnouncementPreference.Id))
                    return;
            }

            foreach (var roomForRentAnnouncement in integrationEvent.RoomForRentAnnouncements)
            {
                if (roomForRentAnnouncementPreference.RoomType.HasValue)
                {
                    if (roomForRentAnnouncement.RoomTypes.Any())
                    {
                        if (roomForRentAnnouncement.RoomTypes.Contains(roomForRentAnnouncementPreference.RoomType.Value))
                        {
                            ProcessRoomForRentAnnouncementPreferenceForPriceMin(roomForRentAnnouncement, roomForRentAnnouncementPreference);
                        }
                    }
                    else
                    {
                        ProcessRoomForRentAnnouncementPreferenceForPriceMin(roomForRentAnnouncement, roomForRentAnnouncementPreference);
                    }
                }
                else
                {
                    ProcessRoomForRentAnnouncementPreferenceForPriceMin(roomForRentAnnouncement, roomForRentAnnouncementPreference);
                }
            }
        }

        private static void ProcessRoomForRentAnnouncementPreferenceForPriceMin(RoomForRentAnnouncement roomForRentAnnouncement,
            RoomForRentAnnouncementPreference roomForRentAnnouncementPreference)
        {
            if (roomForRentAnnouncementPreference.PriceMin.HasValue && roomForRentAnnouncement.Price.HasValue)
            {
                if (PriceMinMatches(roomForRentAnnouncement.Price.Value, roomForRentAnnouncementPreference.PriceMin.Value))
                {
                    ProcessRoomForRentAnnouncementPreferenceForPriceMax(roomForRentAnnouncement, roomForRentAnnouncementPreference);
                }
            }
            else
            {
                ProcessRoomForRentAnnouncementPreferenceForPriceMax(roomForRentAnnouncement, roomForRentAnnouncementPreference);
            }
        }

        private static void ProcessRoomForRentAnnouncementPreferenceForPriceMax(RoomForRentAnnouncement roomForRentAnnouncement,
            RoomForRentAnnouncementPreference roomForRentAnnouncementPreference)
        {
            if (roomForRentAnnouncementPreference.PriceMax.HasValue && roomForRentAnnouncement.Price.HasValue)
            {
                if (PriceMaxMatches(roomForRentAnnouncement.Price.Value, roomForRentAnnouncementPreference.PriceMax.Value) &&
                    CityDistrictsMatch(roomForRentAnnouncement, roomForRentAnnouncementPreference))
                {
                    roomForRentAnnouncementPreference.AnnouncementUrlsToSend.Add(roomForRentAnnouncement.SourceUrl);
                }
            }
            else
            {
                if (CityDistrictsMatch(roomForRentAnnouncement, roomForRentAnnouncementPreference))
                    roomForRentAnnouncementPreference.AnnouncementUrlsToSend.Add(roomForRentAnnouncement.SourceUrl);
            }
        }

        private static bool PriceMinMatches(decimal roomForRentAnnouncementPrice, decimal roomForRentAnnouncementPreferencePriceMin)
        {
            return roomForRentAnnouncementPrice >= roomForRentAnnouncementPreferencePriceMin;
        }

        private static bool PriceMaxMatches(decimal roomForRentAnnouncementPrice, decimal roomForRentAnnouncementPreferencePriceMax)
        {
            return roomForRentAnnouncementPrice <= roomForRentAnnouncementPreferencePriceMax;
        }

        private static bool CityDistrictsMatch(RoomForRentAnnouncement roomForRentAnnouncement, RoomForRentAnnouncementPreference roomForRentAnnouncementPreference)
        {
            if (roomForRentAnnouncement.CityDistricts.Any() && roomForRentAnnouncementPreference.CityDistricts.Any())
                return roomForRentAnnouncementPreference.CityDistricts.Intersect(roomForRentAnnouncementPreference.CityDistricts).Any();

            return true;
        }
    }
}