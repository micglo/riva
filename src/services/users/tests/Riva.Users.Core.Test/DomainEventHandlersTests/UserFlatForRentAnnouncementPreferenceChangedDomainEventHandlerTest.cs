using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Communications.DomainEvents;
using Riva.BuildingBlocks.Core.Stores;
using Riva.BuildingBlocks.Domain;
using Riva.Users.Core.DomainEventHandlers.UserDomainEventHandlers;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Domain.Users.Events;
using Xunit;

namespace Riva.Users.Core.Test.DomainEventHandlersTests
{
    public class UserFlatForRentAnnouncementPreferenceChangedDomainEventHandlerTest
    {
        private readonly Mock<IDomainEventStore> _domainEventStoreMock;
        private readonly IDomainEventHandler<UserFlatForRentAnnouncementPreferenceChangedDomainEvent> _domainEventHandler;

        public UserFlatForRentAnnouncementPreferenceChangedDomainEventHandlerTest()
        {
            _domainEventStoreMock = new Mock<IDomainEventStore>();
            _domainEventHandler = new UserFlatForRentAnnouncementPreferenceChangedDomainEventHandler(_domainEventStoreMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Store_DomainEvent()
        {
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(1)
                .SetPriceMax(1000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .Build();
            var domainEvent = new UserFlatForRentAnnouncementPreferenceChangedDomainEvent(Guid.NewGuid(),
                Guid.NewGuid(), flatForRentAnnouncementPreference);

            _domainEventStoreMock.Setup(x => x.Store(It.IsAny<IDomainEvent>())).Verifiable();

            Func<Task> result = async () => await _domainEventHandler.HandleAsync(domainEvent);

            await result.Should().NotThrowAsync<Exception>();
            _domainEventStoreMock.Verify(x =>
                x.Store(It.Is<IDomainEvent>(e =>
                    e.GetType() == typeof(UserFlatForRentAnnouncementPreferenceChangedDomainEvent))));
        }
    }
}