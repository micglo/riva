using System;
using AutoMapper;
using Riva.Identity.Core.Queries;
using Riva.Identity.Domain.Accounts.Enumerations;
using Riva.Identity.Web.Api.Models.Enums;
using Riva.Identity.Web.Api.Models.Responses.Accounts;

namespace Riva.Identity.Web.Api.AutoMapperProfiles
{
    public class TokenProfile : Profile
    {
        public TokenProfile()
        {
            CreateMap<GetAccountTokenOutputQuery, AccountToken>()
                .ConstructUsing(x => new AccountToken(x.Issued, x.Expires, ConvertToAccountTokenTypeEnum(x.Type), x.Value));
        }

        public static AccountTokenType ConvertToAccountTokenTypeEnum(TokenTypeEnumeration tokenType)
        {
            switch (tokenType)
            {
                case { } tokenTypeEnumeration when Equals(tokenTypeEnumeration, TokenTypeEnumeration.PasswordReset):
                    return AccountTokenType.PasswordReset;
                case { } tokenTypeEnumeration when Equals(tokenTypeEnumeration, TokenTypeEnumeration.AccountConfirmation):
                    return AccountTokenType.AccountConfirmation;
                default:
                    throw new ArgumentException($"{nameof(tokenType.DisplayName)} is not supported by {nameof(AccountTokenType)}.");
            }
        }
    }
}