using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Defaults;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Domain.Users.Enumerations;
using Riva.Users.Domain.Users.Events;
using Riva.Users.Domain.Users.Exceptions.AggregateExceptions;
using Xunit;

namespace Riva.Users.Domain.Test
{
    public class UserTest
    {
        [Fact]
        public void Should_Create_User()
        {
            var userId = Guid.NewGuid();
            const string email = "email@email.com";
            const bool serviceActive = DefaultUserSettings.ServiceActive;
            const int announcementPreferenceLimit = 6;
            var announcementSendingFrequency = DefaultUserSettings.AnnouncementSendingFrequency;
            var cityId = Guid.NewGuid();
            var cityDistricts = new List<Guid> { Guid.NewGuid() };
            const decimal flatForRentAnnouncementPreferencePriceMin = 0;
            const decimal flatForRentAnnouncementPreferencePriceMax = 2000;
            const string picture = "UrlToPicture";
            var flatForRentAnnouncementPreferences = new List<FlatForRentAnnouncementPreference>
            {
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(flatForRentAnnouncementPreferencePriceMin)
                    .SetPriceMax(1500)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(cityDistricts)
                    .Build(),
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(flatForRentAnnouncementPreferencePriceMin)
                    .SetPriceMax(flatForRentAnnouncementPreferencePriceMax)
                    .SetRoomNumbersMin(2)
                    .SetRoomNumbersMax(2)
                    .SetCityDistricts(cityDistricts)
                    .Build(),
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(flatForRentAnnouncementPreferencePriceMin)
                    .SetPriceMax(1500)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build()
            };
            const decimal roomForRentAnnouncementPreferencePriceMin = 0;
            const decimal roomForRentAnnouncementPreferencePriceMax = 1500;
            var roomForRentAnnouncementPreferences = new List<RoomForRentAnnouncementPreference>
            {
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(roomForRentAnnouncementPreferencePriceMin)
                    .SetPriceMax(roomForRentAnnouncementPreferencePriceMax)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(cityDistricts)
                    .Build(),
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(roomForRentAnnouncementPreferencePriceMin)
                    .SetPriceMax(roomForRentAnnouncementPreferencePriceMax)
                    .SetRoomType(RoomTypeEnumeration.Double)
                    .SetCityDistricts(cityDistricts)
                    .Build(),
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(roomForRentAnnouncementPreferencePriceMin)
                    .SetPriceMax(roomForRentAnnouncementPreferencePriceMax)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(new List<Guid>{ Guid.NewGuid() })
                    .Build()
            };

            var user = User.Builder()
                .SetId(userId)
                .SetEmail(email)
                .SetServiceActive(serviceActive)
                .SetAnnouncementPreferenceLimit(announcementPreferenceLimit)
                .SetAnnouncementSendingFrequency(announcementSendingFrequency)
                .SetPicture(picture)
                .SetFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences)
                .SetRoomForRentAnnouncementPreferences(roomForRentAnnouncementPreferences)
                .Build();

            user.Should().NotBeNull();
            user.Id.Should().Be(userId);
            user.Email.Should().Be(email);
            user.Picture.Should().Be(picture);
            user.ServiceActive.Should().Be(serviceActive);
            user.AnnouncementPreferenceLimit.Should().Be(announcementPreferenceLimit);
            user.AnnouncementSendingFrequency.Should().BeEquivalentTo(announcementSendingFrequency);
            user.FlatForRentAnnouncementPreferences.Should().BeEquivalentTo(flatForRentAnnouncementPreferences);
            user.RoomForRentAnnouncementPreferences.Should().BeEquivalentTo(roomForRentAnnouncementPreferences);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Should_Throw_UserEmailNullException_When_Email_Is_Null_Or_Whitespace(string email)
        {
            Action result = () =>
            {
                var unused = User.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail(email)
                    .SetServiceActive(DefaultUserSettings.ServiceActive)
                    .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                    .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                    .Build();
            };

            result.Should().ThrowExactly<UserEmailNullException>()
                .WithMessage("Email argument is required.");
        }

        [Fact]
        public void Should_Throw_UserEmailMaxLengthException_When_Email_Exceed_Allowed_Max_Length_Value()
        {
            var email = CreateString(257) + "@email.com";

            Action result = () =>
            {
                var unused = User.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail(email)
                    .SetServiceActive(DefaultUserSettings.ServiceActive)
                    .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                    .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                    .Build();
            };

            result.Should().ThrowExactly<UserEmailMaxLengthException>()
                .WithMessage("Email argument max length is 256.");
        }

        [Fact]
        public void Should_Throw_UserEmailFormatException_When_Email_Is_In_Incorrect_Format()
        {
            Action result = () =>
            {
                var unused = User.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email.com")
                    .SetServiceActive(DefaultUserSettings.ServiceActive)
                    .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                    .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                    .Build();
            };

            result.Should().ThrowExactly<UserEmailFormatException>()
                .WithMessage("Email argument is not in the form required for an e-mail address.");
        }

        [Fact]
        public void Should_Throw_UserAnnouncementPreferenceLimitMinValueException_When_AnnouncementPreferenceLimit_Is_Lower_Than_Allowed()
        {
            Action result = () =>
            {
                var unused = User.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email@email.com")
                    .SetServiceActive(DefaultUserSettings.ServiceActive)
                    .SetAnnouncementPreferenceLimit(0)
                    .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                    .Build();
            };

            result.Should().ThrowExactly<UserAnnouncementPreferenceLimitMinValueException>()
                .WithMessage("AnnouncementPreferenceLimit minimum value is 1.");
        }

        [Fact]
        public void Should_Throw_UserAnnouncementSendingFrequencyNullException_When_AnnouncementSendingFrequency_Is_Null()
        {
            Action result = () =>
            {
                var unused = User.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email@email.com")
                    .SetServiceActive(DefaultUserSettings.ServiceActive)
                    .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                    .SetAnnouncementSendingFrequency(null)
                    .Build();
            };

            result.Should().ThrowExactly<UserAnnouncementSendingFrequencyNullException>()
                .WithMessage("AnnouncementSendingFrequency argument is required.");
        }

        [Fact]
        public void Should_Throw_UserRoomForRentAnnouncementPreferencesNullException_When_RoomForRentAnnouncementPreferences_Is_Null()
        {
            Action result = () =>
            {
                var unused = User.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email@email.com")
                    .SetServiceActive(DefaultUserSettings.ServiceActive)
                    .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                    .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                    .SetRoomForRentAnnouncementPreferences(null)
                    .Build();
            };

            result.Should().ThrowExactly<UserRoomForRentAnnouncementPreferencesNullException>()
                .WithMessage("RoomForRentAnnouncementPreferences argument is required.");
        }

        [Fact]
        public void Should_Throw_UserRoomForRentAnnouncementPreferencesNullException_When_RoomForRentAnnouncementPreferences_Contains_Null()
        {
            Action result = () =>
            {
                var unused = User.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email@email.com")
                    .SetServiceActive(DefaultUserSettings.ServiceActive)
                    .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                    .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                    .SetRoomForRentAnnouncementPreferences(new List<RoomForRentAnnouncementPreference> { null })
                    .Build();
            };

            result.Should().ThrowExactly<UserRoomForRentAnnouncementPreferencesNullException>()
                .WithMessage("RoomForRentAnnouncementPreferences argument is required.");
        }

        [Fact]
        public void Should_Throw_UserRoomForRentAnnouncementPreferencesExceedAnnouncementPreferenceLimitException_When_Total_Number_Of_AnnouncementPreferences_Exceeded_AnnouncementPreferenceLimit()
        {
            var flatForRentAnnouncementPreferences = new List<FlatForRentAnnouncementPreference>
            {
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(2)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build(),
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(2)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build()
            };
            var roomForRentAnnouncementPreferences = new List<RoomForRentAnnouncementPreference>
            {
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build()
            };
            Action result = () =>
            {
                var unused = User.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email@email.com")
                    .SetServiceActive(DefaultUserSettings.ServiceActive)
                    .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                    .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                    .SetFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences)
                    .SetRoomForRentAnnouncementPreferences(roomForRentAnnouncementPreferences)
                    .Build();
            };

            result.Should().ThrowExactly<UserRoomForRentAnnouncementPreferencesExceedAnnouncementPreferenceLimitException>()
                .WithMessage("RoomForRentAnnouncementPreferences argument exceeded AnnouncementPreferenceLimit.");
        }

        [Fact]
        public void Should_Throw_UserRoomForRentAnnouncementPreferencesInvalidValueException_When_RoomForRentAnnouncementPreferences_Contains_Expansible_CityDistricts()
        {
            var cityId = Guid.NewGuid();
            const int priceMin = 0;
            const int priceMax = 2000;
            var roomType = RoomTypeEnumeration.Single;
            var roomForRentAnnouncementPreferences = new List<RoomForRentAnnouncementPreference>
            {
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(priceMin)
                    .SetPriceMax(priceMax)
                    .SetRoomType(roomType)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build(),
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(priceMin)
                    .SetPriceMax(priceMax)
                    .SetRoomType(roomType)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build()
            };
            Action result = () =>
            {
                var unused = User.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email@email.com")
                    .SetServiceActive(DefaultUserSettings.ServiceActive)
                    .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                    .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                    .SetRoomForRentAnnouncementPreferences(roomForRentAnnouncementPreferences)
                    .Build();
            };

            result.Should().ThrowExactly<UserRoomForRentAnnouncementPreferencesInvalidValueException>()
                .WithMessage($"{nameof(User.RoomForRentAnnouncementPreferences)} should be modified by CityDistricts extension.");
        }

        [Fact]
        public void Should_Throw_UserRoomForRentAnnouncementPreferencesInvalidValueException_When_RoomForRentAnnouncementPreferences_Contains_Changeable_Prices()
        {
            var cityId = Guid.NewGuid();
            const int priceMin = 0;
            var roomType = RoomTypeEnumeration.Single;
            var cityDistricts = new List<Guid> { Guid.NewGuid() };
            var roomForRentAnnouncementPreferences = new List<RoomForRentAnnouncementPreference>
            {
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(priceMin)
                    .SetPriceMax(1000)
                    .SetRoomType(roomType)
                    .SetCityDistricts(cityDistricts)
                    .Build(),
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(priceMin)
                    .SetPriceMax(2000)
                    .SetRoomType(roomType)
                    .SetCityDistricts(cityDistricts)
                    .Build()
            };
            Action result = () =>
            {
                var unused = User.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email@email.com")
                    .SetServiceActive(DefaultUserSettings.ServiceActive)
                    .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                    .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                    .SetRoomForRentAnnouncementPreferences(roomForRentAnnouncementPreferences)
                    .Build();
            };

            result.Should().ThrowExactly<UserRoomForRentAnnouncementPreferencesInvalidValueException>()
                .WithMessage($"{nameof(User.RoomForRentAnnouncementPreferences)} should be modified by Prices change.");
        }

        [Fact]
        public void Should_Throw_UserFlatForRentAnnouncementPreferencesNullException_When_FlatForRentAnnouncementPreferences_Is_Null()
        {
            Action result = () =>
            {
                var unused = User.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email@email.com")
                    .SetServiceActive(DefaultUserSettings.ServiceActive)
                    .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                    .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                    .SetFlatForRentAnnouncementPreferences(null)
                    .Build();
            };

            result.Should().ThrowExactly<UserFlatForRentAnnouncementPreferencesNullException>()
                .WithMessage("FlatForRentAnnouncementPreferences argument is required.");
        }

        [Fact]
        public void Should_Throw_UserFlatForRentAnnouncementPreferencesNullException_When_FlatForRentAnnouncementPreferences_Contains_Null()
        {
            Action result = () =>
            {
                var unused = User.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email@email.com")
                    .SetServiceActive(DefaultUserSettings.ServiceActive)
                    .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                    .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                    .SetFlatForRentAnnouncementPreferences(new List<FlatForRentAnnouncementPreference> { null })
                    .Build();
            };

            result.Should().ThrowExactly<UserFlatForRentAnnouncementPreferencesNullException>()
                .WithMessage("FlatForRentAnnouncementPreferences argument is required.");
        }

        [Fact]
        public void Should_Throw_UserFlatForRentAnnouncementPreferencesExceedAnnouncementPreferenceLimitException_When_Total_Number_Of_AnnouncementPreferences_Exceeded_AnnouncementPreferenceLimit()
        {
            var roomForRentAnnouncementPreferences = new List<RoomForRentAnnouncementPreference>
            {
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build(),
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build()
            }; 
            var flatForRentAnnouncementPreferences = new List<FlatForRentAnnouncementPreference>
            {
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(2)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build()
            };

            Action result = () =>
            {
                var unused = User.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email@email.com")
                    .SetServiceActive(DefaultUserSettings.ServiceActive)
                    .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                    .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                    .SetRoomForRentAnnouncementPreferences(roomForRentAnnouncementPreferences)
                    .SetFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences)
                    .Build();
            };

            result.Should().ThrowExactly<UserFlatForRentAnnouncementPreferencesExceedAnnouncementPreferenceLimitException>()
                .WithMessage("FlatForRentAnnouncementPreferences argument exceeded AnnouncementPreferenceLimit.");
        }

        [Fact]
        public void Should_Throw_UserFlatForRentAnnouncementPreferencesInvalidValueException_When_FlatForRentAnnouncementPreferences_Contains_Expansible_CityDistricts()
        {
            var cityId = Guid.NewGuid();
            const int priceMin = 0;
            const int priceMax = 2000;
            const int roomNumberMin = 1;
            const int roomNumberMax = 2;
            var flatForRentAnnouncementPreferences = new List<FlatForRentAnnouncementPreference>
            {
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(priceMin)
                    .SetPriceMax(priceMax)
                    .SetRoomNumbersMin(roomNumberMin)
                    .SetRoomNumbersMax(roomNumberMax)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build(),
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(priceMin)
                    .SetPriceMax(priceMax)
                    .SetRoomNumbersMin(roomNumberMin)
                    .SetRoomNumbersMax(roomNumberMax)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build()
            };
            Action result = () =>
            {
                var unused = User.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email@email.com")
                    .SetServiceActive(DefaultUserSettings.ServiceActive)
                    .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                    .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                    .SetFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences)
                    .Build();
            };

            result.Should().ThrowExactly<UserFlatForRentAnnouncementPreferencesInvalidValueException>()
                .WithMessage($"{nameof(User.FlatForRentAnnouncementPreferences)} should be modified by CityDistricts extension.");
        }

        [Fact]
        public void Should_Throw_UserFlatForRentAnnouncementPreferencesInvalidValueException_When_FlatForRentAnnouncementPreferences_Contains_Changeable_Prices()
        {
            var cityId = Guid.NewGuid();
            const int roomNumberMin = 1;
            const int roomNumberMax = 2;
            var cityDistricts = new List<Guid> { Guid.NewGuid() };
            var flatForRentAnnouncementPreferences = new List<FlatForRentAnnouncementPreference>
            {
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomNumbersMin(roomNumberMin)
                    .SetRoomNumbersMax(roomNumberMax)
                    .SetCityDistricts(cityDistricts)
                    .Build(),
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(500)
                    .SetPriceMax(2000)
                    .SetRoomNumbersMin(roomNumberMin)
                    .SetRoomNumbersMax(roomNumberMax)
                    .SetCityDistricts(cityDistricts)
                    .Build()
            };
            Action result = () =>
            {
                var unused = User.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email@email.com")
                    .SetServiceActive(DefaultUserSettings.ServiceActive)
                    .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                    .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                    .SetFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences)
                    .Build();
            };

            result.Should().ThrowExactly<UserFlatForRentAnnouncementPreferencesInvalidValueException>()
                .WithMessage($"{nameof(User.FlatForRentAnnouncementPreferences)} should be modified by Prices change.");
        }

        [Fact]
        public void Should_Throw_UserFlatForRentAnnouncementPreferencesInvalidValueException_When_FlatForRentAnnouncementPreferences_Contains_Changeable_RoomNumbers()
        {
            var cityId = Guid.NewGuid();
            const int priceMin = 0;
            const int priceMax = 2000;
            var cityDistricts = new List<Guid> { Guid.NewGuid() };
            var flatForRentAnnouncementPreferences = new List<FlatForRentAnnouncementPreference>
            {
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(priceMin)
                    .SetPriceMax(priceMax)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(cityDistricts)
                    .Build(),
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(priceMin)
                    .SetPriceMax(priceMax)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(2)
                    .SetCityDistricts(cityDistricts)
                    .Build()
            };
            Action result = () =>
            {
                var unused = User.Builder()
                    .SetId(Guid.NewGuid())
                    .SetEmail("email@email.com")
                    .SetServiceActive(DefaultUserSettings.ServiceActive)
                    .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                    .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                    .SetFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences)
                    .Build();
            };

            result.Should().ThrowExactly<UserFlatForRentAnnouncementPreferencesInvalidValueException>()
                .WithMessage($"{nameof(User.FlatForRentAnnouncementPreferences)} should be modified by RoomNumbers change.");
        }

        [Fact]
        public void AddCreatedEvent_Should_Add_UserCreatedDomainEvent()
        {
            var userId = Guid.NewGuid();
            const string email = "email@email.com";
            const bool serviceActive = DefaultUserSettings.ServiceActive;
            const int announcementPreferenceLimit = DefaultUserSettings.AnnouncementPreferenceLimit;
            var announcementSendingFrequency = DefaultUserSettings.AnnouncementSendingFrequency;
            var user = User.Builder()
                .SetId(userId)
                .SetEmail(email)
                .SetServiceActive(serviceActive)
                .SetAnnouncementPreferenceLimit(announcementPreferenceLimit)
                .SetAnnouncementSendingFrequency(announcementSendingFrequency)
                .Build();
            var correlationId = Guid.NewGuid();

            user.AddCreatedEvent(correlationId);

            var domainEvent = (UserCreatedDomainEvent)user.DomainEvents.First();
            domainEvent.AggregateId.Should().Be(userId);
            domainEvent.CorrelationId.Should().Be(correlationId);
            domainEvent.CreationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
            domainEvent.Email.Should().Be(email);
            domainEvent.ServiceActive.Should().Be(serviceActive);
            domainEvent.AnnouncementPreferenceLimit.Should().Be(announcementPreferenceLimit);
            domainEvent.AnnouncementSendingFrequency.Should().Be(announcementSendingFrequency);

        }

        [Fact]
        public void ChangeAnnouncementPreferenceLimit_Should_Change_AnnouncementPreferenceLimit_And_Add_UserAnnouncementPreferenceLimitChangedDomainEvent()
        {
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var correlationId = Guid.NewGuid();
            const int newAnnouncementPreferenceLimit = 10;

            user.ChangeAnnouncementPreferenceLimit(newAnnouncementPreferenceLimit, correlationId);

            user.AnnouncementPreferenceLimit.Should().Be(newAnnouncementPreferenceLimit);
            var domainEvent = (UserAnnouncementPreferenceLimitChangedDomainEvent)user.DomainEvents.First();
            domainEvent.AggregateId.Should().Be(user.Id);
            domainEvent.CorrelationId.Should().Be(correlationId);
            domainEvent.CreationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
            domainEvent.AnnouncementPreferenceLimit.Should().Be(newAnnouncementPreferenceLimit);
        }

        [Fact]
        public void ChangeAnnouncementPreferenceLimit_Should_Throw_UserAnnouncementPreferenceLimitMinValueException_When_AnnouncementPreferenceLimit_Is_Lower_Than_Allowed()
        {
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            const int newAnnouncementPreferenceLimit = 0;

            Action result = () => user.ChangeAnnouncementPreferenceLimit(newAnnouncementPreferenceLimit, Guid.NewGuid());

            result.Should().ThrowExactly<UserAnnouncementPreferenceLimitMinValueException>()
                .WithMessage("AnnouncementPreferenceLimit minimum value is 1.");
        }

        [Fact]
        public void ChangeAnnouncementSendingFrequency_Should_Change_AnnouncementSendingFrequency_And_Add_UserAnnouncementSendingFrequencyChangedDomainEvent()
        {
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var correlationId = Guid.NewGuid();
            var newAnnouncementSendingFrequency = AnnouncementSendingFrequencyEnumeration.EveryHour;

            user.ChangeAnnouncementSendingFrequency(newAnnouncementSendingFrequency, correlationId);

            user.AnnouncementSendingFrequency.Should().Be(newAnnouncementSendingFrequency);
            var domainEvent = (UserAnnouncementSendingFrequencyChangedDomainEvent)user.DomainEvents.First();
            domainEvent.AggregateId.Should().Be(user.Id);
            domainEvent.CorrelationId.Should().Be(correlationId);
            domainEvent.CreationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
            domainEvent.AnnouncementSendingFrequency.Should().Be(newAnnouncementSendingFrequency);
        }

        [Fact]
        public void ChangeAnnouncementSendingFrequency_Should_Throw_UserAnnouncementSendingFrequencyNullException_When_AnnouncementSendingFrequency_Is_Null()
        {
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();

            Action result = () => user.ChangeAnnouncementSendingFrequency(null, Guid.NewGuid());

            result.Should().ThrowExactly<UserAnnouncementSendingFrequencyNullException>()
                .WithMessage("AnnouncementSendingFrequency argument is required.");
        }

        [Fact]
        public void ChangeServiceActive_Should_Change_ServiceActive_And_Add_UserServiceActiveChangedDomainEvent()
        {
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var correlationId = Guid.NewGuid();
            const bool newServiceActive = false;

            user.ChangeServiceActive(newServiceActive, correlationId);

            user.ServiceActive.Should().Be(newServiceActive);
            var domainEvent = (UserServiceActiveChangedDomainEvent)user.DomainEvents.First();
            domainEvent.AggregateId.Should().Be(user.Id);
            domainEvent.CorrelationId.Should().Be(correlationId);
            domainEvent.CreationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
            domainEvent.ServiceActive.Should().Be(newServiceActive);
        }

        [Fact]
        public void ChangePicture_Should_Change_Picture_And_Add_UserPictureChangedDomainEvent()
        {
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var correlationId = Guid.NewGuid();
            const string newPicture = "picture";

            user.ChangePicture(newPicture, correlationId);

            user.Picture.Should().Be(newPicture);
            var domainEvent = (UserPictureChangedDomainEvent)user.DomainEvents.First();
            domainEvent.AggregateId.Should().Be(user.Id);
            domainEvent.CorrelationId.Should().Be(correlationId);
            domainEvent.CreationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
            domainEvent.Picture.Should().Be(newPicture);
        }

        [Fact]
        public void AddFlatForRentAnnouncementPreference_Should_Add_FlatForRentAnnouncementPreference_And_UserFlatForRentAnnouncementPreferenceAddedDomainEvent()
        {
            var cityId = Guid.NewGuid();
            var cityDistricts = new List<Guid> { Guid.NewGuid() };
            var flatForRentAnnouncementPreferences = new List<FlatForRentAnnouncementPreference>
            {
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(0)
                    .SetPriceMax(1500)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(cityDistricts)
                    .Build()
            };
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .SetFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences)
                .Build();
            var correlationId = Guid.NewGuid();
            var newFlatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(cityId)
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(2)
                .SetCityDistricts(cityDistricts)
                .Build();

            user.AddFlatForRentAnnouncementPreference(newFlatForRentAnnouncementPreference, correlationId);

            user.FlatForRentAnnouncementPreferences.Should().HaveCount(2);
            var addedFlatForRentAnnouncementPreference = user.FlatForRentAnnouncementPreferences.Last();
            addedFlatForRentAnnouncementPreference.Should().BeEquivalentTo(newFlatForRentAnnouncementPreference);
            var domainEvent = (UserFlatForRentAnnouncementPreferenceAddedDomainEvent)user.DomainEvents.First();
            domainEvent.AggregateId.Should().Be(user.Id);
            domainEvent.CorrelationId.Should().Be(correlationId);
            domainEvent.CreationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
            domainEvent.FlatForRentAnnouncementPreference.Should().Be(newFlatForRentAnnouncementPreference);
        }

        [Fact]
        public void AddFlatForRentAnnouncementPreference_Should_Throw_UserFlatForRentAnnouncementPreferencesExceedAnnouncementPreferenceLimitException_When_Total_Number_Of_AnnouncementPreferences_Exceeded_AnnouncementPreferenceLimit()
        {
            var roomForRentAnnouncementPreferences = new List<RoomForRentAnnouncementPreference>
            {
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build()
            };
            var flatForRentAnnouncementPreferences = new List<FlatForRentAnnouncementPreference>
            {
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(2)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build()
            };
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .SetRoomForRentAnnouncementPreferences(roomForRentAnnouncementPreferences)
                .SetFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences)
                .Build();
            var newFlatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(2)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => user.AddFlatForRentAnnouncementPreference(newFlatForRentAnnouncementPreference, Guid.NewGuid());

            result.Should().ThrowExactly<UserFlatForRentAnnouncementPreferencesExceedAnnouncementPreferenceLimitException>()
                .WithMessage("FlatForRentAnnouncementPreferences argument exceeded AnnouncementPreferenceLimit.");
        }

        [Fact]
        public void AddFlatForRentAnnouncementPreference_Should_Throw_UserFlatForRentAnnouncementPreferencesInvalidValueException_When_FlatForRentAnnouncementPreferences_Contains_Expansible_CityDistricts()
        {
            var cityId = Guid.NewGuid();
            const int priceMin = 0;
            const int priceMax = 2000;
            const int roomNumberMin = 1;
            const int roomNumberMax = 2;
            var flatForRentAnnouncementPreferences = new List<FlatForRentAnnouncementPreference>
            {
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(priceMin)
                    .SetPriceMax(priceMax)
                    .SetRoomNumbersMin(roomNumberMin)
                    .SetRoomNumbersMax(roomNumberMax)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build()
            };
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .SetFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences)
                .Build();
            var newFlatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(cityId)
                .SetPriceMin(priceMin)
                .SetPriceMax(priceMax)
                .SetRoomNumbersMin(roomNumberMin)
                .SetRoomNumbersMax(roomNumberMax)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => user.AddFlatForRentAnnouncementPreference(newFlatForRentAnnouncementPreference, Guid.NewGuid());

            result.Should().ThrowExactly<UserFlatForRentAnnouncementPreferencesInvalidValueException>()
                .WithMessage($"{nameof(User.FlatForRentAnnouncementPreferences)} should be modified by CityDistricts extension.");
        }

        [Fact]
        public void AddFlatForRentAnnouncementPreference_Should_Throw_UserFlatForRentAnnouncementPreferencesInvalidValueException_When_FlatForRentAnnouncementPreferences_Contains_Changeable_Prices()
        {
            var cityId = Guid.NewGuid();
            const int roomNumberMin = 1;
            const int roomNumberMax = 2;
            var cityDistricts = new List<Guid> { Guid.NewGuid() };
            var flatForRentAnnouncementPreferences = new List<FlatForRentAnnouncementPreference>
            {
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomNumbersMin(roomNumberMin)
                    .SetRoomNumbersMax(roomNumberMax)
                    .SetCityDistricts(cityDistricts)
                    .Build()
            };
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .SetFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences)
                .Build();
            var newFlatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(cityId)
                .SetPriceMin(500)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(roomNumberMin)
                .SetRoomNumbersMax(roomNumberMax)
                .SetCityDistricts(cityDistricts)
                .Build();

            Action result = () => user.AddFlatForRentAnnouncementPreference(newFlatForRentAnnouncementPreference, Guid.NewGuid());

            result.Should().ThrowExactly<UserFlatForRentAnnouncementPreferencesInvalidValueException>()
                .WithMessage($"{nameof(User.FlatForRentAnnouncementPreferences)} should be modified by Prices change.");
        }

        [Fact]
        public void AddFlatForRentAnnouncementPreference_Should_Throw_UserFlatForRentAnnouncementPreferencesInvalidValueException_When_FlatForRentAnnouncementPreferences_Contains_Changeable_RoomNumbers()
        {
            var cityId = Guid.NewGuid();
            const int priceMin = 0;
            const int priceMax = 2000;
            var cityDistricts = new List<Guid> { Guid.NewGuid() };
            var flatForRentAnnouncementPreferences = new List<FlatForRentAnnouncementPreference>
            {
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(priceMin)
                    .SetPriceMax(priceMax)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(cityDistricts)
                    .Build()
            };
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .SetFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences)
                .Build();
            var newFlatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(cityId)
                .SetPriceMin(priceMin)
                .SetPriceMax(priceMax)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(2)
                .SetCityDistricts(cityDistricts)
                .Build();

            Action result = () => user.AddFlatForRentAnnouncementPreference(newFlatForRentAnnouncementPreference, Guid.NewGuid());

            result.Should().ThrowExactly<UserFlatForRentAnnouncementPreferencesInvalidValueException>()
                .WithMessage($"{nameof(User.FlatForRentAnnouncementPreferences)} should be modified by RoomNumbers change.");
        }

        [Fact]
        public void AddFlatForRentAnnouncementPreferenceChangedEvent_Should_Add_UserFlatForRentAnnouncementPreferenceChangedDomainEvent()
        {
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(1000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid>{ Guid.NewGuid() })
                .Build();
            var correlationId = Guid.NewGuid();

            user.AddFlatForRentAnnouncementPreferenceChangedEvent(flatForRentAnnouncementPreference, correlationId);

            var domainEvent = (UserFlatForRentAnnouncementPreferenceChangedDomainEvent)user.DomainEvents.First();
            domainEvent.AggregateId.Should().Be(user.Id);
            domainEvent.CorrelationId.Should().Be(correlationId);
            domainEvent.CreationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
            domainEvent.FlatForRentAnnouncementPreference.Should().Be(flatForRentAnnouncementPreference);
        }

        [Fact]
        public void DeleteFlatForRentAnnouncementPreference_Should_Delete_FlatForRentAnnouncementPreference_And_Add_UserFlatForRentAnnouncementPreferenceDeletedDomainEvent()
        {
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(1500)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> {Guid.NewGuid()})
                .Build();
            var flatForRentAnnouncementPreferences = new List<FlatForRentAnnouncementPreference> { flatForRentAnnouncementPreference };
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .SetFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences)
                .Build();
            var correlationId = Guid.NewGuid();

            user.DeleteFlatForRentAnnouncementPreference(flatForRentAnnouncementPreference, correlationId);

            user.FlatForRentAnnouncementPreferences.Should().BeEmpty();
            var domainEvent = (UserFlatForRentAnnouncementPreferenceDeletedDomainEvent)user.DomainEvents.First();
            domainEvent.AggregateId.Should().Be(user.Id);
            domainEvent.CorrelationId.Should().Be(correlationId);
            domainEvent.CreationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
            domainEvent.FlatForRentAnnouncementPreference.Should().Be(flatForRentAnnouncementPreference);
        }

        [Fact]
        public void AddRoomForRentAnnouncementPreference_Should_Add_RoomForRentAnnouncementPreference_And_UserRoomForRentAnnouncementPreferenceAddedDomainEvent()
        {
            var cityId = Guid.NewGuid();
            const decimal roomForRentAnnouncementPreferencePriceMin = 0;
            const decimal roomForRentAnnouncementPreferencePriceMax = 2000;
            var cityDistricts = new List<Guid> { Guid.NewGuid() };
            var roomForRentAnnouncementPreferences = new List<RoomForRentAnnouncementPreference>
            {
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(roomForRentAnnouncementPreferencePriceMin)
                    .SetPriceMax(roomForRentAnnouncementPreferencePriceMax)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(cityDistricts)
                    .Build()
            };
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .SetRoomForRentAnnouncementPreferences(roomForRentAnnouncementPreferences)
                .Build();
            var correlationId = Guid.NewGuid();
            var newRoomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(cityId)
                .SetPriceMin(roomForRentAnnouncementPreferencePriceMin)
                .SetPriceMax(roomForRentAnnouncementPreferencePriceMax)
                .SetRoomType(RoomTypeEnumeration.Double)
                .SetCityDistricts(cityDistricts)
                .Build();

            user.AddRoomForRentAnnouncementPreference(newRoomForRentAnnouncementPreference, correlationId);

            user.RoomForRentAnnouncementPreferences.Should().HaveCount(2);
            var addedRoomForRentAnnouncementPreference = user.RoomForRentAnnouncementPreferences.Last();
            addedRoomForRentAnnouncementPreference.Should().BeEquivalentTo(newRoomForRentAnnouncementPreference);
            var domainEvent = (UserRoomForRentAnnouncementPreferenceAddedDomainEvent)user.DomainEvents.First();
            domainEvent.AggregateId.Should().Be(user.Id);
            domainEvent.CorrelationId.Should().Be(correlationId);
            domainEvent.CreationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
            domainEvent.RoomForRentAnnouncementPreference.Should().Be(newRoomForRentAnnouncementPreference);
        }

        [Fact]
        public void AddRoomForRentAnnouncementPreference_Should_Throw_UserRoomForRentAnnouncementPreferencesExceedAnnouncementPreferenceLimitException_When_Total_Number_Of_AnnouncementPreferences_Exceeded_AnnouncementPreferenceLimit()
        {
            var flatForRentAnnouncementPreferences = new List<FlatForRentAnnouncementPreference>
            {
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(2)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build()
            };
            var roomForRentAnnouncementPreferences = new List<RoomForRentAnnouncementPreference>
            {
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build()
            }; 
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .SetFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences)
                .SetRoomForRentAnnouncementPreferences(roomForRentAnnouncementPreferences)
                .Build();
            var newRoomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomType(RoomTypeEnumeration.Double)
                .SetCityDistricts(new List<Guid>{ Guid.NewGuid() })
                .Build();

            Action result = () => user.AddRoomForRentAnnouncementPreference(newRoomForRentAnnouncementPreference, Guid.NewGuid());

            result.Should().ThrowExactly<UserRoomForRentAnnouncementPreferencesExceedAnnouncementPreferenceLimitException>()
                .WithMessage("RoomForRentAnnouncementPreferences argument exceeded AnnouncementPreferenceLimit.");
        }

        [Fact]
        public void AddRoomForRentAnnouncementPreference_Should_Throw_UserRoomForRentAnnouncementPreferencesInvalidValueException_When_RoomForRentAnnouncementPreferences_Contains_Expansible_CityDistricts()
        {
            var cityId = Guid.NewGuid();
            const int priceMin = 0;
            const int priceMax = 2000;
            var roomType = RoomTypeEnumeration.Single;
            var roomForRentAnnouncementPreferences = new List<RoomForRentAnnouncementPreference>
            {
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(priceMin)
                    .SetPriceMax(priceMax)
                    .SetRoomType(roomType)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build()
            };
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .SetRoomForRentAnnouncementPreferences(roomForRentAnnouncementPreferences)
                .Build();
            var newRoomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(cityId)
                .SetPriceMin(priceMin)
                .SetPriceMax(priceMax)
                .SetRoomType(roomType)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => user.AddRoomForRentAnnouncementPreference(newRoomForRentAnnouncementPreference, Guid.NewGuid());

            result.Should().ThrowExactly<UserRoomForRentAnnouncementPreferencesInvalidValueException>()
                .WithMessage($"{nameof(User.RoomForRentAnnouncementPreferences)} should be modified by CityDistricts extension.");
        }

        [Fact]
        public void AddRoomForRentAnnouncementPreference_Should_Throw_UserRoomForRentAnnouncementPreferencesInvalidValueException_When_RoomForRentAnnouncementPreferences_Contains_Changeable_Prices()
        {
            var cityId = Guid.NewGuid();
            const int priceMin = 0;
            var roomType = RoomTypeEnumeration.Single;
            var cityDistricts = new List<Guid> { Guid.NewGuid() };
            var roomForRentAnnouncementPreferences = new List<RoomForRentAnnouncementPreference>
            {
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(priceMin)
                    .SetPriceMax(1000)
                    .SetRoomType(roomType)
                    .SetCityDistricts(cityDistricts)
                    .Build()
            };
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .SetRoomForRentAnnouncementPreferences(roomForRentAnnouncementPreferences)
                .Build();
            var newRoomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(cityId)
                .SetPriceMin(priceMin)
                .SetPriceMax(2000)
                .SetRoomType(roomType)
                .SetCityDistricts(cityDistricts)
                .Build();

            Action result = () => user.AddRoomForRentAnnouncementPreference(newRoomForRentAnnouncementPreference, Guid.NewGuid());

            result.Should().ThrowExactly<UserRoomForRentAnnouncementPreferencesInvalidValueException>()
                .WithMessage($"{nameof(User.RoomForRentAnnouncementPreferences)} should be modified by Prices change.");
        }

        [Fact]
        public void AddRoomForRentAnnouncementPreferenceChangedEvent_Should_Add_UserRoomForRentAnnouncementPreferenceChangedDomainEvent()
        {
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid>{ Guid.NewGuid() })
                .Build();
            var correlationId = Guid.NewGuid();

            user.AddRoomForRentAnnouncementPreferenceChangedEvent(roomForRentAnnouncementPreference, correlationId);

            var domainEvent = (UserRoomForRentAnnouncementPreferenceChangedDomainEvent)user.DomainEvents.First();
            domainEvent.AggregateId.Should().Be(user.Id);
            domainEvent.CorrelationId.Should().Be(correlationId);
            domainEvent.CreationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
            domainEvent.RoomForRentAnnouncementPreference.Should().Be(roomForRentAnnouncementPreference);
        }

        [Fact]
        public void DeleteRoomForRentAnnouncementPreference_Should_Delete_FlatForRentAnnouncementPreference_And_Add_UserRoomForRentAnnouncementPreferenceDeletedDomainEvent()
        {
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var roomForRentAnnouncementPreferences = new List<RoomForRentAnnouncementPreference> { roomForRentAnnouncementPreference };
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .SetRoomForRentAnnouncementPreferences(roomForRentAnnouncementPreferences)
                .Build();
            var correlationId = Guid.NewGuid();

            user.DeleteRoomForRentAnnouncementPreference(roomForRentAnnouncementPreference, correlationId);

            user.FlatForRentAnnouncementPreferences.Should().BeEmpty();
            var domainEvent = (UserRoomForRentAnnouncementPreferenceDeletedDomainEvent)user.DomainEvents.First();
            domainEvent.AggregateId.Should().Be(user.Id);
            domainEvent.CorrelationId.Should().Be(correlationId);
            domainEvent.CreationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
            domainEvent.RoomForRentAnnouncementPreference.Should().Be(roomForRentAnnouncementPreference);
        }

        [Fact]
        public void AddDeletedEvent_Should_Add_UserDeletedDomainEvent()
        {
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var correlationId = Guid.NewGuid();

            user.AddDeletedEvent(correlationId);

            var domainEvent = (UserDeletedDomainEvent)user.DomainEvents.First();
            domainEvent.AggregateId.Should().Be(user.Id);
            domainEvent.CorrelationId.Should().Be(correlationId);
            domainEvent.CreationDate.Should().BeCloseTo(DateTimeOffset.UtcNow);
        }

        [Fact]
        public void ApplyEvents_Should_Apply_All_Domain_Events_To_User()
        {
            var userId = Guid.NewGuid();
            const string email = "email@email.com";
            var cityId = Guid.NewGuid();
            var cityDistricts = new List<Guid>{ Guid.NewGuid() };
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(cityId)
                .SetPriceMin(0)
                .SetPriceMax(1500)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(cityDistricts)
                .Build();
            var flatForRentAnnouncementPreferences = new List<FlatForRentAnnouncementPreference> { flatForRentAnnouncementPreference };
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(cityId)
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(cityDistricts)
                .Build();
            var roomForRentAnnouncementPreferences = new List<RoomForRentAnnouncementPreference> { roomForRentAnnouncementPreference };
            var user = User.Builder()
                .SetId(userId)
                .SetEmail(email)
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .SetFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences)
                .SetRoomForRentAnnouncementPreferences(roomForRentAnnouncementPreferences)
                .Build();

            var correlationId = Guid.NewGuid();
            const bool newServiceActive = false;
            const string newPicture = "picture";
            const int newAnnouncementPreferenceLimit = 10;
            var newAnnouncementSendingFrequency = AnnouncementSendingFrequencyEnumeration.EveryHour;
            var newFlatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(2)
                .SetCityDistricts(new List<Guid>{ Guid.NewGuid() })
                .Build();
            var changedFlatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(newFlatForRentAnnouncementPreference.Id)
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(3000)
                .SetRoomNumbersMin(3)
                .SetRoomNumbersMax(3)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var newRoomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(1500)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid>{ Guid.NewGuid() })
                .Build();
            var changedRoomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(newRoomForRentAnnouncementPreference.Id)
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomType(RoomTypeEnumeration.Double)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            user.AddCreatedEvent(correlationId);
            user.ChangeAnnouncementPreferenceLimit(newAnnouncementPreferenceLimit, correlationId);
            user.ChangeAnnouncementSendingFrequency(newAnnouncementSendingFrequency, correlationId);
            user.ChangeServiceActive(newServiceActive, correlationId);
            user.ChangePicture(newPicture, correlationId);
            user.AddFlatForRentAnnouncementPreference(newFlatForRentAnnouncementPreference, correlationId);
            user.AddFlatForRentAnnouncementPreferenceChangedEvent(changedFlatForRentAnnouncementPreference, correlationId);
            user.DeleteFlatForRentAnnouncementPreference(flatForRentAnnouncementPreference, correlationId);
            user.AddRoomForRentAnnouncementPreference(newRoomForRentAnnouncementPreference, correlationId);
            user.AddRoomForRentAnnouncementPreferenceChangedEvent(changedRoomForRentAnnouncementPreference, correlationId);
            user.DeleteRoomForRentAnnouncementPreference(roomForRentAnnouncementPreference, correlationId);
            var userToApplyDomainEvents = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("anyEmail@email.com")
                .SetServiceActive(true)
                .SetAnnouncementPreferenceLimit(100)
                .SetAnnouncementSendingFrequency(AnnouncementSendingFrequencyEnumeration.EverySixHours)
                .Build();
            userToApplyDomainEvents.AddEvents(user.DomainEvents);

            userToApplyDomainEvents.ApplyEvents();

            user.Id.Should().Be(userId);
            user.Email.Should().Be(email);
            user.ServiceActive.Should().Be(newServiceActive);
            user.Picture.Should().Be(newPicture);
            user.AnnouncementPreferenceLimit.Should().Be(newAnnouncementPreferenceLimit);
            user.AnnouncementSendingFrequency.Should().BeEquivalentTo(newAnnouncementSendingFrequency);
            user.FlatForRentAnnouncementPreferences.Should().BeEquivalentTo(new List<FlatForRentAnnouncementPreference> { changedFlatForRentAnnouncementPreference });
            user.RoomForRentAnnouncementPreferences.Should().BeEquivalentTo(new List<RoomForRentAnnouncementPreference> { changedRoomForRentAnnouncementPreference });

        }

        private static string CreateString(int charNumber)
        {
            var secretHash = string.Empty;
            for (var i = 0; i < charNumber; i++)
            {
                secretHash += "a";
            }

            return secretHash;
        }
    }
}