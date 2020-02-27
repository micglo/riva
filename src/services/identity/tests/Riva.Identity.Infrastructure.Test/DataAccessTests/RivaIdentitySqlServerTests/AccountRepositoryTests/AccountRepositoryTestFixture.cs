using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Services;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Entities;
using Riva.Identity.Domain.Accounts.Enumerations;
using Riva.Identity.Domain.Accounts.Repositories;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Extensions;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Repositories;

namespace Riva.Identity.Infrastructure.Test.DataAccessTests.RivaIdentitySqlServerTests.AccountRepositoryTests
{
    public class AccountRepositoryTestFixture
    {
        public RivaIdentityDbContext Context { get; }
        public Mock<IMapper> MapperMock { get; }
        public Mock<IOrderByExpressionCreator<AccountEntity>> OrderByExpressionCreatorMock { get; }
        public IAccountRepository Repository { get; }
        public Account Account { get; }

        public AccountRepositoryTestFixture(DatabaseFixture fixture)
        {
            Context = fixture.Context;
            MapperMock = new Mock<IMapper>();
            OrderByExpressionCreatorMock = new Mock<IOrderByExpressionCreator<AccountEntity>>();
            Repository = new AccountRepository(Context, MapperMock.Object, OrderByExpressionCreatorMock.Object);
            var token = Token.Builder()
                .SetIssued(DateTimeOffset.UtcNow)
                .SetExpires(DateTimeOffset.UtcNow.AddDays(1))
                .SetType(TokenTypeEnumeration.AccountConfirmation)
                .SetValue("123456")
                .Build();
            Account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("AccountRepositoryTest@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .SetTokens(new List<Token>{ token })
                .Build();
            InsertAccountEntity(Account);
        }

        public AccountEntity InsertAccountEntity(Account account)
        {
            var accountEntity = new AccountEntity
            {
                Id = account.Id,
                Email = account.Email,
                Confirmed = account.Confirmed,
                PasswordHash = account.PasswordHash,
                SecurityStamp = account.SecurityStamp,
                Roles = account.Roles.Select(x => new AccountRoleEntity { AccountId = account.Id, RoleId = x }).ToList(),
                Tokens = account.Tokens.Select(x => new TokenEntity
                {
                    Id = Guid.NewGuid(),
                    AccountId = account.Id,
                    Expires = x.Expires,
                    Issued = x.Issued,
                    Type = x.Type.ConvertToEnum(),
                    Value = x.Value
                }).ToList()
            };
            Context.Accounts.Add(accountEntity);
            Context.SaveChanges();
            return accountEntity;
        }
    }
}