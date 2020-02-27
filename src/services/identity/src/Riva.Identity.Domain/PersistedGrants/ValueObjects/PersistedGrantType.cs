using Riva.Identity.Domain.PersistedGrants.Exceptions;

namespace Riva.Identity.Domain.PersistedGrants.ValueObjects
{
    public class PersistedGrantType
    {
        private readonly string _type;

        public PersistedGrantType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                throw new PersistedGrantTypeNullException();

            _type = type;
        }

        public static implicit operator string(PersistedGrantType type)
        {
            return type._type;
        }
    }
}