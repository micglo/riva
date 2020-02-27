using System;
using Riva.Identity.Domain.Accounts.Exceptions.AggregateExceptions;

namespace Riva.Identity.Domain.Accounts.ValueObjects.AggregateValueObjects
{
    public class AccountRole
    {
        private readonly Guid _role;

        public AccountRole(Guid role)
        {
            if (role == Guid.Empty || role == new Guid())
                throw new AccountRoleNullException();

            _role = role;
        }

        public static implicit operator Guid(AccountRole role)
        {
            return role._role;
        }
    }
}