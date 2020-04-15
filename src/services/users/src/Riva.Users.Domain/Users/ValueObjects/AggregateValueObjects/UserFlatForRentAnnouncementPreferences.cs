using System;
using System.Collections.Generic;
using System.Linq;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Domain.Users.Exceptions.AggregateExceptions;

namespace Riva.Users.Domain.Users.ValueObjects.AggregateValueObjects
{
    public class UserFlatForRentAnnouncementPreferences
    {
        private readonly List<FlatForRentAnnouncementPreference> _flatForRentAnnouncementPreferences;

        public UserFlatForRentAnnouncementPreferences(IEnumerable<FlatForRentAnnouncementPreference> flatForRentAnnouncementPreferences, 
            int roomForRentAnnouncementPreferencesCount, int announcementPreferenceLimit)
        {
            if (flatForRentAnnouncementPreferences is null)
                throw new UserFlatForRentAnnouncementPreferencesNullException();

            var flatForRentAnnouncementPreferencesList = flatForRentAnnouncementPreferences.ToList();

            if (flatForRentAnnouncementPreferencesList.Any(x => x is null))
                throw new UserFlatForRentAnnouncementPreferencesNullException();

            if (flatForRentAnnouncementPreferencesList.Count + roomForRentAnnouncementPreferencesCount > announcementPreferenceLimit)
                throw new UserFlatForRentAnnouncementPreferencesExceedAnnouncementPreferenceLimitException();

            var flatForRentAnnouncementPreferencesGroupedByCityId = GetFlatForRentAnnouncementPreferencesGroupedByCityId(flatForRentAnnouncementPreferencesList);

            if (AreAnyWithExpansibleCityDistricts(flatForRentAnnouncementPreferencesGroupedByCityId))
                throw new UserFlatForRentAnnouncementPreferencesInvalidValueException($"{nameof(User.FlatForRentAnnouncementPreferences)} should be modified by CityDistricts extension.");

            if (AreAnyWithChangeablePrices(flatForRentAnnouncementPreferencesGroupedByCityId))
                throw new UserFlatForRentAnnouncementPreferencesInvalidValueException($"{nameof(User.FlatForRentAnnouncementPreferences)} should be modified by Prices change.");

            if (AreAnyWithChangeableRoomNumbers(flatForRentAnnouncementPreferencesGroupedByCityId))
                throw new UserFlatForRentAnnouncementPreferencesInvalidValueException($"{nameof(User.FlatForRentAnnouncementPreferences)} should be modified by RoomNumbers change.");

            _flatForRentAnnouncementPreferences = new List<FlatForRentAnnouncementPreference>(flatForRentAnnouncementPreferencesList);
        }

        public static implicit operator List<FlatForRentAnnouncementPreference>(UserFlatForRentAnnouncementPreferences flatForRentAnnouncementPreferences)
        {
            return flatForRentAnnouncementPreferences._flatForRentAnnouncementPreferences;
        }

        private static List<GroupedFlatForRentAnnouncementPreferences> GetFlatForRentAnnouncementPreferencesGroupedByCityId(IEnumerable<FlatForRentAnnouncementPreference> groupedFlatForRentAnnouncementPreferences)
        {
            return groupedFlatForRentAnnouncementPreferences
                .GroupBy(x => x.CityId)
                .Select(x => new GroupedFlatForRentAnnouncementPreferences(x.ToList()))
                .Where(x => x.FlatForRentAnnouncementPreferences.Count() > 1)
                .ToList();
        }

        private static bool AreAnyWithExpansibleCityDistricts(IEnumerable<GroupedFlatForRentAnnouncementPreferences> groupedFlatForRentAnnouncementPreferencesByCityId)
        {
            return GetFlatForRentAnnouncementPreferencesGroupedByRoomNumbers(groupedFlatForRentAnnouncementPreferencesByCityId)
                .Any(groupedFlatForRentAnnouncementPreferencesByRoomNumbers => GetFlatForRentAnnouncementPreferencesGroupedByPrices(groupedFlatForRentAnnouncementPreferencesByRoomNumbers)
                        .Select(groupedFlatForRentAnnouncementPreferenceByCityDistricts => groupedFlatForRentAnnouncementPreferenceByCityDistricts
                                .SelectMany(x => x.FlatForRentAnnouncementPreferences))
                        .Any(flatForRentAnnouncementPreferences => flatForRentAnnouncementPreferences.Count() > 1));
        }

        private static bool AreAnyWithChangeablePrices(IEnumerable<GroupedFlatForRentAnnouncementPreferences> groupedFlatForRentAnnouncementPreferencesByCityId)
        {
            return GetFlatForRentAnnouncementPreferencesGroupedByCityDistricts(groupedFlatForRentAnnouncementPreferencesByCityId)
                .Any(groupedFlatForRentAnnouncementPreferencesByCityDistricts => GetFlatForRentAnnouncementPreferencesGroupedByRoomNumbers(groupedFlatForRentAnnouncementPreferencesByCityDistricts)
                    .Select(groupedFlatForRentAnnouncementPreferencesByPrices => groupedFlatForRentAnnouncementPreferencesByPrices
                        .SelectMany(x => x.FlatForRentAnnouncementPreferences))
                    .Any(flatForRentAnnouncementPreferences => flatForRentAnnouncementPreferences.Count() > 1));
        }

        private static bool AreAnyWithChangeableRoomNumbers(IEnumerable<GroupedFlatForRentAnnouncementPreferences> groupedFlatForRentAnnouncementPreferencesByCityId)
        {
            return GetFlatForRentAnnouncementPreferencesGroupedByCityDistricts(groupedFlatForRentAnnouncementPreferencesByCityId)
                .Any(groupedFlatForRentAnnouncementPreferenceByCityDistrict => GetFlatForRentAnnouncementPreferencesGroupedByPrices(groupedFlatForRentAnnouncementPreferenceByCityDistrict)
                        .Select(groupedFlatForRentAnnouncementPreferenceByRoomNumbers => groupedFlatForRentAnnouncementPreferenceByRoomNumbers
                                .SelectMany(x => x.FlatForRentAnnouncementPreferences))
                        .Any(flatForRentAnnouncementPreferences => flatForRentAnnouncementPreferences.Count() > 1));
        }

        private static IEnumerable<IEnumerable<GroupedFlatForRentAnnouncementPreferences>> GetFlatForRentAnnouncementPreferencesGroupedByRoomNumbers(IEnumerable<GroupedFlatForRentAnnouncementPreferences> groupedFlatForRentAnnouncementPreferences)
        {
            return groupedFlatForRentAnnouncementPreferences
                .Select(groupedAnnouncementPreferences => groupedAnnouncementPreferences
                    .FlatForRentAnnouncementPreferences
                    .GroupBy(x => new {x.RoomNumbersMin, x.RoomNumbersMax})
                    .Select(x => new GroupedFlatForRentAnnouncementPreferences(x.ToList()))
                    .Where(x => x.FlatForRentAnnouncementPreferences.Count() > 1));
        }

        private static IEnumerable<IEnumerable<GroupedFlatForRentAnnouncementPreferences>> GetFlatForRentAnnouncementPreferencesGroupedByPrices(IEnumerable<GroupedFlatForRentAnnouncementPreferences> groupedFlatForRentAnnouncementPreferences)
        {
            return groupedFlatForRentAnnouncementPreferences
                .Select(groupedAnnouncementPreferences => groupedAnnouncementPreferences
                    .FlatForRentAnnouncementPreferences
                    .GroupBy(x => new {x.PriceMin, x.PriceMax})
                    .Select(x => new GroupedFlatForRentAnnouncementPreferences(x.ToList()))
                    .Where(x => x.FlatForRentAnnouncementPreferences.Count() > 1));
        }

        private static IEnumerable<IEnumerable<GroupedFlatForRentAnnouncementPreferences>> GetFlatForRentAnnouncementPreferencesGroupedByCityDistricts(IEnumerable<GroupedFlatForRentAnnouncementPreferences> groupedFlatForRentAnnouncementPreferences)
        {
            return groupedFlatForRentAnnouncementPreferences
                .Select(groupedAnnouncementPreferences => groupedAnnouncementPreferences
                    .FlatForRentAnnouncementPreferences
                    .GroupBy(x => ConvertCityDistrictsToString(x.CityDistricts))
                    .Select(x => new GroupedFlatForRentAnnouncementPreferences(x.ToList()))
                    .Where(x => x.FlatForRentAnnouncementPreferences.Count() > 1));
        }

        private static string ConvertCityDistrictsToString(IEnumerable<Guid> cityDistricts)
        {
            return string.Join(",", cityDistricts.OrderBy(x => x)).TrimEnd(',');
        }

        private class GroupedFlatForRentAnnouncementPreferences
        {
            public IEnumerable<FlatForRentAnnouncementPreference> FlatForRentAnnouncementPreferences { get; }

            public GroupedFlatForRentAnnouncementPreferences(IEnumerable<FlatForRentAnnouncementPreference> flatForRentAnnouncementPreferences)
            {
                FlatForRentAnnouncementPreferences = flatForRentAnnouncementPreferences;
            }
        }
    }
}