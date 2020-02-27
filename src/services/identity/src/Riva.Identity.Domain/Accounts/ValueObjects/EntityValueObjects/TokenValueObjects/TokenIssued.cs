using System;
using Riva.Identity.Domain.Accounts.Exceptions.EntityExceptions.TokenExceptions;

namespace Riva.Identity.Domain.Accounts.ValueObjects.EntityValueObjects.TokenValueObjects
{
    public class TokenIssued
    {
        private readonly DateTimeOffset _issued;

        public TokenIssued(DateTimeOffset issued)
        {
            if(issued == DateTimeOffset.MinValue)
                throw new TokenIssuedMinValueException();

            _issued = issued;
        }

        public static implicit operator DateTimeOffset(TokenIssued issued)
        {
            return issued._issued;
        }
    }
}