using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Extensions;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Enumerations;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Repositories;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Entities;
using Riva.BuildingBlocks.Core.Extensions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Services;

namespace Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Repositories
{
    public class RoomForRentAnnouncementRepository : IRoomForRentAnnouncementRepository
    {
        private readonly ICosmosStore<RoomForRentAnnouncementEntity> _cosmosStore;
        private readonly IMapper _mapper;
        private readonly IOrderByExpressionCreator<RoomForRentAnnouncementEntity> _orderByExpressionCreator;

        public RoomForRentAnnouncementRepository(ICosmosStore<RoomForRentAnnouncementEntity> cosmosStore, IMapper mapper, 
            IOrderByExpressionCreator<RoomForRentAnnouncementEntity> orderByExpressionCreator)
        {
            _cosmosStore = cosmosStore;
            _mapper = mapper;
            _orderByExpressionCreator = orderByExpressionCreator;
        }

        public async Task<List<RoomForRentAnnouncement>> GetAllAsync()
        {
            var roomForRentAnnouncementEntities = await _cosmosStore.Query().ToListAsync();
            return _mapper.Map<List<RoomForRentAnnouncementEntity>, List<RoomForRentAnnouncement>>(
                roomForRentAnnouncementEntities);
        }

        public async Task<List<RoomForRentAnnouncement>> FindAsync(int? pageNumber, int? pageSize, string sort, Guid? cityId, 
            DateTimeOffset? createdFrom, DateTimeOffset? createdTo, decimal? priceFrom, decimal? priceTo, Guid? cityDistrict, 
            RoomTypeEnumeration roomType)
        {
            var roomForRentAnnouncementEntities = await FindRoomForRentAnnouncementEntitiesAsync(pageNumber, pageSize,
                sort, cityId, createdFrom, createdTo, priceFrom, priceTo, cityDistrict, roomType);
            return _mapper.Map<List<RoomForRentAnnouncementEntity>, List<RoomForRentAnnouncement>>(roomForRentAnnouncementEntities);
        }

        public async Task<RoomForRentAnnouncement> GetByIdAsync(Guid id)
        {
            var roomForRentAnnouncementEntity = await _cosmosStore.FindAsync(id.ToString());
            return roomForRentAnnouncementEntity != null
                ? _mapper.Map<RoomForRentAnnouncementEntity, RoomForRentAnnouncement>(roomForRentAnnouncementEntity)
                : null;
        }

        public Task AddAsync(RoomForRentAnnouncement roomForRentAnnouncement)
        {
            var roomForRentAnnouncementEntity =
                _mapper.Map<RoomForRentAnnouncement, RoomForRentAnnouncementEntity>(roomForRentAnnouncement);
            return _cosmosStore.AddAsync(roomForRentAnnouncementEntity);
        }

        public Task UpdateAsync(RoomForRentAnnouncement roomForRentAnnouncement)
        {
            var roomForRentAnnouncementEntity =
                _mapper.Map<RoomForRentAnnouncement, RoomForRentAnnouncementEntity>(roomForRentAnnouncement);
            return _cosmosStore.UpdateAsync(roomForRentAnnouncementEntity);
        }

        public Task DeleteAsync(RoomForRentAnnouncement roomForRentAnnouncement)
        {
            return _cosmosStore.RemoveByIdAsync(roomForRentAnnouncement.Id.ToString());
        }

        public Task<int> CountAsync()
        {
            return _cosmosStore.Query().CountAsync();
        }

        public Task<int> CountAsync(Guid? cityId, DateTimeOffset? createdFrom, DateTimeOffset? createdTo, decimal? priceFrom,
            decimal? priceTo, Guid? cityDistrict, RoomTypeEnumeration roomType)
        {
            var filterPredicate = CreateFilterPredicate(cityId, createdFrom, createdTo, priceFrom, priceTo,
                cityDistrict, roomType);

            return filterPredicate != null
                ? _cosmosStore.Query().CountAsync(filterPredicate)
                : _cosmosStore.Query().CountAsync();
        }

        private async Task<List<RoomForRentAnnouncementEntity>> FindRoomForRentAnnouncementEntitiesAsync(int? pageNumber, int? pageSize, 
            string sort, Guid? cityId, DateTimeOffset? createdFrom, DateTimeOffset? createdTo, decimal? priceFrom, decimal? priceTo,
            Guid? cityDistrict, RoomTypeEnumeration roomType)
        {
            var query = _cosmosStore.Query();

            if (!string.IsNullOrWhiteSpace(sort))
            {
                var orderByQuery = _orderByExpressionCreator.CreateExpression(sort);
                query = orderByQuery(query);
            }

            var filterPredicate = CreateFilterPredicate(cityId, createdFrom, createdTo, priceFrom, priceTo,
                cityDistrict, roomType);
            query = filterPredicate != null ? query.Where(filterPredicate) : query;

            if (pageNumber.HasValue && pageSize.HasValue)
                query = query.WithPagination(pageNumber.Value, pageSize.Value);

            return await query.ToListAsync();
        }

        private static Expression<Func<RoomForRentAnnouncementEntity, bool>> CreateFilterPredicate(Guid? cityId, DateTimeOffset? createdFrom,
            DateTimeOffset? createdTo, decimal? priceFrom, decimal? priceTo, Guid? cityDistrict, RoomTypeEnumeration roomType)
        {
            Expression<Func<RoomForRentAnnouncementEntity, bool>> filterPredicate = null;
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
            if (cityDistrict.HasValue)
                filterPredicate = filterPredicate.AndAlso(x => x.CityDistricts.Contains(cityDistrict.Value));
            if (roomType != null)
                filterPredicate = filterPredicate.AndAlso(x => x.RoomTypes.Select(r => r.ToString()).Contains(roomType.DisplayName));

            return filterPredicate;
        }
    }
}