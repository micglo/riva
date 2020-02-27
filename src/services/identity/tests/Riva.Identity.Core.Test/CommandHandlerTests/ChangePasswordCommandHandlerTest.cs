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
    public class ChangePasswordCommandHandlerTest
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<IAccountGetterService> _accountGetterServiceMock;
        private readonly Mock<IAccountVerificationService> _accountVerificationServiceMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly Mock<ICommunicationBus> _communicationBusMock;
        private readonly ICommandHandler<ChangePasswordCommand> _commandHandler;

        public ChangePasswordCommandHandlerTest()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _accountGetterServiceMock = new Mock<IAccountGetterService>();
            _accountVerificationServiceMock = new Mock<IAccountVerificationService>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _communicationBusMock = new Mock<ICommunicationBus>();
            _commandHandler = new ChangePasswordCommandHandler(_accountRepositoryMock.Object,
                _accountGetterServiceMock.Object, _accountVerificationServiceMock.Object, 
                _passwordServiceMock.Object, _communicationBusMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Change_Password()
        {
            var changePasswordCommand = new ChangePasswordCommand(Guid.NewGuid(), "OldPassword", "NewPassword");
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(true)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);
            var passwordIsSetVerificationResult = VerificationResult.Ok();
            var passwordIsCorrectVerificationResult = VerificationResult.Ok();
            const string newPasswordHash = "NewPasswordHash";
            var cancellationToken = new CancellationToken();

            _accountGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getAccountResult);
            _accountVerificationServiceMock.Setup(x => x.VerifyPasswordIsSet(It.IsAny<string>()))
                .Returns(passwordIsSetVerificationResult);
            _accountVerificationServiceMock
                .Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(passwordIsCorrectVerificationResult);
            _passwordServiceMock.Setup(x => x.HashPassword(It.IsAny<string>())).Returns(newPasswordHash);
            _communicationBusMock.Setup(x => x.DispatchDomainEventsAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
            _accountRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Account>())).Returns(Task.CompletedTask);

            Func<Task> result = async () => await _commandHandler.HandleAsync(changePasswordCommand, cancellationToken);

            await result.Should().NotThrowAsync<Exception>();
            account.PasswordHash.Should().Be(newPasswordHash);
            _communicationBusMock.Verify(
                x => x.DispatchDomainEventsAsync(It.Is<Account>(a => a == account),
                    It.Is<CancellationToken>(ct => ct == cancellationToken)), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_Account_Is_Not_Found()
        {
            var changePasswordCommand = new ChangePasswordCommand(Guid.NewGuid(), "OldPassword", "NewPassword");
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.NotFound, AccountErrorMessage.NotFound)
            };
            var getAccountResult = GetResult<Account>.Fail(errors);

            _accountGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getAccountResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(changePasswordCommand);
            var exceptionResult = await result.Should().ThrowAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Account_Has_No_Password()
        {
            var changePasswordCommand = new ChangePasswordCommand(Guid.NewGuid(), "OldPassword", "NewPassword");
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(true)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.PasswordIsNotSet, AccountErrorMessage.PasswordIsNotSet)
            };
            var passwordIsSetVerificationResult = VerificationResult.Fail(errors);

            _accountGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getAccountResult);
            _accountVerificationServiceMock.Setup(x => x.VerifyPasswordIsSet(It.IsAny<string>()))
                .Returns(passwordIsSetVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(changePasswordCommand);
            var exceptionResult = await result.Should().ThrowAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Old_Password_Is_Incorrect()
        {
            var changePasswordCommand = new ChangePasswordCommand(Guid.NewGuid(), "OldPassword", "NewPassword");
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(true)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);
            var passwordIsSetVerificationResult = VerificationResult.Ok();
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.IncorrectPassword, AccountErrorMessage.IncorrectPassword)
            };
            var passwordIsCorrectResult = VerificationResult.Fail(errors);

            _accountGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getAccountResult);
            _accountVerificationServiceMock.Setup(x => x.VerifyPasswordIsSet(It.IsAny<string>()))
                .Returns(passwordIsSetVerificationResult);
            _accountVerificationServiceMock
                .Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(passwordIsCorrectResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(changePasswordCommand);

            var exceptionResult = await result.Should().ThrowAsync<ValidationException>();
            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}