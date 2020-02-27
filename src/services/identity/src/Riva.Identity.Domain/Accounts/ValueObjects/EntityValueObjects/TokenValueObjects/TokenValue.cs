using Riva.Identity.Domain.Accounts.Exceptions.EntityExceptions.TokenExceptions;

namespace Riva.Identity.Domain.Accounts.ValueObjects.EntityValueObjects.TokenValueObjects
{
    public class TokenValue
    {
        private readonly string _value;

        public TokenValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new TokenValueNullException();

            _value = value;
        }

        public static implicit operator string(TokenValue value)
        {
            return value._value;
        }
    }
}