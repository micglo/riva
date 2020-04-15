using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Models;
using Riva.Users.Core.Commands;
using Riva.Users.Core.Commands.Handlers;
using Riva.Users.Core.Enumerations;
using Riva.Users.Core.ErrorMessages;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Defaults;
using Riva.Users.Domain.Users.Repositories;
using Xunit;

namespace Riva.Users.Core.Test.CommandHandlerTests
{
    public class CreateUserCommandHandlerTest
    {
        private readonly Mock<IUserVerificationService> _userVerificationServiceMock;
        private readonly Mock<IAccountVerificationService> _accountVerificationServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICommunicationBus> _communicationBusMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly ICommandHandler<CreateUserCommand> _commandHandler;

        public CreateUserCommandHandlerTest()
        {
            _userVerificationServiceMock = new Mock<IUserVerificationService>();
            _accountVerificationServiceMock = new Mock<IAccountVerificationService>();
            _mapperMock = new Mock<IMapper>();
            _communicationBusMock = new Mock<ICommunicationBus>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _commandHandler = new CreateUserCommandHandler(_userVerificationServiceMock.Object,
                _accountVerificationServiceMock.Object, _mapperMock.Object, _communicationBusMock.Object,
                _userRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Create_User()
        {
            var command = new CreateUserCommand(Guid.NewGuid(), "email@email.com", DefaultUserSettings.ServiceActive,
                DefaultUserSettings.AnnouncementPreferenceLimit, DefaultUserSettings.AnnouncementSendingFrequency);
            var userDoesNotExistsVerificationResult = VerificationResult.Ok();
            var accountExistsVerificationResult = VerificationResult.Ok();
            var user = User.Builder()
                .SetId(command.UserId)
                .SetEmail(command.Email)
                .SetServiceActive(command.ServiceActive)
                .SetAnnouncementPreferenceLimit(command.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(command.AnnouncementSendingFrequency)
                .Build();

            _userVerificationServiceMock.Setup(x => x.VerifyUserDoesNotExistAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userDoesNotExistsVerificationResult);
            _accountVerificationServiceMock.Setup(x => x.VerifyAccountExistsAsync(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(accountExistsVerificationResult);
            _mapperMock.Setup(x => x.Map<CreateUserCommand, User>(It.IsAny<CreateUserCommand>())).Returns(user);
            _communicationBusMock
                .Setup(x => x.DispatchDomainEventsAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            await result.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ConflictException_When_User_Already_Exist()
        {
            var command = new CreateUserCommand(Guid.NewGuid(), "email@email.com", DefaultUserSettings.ServiceActive,
                DefaultUserSettings.AnnouncementPreferenceLimit, DefaultUserSettings.AnnouncementSendingFrequency); 
            var errors = new List<IError>
            {
                new Error(UserErrorCodeEnumeration.AlreadyExist, UserErrorMessage.AlreadyExist)
            };
            var userDoesNotExistsVerificationResult = VerificationResult.Fail(errors);

            _userVerificationServiceMock.Setup(x => x.VerifyUserDoesNotExistAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userDoesNotExistsVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ConflictException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Account_Does_Not_Exist()
        {
            var command = new CreateUserCommand(Guid.NewGuid(), "email@email.com", DefaultUserSettings.ServiceActive,
                DefaultUserSettings.AnnouncementPreferenceLimit, DefaultUserSettings.AnnouncementSendingFrequency);
            var userDoesNotExistsVerificationResult = VerificationResult.Ok();
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.NotFound, AccountErrorMessage.NotFound)
            };
            var accountExistsVerificationResult = VerificationResult.Fail(errors);

            _userVerificationServiceMock.Setup(x => x.VerifyUserDoesNotExistAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userDoesNotExistsVerificationResult);
            _accountVerificationServiceMock.Setup(x => x.VerifyAccountExistsAsync(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(accountExistsVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(command);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}