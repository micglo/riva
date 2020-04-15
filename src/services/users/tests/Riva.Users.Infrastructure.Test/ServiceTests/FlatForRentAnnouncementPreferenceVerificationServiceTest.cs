using System;
using System.Collections.Generic;
using FluentAssertions;
using Riva.BuildingBlocks.Core.Models;
using Riva.Users.Core.Enumerations;
using Riva.Users.Core.ErrorMessages;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Infrastructure.Services;
using Xunit;

namespace Riva.Users.Infrastructure.Test.ServiceTests
{
    public class FlatForRentAnnouncementPreferenceVerificationServiceTest
    {
        private readonly IFlatForRentAnnouncementPreferenceVerificationService _service;

        public FlatForRentAnnouncementPreferenceVerificationServiceTest()
        {
            _service = new FlatForRentAnnouncementPreferenceVerificationService();
        }

        [Fact]
        public void VerifyFlatForRentAnnouncementPreferences_Should_Return_Success_VerificationResult()
        {
            var cityId = Guid.NewGuid();
            var cityDistricts = new List<Guid>{ Guid.NewGuid() };
            var flatForRentAnnouncementPreferences = new List<FlatForRentAnnouncementPreference>
            {
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(cityDistricts)
                    .Build(),
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(2)
                    .SetCityDistricts(cityDistricts)
                    .Build(),
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(2)
                    .SetCityDistricts(new List<Guid>{ Guid.NewGuid() })
                    .Build()
            };
            var expectedResult = VerificationResult.Ok();

            var result = _service.VerifyFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyFlatForRentAnnouncementPreferences_Should_Return_Fail_VerificationResult_When_CityDistricts_Can_Be_Expanded()
        {
            var cityId = Guid.NewGuid();
            var flatForRentAnnouncementPreferences = new List<FlatForRentAnnouncementPreference>
            {
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build(),
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build()
            };
            var errors = new List<IError>
            {
                new Error(FlatForRentAnnouncementPreferenceErrorCode.ExpansibleCityDistricts, FlatForRentAnnouncementPreferenceErrorMessage.ExpansibleCityDistricts)
            };
            var expectedResult = VerificationResult.Fail(errors);

            var result = _service.VerifyFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyFlatForRentAnnouncementPreferences_Should_Return_Fail_VerificationResult_When_Prices_Can_Be_Changed()
        {
            var cityId = Guid.NewGuid();
            var cityDistricts = new List<Guid> { Guid.NewGuid() };
            var flatForRentAnnouncementPreferences = new List<FlatForRentAnnouncementPreference>
            {
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(cityDistricts)
                    .Build(),
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(cityDistricts)
                    .Build()
            };
            var errors = new List<IError>
            {
                new Error(FlatForRentAnnouncementPreferenceErrorCode.ChangeablePrices, FlatForRentAnnouncementPreferenceErrorMessage.ChangeablePrices)
            };
            var expectedResult = VerificationResult.Fail(errors);

            var result = _service.VerifyFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyFlatForRentAnnouncementPreferences_Should_Return_Fail_VerificationResult_When_RoomNumbers_Can_Be_Changed()
        {
            var cityId = Guid.NewGuid();
            var cityDistricts = new List<Guid> { Guid.NewGuid() };
            var flatForRentAnnouncementPreferences = new List<FlatForRentAnnouncementPreference>
            {
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(cityDistricts)
                    .Build(),
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(2)
                    .SetCityDistricts(cityDistricts)
                    .Build()
            };
            var errors = new List<IError>
            {
                new Error(FlatForRentAnnouncementPreferenceErrorCode.ChangeableRoomNumbers, FlatForRentAnnouncementPreferenceErrorMessage.ChangeableRoomNumbers)
            };
            var expectedResult = VerificationResult.Fail(errors);

            var result = _service.VerifyFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}