using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Domain;

namespace Riva.BuildingBlocks.Core.Communications.DomainEvents
{
    public abstract class DomainEventHandlerWrapperBase
    {
        public abstract Task HandleAsync(IDomainEvent domainEvent, CancellationToken cancellationToken, ServiceFactory serviceFactory,
            Func<IEnumerable<Func<IDomainEvent, CancellationToken, Task>>, IDomainEvent, CancellationToken, Task> send);
    }

    public class DomainEventHandlerWrapper<TDomainEvent> : DomainEventHandlerWrapperBase where TDomainEvent : IDomainEvent
    {
        public override Task HandleAsync(IDomainEvent domainEvent, CancellationToken cancellationToken, ServiceFactory serviceFactory,
            Func<IEnumerable<Func<IDomainEvent, CancellationToken, Task>>, IDomainEvent, CancellationToken, Task> send)
        {
            var handlers = serviceFactory
                .GetInstances<IDomainEventHandler<TDomainEvent>>()
                .Select(x => new Func<IDomainEvent, CancellationToken, Task>((theDomainEvent, theToken) =>
                    x.HandleAsync((TDomainEvent) theDomainEvent, theToken)));

            return send(handlers, domainEvent, cancellationToken);
        }
    }
}