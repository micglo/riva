using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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
using Xunit;

namespace Riva.Identity.Infrastructure.Test.DataAccessTests.RivaIdentitySqlServerTests.AccountRepositoryTests
{
    [Collection("RivaIdentitySqlServer tests collection")]
    public class AccountRepositoryTest : IClassFixture<AccountRepositoryTestFixture>
    {
        private readonly RivaIdentityDbContext _context;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IOrderByExpressionCreator<AccountEntity>> _orderByExpressionCreatorMock;
        private readonly IAccountRepository _repository;
        private readonly Account _account;
        private readonly AccountRepositoryTestFixture _fixture;

        public AccountRepositoryTest(AccountRepositoryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapperMock = fixture.MapperMock;
            _orderByExpressionCreatorMock = fixture.OrderByExpressionCreatorMock;
            _repository = fixture.Repository;
            _account = fixture.Account;
            _fixture = fixture;
        }
        
        [Fact]
        public async Task GetAllAsync_Should_Return_Accounts_Collection()
        {
            var accountEntities = await _context.Accounts.ToListAsync();
            var accounts = accountEntities
                .Select(x => Account.Builder()
                    .SetId(x.Id)
                    .SetEmail(x.Email)
                    .SetConfirmed(x.Confirmed)
                    .SetPasswordHash(x.PasswordHash)
                    .SetSecurityStamp(x.SecurityStamp)
                    .SetCreated(x.Created)
                    .SetRoles(x.Roles.Select(r => r.RoleId))
                    .SetLastLogin(x.LastLogin)
                    .Build()
                ).ToList();

            _mapperMock.Setup(x => x.Map<List<AccountEntity>, List<Account>>(It.IsAny<List<AccountEntity>>()))
                .Returns(accounts);

            var result = await _repository.GetAllAsync();

            result.Should().BeEquivalentTo(accounts);
        }

        [Fact]
        public async Task FindAsync_Should_Return_Accounts_Collection()
        {
            var email = _account.Email;
            const string sort = "email:desc";
            const int skip = 0;
            const int take = 1;
            var accountEntities = await _context.Accounts.Where(x => x.Email.ToLower().StartsWith(email.ToLower()))
                .OrderByDescending(x => x.Email).Skip(skip).Take(take).ToListAsync();
            var accounts = accountEntities
                .Select(x => Account.Builder()
                    .SetId(x.Id)
                    .SetEmail(x.Email)
                    .SetConfirmed(x.Confirmed)
                    .SetPasswordHash(x.PasswordHash)
                    .SetSecurityStamp(x.SecurityStamp)
                    .SetCreated(x.Created)
                    .SetRoles(x.Roles.Select(r => r.RoleId))
                    .SetLastLogin(x.LastLogin)
                    .Build()
                ).ToList();
            IOrderedQueryable<AccountEntity> OrderByExpression(IQueryable<AccountEntity> o) => o.OrderByDescending(x => x.Email);

            _orderByExpressionCreatorMock.Setup(x => x.CreateExpression(It.IsAny<string>())).Returns(OrderByExpression);
            _mapperMock.Setup(x => x.Map<List<AccountEntity>, List<Account>>(It.IsAny<List<AccountEntity>>()))
                .Returns(accounts);

            var result = await _repository.FindAsync(skip, take, sort, email, null);

            result.Should().BeEquivalentTo(accounts);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Account()
        {
            _mapperMock.Setup(x => x.Map<AccountEntity, Account>(It.IsAny<AccountEntity>()))
                .Returns(_account);

            var result = await _repository.GetByIdAsync(_account.Id);

            result.Should().BeEquivalentTo(_account);
            result.Roles.Should().BeEquivalentTo(_account.Roles);
            result.Tokens.Should().BeEquivalentTo(_account.Tokens);
        }

        [Fact]
        public async Task GetByEmailAsync_Should_Return_Account()
        {
            _mapperMock.Setup(x => x.Map<AccountEntity, Account>(It.IsAny<AccountEntity>()))
                .Returns(_account);

            var result = await _repository.GetByEmailAsync(_account.Email);

            result.Should().BeEquivalentTo(_account);
            result.Roles.Should().BeEquivalentTo(_account.Roles);
            result.Tokens.Should().BeEquivalentTo(_account.Tokens);
        }

        [Fact]
        public async Task AddAsync_Should_Add_Account()
        {
            var role = Guid.NewGuid();
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("AccountRepositoryTestAddAsync@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid>{ role })
                .Build();
            var accountEntity = new AccountEntity
            {
                Id = account.Id,
                Email = account.Email,
                Confirmed = account.Confirmed,
                PasswordHash = account.PasswordHash,
                SecurityStamp = account.SecurityStamp
            };

            _mapperMock.Setup(x => x.Map<Account, AccountEntity>(It.IsAny<Account>()))
                .Returns(accountEntity);

            Func<Task> result = async () => await _repository.AddAsync(account);
            await result.Should().NotThrowAsync<Exception>();

            var addedAccount = await _context.Accounts.FindAsync(account.Id);
            addedAccount.Should().NotBeNull();
            addedAccount.Roles.Select(x => x.RoleId).Should().Contain(role);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Account()
        {
            var oldToken = Token.Builder()
                .SetIssued(DateTimeOffset.UtcNow)
                .SetExpires(DateTimeOffset.UtcNow.AddDays(1))
                .SetType(TokenTypeEnumeration.PasswordReset)
                .SetValue("123456")
                .Build();
            var role = Guid.NewGuid();
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("AccountRepositoryTestUpdateAsync@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { role })
                .SetTokens(new List<Token> { oldToken })
                .Build();
            _fixture.InsertAccountEntity(account);
            var correlationId = Guid.NewGuid();
            account.Confirm(correlationId);
            account.GenerateToken(TokenTypeEnumeration.PasswordReset, correlationId);
            account.RemoveRole(role, correlationId);
            var token = account.Tokens.First();
            var tokenEntity = new TokenEntity
            {
                Id = Guid.NewGuid(),
                Issued = token.Issued,
                Expires = token.Expires,
                Type = token.Type.ConvertToEnum(),
                Value = token.Value
            };

            _mapperMock.SetupSequence(x => x.Map<Token, TokenEntity>(It.IsAny<Token>())).Returns(tokenEntity);

            Func<Task> result = async () => await _repository.UpdateAsync(account);
            await result.Should().NotThrowAsync<Exception>();

            var updatedAccount = await _context.Accounts.Include(x => x.Roles).Include(x => x.Tokens)
                .SingleOrDefaultAsync(x => x.Id == account.Id);
            updatedAccount.Confirmed.Should().BeTrue();
            updatedAccount.Tokens.Should().Contain(tokenEntity);
            updatedAccount.Roles.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_Account()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("AccountRepositoryTestDeleteAsync@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid>{ Guid.NewGuid() })
                .Build();
            var accountEntity = _fixture.InsertAccountEntity(account);

            _mapperMock.Setup(x => x.Map<Account, AccountEntity>(It.IsAny<Account>()))
                .Returns(accountEntity);

            Func<Task> result = async () => await _repository.DeleteAsync(account);
            await result.Should().NotThrowAsync<Exception>();

            var deletedAccount = await _context.Accounts.FindAsync(account.Id);
            deletedAccount.Should().BeNull();
        }

        [Fact]
        public async Task CountAsync_Should_Return_Number_Of_Accounts()
        {
            var expectedResult = await _context.Accounts.LongCountAsync();

            var result = await _repository.CountAsync();

            result.Should().Be(expectedResult);
        }

        [Fact]
        public async Task CountAsync_Should_Return_Number_Of_Accounts_For_Given_Email_And_Confirmed_Values()
        {
            var expectedResult = await _context.Accounts.LongCountAsync(x => x.Email.StartsWith(_account.Email) && x.Confirmed == _account.Confirmed);

            var result = await _repository.CountAsync(_account.Email, _account.Confirmed);

            result.Should().Be(expectedResult);
        }

        [Fact]
        public async Task CountAsync_Should_Return_Number_Of_Accounts_For_Given_Email_Value()
        {
            var expectedResult = await _context.Accounts.LongCountAsync(x => x.Email.StartsWith(_account.Email));

            var result = await _repository.CountAsync(_account.Email, null);

            result.Should().Be(expectedResult);
        }

        [Fact]
        public async Task CountAsync_Should_Return_Number_Of_Accounts_For_Given_Confirmed_Value()
        {
            var expectedResult = await _context.Accounts.LongCountAsync(x => x.Confirmed == _account.Confirmed);

            var result = await _repository.CountAsync(null, _account.Confirmed);

            result.Should().Be(expectedResult);
        }
    }
}