using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Entities;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;

namespace Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.AutoMapperProfiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<AccountEntity, Account>()
                .ForMember(x => x.Roles, opt => opt.Ignore())
                .ForMember(x => x.Tokens, opt => opt.Ignore())
                .ConstructUsing(
                    (accountEntity, context) => Account.Builder()
                            .SetId(accountEntity.Id)
                            .SetEmail(accountEntity.Email)
                            .SetConfirmed(accountEntity.Confirmed)
                            .SetPasswordHash(accountEntity.PasswordHash)
                            .SetSecurityStamp(accountEntity.SecurityStamp)
                            .SetCreated(accountEntity.Created)
                            .SetRoles(accountEntity.Roles.Select(r => r.RoleId))
                            .SetLastLogin(accountEntity.LastLogin)
                            .SetTokens(context.Mapper.Map<ICollection<TokenEntity>, IEnumerable<Token>>(accountEntity.Tokens))
                            .Build()
                    );

            CreateMap<Account, AccountEntity>()
                .ForMember(x => x.Roles, opt => opt.Ignore())
                .ForMember(x => x.Tokens, opt => opt.Ignore());
        }
    }
}