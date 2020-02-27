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
using Riva.Identity.Domain.Accounts.Entities;
using Riva.Identity.Domain.Accounts.Enumerations;
using Riva.Identity.Domain.Accounts.Repositories;
using Xunit;

namespace Riva.Identity.Core.Test.CommandHandlerTests
{
    public class ConfirmAccountCommandHandlerTest
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<IAccountGetterService> _accountGetterServiceMock;
        private readonly Mock<IAccountVerificationService> _accountVerificationServiceMock;
        private readonly Mock<ICommunicationBus> _communicationBusMock;
        private readonly ICommandHandler<ConfirmAccountCommand> _commandHandler;

        public ConfirmAccountCommandHandlerTest()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _accountGetterServiceMock = new Mock<IAccountGetterService>();
            _accountVerificationServiceMock = new Mock<IAccountVerificationService>();
            _communicationBusMock = new Mock<ICommunicationBus>();
            _commandHandler = new ConfirmAccountCommandHandler(_accountRepositoryMock.Object,
                _accountGetterServiceMock.Object, _accountVerificationServiceMock.Object, _communicationBusMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Confirm_Account()
        {
            var confirmAccountCommand = new ConfirmAccountCommand("email@email.com", "123456");
            var token = Token.Builder()
                .SetIssued(DateTimeOffset.UtcNow)
                .SetExpires(DateTimeOffset.UtcNow.AddDays(1))
                .SetType(TokenTypeEnumeration.AccountConfirmation)
                .SetValue(confirmAccountCommand.Code)
                .Build();
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail(confirmAccountCommand.Email)
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .SetTokens(new List<Token> { token })
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);
            var emailIsNotConfirmedVerificationResult = VerificationResult.Ok();
            var tokenIsCorrectVerificationResult = VerificationResult.Ok();
            var cancellationToken = new CancellationToken();

            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(getAccountResult);
            _accountVerificationServiceMock.Setup(x => x.VerifyAccountIsNotConfirmed(It.IsAny<bool>()))
                .Returns(emailIsNotConfirmedVerificationResult);
            _accountVerificationServiceMock
                .Setup(x => x.VerifyConfirmationCode(It.IsAny<Token>(), It.IsAny<string>()))
                .Returns(tokenIsCorrectVerificationResult);
            _communicationBusMock.Setup(x => x.DispatchDomainEventsAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _accountRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Account>())).Returns(Task.CompletedTask);

            Func<Task> result = async () => await _commandHandler.HandleAsync(confirmAccountCommand, cancellationToken);

            await result.Should().NotThrowAsync<Exception>();
            account.Confirmed.Should().BeTrue();
            account.Tokens.Should().NotContain(token); 
            _communicationBusMock.Verify(
                x => x.DispatchDomainEventsAsync(It.Is<Account>(a => a == account),
                    It.Is<CancellationToken>(ct => ct == cancellationToken)), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Account_Is_Not_Found()
        {
            var confirmAccountCommand = new ConfirmAccountCommand("email@email.com", "123456");
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.NotFound, AccountErrorMessage.NotFound)
            };
            var getAccountResult = GetResult<Account>.Fail(errors);

            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(getAccountResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(confirmAccountCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Account_Is_Already_Confirmed()
        {
            var confirmAccountCommand = new ConfirmAccountCommand("email@email.com", "123456");
            var token = Token.Builder()
                .SetIssued(DateTimeOffset.UtcNow)
                .SetExpires(DateTimeOffset.UtcNow.AddDays(1))
                .SetType(TokenTypeEnumeration.AccountConfirmation)
                .SetValue(confirmAccountCommand.Code)
                .Build();
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail(confirmAccountCommand.Email)
                .SetConfirmed(true)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .SetTokens(new List<Token> { token })
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

            Func<Task> result = async () => await _commandHandler.HandleAsync(confirmAccountCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Token_Was_Not_Generated()
        {
            var confirmAccountCommand = new ConfirmAccountCommand("email@email.com", "123456");
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail(confirmAccountCommand.Email)
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);
            var emailIsNotConfirmedVerificationResult = VerificationResult.Ok();
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.ConfirmationCodeWasNotGenerated, AccountErrorMessage.ConfirmationCodeWasNotGenerated)
            };
            var tokenIsCorrectVerificationResult = VerificationResult.Fail(errors);

            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(getAccountResult);
            _accountVerificationServiceMock.Setup(x => x.VerifyAccountIsNotConfirmed(It.IsAny<bool>()))
                .Returns(emailIsNotConfirmedVerificationResult);
            _accountVerificationServiceMock
                .Setup(x => x.VerifyConfirmationCode(It.IsAny<Token>(), It.IsAny<string>()))
                .Returns(tokenIsCorrectVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(confirmAccountCommand);

            var exceptionResult = await result.Should().ThrowAsync<ValidationException>();
            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}