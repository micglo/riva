using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Core.Stores;
using Riva.BuildingBlocks.Domain;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Defaults;
using Riva.Users.Domain.Users.Enumerations;
using Riva.Users.Domain.Users.Events;
using Riva.Users.Domain.Users.Repositories;
using Riva.Users.Infrastructure.Services;
using Xunit;

namespace Riva.Users.Infrastructure.Test.ServiceTests
{
    public class UserRevertServiceTest
    {
        private readonly Mock<IDomainEventStore> _domainEventStoreMock;
        private readonly Mock<IUserGetterService> _userGetterServiceMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IUserRevertService _service;

        public UserRevertServiceTest()
        {
            _domainEventStoreMock = new Mock<IDomainEventStore>();
            _userGetterServiceMock = new Mock<IUserGetterService>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _service = new UserRevertService(_domainEventStoreMock.Object, _userGetterServiceMock.Object,
                _userRepositoryMock.Object);
        }

        [Fact]
        public async Task RevertUserAsync_Should_Revert_User_State()
        {
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetServiceActive(false)
                .SetAnnouncementPreferenceLimit(10)
                .SetAnnouncementSendingFrequency(AnnouncementSendingFrequencyEnumeration.EveryHour)
                .SetPicture("UrlToPicture")
                .Build();
            var userCreatedDomainEvent = new UserCreatedDomainEvent(user.Id, Guid.NewGuid(), user.Email,
                user.Picture, DefaultUserSettings.ServiceActive, DefaultUserSettings.AnnouncementPreferenceLimit,
                DefaultUserSettings.AnnouncementSendingFrequency);
            var userAnnouncementPreferenceLimitChangedDomainEvent =
                new UserAnnouncementPreferenceLimitChangedDomainEvent(user.Id, Guid.NewGuid(), 5);
            var domainEvents = new List<IDomainEvent>{ userCreatedDomainEvent, userAnnouncementPreferenceLimitChangedDomainEvent };
            var getUserResult = GetResult<User>.Ok(user);
            var expectedUser = User.Builder()
                .SetId(user.Id)
                .SetEmail(user.Email)
                .SetServiceActive(userCreatedDomainEvent.ServiceActive)
                .SetAnnouncementPreferenceLimit(userAnnouncementPreferenceLimitChangedDomainEvent
                    .AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(userCreatedDomainEvent.AnnouncementSendingFrequency)
                .SetPicture(userCreatedDomainEvent.Picture)
                .Build();

            _domainEventStoreMock.Setup(x => x.FindAllAsync(It.IsAny<Guid>())).ReturnsAsync(domainEvents);
            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getUserResult);
            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask).Verifiable();

            Func<Task> result = async () => await _service.RevertUserAsync(user.Id, Guid.NewGuid());

            await result.Should().NotThrowAsync<Exception>();
            _userRepositoryMock.Verify(x => x.UpdateAsync(It.Is<User>(u =>
                u.Id == expectedUser.Id && u.Email.Equals(expectedUser.Email) &&
                u.Picture == expectedUser.Picture &&
                u.ServiceActive == expectedUser.ServiceActive &&
                u.AnnouncementPreferenceLimit == expectedUser.AnnouncementPreferenceLimit &&
                Equals(u.AnnouncementSendingFrequency, expectedUser.AnnouncementSendingFrequency))));
        }
    }
}