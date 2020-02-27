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
    public class AssignPasswordCommandHandlerTest
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<IAccountGetterService> _accountGetterServiceMock;
        private readonly Mock<IAccountVerificationService> _accountVerificationServiceMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly Mock<ICommunicationBus> _communicationBusMock;
        private readonly ICommandHandler<AssignPasswordCommand> _commandHandler;

        public AssignPasswordCommandHandlerTest()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _accountGetterServiceMock = new Mock<IAccountGetterService>();
            _accountVerificationServiceMock = new Mock<IAccountVerificationService>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _communicationBusMock = new Mock<ICommunicationBus>();
            _commandHandler = new AssignPasswordCommandHandler(_accountRepositoryMock.Object,
                _accountGetterServiceMock.Object, _accountVerificationServiceMock.Object, 
                _passwordServiceMock.Object, _communicationBusMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Set_Password()
        {
            var assignPasswordCommand = new AssignPasswordCommand(Guid.NewGuid(), "Password1234");
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(true)
                .SetPasswordHash(string.Empty)
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);
            var passwordIsNotSetVerificationResult = VerificationResult.Ok();
            const string passwordHash = "PasswordHash";
            var cancellationToken = new CancellationToken();

            _accountGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getAccountResult)
                .Verifiable();
            _accountVerificationServiceMock.Setup(x => x.VerifyPasswordIsNotSet(It.IsAny<string>()))
                .Returns(passwordIsNotSetVerificationResult)
                .Verifiable();
            _passwordServiceMock.Setup(x => x.HashPassword(It.IsAny<string>())).Returns(passwordHash).Verifiable();
            _communicationBusMock.Setup(x => x.DispatchDomainEventsAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
            _accountRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Account>())).Returns(Task.CompletedTask).Verifiable();

            Func<Task> result = async () => await _commandHandler.HandleAsync(assignPasswordCommand, cancellationToken);

            await result.Should().NotThrowAsync<Exception>();
            account.PasswordHash.Should().Be(passwordHash);
            _communicationBusMock.Verify(
                x => x.DispatchDomainEventsAsync(It.Is<Account>(a => a == account),
                    It.Is<CancellationToken>(ct => ct == cancellationToken)), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_Account_Is_Not_Found()
        {
            var assignPasswordCommand = new AssignPasswordCommand(Guid.NewGuid(), "Password1234");
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.NotFound, AccountErrorMessage.NotFound)
            };
            var getAccountResult = GetResult<Account>.Fail(errors);

            _accountGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getAccountResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(assignPasswordCommand);
            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ValidationException_When_Password_Is_Already_Set()
        {
            var assignPasswordCommand = new AssignPasswordCommand(Guid.NewGuid(), "Password1234");
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
                new Error(AccountErrorCodeEnumeration.PasswordAlreadySet, AccountErrorMessage.PasswordAlreadySet)
            };
            var passwordIsNotSetVerificationResult = VerificationResult.Fail(errors);

            _accountGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getAccountResult);
            _accountVerificationServiceMock.Setup(x => x.VerifyPasswordIsNotSet(It.IsAny<string>()))
                .Returns(passwordIsNotSetVerificationResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(assignPasswordCommand);
            var exceptionResult = await result.Should().ThrowAsync<ValidationException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}