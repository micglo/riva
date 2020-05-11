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
using Riva.AnnouncementPreferences.Core.Extensions;
using Riva.AnnouncementPreferences.Core.IntegrationEvents.AnnouncementIntegrationEvents;
using Riva.AnnouncementPreferences.Core.Models;

namespace Riva.AnnouncementPreferences.Core.Services
{
    public class FlatForRentAnnouncementPreferenceMatchService : IFlatForRentAnnouncementPreferenceMatchService
    {
        private readonly ICosmosStore<FlatForRentAnnouncementPreference> _cosmosStore;
        private readonly IBulkExecutor _bulkExecutor;
        private readonly int _maxDegreeOfParallelism;
        private readonly ParallelOptions _parallelOptions;

        public FlatForRentAnnouncementPreferenceMatchService(ICosmosStore<FlatForRentAnnouncementPreference> cosmosStore,
            IBulkExecutorInitializer bulkExecutorInitializer, IOptions<AppSettings> options)
        {
            _cosmosStore = cosmosStore;
            _bulkExecutor = Task.Run(bulkExecutorInitializer.InitializeBulkExecutorAsync).Result;
            _maxDegreeOfParallelism = Convert.ToInt32(options.Value.MaxDegreeOfParallelism);
            _parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = _maxDegreeOfParallelism };
        }

        public async Task MatchAnnouncementsToPreferencesAsync(FlatForRentAnnouncementsIntegrationEvent integrationEvent)
        {
            var flatForRentAnnouncementPreferences =
                await _cosmosStore.Query().Where(x => x.CityId == integrationEvent.CityId && x.ServiceActive).ToListAsync();

            var processedAnnouncementPreferenceIds = new HashSet<Guid>();
            Parallel.ForEach(
                flatForRentAnnouncementPreferences,
                _parallelOptions,
                flatForRentAnnouncementPreference => ProcessFlatForRentAnnouncementPreference(flatForRentAnnouncementPreference, integrationEvent, processedAnnouncementPreferenceIds));

            var documents = flatForRentAnnouncementPreferences.Select(JsonConvert.SerializeObject);
            await _bulkExecutor.BulkImportAsync(documents, true, true, _maxDegreeOfParallelism);
        }

        private static void ProcessFlatForRentAnnouncementPreference(FlatForRentAnnouncementPreference flatForRentAnnouncementPreference,
            FlatForRentAnnouncementsIntegrationEvent integrationEvent, HashSet<Guid> processedAnnouncementPreferenceIds)
        {
            lock (processedAnnouncementPreferenceIds)
            {
                if (!processedAnnouncementPreferenceIds.Add(flatForRentAnnouncementPreference.Id))
                    return;
            }

            foreach (var flatForRentAnnouncement in integrationEvent.FlatForRentAnnouncements)
            {
                var numberOfRooms = flatForRentAnnouncement.NumberOfRooms.ConvertToInt();

                if (flatForRentAnnouncementPreference.RoomNumbersMin.HasValue && numberOfRooms.HasValue)
                {
                    if (NumberOfRoomsMinMatches(numberOfRooms.Value, flatForRentAnnouncementPreference.RoomNumbersMin.Value))
                    {
                        ProcessFlatForRentAnnouncementPreferenceForNumberOfRoomsMax(flatForRentAnnouncement, flatForRentAnnouncementPreference, numberOfRooms);
                    }
                }
                else
                {
                    ProcessFlatForRentAnnouncementPreferenceForNumberOfRoomsMax(flatForRentAnnouncement, flatForRentAnnouncementPreference, numberOfRooms);
                }
            }
        }

        private static void ProcessFlatForRentAnnouncementPreferenceForNumberOfRoomsMax(FlatForRentAnnouncement flatForRentAnnouncement,
            FlatForRentAnnouncementPreference flatForRentAnnouncementPreference, int? numberOfRooms)
        {
            if (flatForRentAnnouncementPreference.RoomNumbersMax.HasValue && numberOfRooms.HasValue)
            {
                if (NumberOfRoomsMaxMatches(numberOfRooms.Value, flatForRentAnnouncementPreference.RoomNumbersMax.Value))
                {
                    ProcessFlatForRentAnnouncementPreferenceForPriceMin(flatForRentAnnouncement, flatForRentAnnouncementPreference);
                }
            }
            else
            {
                ProcessFlatForRentAnnouncementPreferenceForPriceMin(flatForRentAnnouncement, flatForRentAnnouncementPreference);
            }
        }

        private static void ProcessFlatForRentAnnouncementPreferenceForPriceMin(FlatForRentAnnouncement flatForRentAnnouncement,
            FlatForRentAnnouncementPreference flatForRentAnnouncementPreference)
        {
            if (flatForRentAnnouncementPreference.PriceMin.HasValue && flatForRentAnnouncement.Price.HasValue)
            {
                if (PriceMinMatches(flatForRentAnnouncement.Price.Value, flatForRentAnnouncementPreference.PriceMin.Value))
                {
                    ProcessFlatForRentAnnouncementPreferenceForPriceMax(flatForRentAnnouncement, flatForRentAnnouncementPreference);
                }
            }
            else
            {
                ProcessFlatForRentAnnouncementPreferenceForPriceMax(flatForRentAnnouncement, flatForRentAnnouncementPreference);
            }
        }

        private static void ProcessFlatForRentAnnouncementPreferenceForPriceMax(FlatForRentAnnouncement flatForRentAnnouncement,
            FlatForRentAnnouncementPreference flatForRentAnnouncementPreference)
        {
            if (flatForRentAnnouncementPreference.PriceMax.HasValue && flatForRentAnnouncement.Price.HasValue)
            {
                if (PriceMaxMatches(flatForRentAnnouncement.Price.Value, flatForRentAnnouncementPreference.PriceMax.Value) &&
                    CityDistrictsMatch(flatForRentAnnouncement, flatForRentAnnouncementPreference))
                {
                    flatForRentAnnouncementPreference.AnnouncementUrlsToSend.Add(flatForRentAnnouncement.SourceUrl);
                }
            }
            else
            {
                if (CityDistrictsMatch(flatForRentAnnouncement, flatForRentAnnouncementPreference))
                    flatForRentAnnouncementPreference.AnnouncementUrlsToSend.Add(flatForRentAnnouncement.SourceUrl);
            }
        }

        private static bool NumberOfRoomsMinMatches(int flatForRentAnnouncementNumberOfRooms, int flatForRentAnnouncementPreferenceNumberOfRoomsMin)
        {
            return flatForRentAnnouncementNumberOfRooms >= flatForRentAnnouncementPreferenceNumberOfRoomsMin;
        }

        private static bool NumberOfRoomsMaxMatches(int flatForRentAnnouncementNumberOfRooms, int flatForRentAnnouncementPreferenceNumberOfRoomsMax)
        {
            return flatForRentAnnouncementNumberOfRooms <= flatForRentAnnouncementPreferenceNumberOfRoomsMax;
        }

        private static bool PriceMinMatches(decimal flatForRentAnnouncementPrice, decimal flatForRentAnnouncementPreferencePriceMin)
        {
            return flatForRentAnnouncementPrice >= flatForRentAnnouncementPreferencePriceMin;
        }

        private static bool PriceMaxMatches(decimal flatForRentAnnouncementPrice, decimal flatForRentAnnouncementPreferencePriceMax)
        {
            return flatForRentAnnouncementPrice <= flatForRentAnnouncementPreferencePriceMax;
        }

        private static bool CityDistrictsMatch(FlatForRentAnnouncement flatForRentAnnouncement, FlatForRentAnnouncementPreference flatForRentAnnouncementPreference)
        {
            if (flatForRentAnnouncement.CityDistricts.Any() && flatForRentAnnouncementPreference.CityDistricts.Any())
                return flatForRentAnnouncementPreference.CityDistricts.Intersect(flatForRentAnnouncementPreference.CityDistricts).Any();

            return true;
        }
    }
}