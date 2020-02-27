using System;
using System.Collections.Generic;
using System.Linq;
using Riva.Identity.Domain.Accounts.Exceptions.AggregateExceptions;

namespace Riva.Identity.Domain.Accounts.ValueObjects.AggregateValueObjects
{
    public class AccountRoles
    {
        private readonly List<Guid> _roles;

        public AccountRoles(IEnumerable<Guid> roles)
        {
            if (roles is null)
                throw new AccountRolesNullException();

            var roleList = roles.ToList();

            if (roleList.Any(x => x == Guid.Empty || x == new Guid()))
                throw new AccountRolesInvalidException();

            var anyDuplicate = roleList.GroupBy(x => x).Any(g => g.Count() > 1);
            if (anyDuplicate)
                throw new AccountRolesDuplicateValuesException();

            _roles = roleList;
        }

        public static implicit operator List<Guid>(AccountRoles roles)
        {
            return roles._roles;
        }
    }
}