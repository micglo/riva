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
using Riva.Users.Core.Enums;
using Riva.Users.Core.Extensions;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Domain.Users.Repositories;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Contexts;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Entities;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly RivaUsersDbContext _context;
        private readonly IOrderByExpressionCreator<UserEntity> _orderByExpressionCreator;
        private readonly IMapper _mapper;
        private readonly DbSet<UserEntity> _usersDbSet;
        private readonly DbSet<AnnouncementPreferenceEntity> _announcementPreferencesDbSet;

        public UserRepository(RivaUsersDbContext context, IOrderByExpressionCreator<UserEntity> orderByExpressionCreator, IMapper mapper)
        {
            _context = context;
            _orderByExpressionCreator = orderByExpressionCreator;
            _mapper = mapper;
            _usersDbSet = _context.Set<UserEntity>();
            _announcementPreferencesDbSet = _context.Set<AnnouncementPreferenceEntity>();
        }

        public async Task<List<User>> GetAllAsync()
        {
            var entities = await _usersDbSet.ToListAsync();
            return _mapper.Map<List<UserEntity>, List<User>>(entities);
        }

        public async Task<List<User>> FindAsync(int? skip, int? take, string sort, string email, bool? serviceActive)
        {
            var entities = await FindUserEntitiesAsync(skip, take, sort, email, serviceActive);
            return _mapper.Map<List<UserEntity>, List<User>>(entities);
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var entity = await GetUserEntityByIdWithAnnouncementPreferences(id);
            return entity != null ? _mapper.Map<UserEntity, User>(entity) : null;
        }

        public async Task AddAsync(User user)
        {
            var entity = _mapper.Map<User, UserEntity>(user);
            _usersDbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            var entity = await GetUserEntityByIdWithAnnouncementPreferences(user.Id);
            await UpdateFlatForRentAnnouncementPreferencesAsync(user.Id, user.FlatForRentAnnouncementPreferences.ToList());
            await UpdateRoomForRentAnnouncementPreferencesAsync(user.Id, user.RoomForRentAnnouncementPreferences.ToList());
            entity.Email = user.Email;
            entity.ServiceActive = user.ServiceActive;
            entity.AnnouncementPreferenceLimit = user.AnnouncementPreferenceLimit;
            entity.AnnouncementSendingFrequency = user.AnnouncementSendingFrequency.ConvertToEnum();
            entity.Picture = user.Picture;
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            var entity = new UserEntity { Id = user.Id };
            _context.AttachEntity(entity);
            _context.Entry(entity).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public Task<long> CountAsync()
        {
            return _usersDbSet.LongCountAsync();
        }

        public Task<long> CountAsync(string email, bool? serviceActive)
        {
            var predicate = CreateFilterPredicate(email, serviceActive);
            return predicate != null ? _usersDbSet.LongCountAsync(predicate) : _usersDbSet.LongCountAsync();
        }

        private async Task<List<UserEntity>> FindUserEntitiesAsync(int? skip, int? take, string sort, string email, bool? serviceActive)
        {
            IQueryable<UserEntity> query = _usersDbSet;

            if (!string.IsNullOrWhiteSpace(sort))
            {
                var orderByQuery = _orderByExpressionCreator.CreateExpression(sort);
                query = orderByQuery(query);
            }

            var predicate = CreateFilterPredicate(email, serviceActive);
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

        private static Expression<Func<UserEntity, bool>> CreateFilterPredicate(string email, bool? serviceActive)
        {
            Expression<Func<UserEntity, bool>> predicate = null;
            if (!string.IsNullOrWhiteSpace(email))
                predicate = x => x.Email.ToLower().StartsWith(email.ToLower());
            if (serviceActive.HasValue)
                predicate = predicate.AndAlso(x => x.ServiceActive == serviceActive.Value);

            return predicate;
        }

        private Task<UserEntity> GetUserEntityByIdWithAnnouncementPreferences(Guid id)
        {
            return _usersDbSet.Include(x => x.AnnouncementPreferences)
                .ThenInclude(x => x.AnnouncementPreferenceCityDistricts)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        private async Task UpdateFlatForRentAnnouncementPreferencesAsync(Guid userId, ICollection<FlatForRentAnnouncementPreference> flatForRentAnnouncementPreferences)
        {
            var announcementPreferenceEntities = await _announcementPreferencesDbSet.Where(x =>
                x.UserId == userId && x.AnnouncementPreferenceType ==
                AnnouncementPreferenceType.FlatForRentAnnouncementPreference).ToListAsync();
            foreach (var flatForRentAnnouncementPreference in flatForRentAnnouncementPreferences)
            {
                var announcementPreferenceEntity = announcementPreferenceEntities.SingleOrDefault(x => x.Id == flatForRentAnnouncementPreference.Id);
                if (announcementPreferenceEntity is null)
                {
                    AddNewFlatForRentAnnouncementPreferenceEntity(flatForRentAnnouncementPreference, userId);
                }
                else
                {
                    UpdateFlatForRentAnnouncementPreferenceEntity(announcementPreferenceEntity, flatForRentAnnouncementPreference);
                    await UpdateCityDistrictsAsync(announcementPreferenceEntity, flatForRentAnnouncementPreference.CityDistricts.ToList());
                }
            }

            var announcementPreferenceEntityIds = announcementPreferenceEntities.Select(x => x.Id);
            var flatForRentAnnouncementPreferenceIds = flatForRentAnnouncementPreferences.Select(x => x.Id);
            var announcementPreferenceEntityIdsToRemove = announcementPreferenceEntityIds.Except(flatForRentAnnouncementPreferenceIds);
            var announcementPreferenceEntitiesToRemove =
                announcementPreferenceEntities.Where(x => announcementPreferenceEntityIdsToRemove.Contains(x.Id));
            _announcementPreferencesDbSet.RemoveRange(announcementPreferenceEntitiesToRemove);
        }

        private void AddNewFlatForRentAnnouncementPreferenceEntity(FlatForRentAnnouncementPreference flatForRentAnnouncementPreference, Guid userId)
        {
            var announcementPreferenceEntity = new AnnouncementPreferenceEntity
            {
                Id = flatForRentAnnouncementPreference.Id,
                UserId = userId,
                CityId = flatForRentAnnouncementPreference.CityId,
                PriceMin = flatForRentAnnouncementPreference.PriceMin,
                PriceMax = flatForRentAnnouncementPreference.PriceMax,
                RoomNumbersMin = flatForRentAnnouncementPreference.RoomNumbersMin,
                RoomNumbersMax = flatForRentAnnouncementPreference.RoomNumbersMax,
                AnnouncementPreferenceType = AnnouncementPreferenceType.FlatForRentAnnouncementPreference,
                AnnouncementPreferenceCityDistricts = flatForRentAnnouncementPreference.CityDistricts.Select(
                    cd =>
                        new AnnouncementPreferenceCityDistrictEntity
                        {
                            CityDistrictId = cd,
                            AnnouncementPreferenceId = flatForRentAnnouncementPreference.Id
                        }).ToList()
            };
            _announcementPreferencesDbSet.Add(announcementPreferenceEntity);
        }

        private static void UpdateFlatForRentAnnouncementPreferenceEntity(AnnouncementPreferenceEntity announcementPreferenceEntity, 
            FlatForRentAnnouncementPreference flatForRentAnnouncementPreference)
        {
            announcementPreferenceEntity.CityId = flatForRentAnnouncementPreference.CityId;
            announcementPreferenceEntity.PriceMin = flatForRentAnnouncementPreference.PriceMin;
            announcementPreferenceEntity.PriceMax = flatForRentAnnouncementPreference.PriceMax;
            announcementPreferenceEntity.RoomNumbersMin = flatForRentAnnouncementPreference.RoomNumbersMin;
            announcementPreferenceEntity.RoomNumbersMax = flatForRentAnnouncementPreference.RoomNumbersMax;
        }

        private async Task UpdateRoomForRentAnnouncementPreferencesAsync(Guid userId, ICollection<RoomForRentAnnouncementPreference> roomForRentAnnouncementPreferences)
        {
            var announcementPreferenceEntities = await _announcementPreferencesDbSet.Where(x =>
                x.UserId == userId && x.AnnouncementPreferenceType ==
                AnnouncementPreferenceType.RoomForRentAnnouncementPreference).ToListAsync();
            foreach (var roomForRentAnnouncementPreference in roomForRentAnnouncementPreferences)
            {
                var announcementPreferenceEntity = announcementPreferenceEntities.SingleOrDefault(x => x.Id == roomForRentAnnouncementPreference.Id);
                if (announcementPreferenceEntity is null)
                {
                    AddNewRoomForRentAnnouncementPreferenceEntity(roomForRentAnnouncementPreference, userId);
                }
                else
                {
                    UpdateRoomForRentAnnouncementPreferenceEntity(announcementPreferenceEntity, roomForRentAnnouncementPreference);
                    await UpdateCityDistrictsAsync(announcementPreferenceEntity, roomForRentAnnouncementPreference.CityDistricts.ToList());
                }
            }

            var announcementPreferenceEntityIds = announcementPreferenceEntities.Select(x => x.Id);
            var flatForRentAnnouncementPreferenceIds = roomForRentAnnouncementPreferences.Select(x => x.Id);
            var announcementPreferenceEntityIdsToRemove = announcementPreferenceEntityIds.Except(flatForRentAnnouncementPreferenceIds);
            var announcementPreferenceEntitiesToRemove =
                announcementPreferenceEntities.Where(x => announcementPreferenceEntityIdsToRemove.Contains(x.Id));
            _announcementPreferencesDbSet.RemoveRange(announcementPreferenceEntitiesToRemove);
        }

        private void AddNewRoomForRentAnnouncementPreferenceEntity(RoomForRentAnnouncementPreference roomForRentAnnouncementPreference, Guid userId)
        {
            var announcementPreferenceEntity = new AnnouncementPreferenceEntity
            {
                Id = roomForRentAnnouncementPreference.Id,
                UserId = userId,
                CityId = roomForRentAnnouncementPreference.CityId,
                PriceMin = roomForRentAnnouncementPreference.PriceMin,
                PriceMax = roomForRentAnnouncementPreference.PriceMax,
                RoomType = roomForRentAnnouncementPreference.RoomType.ConvertToEnum(),
                AnnouncementPreferenceType = AnnouncementPreferenceType.RoomForRentAnnouncementPreference,
                AnnouncementPreferenceCityDistricts = roomForRentAnnouncementPreference.CityDistricts.Select(
                    cd =>
                        new AnnouncementPreferenceCityDistrictEntity
                        {
                            CityDistrictId = cd,
                            AnnouncementPreferenceId = roomForRentAnnouncementPreference.Id
                        }).ToList()
            };
            _announcementPreferencesDbSet.Add(announcementPreferenceEntity);
        }

        private static void UpdateRoomForRentAnnouncementPreferenceEntity(AnnouncementPreferenceEntity announcementPreferenceEntity,
            RoomForRentAnnouncementPreference roomForRentAnnouncementPreference)
        {
            announcementPreferenceEntity.CityId = roomForRentAnnouncementPreference.CityId;
            announcementPreferenceEntity.PriceMin = roomForRentAnnouncementPreference.PriceMin;
            announcementPreferenceEntity.PriceMax = roomForRentAnnouncementPreference.PriceMax;
            announcementPreferenceEntity.RoomType = roomForRentAnnouncementPreference.RoomType.ConvertToEnum();
        }

        private async Task UpdateCityDistrictsAsync(AnnouncementPreferenceEntity announcementPreferenceEntity, ICollection<Guid> cityDistricts)
        {
            var currentCityDistrictIds = announcementPreferenceEntity.AnnouncementPreferenceCityDistricts.Select(x => x.CityDistrictId).ToList();
            var cityDistrictIdsToAdd = cityDistricts.Except(currentCityDistrictIds).ToArray();
            var cityDistrictIdsToRemove = currentCityDistrictIds.Except(cityDistricts).ToArray();

            if (cityDistrictIdsToAdd.Any())
            {
                foreach (var cityDistrictId in cityDistrictIdsToAdd)
                {
                    announcementPreferenceEntity.AnnouncementPreferenceCityDistricts.Add(new AnnouncementPreferenceCityDistrictEntity
                    {
                        CityDistrictId = cityDistrictId,
                        AnnouncementPreferenceId = announcementPreferenceEntity.Id
                    });
                }
            }

            if (cityDistrictIdsToRemove.Any())
            {
                var announcementPreferenceCityDistrictsToRemove = await _context.AnnouncementPreferenceCityDistricts
                    .Where(x => cityDistrictIdsToRemove.Contains(x.CityDistrictId)).ToListAsync();
                foreach (var announcementPreferenceCityDistrict in announcementPreferenceCityDistrictsToRemove)
                {
                    announcementPreferenceEntity.AnnouncementPreferenceCityDistricts.Remove(announcementPreferenceCityDistrict);
                }
            }
        }
    }
}