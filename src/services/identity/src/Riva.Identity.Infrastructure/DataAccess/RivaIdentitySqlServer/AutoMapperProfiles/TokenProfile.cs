using System;
using AutoMapper;
using Riva.Identity.Domain.Accounts.Entities;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Enums;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Extensions;

namespace Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.AutoMapperProfiles
{
    public class TokenProfile : Profile
    {
        public TokenProfile()
        {
            CreateMap<TokenEntity, Token>()
                .ConvertUsing(
                    x => Token.Builder()
                        .SetIssued(x.Issued)
                        .SetExpires(x.Expires)
                        .SetType(x.Type.ConvertToEnumeration())
                        .SetValue(x.Value)
                        .Build()
                    );

            CreateMap<Token, TokenEntity>()
                .ForMember(x => x.Id, opt => opt.MapFrom(m => Guid.NewGuid()))
                .ForMember(x => x.Type, opt => opt.MapFrom<TokenEntityTypeValueResolver>())
                .ForMember(x => x.AccountId, opt => opt.Ignore())
                .ForMember(x => x.Account, opt => opt.Ignore());

        }

        private class TokenEntityTypeValueResolver : IValueResolver<Token, TokenEntity, TokenType>
        {
            public TokenType Resolve(Token source, TokenEntity destination, TokenType destMember, ResolutionContext context)
            {
                return source.Type.ConvertToEnum();
            }
        }
    }
}