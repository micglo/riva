﻿using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Communications.DomainEvents;
using Riva.BuildingBlocks.Core.Stores;
using Riva.BuildingBlocks.Domain;
using Riva.Identity.Core.DomainEventHandlers.AccountDomainEventHandlers;
using Riva.Identity.Domain.Accounts.Events;
using Xunit;

namespace Riva.Identity.Core.Test.DomainEventHandlerTests.AccountDomainEventHandlerTests
{
    public class AccountRoleDeletedDomainEventHandlerTest
    {
        private readonly Mock<IDomainEventStore> _domainEventStoreMock;
        private readonly IDomainEventHandler<AccountRoleDeletedDomainEvent> _domainEventHandler;

        public AccountRoleDeletedDomainEventHandlerTest()
        {
            _domainEventStoreMock = new Mock<IDomainEventStore>();
            _domainEventHandler = new AccountRoleDeletedDomainEventHandler(_domainEventStoreMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Store_DomainEvent()
        {
            var domainEvent = new AccountRoleDeletedDomainEvent(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            _domainEventStoreMock.Setup(x => x.Store(It.IsAny<IDomainEvent>())).Verifiable();

            Func<Task> result = async () => await _domainEventHandler.HandleAsync(domainEvent);

            await result.Should().NotThrowAsync<Exception>();
            _domainEventStoreMock.Verify(x => x.Store(It.Is<IDomainEvent>(e => e.GetType() == typeof(AccountRoleDeletedDomainEvent))));
        }
    }
}