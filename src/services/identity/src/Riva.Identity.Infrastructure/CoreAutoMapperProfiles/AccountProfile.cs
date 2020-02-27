using System.Collections.Generic;
using AutoMapper;
using Riva.Identity.Core.Queries;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Entities;

namespace Riva.Identity.Infrastructure.CoreAutoMapperProfiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<Account, GetAccountOutputQuery>()
                .ForMember(x => x.Roles, opt => opt.Ignore())
                .ForMember(x => x.Tokens, opt => opt.Ignore())
                .ConstructUsing((account, context) => new GetAccountOutputQuery(account.Id, account.Email,
                    account.Confirmed, account.Created, !string.IsNullOrWhiteSpace(account.PasswordHash),
                    account.LastLogin, account.Roles,
                    context.Mapper.Map<IReadOnlyCollection<Token>, IEnumerable<GetAccountTokenOutputQuery>>(
                        account.Tokens)));

            CreateMap<Account, GetAccountsOutputQuery>()
                .ConstructUsing((account, context) => new GetAccountsOutputQuery(account.Id, account.Email,
                    account.Confirmed, account.Created, !string.IsNullOrWhiteSpace(account.PasswordHash),
                    account.LastLogin));
        }
    }
}