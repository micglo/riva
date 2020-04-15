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
    public class RoomForRentAnnouncementPreferenceVerificationService : IRoomForRentAnnouncementPreferenceVerificationService
    {
        public VerificationResult VerifyRoomForRentAnnouncementPreferences(IEnumerable<RoomForRentAnnouncementPreference> roomForRentAnnouncementPreferences)
        {
            var roomForRentAnnouncementPreferencesGroupedByCityId = GetRoomForRentAnnouncementPreferencesGroupedByCityId(roomForRentAnnouncementPreferences);
            var errors = new List<IError>();

            if (AreAnyWithExpansibleCityDistricts(roomForRentAnnouncementPreferencesGroupedByCityId))
            {
                var error = new Error(RoomForRentAnnouncementPreferenceErrorCode.ExpansibleCityDistricts, RoomForRentAnnouncementPreferenceErrorMessage.ExpansibleCityDistricts);
                errors.Add(error);
            }

            if (AreAnyWithChangeablePrices(roomForRentAnnouncementPreferencesGroupedByCityId))
            {
                var error = new Error(RoomForRentAnnouncementPreferenceErrorCode.ChangeablePrices, RoomForRentAnnouncementPreferenceErrorMessage.ChangeablePrices);
                errors.Add(error);
            }

            return errors.Any() ? VerificationResult.Fail(errors) : VerificationResult.Ok();
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
                    .GroupBy(x => new { x.PriceMin, x.PriceMax })
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