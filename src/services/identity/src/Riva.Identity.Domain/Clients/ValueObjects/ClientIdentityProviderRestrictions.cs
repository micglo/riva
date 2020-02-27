using System.Collections.Generic;
using System.Linq;
using Riva.Identity.Domain.Clients.Exceptions;

namespace Riva.Identity.Domain.Clients.ValueObjects
{
    public class ClientIdentityProviderRestrictions
    {
        private readonly List<string> _identityProviderRestrictions;

        public ClientIdentityProviderRestrictions(IEnumerable<string> identityProviderRestrictions)
        {
            if (identityProviderRestrictions is null)
                throw new ClientIdentityProviderRestrictionsNullException();

            var identityProviderRestrictionList = identityProviderRestrictions.ToList();

            if (identityProviderRestrictionList.Any(string.IsNullOrWhiteSpace))
                throw new ClientIdentityProviderRestrictionsInvalidValuesException();

            _identityProviderRestrictions = new List<string>(identityProviderRestrictionList);
        }

        public static implicit operator List<string>(ClientIdentityProviderRestrictions identityProviderRestrictions)
        {
            return identityProviderRestrictions._identityProviderRestrictions;
        }
    }
}