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
using Riva.Users.Core.Models;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Defaults;
using Riva.Users.Domain.Users.Enumerations;
using Riva.Users.Domain.Users.Repositories;
using Xunit;

namespace Riva.Users.Core.Test.CommandHandlerTests
{
    public class UpdateUserCommandHandlerTest
    {
        private readonly Mock<IUserGetterService> _userGetterServiceMock;
        private readonly Mock<IUserVerificationService> _userVerificationServiceMock;
        private readonly Mock<ICommunicationBus> _communicationBusMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IIntegrationEventBus> _integrationEventBusMock;
        private readonly Mock<IBlobContainerService> _blobContainerServiceMock;
        private readonly ICommandHandler<UpdateUserCommand> _commandHandler;

        public UpdateUserCommandHandlerTest()
        {
            _userGetterServiceMock = new Mock<IUserGetterService>();
            _userVerificationServiceMock = new Mock<IUserVerificationService>();
            _communicationBusMock = new Mock<ICommunicationBus>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _integrationEventBusMock = new Mock<IIntegrationEventBus>();
            _blobContainerServiceMock = new Mock<IBlobContainerService>();
            _commandHandler = new UpdateUserCommandHandler(_userGetterServiceMock.Object,
                _userVerificationServiceMock.Object, _communicationBusMock.Object, _userRepositoryMock.Object,
                _integrationEventBusMock.Object, _blobContainerServiceMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Update_User_When_Picture_Is_Null()
        {
            var command = new UpdateUserCommand(Guid.NewGuid(), DefaultUserSettings.ServiceActive,
                DefaultUserSettings.AnnouncementPreferenceLimit, DefaultUserSettings.AnnouncementSendingFrequency,
                null);
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(command.ServiceActive)
                .SetAnnouncementPreferenceLimit(command.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(command.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
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
        public async Task HandleAsync_Should_Update_User_When_Picture_Is_Not_Null()
        {
            var pictureDto = new PictureDto(Array.Empty<byte>(), "image/jpg");
            var command = new UpdateUserCommand(Guid.NewGuid(), DefaultUserSettings.ServiceActive,
                DefaultUserSettings.AnnouncementPreferenceLimit, DefaultUserSettings.AnnouncementSendingFrequency,
                pictureDto);
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(command.ServiceActive)
                .SetAnnouncementPreferenceLimit(command.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(command.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            const string pictureUrl = "pictureUrl";

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _blobContainerServiceMock
                .Setup(x => x.UploadFileAsync(It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(pictureUrl);
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
            var command = new UpdateUserCommand(Guid.NewGuid(), DefaultUserSettings.ServiceActive,
                DefaultUserSettings.AnnouncementPreferenceLimit, DefaultUserSettings.AnnouncementSendingFrequency,
                null);
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
        public async Task HandleAsync_Should_Throw_ValidationException_When_User_Is_Not_Allowed_To_Change_Announcement_Preference_Limit()
        {
            var command = new UpdateUserCommand(Guid.NewGuid(), DefaultUserSettings.ServiceActive,
                10, DefaultUserSettings.AnnouncementSendingFrequency,
                null);
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(command.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(command.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var errors = new Collection<IError>
            {
                new Error(UserErrorCodeEnumeration.InsufficientPrivileges,
                    UserErrorMessage.InsufficientPrivilegesToEditAnnouncementPreferenceLimit)
            };
            var announcementPreferenceLimitCanBeChangedVerificationResult = VerificationResult.Fail(errors);

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _userVerificationServiceMock.Setup(x => x.VerifyAnnouncementPreferenceLimitCanBeChanged())
                .Returns(announcementPreferenceLimitCanBeChangedVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_User_Is_Not_Allowed_To_Change_Announcement_Sending_Frequency()
        {
            var command = new UpdateUserCommand(Guid.NewGuid(), DefaultUserSettings.ServiceActive,
                DefaultUserSettings.AnnouncementPreferenceLimit, AnnouncementSendingFrequencyEnumeration.EveryHour, 
                null);
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail("email@email.com")
                .SetServiceActive(command.ServiceActive)
                .SetAnnouncementPreferenceLimit(command.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var getUserResult = GetResult<User>.Ok(user);
            var errors = new Collection<IError>
            {
                new Error(UserErrorCodeEnumeration.InsufficientPrivileges,
                    UserErrorMessage.InsufficientPrivilegesToEditAnnouncementSendingFrequency)
            };
            var announcementSendingFrequencyCanBeChangedVerificationResult = VerificationResult.Fail(errors);

            _userGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getUserResult);
            _userVerificationServiceMock.Setup(x => x.VerifyAnnouncementSendingFrequencyCanBeChanged())
                .Returns(announcementSendingFrequencyCanBeChangedVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}