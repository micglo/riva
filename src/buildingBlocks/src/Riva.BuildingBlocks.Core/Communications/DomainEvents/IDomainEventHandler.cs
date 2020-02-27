using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Domain;

namespace Riva.BuildingBlocks.Core.Communications.DomainEvents
{
    public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : IDomainEvent
    {
        Task HandleAsync(TDomainEvent domainEvent, CancellationToken cancellationToken = default);
    }
}