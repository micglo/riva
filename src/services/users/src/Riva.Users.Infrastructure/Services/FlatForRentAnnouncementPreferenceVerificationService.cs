using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Models;
using Riva.Users.Core.Enumerations;
using Riva.Users.Core.ErrorMessages;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Entities;

namespace Riva.Users.Infrastructure.Services
{
    public class FlatForRentAnnouncementPreferenceVerificationService : IFlatForRentAnnouncementPreferenceVerificationService
    {
        public VerificationResult VerifyFlatForRentAnnouncementPreferences(IEnumerable<FlatForRentAnnouncementPreference> flatForRentAnnouncementPreferences)
        {
            var flatForRentAnnouncementPreferencesGroupedByCityId = GetFlatForRentAnnouncementPreferencesGroupedByCityId(flatForRentAnnouncementPreferences);
            var errors = new List<IError>();

            if (AreAnyWithExpansibleCityDistricts(flatForRentAnnouncementPreferencesGroupedByCityId))
            {
                var error = new Error(FlatForRentAnnouncementPreferenceErrorCode.ExpansibleCityDistricts, FlatForRentAnnouncementPreferenceErrorMessage.ExpansibleCityDistricts);
                errors.Add(error);
            }

            if (AreAnyWithChangeablePrices(flatForRentAnnouncementPreferencesGroupedByCityId))
            {
                var error = new Error(FlatForRentAnnouncementPreferenceErrorCode.ChangeablePrices, FlatForRentAnnouncementPreferenceErrorMessage.ChangeablePrices);
                errors.Add(error);
            }

            if (AreAnyWithChangeableRoomNumbers(flatForRentAnnouncementPreferencesGroupedByCityId))
            {
                var error = new Error(FlatForRentAnnouncementPreferenceErrorCode.ChangeableRoomNumbers, FlatForRentAnnouncementPreferenceErrorMessage.ChangeableRoomNumbers);
                errors.Add(error);
            }

            return errors.Any() ? VerificationResult.Fail(errors) : VerificationResult.Ok();
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
                    .GroupBy(x => new { x.RoomNumbersMin, x.RoomNumbersMax })
                    .Select(x => new GroupedFlatForRentAnnouncementPreferences(x.ToList()))
                    .Where(x => x.FlatForRentAnnouncementPreferences.Count() > 1));
        }

        private static IEnumerable<IEnumerable<GroupedFlatForRentAnnouncementPreferences>> GetFlatForRentAnnouncementPreferencesGroupedByPrices(IEnumerable<GroupedFlatForRentAnnouncementPreferences> groupedFlatForRentAnnouncementPreferences)
        {
            return groupedFlatForRentAnnouncementPreferences
                .Select(groupedAnnouncementPreferences => groupedAnnouncementPreferences
                    .FlatForRentAnnouncementPreferences
                    .GroupBy(x => new { x.PriceMin, x.PriceMax })
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