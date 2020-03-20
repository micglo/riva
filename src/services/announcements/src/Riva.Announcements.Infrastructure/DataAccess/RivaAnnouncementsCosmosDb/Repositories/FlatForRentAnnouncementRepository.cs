using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Extensions;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Repositories;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Enumerations;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Entities;
using Riva.BuildingBlocks.Core.Extensions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Services;

namespace Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Repositories
{
    public class FlatForRentAnnouncementRepository : IFlatForRentAnnouncementRepository
    {
        private readonly ICosmosStore<FlatForRentAnnouncementEntity> _cosmosStore;
        private readonly IMapper _mapper;
        private readonly IOrderByExpressionCreator<FlatForRentAnnouncementEntity> _orderByExpressionCreator;

        public FlatForRentAnnouncementRepository(ICosmosStore<FlatForRentAnnouncementEntity> cosmosStore, IMapper mapper,
            IOrderByExpressionCreator<FlatForRentAnnouncementEntity> orderByExpressionCreator)
        {
            _cosmosStore = cosmosStore;
            _mapper = mapper;
            _orderByExpressionCreator = orderByExpressionCreator;
        }

        public async Task<List<FlatForRentAnnouncement>> GetAllAsync()
        {
            var flatForRentAnnouncementEntities = await _cosmosStore.Query().ToListAsync();
            return _mapper.Map<List<FlatForRentAnnouncementEntity>, List<FlatForRentAnnouncement>>(
                flatForRentAnnouncementEntities);
        }

        public async Task<List<FlatForRentAnnouncement>> FindAsync(int? pageNumber, int? pageSize, string sort, Guid? cityId,
            DateTimeOffset? createdFrom, DateTimeOffset? createdTo, decimal? priceFrom, decimal? priceTo, Guid? cityDistrict,
            NumberOfRoomsEnumeration numberOfRooms)
        {
            var flatForRentAnnouncementEntities = await FindFlatForRentAnnouncementEntitiesAsync(pageNumber, pageSize,
                sort, cityId, createdFrom, createdTo, priceFrom, priceTo, cityDistrict, numberOfRooms);
            return _mapper.Map<List<FlatForRentAnnouncementEntity>, List<FlatForRentAnnouncement>>(flatForRentAnnouncementEntities);
        }

        public async Task<FlatForRentAnnouncement> GetByIdAsync(Guid id)
        {
            var flatForRentAnnouncementEntity = await _cosmosStore.FindAsync(id.ToString());
            return flatForRentAnnouncementEntity != null
                ? _mapper.Map<FlatForRentAnnouncementEntity, FlatForRentAnnouncement>(flatForRentAnnouncementEntity)
                : null;
        }

        public Task AddAsync(FlatForRentAnnouncement flatForRentAnnouncement)
        {
            var flatForRentAnnouncementEntity =
                _mapper.Map<FlatForRentAnnouncement, FlatForRentAnnouncementEntity>(flatForRentAnnouncement);
            return _cosmosStore.AddAsync(flatForRentAnnouncementEntity);
        }

        public Task UpdateAsync(FlatForRentAnnouncement flatForRentAnnouncement)
        {
            var flatForRentAnnouncementEntity =
                _mapper.Map<FlatForRentAnnouncement, FlatForRentAnnouncementEntity>(flatForRentAnnouncement);
            return _cosmosStore.UpdateAsync(flatForRentAnnouncementEntity);
        }

        public Task DeleteAsync(FlatForRentAnnouncement flatForRentAnnouncement)
        {
            return _cosmosStore.RemoveByIdAsync(flatForRentAnnouncement.Id.ToString());
        }

        public Task<int> CountAsync()
        {
            return _cosmosStore.Query().CountAsync();
        }

        public Task<int> CountAsync(Guid? cityId, DateTimeOffset? createdFrom, DateTimeOffset? createdTo, decimal? priceFrom,
            decimal? priceTo, Guid? cityDistrict, NumberOfRoomsEnumeration numberOfRooms)
        {
            var filterPredicate = CreateFilterPredicate(cityId, createdFrom, createdTo, priceFrom, priceTo,
                cityDistrict, numberOfRooms);

            return filterPredicate != null
                ? _cosmosStore.Query().CountAsync(filterPredicate)
                : _cosmosStore.Query().CountAsync();
        }

        private async Task<List<FlatForRentAnnouncementEntity>> FindFlatForRentAnnouncementEntitiesAsync(int? pageNumber, int? pageSize,
            string sort, Guid? cityId, DateTimeOffset? createdFrom, DateTimeOffset? createdTo, decimal? priceFrom, decimal? priceTo,
            Guid? cityDistrict, NumberOfRoomsEnumeration numberOfRooms)
        {
            var query = _cosmosStore.Query();

            if (!string.IsNullOrWhiteSpace(sort))
            {
                var orderByQuery = _orderByExpressionCreator.CreateExpression(sort);
                query = orderByQuery(query);
            }

            var filterPredicate = CreateFilterPredicate(cityId, createdFrom, createdTo, priceFrom, priceTo,
                cityDistrict, numberOfRooms);
            query = filterPredicate != null ? query.Where(filterPredicate) : query;

            if (pageNumber.HasValue && pageSize.HasValue)
                query = query.WithPagination(pageNumber.Value, pageSize.Value);

            return await query.ToListAsync();
        }

        private static Expression<Func<FlatForRentAnnouncementEntity, bool>> CreateFilterPredicate(Guid? cityId, DateTimeOffset? createdFrom,
            DateTimeOffset? createdTo, decimal? priceFrom, decimal? priceTo, Guid? cityDistrict, NumberOfRoomsEnumeration numberOfRooms)
        {
            Expression<Func<FlatForRentAnnouncementEntity, bool>> filterPredicate = null;
            if (cityId.HasValue)
                filterPredicate = x => x.CityId == cityId.Value;
            if (createdFrom.HasValue)
                filterPredicate = filterPredicate.AndAlso(x => x.Created >= createdFrom.Value);
            if (createdTo.HasValue)
                filterPredicate = filterPredicate.AndAlso(x => x.Created <= createdTo.Value);
            if (priceFrom.HasValue)
                filterPredicate = filterPredicate.AndAlso(x => x.Price >= priceFrom.Value);
            if (priceTo.HasValue)
                filterPredicate = filterPredicate.AndAlso(x => x.Price <= priceTo.Value);
            if (numberOfRooms != null)
                filterPredicate = filterPredicate.AndAlso(x => x.NumberOfRooms.ToString().Equals(numberOfRooms.DisplayName));
            if (cityDistrict.HasValue)
                filterPredicate = filterPredicate.AndAlso(x => x.CityDistricts.Contains(cityDistrict.Value));

            return filterPredicate;
        }
    }
}