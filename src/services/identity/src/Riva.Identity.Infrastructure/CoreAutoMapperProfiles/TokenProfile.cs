using AutoMapper;
using Riva.Identity.Core.Queries;
using Riva.Identity.Domain.Accounts.Entities;

namespace Riva.Identity.Infrastructure.CoreAutoMapperProfiles
{
    public class TokenProfile : Profile
    {
        public TokenProfile()
        {
            CreateMap<Token, GetAccountTokenOutputQuery>();
        }
    }
}