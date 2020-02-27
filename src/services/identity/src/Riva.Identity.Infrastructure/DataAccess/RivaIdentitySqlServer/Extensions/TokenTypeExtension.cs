using System;
using System.Linq;
using Riva.BuildingBlocks.Domain;
using Riva.Identity.Domain.Accounts.Enumerations;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Enums;

namespace Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Extensions
{
    public static class TokenTypeExtension
    {
        public static TokenType ConvertToEnum(this TokenTypeEnumeration tokenType)
        {
            switch (tokenType)
            {
                case { } tokenTypeEnumeration when Equals(tokenTypeEnumeration, TokenTypeEnumeration.PasswordReset):
                    return TokenType.PasswordReset;
                case { } tokenTypeEnumeration when Equals(tokenTypeEnumeration, TokenTypeEnumeration.AccountConfirmation):
                    return TokenType.AccountConfirmation;
                default:
                    throw new ArgumentException($"{nameof(tokenType.DisplayName)} is not supported by {nameof(TokenType)}.");
            }
        }

        public static TokenTypeEnumeration ConvertToEnumeration(this TokenType type)
        {
            return EnumerationBase.GetAll<TokenTypeEnumeration>()
                .SingleOrDefault(x => x.DisplayName.ToLower().Equals(type.ToString().ToLower()));
        }
    }
}