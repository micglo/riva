using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Riva.BuildingBlocks.Core.Models;
using Riva.Users.Core.Enumerations;
using Riva.Users.Core.ErrorMessages;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Defaults;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Domain.Users.Enumerations;
using Riva.Users.Infrastructure.Services;
using Xunit;

namespace Riva.Users.Infrastructure.Test.ServiceTests
{
    public class RoomForRentAnnouncementPreferenceGetterServiceTest
    {
        private readonly IRoomForRentAnnouncementPreferenceGetterService _service;

        public RoomForRentAnnouncementPreferenceGetterServiceTest()
        {
            _service = new RoomForRentAnnouncementPreferenceGetterService();
        }

        [Fact]
        public void GetByByUserAndId_Should_Return_GetResult_Ok_With_RoomForRentAnnouncementPreference()
        {
            var id = Guid.NewGuid();
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(id)
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(1500)
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
            var expectedResult = GetResult<RoomForRentAnnouncementPreference>.Ok(roomForRentAnnouncementPreference);

            var result = _service.GetByByUserAndId(user, id);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void GetByByUserAndId_Should_Return_GetResult_Fail()
        {
            var id = Guid.NewGuid();
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var errors = new Collection<IError>
            {
                new Error(RoomForRentAnnouncementPreferenceErrorCode.NotFound, RoomForRentAnnouncementPreferenceErrorMessage.NotFound)
            };
            var expectedResult = GetResult<User>.Fail(errors);

            var result = _service.GetByByUserAndId(user, id);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}