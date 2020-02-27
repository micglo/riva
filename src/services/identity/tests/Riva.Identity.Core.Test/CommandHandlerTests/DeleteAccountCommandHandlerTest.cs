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
using Riva.Identity.Core.Commands;
using Riva.Identity.Core.Commands.Handlers;
using Riva.Identity.Core.Enumerations;
using Riva.Identity.Core.ErrorMessages;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Aggregates;
using Xunit;

namespace Riva.Identity.Core.Test.CommandHandlerTests
{
    public class DeleteAccountCommandHandlerTest
    {
        private readonly Mock<IAccountGetterService> _accountGetterServiceMock;
        private readonly Mock<ICommunicationBus> _communicationBusMock;
        private readonly Mock<IIntegrationEventBus> _integrationEventBusMock;
        private readonly Mock<IAccountDataConsistencyService> _accountDataConsistencyServiceMock;
        private readonly ICommandHandler<DeleteAccountCommand> _commandHandler;

        public DeleteAccountCommandHandlerTest()
        {
            _accountGetterServiceMock = new Mock<IAccountGetterService>();
            _communicationBusMock = new Mock<ICommunicationBus>();
            _integrationEventBusMock = new Mock<IIntegrationEventBus>();
            _accountDataConsistencyServiceMock = new Mock<IAccountDataConsistencyService>();
            _commandHandler = new DeleteAccountCommandHandler(_accountGetterServiceMock.Object,
                _communicationBusMock.Object, _integrationEventBusMock.Object,
                _accountDataConsistencyServiceMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Delete_Account()
        {
            var deleteAccountCommand = new DeleteAccountCommand(Guid.NewGuid());
            var account = Account.Builder()
                .SetId(deleteAccountCommand.AccountId)
                .SetEmail("email@email.com")
                .SetConfirmed(true)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);
            var cancellationToken = new CancellationToken();

            _accountGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getAccountResult);
            _communicationBusMock.Setup(x => x.DispatchDomainEventsAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _accountDataConsistencyServiceMock.Setup(x => x.DeleteAccountWithRelatedPersistedGrants(It.IsAny<Account>())).Returns(Task.CompletedTask);
            _integrationEventBusMock.Setup(x => x.PublishIntegrationEventAsync(It.IsAny<IIntegrationEvent>())).Returns(Task.CompletedTask).Verifiable();

            Func<Task> result = async () => await _commandHandler.HandleAsync(deleteAccountCommand, cancellationToken);

            await result.Should().NotThrowAsync<Exception>();
            _communicationBusMock.Verify(
                x => x.DispatchDomainEventsAsync(It.Is<Account>(a => a == account),
                    It.Is<CancellationToken>(ct => ct == cancellationToken)), Times.Once);
            _integrationEventBusMock
                .Verify(x => x.PublishIntegrationEventAsync(It.Is<IIntegrationEvent>(ie => ie.CorrelationId == deleteAccountCommand.CorrelationId)));
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_Account_Is_Not_Found()
        {
            var deleteAccountCommand = new DeleteAccountCommand(Guid.NewGuid());
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.NotFound, AccountErrorMessage.NotFound)
            };
            var getAccountResult = GetResult<Account>.Fail(errors);

            _accountGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getAccountResult);

            Func<Task> result = async () => await _commandHandler.HandleAsync(deleteAccountCommand);
            var exceptionResult = await result.Should().ThrowAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}