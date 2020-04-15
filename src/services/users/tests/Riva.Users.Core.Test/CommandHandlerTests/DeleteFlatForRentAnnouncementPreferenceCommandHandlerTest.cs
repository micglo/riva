using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Models;
using Riva.Users.Core.Commands;
using Riva.Users.Core.Commands.Handlers;
using Riva.Users.Core.Enumerations;
using Riva.Users.Core.ErrorMessages;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Defaults;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Domain.Users.Repositories;
using Xunit;

namespace Riva.Users.Core.Test.CommandHandlerTests
{
    public class DeleteFlatForRentAnnouncementPreferenceCommandHandlerTest
    {
        private readonly Mock<IUserGetterService> _userGetterServiceMock;
        private readonly Mock<IFlatForRentAnnouncementPreferenceGetterService> _flatForRentAnnouncementPreferenceGetterServiceMock;
        private readonly Mock<ICommunicationBus> _communicationBusMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IIntegrationEventBus> _integrationEventBusMock;
        private readonly ICommandHandler<DeleteFlatForRentAnnouncementPreferenceCommand> _commandHandler;

        public DeleteFlatForRentAnnouncementPreferenceCommandHandlerTest()
        {
            _userGetterServiceMock = new Mock<IUserGetterService>();
            _flatForRentAnnouncementPreferenceGetterServiceMock = new Mock<IFlatForRentAnnouncementPreferenceGetterService>();
            _communicationBusMock = new Mock<ICommunicationBus>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _integrationEventBusMock = new Mock<IIntegrationEventBus>();
            _commandHandler = new DeleteFlatForRentAnnouncementPreferenceCommandHandler(_userGetterServiceMock.Object,
                _flatForRentAnnouncementPreferenceGetterServiceMock.Object, _communicationBusMock.Object,
                _userRepositoryMock.Object, _integrationEventBusMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Delete_Flat_For_Rent_Announcement_Preference()
        {
            var command = new DeleteFlatForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid());
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user); 
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(command.FlatForRentAnnouncementPreferenceId)
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(1)
                .SetPriceMax(1000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid>{ Guid.NewGuid() })
                .Build();
            var getFlatForRentAnnouncementPreferenceResult =
                GetResult<FlatForRentAnnouncementPreference>.Ok(flatForRentAnnouncementPreference);

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _flatForRentAnnouncementPreferenceGetterServiceMock
                .Setup(x => x.GetByByUserAndId(It.IsAny<User>(), It.IsAny<Guid>()))
                .Returns(getFlatForRentAnnouncementPreferenceResult);
            _communicationBusMock
                .Setup(x => x.DispatchDomainEventsAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _integrationEventBusMock.Setup(x => x.PublishIntegrationEventAsync(It.IsAny<IIntegrationEvent>()))
                .Returns(Task.CompletedTask);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            await result.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_User_Is_Not_Found()
        {
            var command = new DeleteFlatForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid());
            var errors = new List<IError>
            {
                new Error(UserErrorCodeEnumeration.NotFound, UserErrorMessage.NotFound)
            };
            var getUserResult = GetResult<User>.Fail(errors);

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_Flat_For_Rent_Announcement_Preference_Is_Not_Found()
        {
            var command = new DeleteFlatForRentAnnouncementPreferenceCommand(Guid.NewGuid(), Guid.NewGuid());
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var errors = new Collection<IError>
            {
                new Error(FlatForRentAnnouncementPreferenceErrorCode.NotFound,
                    FlatForRentAnnouncementPreferenceErrorMessage.NotFound)
            };
            var getFlatForRentAnnouncementPreferenceResult = GetResult<FlatForRentAnnouncementPreference>.Fail(errors);

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _flatForRentAnnouncementPreferenceGetterServiceMock
                .Setup(x => x.GetByByUserAndId(It.IsAny<User>(), It.IsAny<Guid>()))
                .Returns(getFlatForRentAnnouncementPreferenceResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}