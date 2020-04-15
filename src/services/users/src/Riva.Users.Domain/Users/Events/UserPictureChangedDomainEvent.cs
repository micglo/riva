using System;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Events
{
    public class UserPictureChangedDomainEvent : DomainEventBase
    {
        public string Picture { get; }

        public UserPictureChangedDomainEvent(Guid aggregateId, Guid correlationId, string picture)
            : base(aggregateId, correlationId)
        {
            Picture = picture;
        }
    }
}