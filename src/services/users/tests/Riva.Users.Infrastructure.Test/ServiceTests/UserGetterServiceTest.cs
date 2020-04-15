using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Models;
using Riva.Users.Core.Enumerations;
using Riva.Users.Core.ErrorMessages;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Defaults;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Domain.Users.Repositories;
using Riva.Users.Infrastructure.Services;
using Xunit;

namespace Riva.Users.Infrastructure.Test.ServiceTests
{
    public class UserGetterServiceTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IUserGetterService _service;

        public UserGetterServiceTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _service = new UserGetterService(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_GetResult_Ok_With_User()
        {
            var id = Guid.NewGuid();
            var flatForRentAnnouncementPreferences = new List<FlatForRentAnnouncementPreference>
            {
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(3000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(3)
                    .SetCityDistricts(new List<Guid> {Guid.NewGuid()})
                    .Build()
            };
            var user = User.Builder()
                .SetId(id)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .SetFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences)
                .Build();
            var expectedResult = GetResult<User>.Ok(user);

            _userRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(user);

            var result = await _service.GetByIdAsync(id);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_GetResult_Fail()
        {
            var id = Guid.NewGuid();
            var errors = new Collection<IError>
            {
                new Error(UserErrorCodeEnumeration.NotFound, UserErrorMessage.NotFound)
            };
            var expectedResult = GetResult<User>.Fail(errors);

            _userRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult<User>(null));

            var result = await _service.GetByIdAsync(id);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}