using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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
using Xunit;

namespace Riva.Users.Infrastructure.Test.DataAccessTests.UserRepositoryTests
{
    [Collection("RivaUsersSqlServer tests collection")]
    public class UserRepositoryTest : IClassFixture<UserRepositoryTestFixture>
    {
        private readonly RivaUsersDbContext _context;
        private readonly Mock<IOrderByExpressionCreator<UserEntity>> _orderByExpressionCreatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IUserRepository _repository;
        private readonly User _user;
        private readonly UserRepositoryTestFixture _fixture;

        public UserRepositoryTest(UserRepositoryTestFixture fixture)
        {
            _context = fixture.Context;
            _orderByExpressionCreatorMock = fixture.OrderByExpressionCreatorMock;
            _mapperMock = fixture.MapperMock;
            _repository = fixture.Repository;
            _user = fixture.User;
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Users_Collection()
        {
            var userEntities = await _context.Users.ToListAsync();
            var users = userEntities.Select(MapUserEntityToUser).ToList();

            _mapperMock.Setup(x => x.Map<List<UserEntity>, List<User>>(It.IsAny<List<UserEntity>>()))
                .Returns(users);

            var result = await _repository.GetAllAsync();

            result.Should().BeEquivalentTo(users);
        }

        [Fact]
        public async Task FindAsync_Should_Return_Users_Collection()
        {
            var email = _user.Email;
            var serviceActive = _user.ServiceActive;
            const string sort = "email:desc";
            const int skip = 0;
            const int take = 1;
            var userEntities = await _context.Users.Where(x => x.Email.ToLower().StartsWith(email.ToLower()) && x.ServiceActive == serviceActive)
                .OrderByDescending(x => x.Email).Skip(skip).Take(take).ToListAsync();
            var users = userEntities.Select(MapUserEntityToUser).ToList();
            IOrderedQueryable<UserEntity> OrderByExpression(IQueryable<UserEntity> o) => o.OrderByDescending(x => x.Email);

            _orderByExpressionCreatorMock.Setup(x => x.CreateExpression(It.IsAny<string>())).Returns(OrderByExpression);
            _mapperMock.Setup(x => x.Map<List<UserEntity>, List<User>>(It.IsAny<List<UserEntity>>()))
                .Returns(users);

            var result = await _repository.FindAsync(skip, take, sort, email, serviceActive);

            result.Should().BeEquivalentTo(users);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_User()
        {
            _mapperMock.Setup(x => x.Map<UserEntity, User>(It.IsAny<UserEntity>()))
                .Returns(_user);

            var result = await _repository.GetByIdAsync(_user.Id);

            result.Should().BeEquivalentTo(_user);
            result.FlatForRentAnnouncementPreferences.Should().BeEquivalentTo(_user.FlatForRentAnnouncementPreferences);
            result.RoomForRentAnnouncementPreferences.Should().BeEquivalentTo(_user.RoomForRentAnnouncementPreferences);
        }

        [Fact]
        public async Task AddAsync_Should_Add_User()
        {
            var user = User.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("AddAsync@email.com")
                .SetServiceActive(DefaultUserSettings.ServiceActive)
                .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                .Build();
            var userEntity = _fixture.CreateUserEntity(user);

            _mapperMock.Setup(x => x.Map<User, UserEntity>(It.IsAny<User>()))
                .Returns(userEntity);

            Func<Task> result = async () => await _repository.AddAsync(user);
            await result.Should().NotThrowAsync<Exception>();

            var addedUser = await _context.Users.FindAsync(user.Id);
            addedUser.Should().NotBeNull();
            addedUser.AnnouncementPreferences.Should().BeEmpty();
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_User()
        {
            var user = _fixture.InsertUser("UpdateAsync@email.com");
            var correlationId = Guid.NewGuid();
            user.ChangeServiceActive(!user.ServiceActive, correlationId);
            user.ChangeAnnouncementPreferenceLimit(10, correlationId);
            user.ChangeAnnouncementSendingFrequency(AnnouncementSendingFrequencyEnumeration.EveryHour, correlationId);
            var flatForRentAnnouncementPreferenceToUpdate = user.FlatForRentAnnouncementPreferences.First();
            flatForRentAnnouncementPreferenceToUpdate.ChangeCityId(Guid.NewGuid());
            flatForRentAnnouncementPreferenceToUpdate.ChangePriceMin(1000);
            flatForRentAnnouncementPreferenceToUpdate.ChangePriceMax(3000);
            flatForRentAnnouncementPreferenceToUpdate.ChangeRoomNumbersMin(2);
            flatForRentAnnouncementPreferenceToUpdate.ChangeRoomNumbersMax(3);
            flatForRentAnnouncementPreferenceToUpdate.ChangeCityDistricts(new List<Guid> { Guid.NewGuid() });
            var roomForRentAnnouncementPreferenceToUpdate = user.RoomForRentAnnouncementPreferences.First();
            roomForRentAnnouncementPreferenceToUpdate.ChangeCityId(Guid.NewGuid());
            roomForRentAnnouncementPreferenceToUpdate.ChangePriceMin(1000);
            roomForRentAnnouncementPreferenceToUpdate.ChangePriceMax(3000);
            roomForRentAnnouncementPreferenceToUpdate.ChangeRoomType(RoomTypeEnumeration.Double);
            roomForRentAnnouncementPreferenceToUpdate.ChangeCityDistricts(new List<Guid> { Guid.NewGuid() });
            var newFlatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(2)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            user.AddFlatForRentAnnouncementPreference(newFlatForRentAnnouncementPreference, correlationId);
            var newRoomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            user.AddRoomForRentAnnouncementPreference(newRoomForRentAnnouncementPreference, correlationId);


            Func<Task> result = async () => await _repository.UpdateAsync(user);
            await result.Should().NotThrowAsync<Exception>();

            var updatedUserEntity = await _context.Users.Include(x => x.AnnouncementPreferences)
                .SingleAsync(x => x.Id == user.Id);
            var updatedUser = MapUserEntityToUser(updatedUserEntity);
            updatedUser.AddEvents(user.DomainEvents);
            updatedUser.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_User()
        {
            var user = _fixture.InsertUser("DeleteAsync@email.com");

            Func<Task> result = async () => await _repository.DeleteAsync(user);
            await result.Should().NotThrowAsync<Exception>();

            var deletedUser = await _context.Users.FindAsync(user.Id);
            deletedUser.Should().BeNull();
        }

        [Fact]
        public async Task CountAsync_Should_Return_Number_Of_Users()
        {
            var expectedResult = await _context.Users.LongCountAsync();

            var result = await _repository.CountAsync();

            result.Should().Be(expectedResult);
        }

        [Fact]
        public async Task CountAsync_Should_Return_Number_Of_Users_For_Given_Parameters()
        {
            var expectedResult = await _context.Users.LongCountAsync(x => x.Email.StartsWith(_user.Email) && x.ServiceActive == _user.ServiceActive);

            var result = await _repository.CountAsync(_user.Email, _user.ServiceActive);

            result.Should().Be(expectedResult);
        }

        private static User MapUserEntityToUser(UserEntity userEntity)
        {
            var flatForRentAnnouncementPreferenceEntities = userEntity.AnnouncementPreferences.Where(a =>
                        a.AnnouncementPreferenceType == AnnouncementPreferenceType.FlatForRentAnnouncementPreference);
            var flatForRentAnnouncementPreferences = flatForRentAnnouncementPreferenceEntities.Select(f =>
                FlatForRentAnnouncementPreference.Builder()
                    .SetId(f.Id)
                    .SetCityId(f.CityId)
                    .SetPriceMin(f.PriceMin)
                    .SetPriceMax(f.PriceMax)
                    .SetRoomNumbersMin(f.RoomNumbersMin)
                    .SetRoomNumbersMax(f.RoomNumbersMax)
                    .SetCityDistricts(f.AnnouncementPreferenceCityDistricts.Select(a => a.CityDistrictId))
                    .Build()
            );
            var roomForRentAnnouncementPreferenceEntities = userEntity.AnnouncementPreferences.Where(a =>
                a.AnnouncementPreferenceType == AnnouncementPreferenceType.RoomForRentAnnouncementPreference);
            var roomForRentAnnouncementPreferences = roomForRentAnnouncementPreferenceEntities.Select(r =>
                RoomForRentAnnouncementPreference.Builder()
                    .SetId(r.Id)
                    .SetCityId(r.CityId)
                    .SetPriceMin(r.PriceMin)
                    .SetPriceMax(r.PriceMax)
                    .SetRoomType(r.RoomType.ConvertToEnumeration())
                    .SetCityDistricts(r.AnnouncementPreferenceCityDistricts.Select(a => a.CityDistrictId))
                    .Build()
            );
            return User.Builder()
                .SetId(userEntity.Id)
                .SetEmail(userEntity.Email)
                .SetServiceActive(userEntity.ServiceActive)
                .SetAnnouncementPreferenceLimit(userEntity.AnnouncementPreferenceLimit)
                .SetAnnouncementSendingFrequency(userEntity.AnnouncementSendingFrequency.ConvertToEnumeration())
                .SetFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences)
                .SetRoomForRentAnnouncementPreferences(roomForRentAnnouncementPreferences)
                .Build();
        }
    }
}