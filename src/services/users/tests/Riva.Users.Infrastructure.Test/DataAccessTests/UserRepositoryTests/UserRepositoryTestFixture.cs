using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Services;
using Riva.Users.Core.Enums;
using Riva.Users.Core.Extensions;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Defaults;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Domain.Users.Enumerations;
using Riva.Users.Domain.Users.Repositories;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Contexts;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Entities;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Repositories;

namespace Riva.Users.Infrastructure.Test.DataAccessTests.UserRepositoryTests
{
    public class UserRepositoryTestFixture
    {
        public RivaUsersDbContext Context { get; }
        public Mock<IOrderByExpressionCreator<UserEntity>> OrderByExpressionCreatorMock { get; }
        public Mock<IMapper> MapperMock { get; }
        public IUserRepository Repository { get; }
        public User User { get; }

        public UserRepositoryTestFixture(DatabaseFixture fixture)
        {
            Context = fixture.Context;
            OrderByExpressionCreatorMock = new Mock<IOrderByExpressionCreator<UserEntity>>();
            MapperMock = new Mock<IMapper>();
            Repository = new UserRepository(Context, OrderByExpressionCreatorMock.Object, MapperMock.Object);
            User = InsertUser("UserRepositoryTest@email.com");
        }

        public UserEntity CreateUserEntity(User user)
        {

            var flatForRentAnnouncementPreferenceEntities = user.FlatForRentAnnouncementPreferences.Select(x =>
                new AnnouncementPreferenceEntity
                {
                    Id = x.Id,
                    CityId = x.CityId,
                    PriceMin = x.PriceMin,
                    PriceMax = x.PriceMax,
                    RoomNumbersMin = x.RoomNumbersMin,
                    RoomNumbersMax = x.RoomNumbersMax,
                    AnnouncementPreferenceType = AnnouncementPreferenceType.FlatForRentAnnouncementPreference,
                    AnnouncementPreferenceCityDistricts = x.CityDistricts.Select(cd =>
                        new AnnouncementPreferenceCityDistrictEntity
                        {
                            CityDistrictId = cd,
                            AnnouncementPreferenceId = x.Id
                        }).ToList()
                });
            var roomForRentAnnouncementPreferenceEntities = user.RoomForRentAnnouncementPreferences.Select(x =>
                new AnnouncementPreferenceEntity
                {
                    Id = x.Id,
                    CityId = x.CityId,
                    PriceMin = x.PriceMin,
                    PriceMax = x.PriceMax,
                    RoomType = x.RoomType.ConvertToEnum(),
                    AnnouncementPreferenceType = AnnouncementPreferenceType.RoomForRentAnnouncementPreference,
                    AnnouncementPreferenceCityDistricts = x.CityDistricts.Select(cd =>
                        new AnnouncementPreferenceCityDistrictEntity
                        {
                            CityDistrictId = cd,
                            AnnouncementPreferenceId = x.Id
                        }).ToList()
                });
            
            return new UserEntity
            {
                Id = user.Id,
                Email = user.Email,
                ServiceActive = user.ServiceActive,
                AnnouncementPreferenceLimit = user.AnnouncementPreferenceLimit,
                AnnouncementSendingFrequency = user.AnnouncementSendingFrequency.ConvertToEnum(),
                AnnouncementPreferences = flatForRentAnnouncementPreferenceEntities.Concat(roomForRentAnnouncementPreferenceEntities).ToList()
            };
        }

        public User InsertUser(string email)
        {
            var user = CreateUser(email);
            Context.Users.Add(CreateUserEntity(user));
            Context.SaveChanges();
            return user;
        }

        private User CreateUser(string email)
        {
            var cityId = Guid.NewGuid();
            var cityDistricts = new List<Guid> { Guid.NewGuid() };
            var flatForRentAnnouncementPreferences = new List<FlatForRentAnnouncementPreference>
            {
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(2)
                    .SetCityDistricts(cityDistricts)
                    .Build()
            };
            var roomForRentAnnouncementPreferences = new List<RoomForRentAnnouncementPreference>
            {
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(cityId)
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(cityDistricts)
                    .Build()
            };

            return User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail(email)
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .SetFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences)
                .SetRoomForRentAnnouncementPreferences(roomForRentAnnouncementPreferences)
                .Build();
        }
    }
}