using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Communications.DomainEvents;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.BuildingBlocks.Domain;

namespace Riva.BuildingBlocks.Core.Communications
{
    public class CommunicationBus : ICommunicationBus
    {
        private readonly ServiceFactory _serviceFactory;

        private static readonly ConcurrentDictionary<Type, CommandHandlerWrapperBase> CommandHandlers =
            new ConcurrentDictionary<Type, CommandHandlerWrapperBase>();
        private static readonly ConcurrentDictionary<Type, DomainEventHandlerWrapperBase> DomainEventHandlers =
            new ConcurrentDictionary<Type, DomainEventHandlerWrapperBase>();
        private static readonly ConcurrentDictionary<Type, IntegrationEventHandlerWrapperBase> IntegrationEventHandlers =
            new ConcurrentDictionary<Type, IntegrationEventHandlerWrapperBase>();

        public CommunicationBus(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public Task SendCommandAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : ICommand
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));

            var commandType = command.GetType();
            var handler = CommandHandlers.GetOrAdd(commandType,
                t => (CommandHandlerWrapperBase) Activator.CreateInstance(
                    typeof(CommandHandlerWrapper<>).MakeGenericType(commandType)));

            return handler.HandleAsync(command, cancellationToken, _serviceFactory, SendCommandsAsync);
        }

        public async Task DispatchDomainEventsAsync<TAggregate>(TAggregate aggregate, CancellationToken cancellationToken = default) 
            where TAggregate : AggregateBase
        {
            if (aggregate.DomainEvents.Any())
            {
                foreach (var domainEvent in aggregate.DomainEvents)
                {
                    await PublishDomainEventAsync(domainEvent, cancellationToken);
                }
            }
        }

        public Task PublishIntegrationEventAsync<TIntegrationEvent>(TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
            where TIntegrationEvent : IIntegrationEvent
        {
            if (integrationEvent is null)
                throw new ArgumentNullException(nameof(integrationEvent));

            var integrationEventType = integrationEvent.GetType();
            var handler = IntegrationEventHandlers.GetOrAdd(integrationEventType,
                t => (IntegrationEventHandlerWrapperBase) Activator.CreateInstance(
                    typeof(IntegrationEventHandlerWrapper<>).MakeGenericType(integrationEventType)));

            return handler.HandleAsync(integrationEvent, cancellationToken, _serviceFactory, SendIntegrationEventsAsync);
        }

        private static async Task SendCommandsAsync(IEnumerable<Func<ICommand, CancellationToken, Task>> allHandlers, 
            ICommand command, CancellationToken cancellationToken)
        {
            foreach (var handler in allHandlers)
            {
                await handler(command, cancellationToken).ConfigureAwait(false);
            }
        }

        private Task PublishDomainEventAsync<TDomainEvent>(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
            where TDomainEvent : IDomainEvent
        {
            if (domainEvent is null)
                throw new ArgumentNullException(nameof(domainEvent));

            var domainEventType = domainEvent.GetType();
            var handler = DomainEventHandlers.GetOrAdd(domainEventType,
                t => (DomainEventHandlerWrapperBase)Activator.CreateInstance(
                    typeof(DomainEventHandlerWrapper<>).MakeGenericType(domainEventType)));

            return handler.HandleAsync(domainEvent, cancellationToken, _serviceFactory, SendDomainEventsAsync);
        }

        private static async Task SendDomainEventsAsync(IEnumerable<Func<IDomainEvent, CancellationToken, Task>> allHandlers, 
            IDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            foreach (var handler in allHandlers)
            {
                await handler(domainEvent, cancellationToken).ConfigureAwait(false);
            }
        }

        private static async Task SendIntegrationEventsAsync(IEnumerable<Func<IIntegrationEvent, CancellationToken, Task>> allHandlers,
            IIntegrationEvent integrationEvent, CancellationToken cancellationToken)
        {
            foreach (var handler in allHandlers)
            {
                await handler(integrationEvent, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}