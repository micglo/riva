using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Riva.BuildingBlocks.Core.Extensions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Services;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Extensions;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Entities;
using Riva.Identity.Domain.Accounts.Repositories;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;

namespace Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly RivaIdentityDbContext _context;
        private readonly DbSet<AccountEntity> _dbSet;
        private readonly IMapper _mapper;
        private readonly IOrderByExpressionCreator<AccountEntity> _orderByExpressionCreator;
        
        public AccountRepository(RivaIdentityDbContext context, IMapper mapper, 
            IOrderByExpressionCreator<AccountEntity> orderByExpressionCreator)
        {
            _context = context;
            _dbSet = context.Set<AccountEntity>();
            _mapper = mapper;
            _orderByExpressionCreator = orderByExpressionCreator;
        }

        public async Task<List<Account>> GetAllAsync()
        {
            var entities = await _dbSet.ToListAsync();
            return _mapper.Map<List<AccountEntity>, List<Account>>(entities);
        }

        public async Task<List<Account>> FindAsync(int? skip, int? take, string sort, string email, bool? confirmed)
        {
            var entities = await FindAccountEntitiesAsync(skip, take, sort, email, confirmed);
            return _mapper.Map<List<AccountEntity>, List<Account>>(entities);
        }

        public async Task<Account> GetByIdAsync(Guid id)
        {
            var entity = await GetAccountEntityByIdWithRolesAndTokensAsync(id);
            return entity != null ? _mapper.Map<AccountEntity, Account>(entity) : null;
        }

        public async Task<Account> GetByEmailAsync(string email)
        {
            var entity = await _dbSet
                .Include(x => x.Roles)
                .Include(x => x.Tokens)
                .SingleOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
            return entity != null ? _mapper.Map<AccountEntity, Account>(entity) : null;
        }

        public Task AddAsync(Account account)
        {
            var accountEntity = _mapper.Map<Account, AccountEntity>(account);
            foreach (var role in account.Roles)
            {
                accountEntity.Roles.Add(new AccountRoleEntity { AccountId = account.Id, RoleId = role });
            }
            foreach (var token in account.Tokens)
            {
                var tokenEntity = _mapper.Map<Token, TokenEntity>(token);
                tokenEntity.AccountId = account.Id;
                accountEntity.Tokens.Add(tokenEntity);
            }
            _dbSet.Add(accountEntity);

            return _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Account account)
        {
            var entity = await GetAccountEntityByIdWithRolesAndTokensAsync(account.Id);
            UpdateAccountEntity(entity, account);
            var updateTokensTask = UpdateTokensAsync(entity, account.Tokens.ToList());
            var updateRolesTask = UpdateRolesAsync(entity, account.Roles.ToList());
            await Task.WhenAll(updateTokensTask, updateRolesTask);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(Account account)
        {
            var entity = _mapper.Map<Account, AccountEntity>(account);
            _context.AttachEntity(entity);
            _context.Entry(entity).State = EntityState.Deleted;
            return _context.SaveChangesAsync();
        }

        public Task<long> CountAsync()
        {
            return _dbSet.LongCountAsync();
        }

        public Task<long> CountAsync(string email, bool? confirmed)
        {
            var predicate = CreateFilterPredicate(email, confirmed);
            return predicate != null
                ? _dbSet.LongCountAsync(predicate)
                : _dbSet.LongCountAsync();
        }

        private async Task<List<AccountEntity>> FindAccountEntitiesAsync(int? skip, int? take, string sort, string email, bool? confirmed)
        {
            IQueryable<AccountEntity> query = _dbSet;

            if (!string.IsNullOrWhiteSpace(sort))
            {
                var orderByQuery = _orderByExpressionCreator.CreateExpression(sort);
                query = orderByQuery(query);
            }

            var predicate = CreateFilterPredicate(email, confirmed);
            query = predicate != null ? query.Where(predicate) : query;

            if (skip.HasValue && take.HasValue)
            {
                var skipVal = take.Value * (skip.Value - 1);
                query = query.Skip(skipVal);
            }

            if (take.HasValue)
                query = query.Take(take.Value);

            return await query.ToListAsync();
        }

        private static Expression<Func<AccountEntity, bool>> CreateFilterPredicate(string email, bool? confirmed)
        {
            Expression<Func<AccountEntity, bool>> predicate = null;
            if (!string.IsNullOrWhiteSpace(email))
                predicate = x => x.Email.ToLower().StartsWith(email.ToLower());
            if (confirmed.HasValue)
                predicate = predicate.AndAlso(x => x.Confirmed == confirmed.Value);

            return predicate;
        }

        private async Task<AccountEntity> GetAccountEntityByIdWithRolesAndTokensAsync(Guid id)
        {
            return await _dbSet
                .Include(x => x.Roles)
                .Include(x => x.Tokens)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        private static void UpdateAccountEntity(AccountEntity entity, Account account)
        {
            entity.Email = account.Email;
            entity.Confirmed = account.Confirmed;
            entity.PasswordHash = account.PasswordHash;
            entity.SecurityStamp = account.SecurityStamp;
            entity.LastLogin = account.LastLogin;
        }

        private async Task UpdateTokensAsync(AccountEntity accountEntity, IEnumerable<Token> tokens)
        {
            var currentTokens = await _context.Tokens.Where(x => x.AccountId == accountEntity.Id).ToListAsync();
            _context.Tokens.RemoveRange(currentTokens);
            foreach (var tokenEntity in currentTokens)
            {
                accountEntity.Tokens.Remove(tokenEntity);
            }
            foreach (var token in tokens)
            {
                var tokenEntity = _mapper.Map<Token, TokenEntity>(token);
                tokenEntity.AccountId = accountEntity.Id;
                _context.Tokens.Add(tokenEntity);
            }
        }

        private async Task UpdateRolesAsync(AccountEntity accountEntity, ICollection<Guid> roleIds)
        {
            var currentRoleIds = accountEntity.Roles.Select(x => x.RoleId).ToList();
            var roleIdsToAdd = roleIds.Except(currentRoleIds).ToArray();
            var roleIdsToRemove = currentRoleIds.Except(roleIds).ToArray();

            if (roleIdsToAdd.Any())
            {
                foreach (var roleId in roleIdsToAdd)
                {
                    accountEntity.Roles.Add(new AccountRoleEntity
                    {
                        AccountId = accountEntity.Id,
                        RoleId = roleId
                    });
                }
            }

            if (roleIdsToRemove.Any())
            {
                var accountRolesToRemove = await _context.AccountRoles.Where(x => roleIdsToRemove.Contains(x.RoleId)).ToListAsync();
                foreach (var accountRole in accountRolesToRemove)
                {
                    accountEntity.Roles.Remove(accountRole);
                }
            }
        }
    }
}