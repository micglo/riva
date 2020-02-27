using System;
using Riva.Identity.Domain.Accounts.Builders;
using Riva.Identity.Domain.Accounts.ValueObjects.EntityValueObjects.TokenValueObjects;

namespace Riva.Identity.Domain.Accounts.Entities
{
    public class Token
    {
        public DateTimeOffset Issued { get; }
        public DateTimeOffset Expires { get; }
        public Enumerations.TokenTypeEnumeration Type { get; }
        public string Value { get; }
        
        private Token(TokenBuilder builder)
        {
            Issued = builder.Issued;
            Expires = builder.Expires;
            Type = builder.Type;
            Value = builder.Value;
        }

        public static ITokenIssuedSetter Builder()
        {
            return new TokenBuilder();
        }

        private class TokenBuilder : ITokenIssuedSetter, ITokenExpiresSetter, ITokenTypeSetter, ITokenValueSetter, ITokenBuilder
        {
            public DateTimeOffset Issued { get; private set; }
            public DateTimeOffset Expires { get; private set; }
            public Enumerations.TokenTypeEnumeration Type { get; private set; }
            public string Value { get; private set; }

            public ITokenExpiresSetter SetIssued(DateTimeOffset issued)
            {
                Issued = new TokenIssued(issued);
                return this;
            }

            public ITokenTypeSetter SetExpires(DateTimeOffset expires)
            {
                Expires = new TokenExpires(expires, Issued);
                return this;
            }

            public ITokenValueSetter SetType(Enumerations.TokenTypeEnumeration type)
            {
                Type = new TokenType(type);
                return this;
            }

            public ITokenBuilder SetValue(string value)
            {
                Value = new TokenValue(value);
                return this;
            }

            public Token Build()
            {
                return new Token(this);
            }
        }
    }
}