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
using Riva.BuildingBlocks.Core.Stores;
using Riva.BuildingBlocks.Domain;
using Riva.Identity.Core.IntegrationEvents;
using Riva.Identity.Core.IntegrationEvents.Handlers;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Events;
using Riva.Identity.Domain.Accounts.Repositories;
using Xunit;

namespace Riva.Identity.Core.Test.IntegrationEventHandlerTests
{
    public class UserDeletionCompletedIntegrationEventFailureHandlerTest
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<IIntegrationEventBus> _integrationEventBusMock;
        private readonly Mock<IDomainEventStore> _domainEventStoreMock;
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly IIntegrationEventHandler<UserDeletionCompletedIntegrationEventFailure> _userDeletionCompletedIntegrationEventFailure;

        public UserDeletionCompletedIntegrationEventFailureHandlerTest()
        {
            _loggerMock = new Mock<ILogger>();
            _integrationEventBusMock = new Mock<IIntegrationEventBus>();
            _domainEventStoreMock = new Mock<IDomainEventStore>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _userDeletionCompletedIntegrationEventFailure = new UserDeletionCompletedIntegrationEventFailureHandler(
                _loggerMock.Object, _integrationEventBusMock.Object, _domainEventStoreMock.Object,
                _accountRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Complete_Account_Deletion_With_Failure_When_UserDeletionCompletedIntegrationEventFailure_Is_Received()
        {
            var userDeletionCompletedIntegrationEventFailure = new UserDeletionCompletedIntegrationEventFailure(Guid.NewGuid(),
                DateTimeOffset.UtcNow, "AnyCode", "AnyReason", Guid.NewGuid());
            var message = $"Could not finish {nameof(Account)} deletion process.";
            const string logMessage = "accountId={accountId}, message={message}, reason={reason}, code={code}";
            var logParams = new object[]
            {
                userDeletionCompletedIntegrationEventFailure.UserId,
                message,
                userDeletionCompletedIntegrationEventFailure.Reason,
                userDeletionCompletedIntegrationEventFailure.Code
            };
            var accountCreatedDomainEvent = new AccountCreatedDomainEvent(
                userDeletionCompletedIntegrationEventFailure.UserId,
                userDeletionCompletedIntegrationEventFailure.UserId, "email@email.com", true, "PasswordHash",
                Guid.NewGuid(), DateTimeOffset.UtcNow, null);
            var domainEvents = new List<IDomainEvent> { accountCreatedDomainEvent };

            _loggerMock.Setup(x => x.LogIntegrationEventError(It.IsAny<ServiceComponentEnumeration>(),
                It.IsAny<IIntegrationEvent>(), It.IsAny<string>(), It.IsAny<object[]>()))
                .Verifiable();
            _integrationEventBusMock.Setup(x => x.PublishIntegrationEventAsync(It.IsAny<IIntegrationEvent>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
            _domainEventStoreMock.Setup(x => x.FindAllAsync(It.IsAny<Guid>())).ReturnsAsync(domainEvents);
            _accountRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Account>())).Returns(Task.CompletedTask);
            _accountRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Account>())).Returns(Task.CompletedTask);


            Func<Task> result = async () => await _userDeletionCompletedIntegrationEventFailure.HandleAsync(userDeletionCompletedIntegrationEventFailure);

            await result.Should().NotThrowAsync<Exception>();
            _loggerMock.Verify(x => x.LogIntegrationEventError(
                It.Is<ServiceComponentEnumeration>(s => Equals(s, ServiceComponentEnumeration.RivaIdentity)),
                It.Is<IIntegrationEvent>(ie => ie == userDeletionCompletedIntegrationEventFailure),
                It.Is<string>(m => m.Equals(logMessage)), It.Is<object[]>(p => !p.Except(logParams).Any())));
            _integrationEventBusMock.Verify(x => x.PublishIntegrationEventAsync(It.Is<IIntegrationEvent>(ie =>
                IsPublishedIntegrationEventCorrect((AccountDeletionCompletedIntegrationEventFailure)ie,
                    userDeletionCompletedIntegrationEventFailure.CorrelationId, userDeletionCompletedIntegrationEventFailure.UserId,
                    userDeletionCompletedIntegrationEventFailure.Code, userDeletionCompletedIntegrationEventFailure.Reason))));
        }

        [Fact]
        public async Task HandleAsync_Should_Complete_Account_Deletion_With_Failure_When_UserDeletionCompletedIntegrationEventFailure_Is_Received_And_Any_Exception_Is_Thrown()
        {
            var userDeletionCompletedIntegrationEventFailure = new UserDeletionCompletedIntegrationEventFailure(Guid.NewGuid(),
                DateTimeOffset.UtcNow, "AnyCode", "AnyReason", Guid.NewGuid());
            var message = $"Could not finish {nameof(Account)} deletion process.";
            const string logMessage = "accountId={accountId}, message={message}, reason={reason}, code={code}";
            var logParams = new object[]
            {
                userDeletionCompletedIntegrationEventFailure.UserId,
                message,
                userDeletionCompletedIntegrationEventFailure.Reason,
                userDeletionCompletedIntegrationEventFailure.Code
            };
            var exception = new Exception("Exception occured.");
            const string exceptionLogMessage = "userId={userId}, message={message}, stackTrace={stackTrace}";

            _loggerMock.Setup(x => x.LogIntegrationEventError(It.IsAny<ServiceComponentEnumeration>(),
                    It.IsAny<IIntegrationEvent>(), It.IsAny<string>(), It.IsAny<object[]>()))
                .Verifiable();
            _integrationEventBusMock.Setup(x => x.PublishIntegrationEventAsync(It.IsAny<IIntegrationEvent>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
            _domainEventStoreMock.Setup(x => x.FindAllAsync(It.IsAny<Guid>())).ThrowsAsync(exception);


            Func<Task> result = async () => await _userDeletionCompletedIntegrationEventFailure.HandleAsync(userDeletionCompletedIntegrationEventFailure);

            await result.Should().NotThrowAsync<Exception>();
            _loggerMock.Verify(x => x.LogIntegrationEventError(
                It.Is<ServiceComponentEnumeration>(s => Equals(s, ServiceComponentEnumeration.RivaIdentity)),
                It.Is<IIntegrationEvent>(ie => ie == userDeletionCompletedIntegrationEventFailure),
                It.Is<string>(m => m.Equals(logMessage)), It.Is<object[]>(p => !p.Except(logParams).Any())));
            _integrationEventBusMock.Verify(x => x.PublishIntegrationEventAsync(It.Is<IIntegrationEvent>(ie =>
                IsPublishedIntegrationEventCorrect((AccountDeletionCompletedIntegrationEventFailure)ie,
                    userDeletionCompletedIntegrationEventFailure.CorrelationId, userDeletionCompletedIntegrationEventFailure.UserId,
                    userDeletionCompletedIntegrationEventFailure.Code, userDeletionCompletedIntegrationEventFailure.Reason))));
            _loggerMock.Verify(
                x => x.LogIntegrationEventError(
                    It.Is<ServiceComponentEnumeration>(s => Equals(s, ServiceComponentEnumeration.RivaIdentity)),
                    It.Is<IIntegrationEvent>(ie => ie == userDeletionCompletedIntegrationEventFailure),
                    It.Is<string>(m => m.Equals(exceptionLogMessage)),
                    It.IsAny<object[]>()));
        }

        private static bool IsPublishedIntegrationEventCorrect(AccountDeletionCompletedIntegrationEventFailure integrationEvent, Guid correlationId, Guid accountId,
            string code, string reason)
        {
            return integrationEvent.CorrelationId == correlationId && integrationEvent.AccountId == accountId &&
                   integrationEvent.Code.Equals(code) && integrationEvent.Reason.Equals(reason);
        }
    }
}