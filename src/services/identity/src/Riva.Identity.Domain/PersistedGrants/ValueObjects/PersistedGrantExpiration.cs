using System;
using Riva.Identity.Domain.PersistedGrants.Exceptions;

namespace Riva.Identity.Domain.PersistedGrants.ValueObjects
{
    public class PersistedGrantExpiration
    {
        private readonly DateTime? _expiration;

        public PersistedGrantExpiration(DateTime? expiration, DateTime creationTime)
        {
            if (expiration.HasValue && expiration <= creationTime)
                throw new PersistedGrantExpirationMinValueException();

            _expiration = expiration;
        }

        public static implicit operator DateTime?(PersistedGrantExpiration expiration)
        {
            return expiration._expiration;
        }
    }
}