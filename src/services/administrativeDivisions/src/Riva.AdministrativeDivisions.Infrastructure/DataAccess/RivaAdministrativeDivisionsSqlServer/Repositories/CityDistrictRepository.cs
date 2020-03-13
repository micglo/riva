using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Aggregates;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Repositories;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Contexts;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;
using Riva.AdministrativeDivisions.Infrastructure.Models;
using Riva.BuildingBlocks.Core.Extensions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Services;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Extensions;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Repositories
{
    public class CityDistrictRepository : ICityDistrictRepository
    {
        private readonly RivaAdministrativeDivisionsDbContext _dbContext;
        private readonly DbSet<CityDistrictEntity> _cityDistrictEntityDbSet;
        private readonly DbSet<CityDistrictNameVariantEntity> _cityDistrictNameVariantEntityDbSet;
        private readonly IMapper _mapper;
        private readonly IOrderByExpressionCreator<CityDistrict> _orderByExpressionCreator;
        private readonly IMemoryCache _cache;

        public CityDistrictRepository(RivaAdministrativeDivisionsDbContext dbContext, IMapper mapper,
            IOrderByExpressionCreator<CityDistrict> orderByExpressionCreator, IMemoryCache cache)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _orderByExpressionCreator = orderByExpressionCreator;
            _cache = cache;
            _cityDistrictEntityDbSet = dbContext.Set<CityDistrictEntity>();
            _cityDistrictNameVariantEntityDbSet = dbContext.Set<CityDistrictNameVariantEntity>();
        }

        public async Task<List<CityDistrict>> GetAllAsync()
        {
            if (_cache.TryGetValue(CacheKeys.CityDistrictsKey, out List<CityDistrict> cityDistricts))
                return cityDistricts;

            var cityDistrictEntities = await _cityDistrictEntityDbSet
                .Include(x => x.NameVariants)
                .ToListAsync();
            cityDistricts = _mapper.Map<List<CityDistrictEntity>, List<CityDistrict>>(cityDistrictEntities);
            _cache.Set(CacheKeys.CityDistrictsKey, cityDistricts,
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(365)));

            return cityDistricts;
        }

        public async Task<List<CityDistrict>> FindAsync(int? skip, int? take, string sort, string name, 
            string polishName, Guid? cityId, Guid? parentId, IEnumerable<Guid> cityIds)
        {
            var cityDistricts = await GetAllAsync();
            return ProcessCityDistricts(cityDistricts, skip, take, sort, name, polishName, cityId, parentId, cityIds);
        }

        public async Task<CityDistrict> GetByIdAsync(Guid id)
        {
            var cityDistrictEntity = await GetCityDistrictEntityByIdWithNameVariantEntitiesAsync(id);
            return cityDistrictEntity != null
                ? _mapper.Map<CityDistrictEntity, CityDistrict>(cityDistrictEntity)
                : null;
        }

        public async Task<CityDistrict> GetByNameAndCityIdAsync(string name, Guid cityId)
        {
            var cityDistrictEntity = await _cityDistrictEntityDbSet.Include(x => x.NameVariants)
                .SingleOrDefaultAsync(x => x.Name.Equals(name) && x.CityId == cityId);
            return cityDistrictEntity != null
                ? _mapper.Map<CityDistrictEntity, CityDistrict>(cityDistrictEntity)
                : null;
        }

        public async Task<CityDistrict> GetByPolishNameAndCityIdAsync(string polishName, Guid cityId)
        {
            var cityDistrictEntity = await _cityDistrictEntityDbSet.Include(x => x.NameVariants)
                .SingleOrDefaultAsync(x => x.PolishName.Equals(polishName) && x.CityId == cityId);
            return cityDistrictEntity != null
                ? _mapper.Map<CityDistrictEntity, CityDistrict>(cityDistrictEntity)
                : null;
        }

        public async Task AddAsync(CityDistrict cityDistrict)
        {
            var cityDistrictEntity = _mapper.Map<CityDistrict, CityDistrictEntity>(cityDistrict);
            _cityDistrictEntityDbSet.Add(cityDistrictEntity);
            await _dbContext.SaveChangesAsync();
            cityDistrict.SetRowVersion(cityDistrictEntity.RowVersion);
            _cache.Remove(CacheKeys.CityDistrictsKey);
        }

        public async Task UpdateAsync(CityDistrict cityDistrict)
        {
            var cityDistrictEntity = await GetCityDistrictEntityByIdWithNameVariantEntitiesAsync(cityDistrict.Id);

            UpdateCityDistrictEntityAttributes(cityDistrictEntity, cityDistrict);
            UpdateCityDistrictNameVariantEntities(cityDistrictEntity, cityDistrict);

            _dbContext.Entry(cityDistrictEntity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            cityDistrict.SetRowVersion(cityDistrictEntity.RowVersion);
            _cache.Remove(CacheKeys.CityDistrictsKey);
        }

        public async Task DeleteAsync(CityDistrict cityDistrict)
        {
            var cityDistrictEntity = new CityDistrictEntity
            {
                Id = cityDistrict.Id,
                RowVersion = cityDistrict.RowVersion.ToArray()
            };
            _dbContext.AttachEntity(cityDistrictEntity);
            _dbContext.Entry(cityDistrictEntity).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
            _cache.Remove(CacheKeys.CityDistrictsKey);
        }

        public async Task<long> CountAsync()
        {
            var cityDistricts = await GetAllAsync();
            return cityDistricts.LongCount();
        }

        public async Task<long> CountAsync(string name, string polishName, Guid? cityId, Guid? parentId, IEnumerable<Guid> cityIds)
        {
            var cityDistricts = await GetAllAsync();
            var cityDistrictsQuery = cityDistricts.AsQueryable();
            var filterPredicate = CreateFilterPredicate(name, polishName, cityId, parentId, cityIds.ToList());

            if (filterPredicate != null)
                cityDistrictsQuery = cityDistrictsQuery.Where(filterPredicate);

            return cityDistrictsQuery.LongCount();
        }

        private List<CityDistrict> ProcessCityDistricts(IEnumerable<CityDistrict> cityDistrict, int? skip, int? take, string sort, 
            string name, string polishName, Guid? cityId, Guid? parentId, IEnumerable<Guid> cityIds)
        {
            var query = cityDistrict.AsQueryable();

            if (!string.IsNullOrWhiteSpace(sort))
            {
                var orderByQuery = _orderByExpressionCreator.CreateExpression(sort);
                query = orderByQuery(query);
            }

            var filterPredicate = CreateFilterPredicate(name, polishName, cityId, parentId, cityIds.ToList());
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

        private static Expression<Func<CityDistrict, bool>> CreateFilterPredicate(string name, string polishName, Guid? cityId, Guid? parentId, ICollection<Guid> cityIds)
        {
            Expression<Func<CityDistrict, bool>> filterPredicate = null;
            if (!string.IsNullOrWhiteSpace(name))
                filterPredicate = x => x.Name.ToLower().StartsWith(name.ToLower());
            if (!string.IsNullOrWhiteSpace(polishName))
                filterPredicate = x => x.PolishName.ToLower().StartsWith(polishName.ToLower());
            if (cityId.HasValue)
                filterPredicate = filterPredicate.AndAlso(x => x.CityId == cityId.Value);
            if (parentId.HasValue)
                filterPredicate = filterPredicate.AndAlso(x => x.ParentId == parentId.Value);
            if (cityIds != null && cityIds.Any())
                filterPredicate = filterPredicate.AndAlso(x => cityIds.Contains(x.CityId));

            return filterPredicate;
        }

        private async Task<CityDistrictEntity> GetCityDistrictEntityByIdWithNameVariantEntitiesAsync(Guid id)
        {
            return await _cityDistrictEntityDbSet.Include(x => x.NameVariants)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        private static void UpdateCityDistrictEntityAttributes(CityDistrictEntity cityDistrictEntity, CityDistrict cityDistrict)
        {
            cityDistrictEntity.Name = cityDistrict.Name;
            cityDistrictEntity.PolishName = cityDistrict.PolishName;
            cityDistrictEntity.CityId = cityDistrict.CityId;
            cityDistrictEntity.ParentId = cityDistrict.ParentId;
        }

        private void UpdateCityDistrictNameVariantEntities(CityDistrictEntity cityDistrictEntity, CityDistrict cityDistrict)
        {
            _cityDistrictNameVariantEntityDbSet.RemoveRange(cityDistrictEntity.NameVariants);

            var nameVariantEntities = cityDistrict.NameVariants.Select(x => new CityDistrictNameVariantEntity
            {
                Id = Guid.NewGuid(),
                Value = x,
                CityDistrictId = cityDistrict.Id
            });
            _cityDistrictNameVariantEntityDbSet.AddRange(nameVariantEntities);
        }
    }
}