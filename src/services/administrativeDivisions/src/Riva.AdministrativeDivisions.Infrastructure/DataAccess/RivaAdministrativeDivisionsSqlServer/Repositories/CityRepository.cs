using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Riva.AdministrativeDivisions.Domain.Cities.Aggregates;
using Riva.AdministrativeDivisions.Domain.Cities.Repositories;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Contexts;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;
using Riva.AdministrativeDivisions.Infrastructure.Models;
using Riva.BuildingBlocks.Core.Extensions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Services;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Extensions;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly RivaAdministrativeDivisionsDbContext _dbContext;
        private readonly DbSet<CityEntity> _dbSet;
        private readonly IMapper _mapper;
        private readonly IOrderByExpressionCreator<City> _orderByExpressionCreator;
        private readonly IMemoryCache _cache;

        public CityRepository(RivaAdministrativeDivisionsDbContext dbContext, IMapper mapper, 
            IOrderByExpressionCreator<City> orderByExpressionCreator, IMemoryCache cache)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _orderByExpressionCreator = orderByExpressionCreator;
            _cache = cache;
            _dbSet = dbContext.Set<CityEntity>();
        }
        
        public async Task<List<City>> GetAllAsync()
        {
            if(_cache.TryGetValue(CacheKeys.CitiesKey, out List<City> cities))
                return cities;

            var cityEntities = await _dbSet.ToListAsync();
            cities = _mapper.Map<List<CityEntity>, List<City>>(cityEntities);
            _cache.Set(CacheKeys.CitiesKey, cities,
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(365)));

            return cities;
        }

        public async Task<List<City>> FindAsync(int? skip, int? take, string sort, Guid? stateId, string name, string polishName)
        {
            var cities = await GetAllAsync();
            return ProcessCities(cities, skip, take, sort, stateId, name, polishName);
        }

        public async Task<City> GetByIdAsync(Guid id)
        {
            var cityEntity = await _dbSet.FindAsync(id);
            return cityEntity != null ? _mapper.Map<CityEntity, City>(cityEntity) : null;
        }

        public async Task<City> GetByNameAndStateIdAsync(string name, Guid stateId)
        {
            var cityEntity = await _dbSet.SingleOrDefaultAsync(x =>
                x.Name.ToLower().Equals(name.ToLower()) && x.StateId == stateId);
            return cityEntity != null ? _mapper.Map<CityEntity, City>(cityEntity) : null;
        }

        public async Task<City> GetByPolishNameAndStateIdAsync(string polishName, Guid stateId)
        {
            var cityEntity = await _dbSet.SingleOrDefaultAsync(x =>
                x.PolishName.ToLower().Equals(polishName.ToLower()) && x.StateId == stateId);
            return cityEntity != null ? _mapper.Map<CityEntity, City>(cityEntity) : null;
        }

        public async Task AddAsync(City city)
        {
            var cityEntity = _mapper.Map<City, CityEntity>(city);
            _dbSet.Add(cityEntity);
            await _dbContext.SaveChangesAsync();
            city.SetRowVersion(cityEntity.RowVersion);
            _cache.Remove(CacheKeys.CitiesKey);
        }

        public async Task UpdateAsync(City city)
        {
            var cityEntity = _mapper.Map<City, CityEntity>(city);
            _dbContext.AttachEntity(cityEntity);
            _dbContext.Entry(cityEntity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            city.SetRowVersion(cityEntity.RowVersion);
            _cache.Remove(CacheKeys.CitiesKey);
        }

        public async Task DeleteAsync(City city)
        {
            var cityEntity = new CityEntity
            {
                Id = city.Id,
                RowVersion = city.RowVersion.ToArray()
            };
            _dbContext.AttachEntity(cityEntity);
            _dbContext.Entry(cityEntity).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
            _cache.Remove(CacheKeys.CitiesKey);
        }

        public async Task<long> CountAsync()
        {
            var cities = await GetAllAsync();
            return cities.LongCount();
        }

        public async Task<long> CountAsync(Guid? stateId, string name, string polishName)
        {
            var cities = await GetAllAsync();
            var citiesQuery = cities.AsQueryable();
            var filterPredicate = CreateFilterPredicate(name, polishName, stateId);

            if (filterPredicate != null)
                citiesQuery = citiesQuery.Where(filterPredicate);

            return citiesQuery.LongCount();
        }

        private List<City> ProcessCities(IEnumerable<City> cities, int? skip, int? take, string sort, Guid? stateId, string name, string polishName)
        {
            var query = cities.AsQueryable();

            if (!string.IsNullOrWhiteSpace(sort))
            {
                var orderByQuery = _orderByExpressionCreator.CreateExpression(sort);
                query = orderByQuery(query);
            }

            var filterPredicate = CreateFilterPredicate(name, polishName, stateId);
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

        private static Expression<Func<City, bool>> CreateFilterPredicate(string name, string polishName, Guid? stateId)
        {
            Expression<Func<City, bool>> filterPredicate = null;
            if (!string.IsNullOrWhiteSpace(name))
                filterPredicate = x => x.Name.ToLower().StartsWith(name.ToLower());
            if (!string.IsNullOrWhiteSpace(polishName))
                filterPredicate = x => x.PolishName.ToLower().StartsWith(polishName.ToLower());
            if (stateId.HasValue)
                filterPredicate = filterPredicate.AndAlso(x => x.StateId == stateId.Value);

            return filterPredicate;
        }
    }
}