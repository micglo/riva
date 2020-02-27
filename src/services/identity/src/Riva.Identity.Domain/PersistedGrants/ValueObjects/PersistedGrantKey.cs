using Riva.Identity.Domain.PersistedGrants.Exceptions;

namespace Riva.Identity.Domain.PersistedGrants.ValueObjects
{
    public class PersistedGrantKey
    {
        private readonly string _key;

        public PersistedGrantKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new PersistedGrantKeyNullException();

            _key = key;
        }

        public static implicit operator string(PersistedGrantKey key)
        {
            return key._key;
        }
    }
}