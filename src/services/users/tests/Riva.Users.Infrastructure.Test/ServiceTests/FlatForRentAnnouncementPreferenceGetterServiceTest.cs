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
using Riva.Users.Infrastructure.Services;
using Xunit;

namespace Riva.Users.Infrastructure.Test.ServiceTests
{
    public class FlatForRentAnnouncementPreferenceGetterServiceTest
    {
        private readonly IFlatForRentAnnouncementPreferenceGetterService _service;

        public FlatForRentAnnouncementPreferenceGetterServiceTest()
        {
            _service = new FlatForRentAnnouncementPreferenceGetterService();
        }

        [Fact]
        public void GetByByUserAndId_Should_Return_GetResult_Ok_With_FlatForRentAnnouncementPreference()
        {
            var id = Guid.NewGuid();
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(id)
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(3000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(3)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
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
            var expectedResult = GetResult<FlatForRentAnnouncementPreference>.Ok(flatForRentAnnouncementPreference);

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
                new Error(FlatForRentAnnouncementPreferenceErrorCode.NotFound, FlatForRentAnnouncementPreferenceErrorMessage.NotFound)
            };
            var expectedResult = GetResult<User>.Fail(errors);

            var result = _service.GetByByUserAndId(user, id);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}