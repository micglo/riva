using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.BuildingBlocks.Core.Enumerations;
using Riva.BuildingBlocks.Core.ErrorMessages;
using Riva.BuildingBlocks.Core.Logger;
using Riva.BuildingBlocks.Core.Models;
using Riva.Identity.Core.IntegrationEvents;
using Riva.Identity.Core.IntegrationEvents.Handlers;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Entities;
using Riva.Identity.Domain.Accounts.Enumerations;
using Xunit;

namespace Riva.Identity.Core.Test.IntegrationEventHandlerTests
{
    public class UserCreationCompletedIntegrationEventHandlerTest
    {
        private readonly Mock<IAccountGetterService> _accountGetterServiceMock;
        private readonly Mock<IAccountConfirmationRequestService> _accountConfirmationRequestServiceMock;
        private readonly Mock<IIntegrationEventBus> _integrationEventBusMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly IIntegrationEventHandler<UserCreationCompletedIntegrationEvent> _userCreatedIntegrationEventHandler;

        public UserCreationCompletedIntegrationEventHandlerTest()
        {
            _accountGetterServiceMock = new Mock<IAccountGetterService>();
            _accountConfirmationRequestServiceMock = new Mock<IAccountConfirmationRequestService>();
            _integrationEventBusMock = new Mock<IIntegrationEventBus>();
            _loggerMock = new Mock<ILogger>();
            _userCreatedIntegrationEventHandler = new UserCreationCompletedIntegrationEventHandler(_accountGetterServiceMock.Object,
                _accountConfirmationRequestServiceMock.Object, _integrationEventBusMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Complete_Account_Creation_With_Success_When_UserCreationCompletedIntegrationEvent_Is_Received_And_Account_Has_Not_Confirmation_Token()
        {
            var userCreationCompletedIntegrationEvent = new UserCreationCompletedIntegrationEvent(Guid.NewGuid(), DateTimeOffset.UtcNow, Guid.NewGuid());
            var account = Account.Builder()
                .SetId(userCreationCompletedIntegrationEvent.UserId)
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> {Guid.NewGuid()})
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);

            _accountGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getAccountResult);
            _integrationEventBusMock.Setup(x => x.PublishIntegrationEventAsync(It.IsAny<IIntegrationEvent>()))
                .Returns(Task.CompletedTask).Verifiable();


            Func<Task> result = async () => await _userCreatedIntegrationEventHandler.HandleAsync(userCreationCompletedIntegrationEvent);

            await result.Should().NotThrowAsync<Exception>();
            _integrationEventBusMock.Verify(x => x.PublishIntegrationEventAsync(It.Is<IIntegrationEvent>(ie =>
                IsPublishedIntegrationEventCorrect((AccountCreationCompletedIntegrationEvent) ie, userCreationCompletedIntegrationEvent.CorrelationId,
                    account.Id))));
        }

        [Fact]
        public async Task HandleAsync_Should_Complete_Account_Creation_With_Success_When_UserCreationCompletedIntegrationEvent_Is_Received_Account_Has_Confirmation_Token()
        {
            var userCreationCompletedIntegrationEvent = new UserCreationCompletedIntegrationEvent(Guid.NewGuid(), DateTimeOffset.UtcNow, Guid.NewGuid());
            var token = Token.Builder()
                .SetIssued(DateTimeOffset.UtcNow.AddHours(-1))
                .SetExpires(DateTimeOffset.UtcNow.AddDays(1))
                .SetType(TokenTypeEnumeration.AccountConfirmation)
                .SetValue("12345")
                .Build();
            var account = Account.Builder()
                .SetId(userCreationCompletedIntegrationEvent.UserId)
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .SetTokens(new[] { token })
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);

            _accountGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getAccountResult);
            _integrationEventBusMock.Setup(x => x.PublishIntegrationEventAsync(It.IsAny<IIntegrationEvent>()))
                .Returns(Task.CompletedTask).Verifiable();
            _accountConfirmationRequestServiceMock
                .Setup(x => x.PublishAccountConfirmationRequestedIntegrationEventAsync(It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(Task.CompletedTask)
                .Verifiable();


            Func<Task> result = async () => await _userCreatedIntegrationEventHandler.HandleAsync(userCreationCompletedIntegrationEvent);

            await result.Should().NotThrowAsync<Exception>();
            _integrationEventBusMock.Verify(x => x.PublishIntegrationEventAsync(It.Is<IIntegrationEvent>(ie =>
                IsPublishedIntegrationEventCorrect((AccountCreationCompletedIntegrationEvent)ie, userCreationCompletedIntegrationEvent.CorrelationId,
                    account.Id))));
            _accountConfirmationRequestServiceMock.Verify(x =>
                x.PublishAccountConfirmationRequestedIntegrationEventAsync(It.Is<string>(e => e == account.Email),
                    It.Is<string>(t => t == token.Value),
                    It.Is<Guid>(c => c == userCreationCompletedIntegrationEvent.CorrelationId)));
        }

        [Fact]
        public async Task HandleAsync_Should_Complete_Account_Creation_With_Failure_When_UserCreationCompletedIntegrationEvent_Is_Received_And_Getting_Account_Throws_Any_Exception()
        {
            var userCreationCompletedIntegrationEvent = new UserCreationCompletedIntegrationEvent(Guid.NewGuid(), DateTimeOffset.UtcNow, Guid.NewGuid());
            var exception = new Exception("Exception occured.");
            const string logMessage = "accountId={accountId}, message={message}, stackTrace={stackTrace}";

            _accountGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ThrowsAsync(exception);
            _loggerMock.Setup(x => x.LogIntegrationEventError(It.IsAny<ServiceComponentEnumeration>(),
                It.IsAny<IIntegrationEvent>(), It.IsAny<string>(), It.IsAny<object[]>()))
                .Verifiable();
            _integrationEventBusMock.Setup(x => x.PublishIntegrationEventAsync(It.IsAny<IIntegrationEvent>()))
                .Returns(Task.CompletedTask)
                .Verifiable();


            Func<Task> result = async () => await _userCreatedIntegrationEventHandler.HandleAsync(userCreationCompletedIntegrationEvent);

            await result.Should().NotThrowAsync<Exception>();
            _loggerMock.Verify(x => x.LogIntegrationEventError(
                It.Is<ServiceComponentEnumeration>(s => Equals(s, ServiceComponentEnumeration.RivaIdentity)),
                It.Is<IIntegrationEvent>(ie => ie == userCreationCompletedIntegrationEvent),
                It.Is<string>(m => m.Equals(logMessage)), It.IsAny<object[]>()), Times.Once);
            _integrationEventBusMock.Verify(x => x.PublishIntegrationEventAsync(It.Is<IIntegrationEvent>(ie =>
                IsPublishedIntegrationEventCorrect((AccountCreationCompletedIntegrationEventFailure) ie,
                    userCreationCompletedIntegrationEvent.CorrelationId, userCreationCompletedIntegrationEvent.UserId,
                    IntegrationEventErrorCodeEnumeration.UnexpectedError.DisplayName,
                    IntegrationEventErrorMessage.UnexpectedError))));
        }

        private static bool IsPublishedIntegrationEventCorrect(AccountCreationCompletedIntegrationEvent integrationEvent, Guid correlationId, Guid accountId)
        {
            return integrationEvent.CorrelationId == correlationId && integrationEvent.AccountId == accountId;
        }

        private static bool IsPublishedIntegrationEventCorrect(AccountCreationCompletedIntegrationEventFailure integrationEvent, Guid correlationId, Guid accountId, 
            string code, string reason)
        {
            return integrationEvent.CorrelationId == correlationId && integrationEvent.AccountId == accountId &&
                   integrationEvent.Code.Equals(code) && integrationEvent.Reason.Equals(reason);
        }
    }
}