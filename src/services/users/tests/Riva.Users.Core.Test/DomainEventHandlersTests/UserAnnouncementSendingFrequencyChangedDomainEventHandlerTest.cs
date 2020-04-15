using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Communications.DomainEvents;
using Riva.BuildingBlocks.Core.Stores;
using Riva.BuildingBlocks.Domain;
using Riva.Users.Core.DomainEventHandlers.UserDomainEventHandlers;
using Riva.Users.Domain.Users.Enumerations;
using Riva.Users.Domain.Users.Events;
using Xunit;

namespace Riva.Users.Core.Test.DomainEventHandlersTests
{
    public class UserAnnouncementSendingFrequencyChangedDomainEventHandlerTest
    {
        private readonly Mock<IDomainEventStore> _domainEventStoreMock;
        private readonly IDomainEventHandler<UserAnnouncementSendingFrequencyChangedDomainEvent> _domainEventHandler;

        public UserAnnouncementSendingFrequencyChangedDomainEventHandlerTest()
        {
            _domainEventStoreMock = new Mock<IDomainEventStore>();
            _domainEventHandler = new UserAnnouncementSendingFrequencyChangedDomainEventHandler(_domainEventStoreMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Store_DomainEvent()
        {
            var domainEvent = new UserAnnouncementSendingFrequencyChangedDomainEvent(Guid.NewGuid(), Guid.NewGuid(),
                AnnouncementSendingFrequencyEnumeration.EveryHour);

            _domainEventStoreMock.Setup(x => x.Store(It.IsAny<IDomainEvent>())).Verifiable();

            Func<Task> result = async () => await _domainEventHandler.HandleAsync(domainEvent);

            await result.Should().NotThrowAsync<Exception>();
            _domainEventStoreMock.Verify(x =>
                x.Store(It.Is<IDomainEvent>(e =>
                    e.GetType() == typeof(UserAnnouncementSendingFrequencyChangedDomainEvent))));
        }
    }
}