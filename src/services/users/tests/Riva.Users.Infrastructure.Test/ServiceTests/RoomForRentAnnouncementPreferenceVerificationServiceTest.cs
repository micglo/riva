using System;
using System.Collections.Generic;
using FluentAssertions;
using Riva.BuildingBlocks.Core.Models;
using Riva.Users.Core.Enumerations;
using Riva.Users.Core.ErrorMessages;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Domain.Users.Enumerations;
using Riva.Users.Infrastructure.Services;
using Xunit;

namespace Riva.Users.Infrastructure.Test.ServiceTests
{
    public class RoomForRentAnnouncementPreferenceVerificationServiceTest
    {
        private readonly IRoomForRentAnnouncementPreferenceVerificationService _service;

        public RoomForRentAnnouncementPreferenceVerificationServiceTest()
        {
            _service = new RoomForRentAnnouncementPreferenceVerificationService();
        }

        [Fact]
        public void VerifyRoomForRentAnnouncementPreferences_Should_Return_Success_VerificationResult()
        {
            var cityId = Guid.NewGuid();
            var cityDistricts = new List<Guid>{ Guid.NewGuid() };
            var roomForRentAnnouncementPreferences = new List<RoomForRentAnnouncementPreference>
            {
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(cityDistricts)
                    .Build(),
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomType(RoomTypeEnumeration.Double)
                    .SetCityDistricts(cityDistricts)
                    .Build(),
                RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(1000)
                .SetRoomType(RoomTypeEnumeration.Double)
                .SetCityDistricts(new List<Guid>{ Guid.NewGuid() })
                .Build()
            };
            var expectedResult = VerificationResult.Ok();

            var result = _service.VerifyRoomForRentAnnouncementPreferences(roomForRentAnnouncementPreferences);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyRoomForRentAnnouncementPreferences_Should_Return_Fail_VerificationResult_When_CityDistricts_Can_Be_Expanded()
        {
            var cityId = Guid.NewGuid();
            var roomForRentAnnouncementPreferences = new List<RoomForRentAnnouncementPreference>
            {
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build(),
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build()
            };
            var errors = new List<IError>
            {
                new Error(RoomForRentAnnouncementPreferenceErrorCode.ExpansibleCityDistricts, RoomForRentAnnouncementPreferenceErrorMessage.ExpansibleCityDistricts)
            };
            var expectedResult = VerificationResult.Fail(errors);

            var result = _service.VerifyRoomForRentAnnouncementPreferences(roomForRentAnnouncementPreferences);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyRoomForRentAnnouncementPreferences_Should_Return_Fail_VerificationResult_When_Prices_Can_Be_Changed()
        {
            var cityId = Guid.NewGuid();
            var cityDistricts = new List<Guid> { Guid.NewGuid() };
            var roomForRentAnnouncementPreferences = new List<RoomForRentAnnouncementPreference>
            {
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(cityDistricts)
                    .Build(),
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(0)
                    .SetPriceMax(1500)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(cityDistricts)
                    .Build()
            };
            var errors = new List<IError>
            {
                new Error(RoomForRentAnnouncementPreferenceErrorCode.ChangeablePrices, RoomForRentAnnouncementPreferenceErrorMessage.ChangeablePrices)
            };
            var expectedResult = VerificationResult.Fail(errors);

            var result = _service.VerifyRoomForRentAnnouncementPreferences(roomForRentAnnouncementPreferences);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}