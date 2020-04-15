using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Communications.DomainEvents;
using Riva.BuildingBlocks.Core.Stores;
using Riva.BuildingBlocks.Domain;
using Riva.Users.Core.DomainEventHandlers.UserDomainEventHandlers;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Domain.Users.Enumerations;
using Riva.Users.Domain.Users.Events;
using Xunit;

namespace Riva.Users.Core.Test.DomainEventHandlersTests
{
    public class UserRoomForRentAnnouncementPreferenceAddedDomainEventHandlerTest
    {
        private readonly Mock<IDomainEventStore> _domainEventStoreMock;
        private readonly IDomainEventHandler<UserRoomForRentAnnouncementPreferenceAddedDomainEvent> _domainEventHandler;

        public UserRoomForRentAnnouncementPreferenceAddedDomainEventHandlerTest()
        {
            _domainEventStoreMock = new Mock<IDomainEventStore>();
            _domainEventHandler = new UserRoomForRentAnnouncementPreferenceAddedDomainEventHandler(_domainEventStoreMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Store_DomainEvent()
        {
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(1)
                .SetPriceMax(1000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .Build();
            var domainEvent = new UserRoomForRentAnnouncementPreferenceAddedDomainEvent(Guid.NewGuid(), Guid.NewGuid(),
                roomForRentAnnouncementPreference);

            _domainEventStoreMock.Setup(x => x.Store(It.IsAny<IDomainEvent>())).Verifiable();

            Func<Task> result = async () => await _domainEventHandler.HandleAsync(domainEvent);

            await result.Should().NotThrowAsync<Exception>();
            _domainEventStoreMock.Verify(x =>
                x.Store(It.Is<IDomainEvent>(e =>
                    e.GetType() == typeof(UserRoomForRentAnnouncementPreferenceAddedDomainEvent))));
        }
    }
}