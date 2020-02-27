using System;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Events
{
    public class AccountCreatedDomainEvent : DomainEventBase
    {
        public string Email { get;  }
        public bool Confirmed { get;  }
        public string PasswordHash { get;  }
        public Guid SecurityStamp { get;  }
        public DateTimeOffset Created { get; }
        public DateTimeOffset? LastLogin { get; }

        public AccountCreatedDomainEvent(Guid aggregateId, Guid correlationId, string email, bool confirmed, 
            string passwordHash, Guid securityStamp, DateTimeOffset created, DateTimeOffset? lastLogin) : base(aggregateId, correlationId)
        {
            Email = email;
            Confirmed = confirmed;
            PasswordHash = passwordHash;
            SecurityStamp = securityStamp;
            Created = created;
            LastLogin = lastLogin;
        }
    }
}