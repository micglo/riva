using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.Identity.Core.IntegrationEvents;
using Riva.Identity.Core.IntegrationEvents.Handlers;
using Xunit;

namespace Riva.Identity.Core.Test.IntegrationEventHandlerTests
{
    public class UserDeletionCompletedIntegrationEventHandlerTest
    {
        private readonly Mock<IIntegrationEventBus> _integrationEventBusMock;
        private readonly IIntegrationEventHandler<UserDeletionCompletedIntegrationEvent> _userDeletionCompletedIntegrationEvent;

        public UserDeletionCompletedIntegrationEventHandlerTest()
        {
            _integrationEventBusMock = new Mock<IIntegrationEventBus>();
            _userDeletionCompletedIntegrationEvent = new UserDeletionCompletedIntegrationEventHandler(_integrationEventBusMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Complete_Account_Deletion_With_Success_When_UserDeletionCompletedIntegrationEvent_Is_Received()
        {
            var userDeletionCompletedIntegrationEvent = new UserDeletionCompletedIntegrationEvent(Guid.NewGuid(), DateTimeOffset.UtcNow, Guid.NewGuid());

            _integrationEventBusMock.Setup(x => x.PublishIntegrationEventAsync(It.IsAny<IIntegrationEvent>()))
                .Returns(Task.CompletedTask)
                .Verifiable();


            Func<Task> result = async () => await _userDeletionCompletedIntegrationEvent.HandleAsync(userDeletionCompletedIntegrationEvent);

            await result.Should().NotThrowAsync<Exception>();
            _integrationEventBusMock.Verify(x => x.PublishIntegrationEventAsync(It.Is<IIntegrationEvent>(ie =>
                IsPublishedIntegrationEventCorrect((AccountDeletionCompletedIntegrationEvent)ie, userDeletionCompletedIntegrationEvent.CorrelationId,
                    userDeletionCompletedIntegrationEvent.UserId))));
        }

        private static bool IsPublishedIntegrationEventCorrect(AccountDeletionCompletedIntegrationEvent integrationEvent, Guid correlationId, Guid accountId)
        {
            return integrationEvent.CorrelationId == correlationId && integrationEvent.AccountId == accountId;
        }
    }
}