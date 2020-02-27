using System;
using Riva.Identity.Domain.PersistedGrants.Exceptions;

namespace Riva.Identity.Domain.PersistedGrants.ValueObjects
{
    public class PersistedGrantClientId
    {
        private readonly Guid _clientId;

        public PersistedGrantClientId(Guid clientId)
        {
            if (clientId == Guid.Empty || Equals(clientId, new Guid?()) || clientId == new Guid())
                throw new PersistedGrantClientIdNullException();

            _clientId = clientId;
        }

        public static implicit operator Guid(PersistedGrantClientId clientId)
        {
            return clientId._clientId;
        }
    }
}