using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.BuildingBlocks.Core.Enumerations;
using Riva.BuildingBlocks.Core.Logger;
using Riva.BuildingBlocks.Core.Models;
using Riva.Identity.Core.IntegrationEvents;
using Riva.Identity.Core.IntegrationEvents.Handlers;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Aggregates;
using Xunit;

namespace Riva.Identity.Core.Test.IntegrationEventHandlerTests
{
    public class UserCreationCompletedIntegrationEventFailureHandlerTest
    {
        private readonly Mock<IAccountGetterService> _accountGetterServiceMock;
        private readonly Mock<IAccountDataConsistencyService> _accountDataConsistencyServiceMock;
        private readonly Mock<IIntegrationEventBus> _integrationEventBusMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly IIntegrationEventHandler<UserCreationCompletedIntegrationEventFailure> _userCreatedIntegrationEventHandlerFailure;

        public UserCreationCompletedIntegrationEventFailureHandlerTest()
        {
            _accountGetterServiceMock = new Mock<IAccountGetterService>();
            _accountDataConsistencyServiceMock = new Mock<IAccountDataConsistencyService>();
            _integrationEventBusMock = new Mock<IIntegrationEventBus>();
            _loggerMock = new Mock<ILogger>();
            _userCreatedIntegrationEventHandlerFailure = new UserCreationCompletedIntegrationEventFailureHandler(_accountGetterServiceMock.Object,
                _accountDataConsistencyServiceMock.Object, _integrationEventBusMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Complete_Account_Creation_With_Failure_When_UserCreationCompletedIntegrationEventFailure_Is_Received()
        {
            var userCreationCompletedIntegrationEventFailure = new UserCreationCompletedIntegrationEventFailure(Guid.NewGuid(),
                DateTimeOffset.UtcNow, "AnyCode", "AnyReason", Guid.NewGuid());
            var account = Account.Builder()
                .SetId(userCreationCompletedIntegrationEventFailure.UserId)
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);
            var message = $"Could not finish {nameof(Account)} creation process.";
            const string logMessage = "accountId={accountId}, message={message}, reason={reason}, code={code}";
            var logParams = new object[]
            {
                userCreationCompletedIntegrationEventFailure.UserId,
                message,
                userCreationCompletedIntegrationEventFailure.Reason,
                userCreationCompletedIntegrationEventFailure.Code
            };

            _loggerMock.Setup(x => x.LogIntegrationEventError(It.IsAny<ServiceComponentEnumeration>(),
                It.IsAny<IIntegrationEvent>(), It.IsAny<string>(), It.IsAny<object[]>()))
                .Verifiable();
            _integrationEventBusMock.Setup(x => x.PublishIntegrationEventAsync(It.IsAny<IIntegrationEvent>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
            _accountGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getAccountResult);
            _accountDataConsistencyServiceMock.Setup(x => x.DeleteAccountWithRelatedPersistedGrants(It.IsAny<Account>()))
                .Returns(Task.CompletedTask);


            Func<Task> result = async () => await _userCreatedIntegrationEventHandlerFailure.HandleAsync(userCreationCompletedIntegrationEventFailure);

            await result.Should().NotThrowAsync<Exception>();
            _loggerMock.Verify(x => x.LogIntegrationEventError(
                It.Is<ServiceComponentEnumeration>(s => Equals(s, ServiceComponentEnumeration.RivaIdentity)),
                It.Is<IIntegrationEvent>(ie => ie == userCreationCompletedIntegrationEventFailure),
                It.Is<string>(m => m.Equals(logMessage)), It.Is<object[]>(p => !p.Except(logParams).Any())));
            _integrationEventBusMock.Verify(x => x.PublishIntegrationEventAsync(It.Is<IIntegrationEvent>(ie =>
                IsPublishedIntegrationEventCorrect((AccountCreationCompletedIntegrationEventFailure)ie,
                    userCreationCompletedIntegrationEventFailure.CorrelationId, userCreationCompletedIntegrationEventFailure.UserId,
                    userCreationCompletedIntegrationEventFailure.Code, userCreationCompletedIntegrationEventFailure.Reason))));
        }

        [Fact]
        public async Task HandleAsync_Should_Complete_Account_Creation_With_Failure_When_UserCreationCompletedIntegrationEventFailure_Is_Received_And_Getting_Or_Deleting_Account_Throws_Any_Exception()
        {
            var userCreationCompletedIntegrationEventFailure = new UserCreationCompletedIntegrationEventFailure(Guid.NewGuid(),
                DateTimeOffset.UtcNow, "AnyCode", "AnyReason", Guid.NewGuid());
            var message = $"Could not finish {nameof(Account)} creation process.";
            const string logMessage = "accountId={accountId}, message={message}, reason={reason}, code={code}";
            var logParams = new object[]
            {
                userCreationCompletedIntegrationEventFailure.UserId,
                message,
                userCreationCompletedIntegrationEventFailure.Reason,
                userCreationCompletedIntegrationEventFailure.Code
            };
            var exception = new Exception("Exception occured.");
            const string getAccountLogMessage = "accountId={accountId}, message={message}, stackTrace={stackTrace}";

            _loggerMock.Setup(x => x.LogIntegrationEventError(It.IsAny<ServiceComponentEnumeration>(),
                It.IsAny<IIntegrationEvent>(), It.IsAny<string>(), It.IsAny<object[]>()))
                .Verifiable();
            _integrationEventBusMock.Setup(x => x.PublishIntegrationEventAsync(It.IsAny<IIntegrationEvent>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
            _accountGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(exception);


            Func<Task> result = async () => await _userCreatedIntegrationEventHandlerFailure.HandleAsync(userCreationCompletedIntegrationEventFailure);

            await result.Should().NotThrowAsync<Exception>();
            _loggerMock.Verify(x => x.LogIntegrationEventError(
                It.Is<ServiceComponentEnumeration>(s => Equals(s, ServiceComponentEnumeration.RivaIdentity)),
                It.Is<IIntegrationEvent>(ie => ie == userCreationCompletedIntegrationEventFailure),
                It.Is<string>(m => m.Equals(logMessage)), It.Is<object[]>(p => !p.Except(logParams).Any())));
            _integrationEventBusMock.Verify(x => x.PublishIntegrationEventAsync(It.Is<IIntegrationEvent>(ie =>
                IsPublishedIntegrationEventCorrect((AccountCreationCompletedIntegrationEventFailure)ie,
                    userCreationCompletedIntegrationEventFailure.CorrelationId, userCreationCompletedIntegrationEventFailure.UserId,
                    userCreationCompletedIntegrationEventFailure.Code, userCreationCompletedIntegrationEventFailure.Reason))));
            _loggerMock.Verify(
                x => x.LogIntegrationEventError(
                    It.Is<ServiceComponentEnumeration>(s => Equals(s, ServiceComponentEnumeration.RivaIdentity)),
                    It.Is<IIntegrationEvent>(ie => ie == userCreationCompletedIntegrationEventFailure),
                    It.Is<string>(m => m.Equals(getAccountLogMessage)),
                    It.IsAny<object[]>()));
        }

        private static bool IsPublishedIntegrationEventCorrect(AccountCreationCompletedIntegrationEventFailure integrationEvent, Guid correlationId, Guid accountId,
            string code, string reason)
        {
            return integrationEvent.CorrelationId == correlationId && integrationEvent.AccountId == accountId &&
                   integrationEvent.Code.Equals(code) && integrationEvent.Reason.Equals(reason);
        }
    }
}