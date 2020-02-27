using Riva.Identity.Domain.Accounts.Exceptions.EntityExceptions.TokenExceptions;

namespace Riva.Identity.Domain.Accounts.ValueObjects.EntityValueObjects.TokenValueObjects
{
    public class TokenType
    {
        private readonly Enumerations.TokenTypeEnumeration _type;

        public TokenType(Enumerations.TokenTypeEnumeration type)
        {
            _type = type ?? throw new TokenTypeNullException();
        }

        public static implicit operator Enumerations.TokenTypeEnumeration(TokenType type)
        {
            return type._type;
        }
    }
}