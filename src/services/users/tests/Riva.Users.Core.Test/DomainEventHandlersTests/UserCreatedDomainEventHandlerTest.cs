using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Communications.DomainEvents;
using Riva.BuildingBlocks.Core.Stores;
using Riva.BuildingBlocks.Domain;
using Riva.Users.Core.DomainEventHandlers.UserDomainEventHandlers;
using Riva.Users.Domain.Users.Defaults;
using Riva.Users.Domain.Users.Events;
using Xunit;

namespace Riva.Users.Core.Test.DomainEventHandlersTests
{
    public class UserCreatedDomainEventHandlerTest
    {
        private readonly Mock<IDomainEventStore> _domainEventStoreMock;
        private readonly IDomainEventHandler<UserCreatedDomainEvent> _domainEventHandler;

        public UserCreatedDomainEventHandlerTest()
        {
            _domainEventStoreMock = new Mock<IDomainEventStore>();
            _domainEventHandler = new UserCreatedDomainEventHandler(_domainEventStoreMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Store_DomainEvent()
        {
            var domainEvent = new UserCreatedDomainEvent(Guid.NewGuid(), Guid.NewGuid(), "email@email.com",
                string.Empty, DefaultUserSettings.ServiceActive, DefaultUserSettings.AnnouncementPreferenceLimit,
                DefaultUserSettings.AnnouncementSendingFrequency);

            _domainEventStoreMock.Setup(x => x.Store(It.IsAny<IDomainEvent>())).Verifiable();

            Func<Task> result = async () => await _domainEventHandler.HandleAsync(domainEvent);

            await result.Should().NotThrowAsync<Exception>();
            _domainEventStoreMock.Verify(x => x.Store(It.Is<IDomainEvent>(e => e.GetType() == typeof(UserCreatedDomainEvent))));
        }
    }
}