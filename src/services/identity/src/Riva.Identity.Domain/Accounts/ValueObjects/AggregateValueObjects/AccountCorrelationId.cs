using System;
using Riva.Identity.Domain.Accounts.Exceptions.AggregateExceptions;

namespace Riva.Identity.Domain.Accounts.ValueObjects.AggregateValueObjects
{
    public class AccountCorrelationId
    {
        private readonly Guid _correlationId;

        public AccountCorrelationId(Guid correlationId)
        {
            if (correlationId == Guid.Empty || correlationId == new Guid())
                throw new AccountCorrelationIdNullException();

            _correlationId = correlationId;
        }

        public static implicit operator Guid(AccountCorrelationId correlationId)
        {
            return correlationId._correlationId;
        }
    }
}