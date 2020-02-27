using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riva.BuildingBlocks.Core.Communications.IntegrationEvents
{
    public abstract class IntegrationEventHandlerWrapperBase
    {
        public abstract Task HandleAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken, ServiceFactory serviceFactory,
            Func<IEnumerable<Func<IIntegrationEvent, CancellationToken, Task>>, IIntegrationEvent, CancellationToken, Task> send);
    }

    public class IntegrationEventHandlerWrapper<TIntegrationEvent> : IntegrationEventHandlerWrapperBase where TIntegrationEvent : IIntegrationEvent
    {
        public override Task HandleAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken, ServiceFactory serviceFactory,
            Func<IEnumerable<Func<IIntegrationEvent, CancellationToken, Task>>, IIntegrationEvent, CancellationToken, Task> send)
        {
            var handlers = serviceFactory
                .GetInstances<IIntegrationEventHandler<TIntegrationEvent>>()
                .Select(x => new Func<IIntegrationEvent, CancellationToken, Task>((theIntegrationEvent, theToken) =>
                    x.HandleAsync((TIntegrationEvent) theIntegrationEvent, theToken)));

            return send(handlers, integrationEvent, cancellationToken);
        }
    }
}