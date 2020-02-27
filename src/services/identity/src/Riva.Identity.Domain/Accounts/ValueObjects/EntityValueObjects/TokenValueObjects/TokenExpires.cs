using System;
using Riva.Identity.Domain.Accounts.Exceptions.EntityExceptions.TokenExceptions;

namespace Riva.Identity.Domain.Accounts.ValueObjects.EntityValueObjects.TokenValueObjects
{
    public class TokenExpires
    {
        private readonly DateTimeOffset _expires;

        public TokenExpires(DateTimeOffset expires, DateTimeOffset issued)
        {
            if (expires <= issued)
                throw new TokenExpiresMinValueException();

            _expires = expires;
        }

        public static implicit operator DateTimeOffset(TokenExpires expires)
        {
            return expires._expires;
        }
    }
}