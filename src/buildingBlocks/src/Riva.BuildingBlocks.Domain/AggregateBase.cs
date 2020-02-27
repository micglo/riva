using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Riva.BuildingBlocks.Domain
{
    public abstract class AggregateBase
    {
        protected readonly List<IDomainEvent> Events;

        public Guid Id { get; protected set; }
        public IReadOnlyCollection<IDomainEvent> DomainEvents => Events.AsReadOnly();

        protected AggregateBase(Guid id)
        {
            Id = new AggregateId(id);
            Events = new List<IDomainEvent>();
        }

        public void AddEvents(IEnumerable<IDomainEvent> domainEvents)
        {
            Events.AddRange(domainEvents);
        }

        public virtual void ApplyEvents()
        {
            if (!Events.Any())
                throw new NoDomainEventsToApplyException();
        }

        protected void AddEvent(IDomainEvent domainEvent)
        {
            Events.Add(domainEvent);
        }

        protected void RemoveEvent(IDomainEvent domainEvent)
        {
            Events.Remove(domainEvent);
        }

        protected void ClearEvents()
        {
            Events.Clear();
        }
    }

    public class AggregateId
    {
        private readonly Guid _id;

        public AggregateId(Guid id)
        {
            if (id == Guid.Empty || Equals(id, new Guid?()) || id == new Guid())
                throw new AggregateIdNullException();

            _id = id;
        }

        public static implicit operator Guid(AggregateId id)
        {
            return id._id;
        }
    }

    [Serializable]
    public class AggregateIdNullException : DomainException
    {
        private const string ErrorMessage = "Id argument is required.";

        public override string Message { get; }

        public AggregateIdNullException() : base(ErrorMessage)
        {
        }

        public AggregateIdNullException(string message) : base(message)
        {
        }

        public AggregateIdNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        private AggregateIdNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
            Message = ErrorMessage;
        }
    }

    [Serializable]
    public class NoDomainEventsToApplyException : DomainException
    {
        private const string ErrorMessage = "There are no domain events to apply.";

        public override string Message { get; }

        public NoDomainEventsToApplyException() : base(ErrorMessage)
        {
        }

        public NoDomainEventsToApplyException(string message) : base(message)
        {
        }

        public NoDomainEventsToApplyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        private NoDomainEventsToApplyException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
            Message = ErrorMessage;
        }
    }
}