using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Riva.AdministrativeDivisions.Domain.States.Aggregates;
using Riva.AdministrativeDivisions.Domain.States.Repositories;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Contexts;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;
using Riva.AdministrativeDivisions.Infrastructure.Models;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Services;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Extensions;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Repositories
{
    public class StateRepository : IStateRepository
    {
        private readonly RivaAdministrativeDivisionsDbContext _dbContext;
        private readonly DbSet<StateEntity> _dbSet;
        private readonly IMapper _mapper;
        private readonly IOrderByExpressionCreator<State> _orderByExpressionCreator;
        private readonly IMemoryCache _cache;

        public StateRepository(RivaAdministrativeDivisionsDbContext dbContext, IMapper mapper, 
            IOrderByExpressionCreator<State> orderByExpressionCreator, IMemoryCache cache)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _orderByExpressionCreator = orderByExpressionCreator;
            _cache = cache;
            _dbSet = dbContext.Set<StateEntity>();
        }

        public async Task<List<State>> GetAllAsync()
        {
            if (_cache.TryGetValue(CacheKeys.StatesKey, out List<State> states))
                return states;

            var stateEntities = await _dbSet.ToListAsync();
            states = _mapper.Map<List<StateEntity>, List<State>>(stateEntities);
            _cache.Set(CacheKeys.StatesKey, states,
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(365)));

            return states;
        }

        public async Task<List<State>> FindAsync(int? skip, int? take, string sort, string name, string polishName)
        {
            var states = await GetAllAsync();
            return ProcessStates(states, skip, take, sort, name, polishName);
        }

        public async Task<State> GetByIdAsync(Guid id)
        {
            var stateEntity = await _dbSet.FindAsync(id);
            return stateEntity != null ? _mapper.Map<StateEntity, State>(stateEntity) : null;
        }

        public async Task<State> GetByNameAsync(string name)
        {
            var stateEntity =
                await _dbSet.SingleOrDefaultAsync(x => x.Name.ToLower().Equals(name.ToLower()));
            return stateEntity != null ? _mapper.Map<StateEntity, State>(stateEntity) : null;
        }

        public async Task<State> GetByPolishNameAsync(string polishName)
        {
            var stateEntity =
                await _dbSet.SingleOrDefaultAsync(x => x.PolishName.ToLower().Equals(polishName.ToLower()));
            return stateEntity != null ? _mapper.Map<StateEntity, State>(stateEntity) : null;
        }

        public async Task AddAsync(State state)
        {
            var stateEntity = _mapper.Map<State, StateEntity>(state);
            _dbSet.Add(stateEntity);
            await _dbContext.SaveChangesAsync();
            state.SetRowVersion(stateEntity.RowVersion);
            _cache.Remove(CacheKeys.StatesKey);
        }

        public async Task UpdateAsync(State state)
        {
            var stateEntity = _mapper.Map<State, StateEntity>(state);
            _dbContext.AttachEntity(stateEntity);
            _dbContext.Entry(stateEntity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            state.SetRowVersion(stateEntity.RowVersion);
            _cache.Remove(CacheKeys.StatesKey);
        }

        public async Task DeleteAsync(State state)
        {
            var stateEntity = new StateEntity
            {
                Id = state.Id,
                RowVersion = state.RowVersion.ToArray()
            };
            _dbContext.AttachEntity(stateEntity);
            _dbContext.Entry(stateEntity).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
            _cache.Remove(CacheKeys.StatesKey);
        }

        public async Task<long> CountAsync()
        {
            var states = await GetAllAsync();
            return states.LongCount();
        }

        public async Task<long> CountAsync(string name, string polishName)
        {
            var states = await GetAllAsync();
            var statesQuery = states.AsQueryable();
            var filterPredicate = CreateFilterPredicate(name, polishName);

            if (filterPredicate != null)
                statesQuery = statesQuery.Where(filterPredicate);

            return statesQuery.LongCount();
        }

        private List<State> ProcessStates(IEnumerable<State> states, int? skip, int? take, string sort, string name, string polishName)
        {
            var query = states.AsQueryable();

            if (!string.IsNullOrWhiteSpace(sort))
            {
                var orderByQuery = _orderByExpressionCreator.CreateExpression(sort);
                query = orderByQuery(query);
            }

            var filterPredicate = CreateFilterPredicate(name, polishName);
            query = filterPredicate != null ? query.Where(filterPredicate) : query;

            if (skip.HasValue && take.HasValue)
            {
                var skipVal = take.Value * (skip.Value - 1);
                query = query.Skip(skipVal);
            }

            if (take.HasValue)
                query = query.Take(take.Value);

            return query.ToList();
        }

        private static Expression<Func<State, bool>> CreateFilterPredicate(string name, string polishName)
        {
            Expression<Func<State, bool>> filterPredicate = null;
            if (!string.IsNullOrWhiteSpace(name))
                filterPredicate = x => x.Name.ToLower().StartsWith(name.ToLower());
            if (!string.IsNullOrWhiteSpace(polishName))
                filterPredicate = x => x.PolishName.ToLower().StartsWith(polishName.ToLower());

            return filterPredicate;
        }
    }
}