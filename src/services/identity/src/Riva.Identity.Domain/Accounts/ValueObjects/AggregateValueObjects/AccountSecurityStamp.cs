using System;
using Riva.Identity.Domain.Accounts.Exceptions.AggregateExceptions;

namespace Riva.Identity.Domain.Accounts.ValueObjects.AggregateValueObjects
{
    public class AccountSecurityStamp
    {
        private readonly Guid _securityStamp;

        public AccountSecurityStamp(Guid securityStamp)
        {
            if (securityStamp == Guid.Empty || securityStamp == new Guid())
                throw new AccountSecurityStampNullException();

            _securityStamp = securityStamp;
        }

        public static implicit operator Guid(AccountSecurityStamp securityStamp)
        {
            return securityStamp._securityStamp;
        }
    }
}