using System;
using System.Collections.Generic;
using System.Linq;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Domain.Users.Exceptions.AggregateExceptions;

namespace Riva.Users.Domain.Users.ValueObjects.AggregateValueObjects
{
    public class UserRoomForRentAnnouncementPreferences
    {
        private readonly List<RoomForRentAnnouncementPreference> _roomForRentAnnouncementPreferences;

        public UserRoomForRentAnnouncementPreferences(IEnumerable<RoomForRentAnnouncementPreference> roomForRentAnnouncementPreferences, 
            int flatForRentAnnouncementPreferencesCount, int announcementPreferenceLimit)
        {
            if (roomForRentAnnouncementPreferences is null)
                throw new UserRoomForRentAnnouncementPreferencesNullException();

            var roomForRentAnnouncementPreferencesList = roomForRentAnnouncementPreferences.ToList();

            if (roomForRentAnnouncementPreferencesList.Any(x => x is null))
                throw new UserRoomForRentAnnouncementPreferencesNullException();

            if(roomForRentAnnouncementPreferencesList.Count + flatForRentAnnouncementPreferencesCount > announcementPreferenceLimit)
                throw new UserRoomForRentAnnouncementPreferencesExceedAnnouncementPreferenceLimitException();

            var roomForRentAnnouncementPreferencesGroupedByCityId = GetRoomForRentAnnouncementPreferencesGroupedByCityId(roomForRentAnnouncementPreferencesList);

            if (AreAnyWithExpansibleCityDistricts(roomForRentAnnouncementPreferencesGroupedByCityId))
                throw new UserRoomForRentAnnouncementPreferencesInvalidValueException($"{nameof(User.RoomForRentAnnouncementPreferences)} should be modified by CityDistricts extension.");

            if (AreAnyWithChangeablePrices(roomForRentAnnouncementPreferencesGroupedByCityId))
                throw new UserRoomForRentAnnouncementPreferencesInvalidValueException($"{nameof(User.RoomForRentAnnouncementPreferences)} should be modified by Prices change.");

            _roomForRentAnnouncementPreferences = new List<RoomForRentAnnouncementPreference>(roomForRentAnnouncementPreferencesList);
        }

        public static implicit operator List<RoomForRentAnnouncementPreference>(UserRoomForRentAnnouncementPreferences roomForRentAnnouncementPreferences)
        {
            return roomForRentAnnouncementPreferences._roomForRentAnnouncementPreferences;
        }

        private static List<GroupedRoomForRentAnnouncementPreferences> GetRoomForRentAnnouncementPreferencesGroupedByCityId(IEnumerable<RoomForRentAnnouncementPreference> roomForRentAnnouncementPreferences)
        {
            return roomForRentAnnouncementPreferences
                .GroupBy(x => x.CityId)
                .Select(x => new GroupedRoomForRentAnnouncementPreferences(x.ToList()))
                .Where(x => x.RoomForRentAnnouncementPreferences.Count() > 1)
                .ToList();
        }

        private static bool AreAnyWithExpansibleCityDistricts(IEnumerable<GroupedRoomForRentAnnouncementPreferences> groupedRoomForRentAnnouncementPreferencesByCityId)
        {
            return GetRoomForRentAnnouncementPreferencesGroupedByRoomType(groupedRoomForRentAnnouncementPreferencesByCityId)
                .Any(groupedRoomForRentAnnouncementPreferencesByRoomType => GetRoomForRentAnnouncementPreferencesGroupedByPrices(groupedRoomForRentAnnouncementPreferencesByRoomType)
                        .Select(groupedRoomForRentAnnouncementPreferenceByPrices => groupedRoomForRentAnnouncementPreferenceByPrices
                                .SelectMany(x => x.RoomForRentAnnouncementPreferences))
                        .Any(roomForRentAnnouncementPreferences => roomForRentAnnouncementPreferences.Count() > 1));
        }

        private static bool AreAnyWithChangeablePrices(IEnumerable<GroupedRoomForRentAnnouncementPreferences> groupedRoomForRentAnnouncementPreferencesByCityId)
        {
            return GetRoomForRentAnnouncementPreferencesGroupedByCityDistricts(groupedRoomForRentAnnouncementPreferencesByCityId)
                .Any(groupedRoomForRentAnnouncementPreferencesByCityDistricts => GetRoomForRentAnnouncementPreferencesGroupedByRoomType(groupedRoomForRentAnnouncementPreferencesByCityDistricts)
                        .Select(groupedRoomForRentAnnouncementPreferencesByPrices => groupedRoomForRentAnnouncementPreferencesByPrices
                                .SelectMany(x => x.RoomForRentAnnouncementPreferences))
                        .Any(roomForRentAnnouncementPreferences => roomForRentAnnouncementPreferences.Count() > 1));
        }

        private static IEnumerable<IEnumerable<GroupedRoomForRentAnnouncementPreferences>> GetRoomForRentAnnouncementPreferencesGroupedByRoomType(IEnumerable<GroupedRoomForRentAnnouncementPreferences> groupedRoomForRentAnnouncementPreferences)
        {
            return groupedRoomForRentAnnouncementPreferences
                .Select(groupedAnnouncementPreferences => groupedAnnouncementPreferences
                    .RoomForRentAnnouncementPreferences
                    .GroupBy(x => x.RoomType)
                    .Select(x => new GroupedRoomForRentAnnouncementPreferences(x.ToList()))
                    .Where(x => x.RoomForRentAnnouncementPreferences.Count() > 1));
        }

        private static IEnumerable<IEnumerable<GroupedRoomForRentAnnouncementPreferences>> GetRoomForRentAnnouncementPreferencesGroupedByPrices(IEnumerable<GroupedRoomForRentAnnouncementPreferences> groupedRoomForRentAnnouncementPreferences)
        {
            return groupedRoomForRentAnnouncementPreferences
                .Select(groupedAnnouncementPreferences => groupedAnnouncementPreferences
                    .RoomForRentAnnouncementPreferences
                    .GroupBy(x => new {x.PriceMin, x.PriceMax})
                    .Select(x => new GroupedRoomForRentAnnouncementPreferences(x.ToList()))
                    .Where(x => x.RoomForRentAnnouncementPreferences.Count() > 1));
        }

        private static IEnumerable<IEnumerable<GroupedRoomForRentAnnouncementPreferences>> GetRoomForRentAnnouncementPreferencesGroupedByCityDistricts(IEnumerable<GroupedRoomForRentAnnouncementPreferences> groupedRoomForRentAnnouncementPreferences)
        {
            return groupedRoomForRentAnnouncementPreferences
                .Select(groupedAnnouncementPreferences => groupedAnnouncementPreferences
                    .RoomForRentAnnouncementPreferences
                    .GroupBy(x => ConvertCityDistrictsToString(x.CityDistricts))
                    .Select(x => new GroupedRoomForRentAnnouncementPreferences(x.ToList()))
                    .Where(x => x.RoomForRentAnnouncementPreferences.Count() > 1));
        }

        private static string ConvertCityDistrictsToString(IEnumerable<Guid> cityDistricts)
        {
            return string.Join(",", cityDistricts.OrderBy(x => x)).TrimEnd(',');
        }

        private class GroupedRoomForRentAnnouncementPreferences
        {
            public IEnumerable<RoomForRentAnnouncementPreference> RoomForRentAnnouncementPreferences { get; }

            public GroupedRoomForRentAnnouncementPreferences(IEnumerable<RoomForRentAnnouncementPreference> roomForRentAnnouncementPreferences)
            {
                RoomForRentAnnouncementPreferences = roomForRentAnnouncementPreferences;
            }
        }
    }
}