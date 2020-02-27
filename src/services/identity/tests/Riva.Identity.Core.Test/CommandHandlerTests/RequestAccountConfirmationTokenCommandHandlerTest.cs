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
using Riva.BuildingBlocks.Core.Models;
using Riva.Identity.Core.Commands;
using Riva.Identity.Core.Commands.Handlers;
using Riva.Identity.Core.Enumerations;
using Riva.Identity.Core.ErrorMessages;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Repositories;
using Xunit;

namespace Riva.Identity.Core.Test.CommandHandlerTests
{
    public class RequestAccountConfirmationTokenCommandHandlerTest
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<IAccountGetterService> _accountGetterServiceMock;
        private readonly Mock<IAccountVerificationService> _accountVerificationServiceMock;
        private readonly Mock<IAccountConfirmationRequestService> _accountConfirmationRequestServiceMock;
        private readonly Mock<ICommunicationBus> _communicationBusMock;
        private readonly ICommandHandler<RequestAccountConfirmationTokenCommand> _commandHandler;

        public RequestAccountConfirmationTokenCommandHandlerTest()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _accountGetterServiceMock = new Mock<IAccountGetterService>();
            _accountVerificationServiceMock = new Mock<IAccountVerificationService>();
            _accountConfirmationRequestServiceMock = new Mock<IAccountConfirmationRequestService>();
            _communicationBusMock = new Mock<ICommunicationBus>();
            _commandHandler = new RequestAccountConfirmationTokenCommandHandler(_accountRepositoryMock.Object,
                _accountGetterServiceMock.Object, _accountVerificationServiceMock.Object,
                _accountConfirmationRequestServiceMock.Object, _communicationBusMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Request_Account_Confirmation_Token()
        {
            var requestAccountConfirmationTokenCommand = new RequestAccountConfirmationTokenCommand("email@email.com");
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail(requestAccountConfirmationTokenCommand.Email)
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);
            var emailIsNotConfirmedVerificationResult = VerificationResult.Ok();
            var cancellationToken = new CancellationToken();

            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(getAccountResult);
            _accountVerificationServiceMock.Setup(x => x.VerifyAccountIsNotConfirmed(It.IsAny<bool>()))
                .Returns(emailIsNotConfirmedVerificationResult);
            _communicationBusMock.Setup(x => x.DispatchDomainEventsAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _accountRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Account>())).Returns(Task.CompletedTask);
            _accountConfirmationRequestServiceMock
                .Setup(x => x.PublishAccountConfirmationRequestedIntegrationEventAsync(It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<Guid>())).Returns(Task.CompletedTask)
                .Verifiable();

            Func<Task> result = async () => await _commandHandler.HandleAsync(requestAccountConfirmationTokenCommand, cancellationToken);

            await result.Should().NotThrowAsync<Exception>();
            _communicationBusMock.Verify(
                x => x.DispatchDomainEventsAsync(It.Is<Account>(a => a == account),
                    It.Is<CancellationToken>(ct => ct == cancellationToken)), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Account_Is_Not_Found()
        {
            var requestAccountConfirmationTokenCommand = new RequestAccountConfirmationTokenCommand("email@email.com");
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.NotFound, AccountErrorMessage.NotFound)
            };
            var getAccountResult = GetResult<Account>.Fail(errors);

            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(getAccountResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(requestAccountConfirmationTokenCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Account_Is_Already_Confirmed()
        {
            var requestAccountConfirmationTokenCommand = new RequestAccountConfirmationTokenCommand("email@email.com");
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail(requestAccountConfirmationTokenCommand.Email)
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.AlreadyConfirmed, AccountErrorMessage.AlreadyConfirmed)
            };
            var emailIsNotConfirmedVerificationResult = VerificationResult.Fail(errors);

            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(getAccountResult);
            _accountVerificationServiceMock.Setup(x => x.VerifyAccountIsNotConfirmed(It.IsAny<bool>()))
                .Returns(emailIsNotConfirmedVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(requestAccountConfirmationTokenCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}