using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Riva.AdministrativeDivisions.Domain.Cities.Aggregates;
using Riva.AdministrativeDivisions.Domain.Cities.Repositories;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Contexts;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;
using Riva.AdministrativeDivisions.Infrastructure.Models;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Services;
using Xunit;

namespace Riva.AdministrativeDivisions.Infrastructure.Test.DataAccessTests.RivaAdministrativeDivisionsSqlServerTests.CityRepositoryTests
{
    [Collection("RivaAdministrativeDivisionsSqlServer tests collection")]
    public class CityRepositoryTest : IClassFixture<CityRepositoryTestFixture>
    {
        private readonly RivaAdministrativeDivisionsDbContext _dbContext;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IOrderByExpressionCreator<City>> _orderByExpressionCreatorMock;
        private readonly Mock<IMemoryCache> _memoryCacheMock;
        private readonly ICityRepository _repository;
        private readonly City _city;
        private readonly StateEntity _stateEntity;
        private readonly CityRepositoryTestFixture _fixture;

        public CityRepositoryTest(CityRepositoryTestFixture fixture)
        {
            _dbContext = fixture.DbContext;
            _mapperMock = fixture.MapperMock;
            _orderByExpressionCreatorMock = fixture.OrderByExpressionCreatorMock;
            _memoryCacheMock = fixture.MemoryCacheMock;
            _repository = fixture.Repository;
            _city = fixture.City;
            _stateEntity = fixture.StateEntity;
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Cities_Collection_From_Cache()
        {
            var cities = new List<City> { _city };
            object cachedStates = cities;

            _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedStates)).Returns(true);

            var result = await _repository.GetAllAsync();

            result.Should().BeEquivalentTo(cities);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Cities_Collection_From_Database()
        {
            var cityEntities = await _dbContext.Cities.ToListAsync();
            var cities = cityEntities.Select(x =>
                City.Builder()
                    .SetId(x.Id)
                    .SetRowVersion(x.RowVersion)
                    .SetName(x.Name)
                    .SetPolishName(x.PolishName)
                    .SetStateId(x.StateId)
                    .Build()
            ).ToList();
            object cachedCities = null;
            var cacheEntryMock = new Mock<ICacheEntry>();

            _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedCities)).Returns(false);
            _memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock.Object);
            _mapperMock.Setup(x => x.Map<List<CityEntity>, List<City>>(It.IsAny<List<CityEntity>>()))
                .Returns(cities);

            var result = await _repository.GetAllAsync();

            result.Should().BeEquivalentTo(cities);
        }

        [Fact]
        public async Task FindAsync_Should_Return_Cities_Collection()
        {
            var cities = new List<City> { _city };
            object cachedCities = cities;
            IOrderedQueryable<City> OrderByExpression(IQueryable<City> o) => o.OrderByDescending(x => x.Name);

            _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedCities)).Returns(true);
            _orderByExpressionCreatorMock.Setup(x => x.CreateExpression(It.IsAny<string>())).Returns(OrderByExpression);

            var result = await _repository.FindAsync(1, 100, "name:asc", _city.StateId, _city.Name, _city.PolishName);

            result.Should().BeEquivalentTo(cities);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_City()
        {
            _mapperMock.Setup(x => x.Map<CityEntity, City>(It.IsAny<CityEntity>()))
                .Returns(_city);

            var result = await _repository.GetByIdAsync(_city.Id);

            result.Should().BeEquivalentTo(_city);
        }

        [Fact]
        public async Task GetByNameAndStateIdAsync_Should_Return_City()
        {
            _mapperMock.Setup(x => x.Map<CityEntity, City>(It.IsAny<CityEntity>()))
                .Returns(_city);

            var result = await _repository.GetByNameAndStateIdAsync(_city.Name, _city.StateId);

            result.Should().BeEquivalentTo(_city);
        }

        [Fact]
        public async Task GetByPolishNameAndStateIdAsync_Should_Return_City()
        {
            _mapperMock.Setup(x => x.Map<CityEntity, City>(It.IsAny<CityEntity>()))
                .Returns(_city);

            var result = await _repository.GetByPolishNameAndStateIdAsync(_city.PolishName, _city.StateId);

            result.Should().BeEquivalentTo(_city);
        }

        [Fact]
        public async Task AddAsync_Should_Add_City()
        {
            var city = City.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("CityRepositoryTestAddAsync")
                .SetPolishName("CityRepositoryTestAddAsync")
                .SetStateId(_stateEntity.Id)
                .Build();
            var cityEntity = new CityEntity
            {
                Id = city.Id,
                Name = city.Name,
                PolishName = city.PolishName,
                RowVersion = city.RowVersion.ToArray(),
                StateId = city.StateId
            };

            _mapperMock.Setup(x => x.Map<City, CityEntity>(It.IsAny<City>()))
                .Returns(cityEntity);
            _memoryCacheMock.Setup(x => x.Remove(It.IsAny<object>()));

            Func<Task> result = async () => await _repository.AddAsync(city);
            await result.Should().NotThrowAsync<Exception>();

            var addedCity = await _dbContext.Cities.FindAsync(city.Id);
            addedCity.Should().NotBeNull();
            _memoryCacheMock.Verify(x => x.Remove(It.Is<object>(key => ReferenceEquals(key, CacheKeys.CitiesKey))));
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_City()
        {
            var city = _fixture.InsertCity("CityRepositoryTestUpdateAsync");
            city.ChangeName("CityRepositoryTestUpdateAsyncNewName");
            var cityEntity = new CityEntity
            {
                Id = city.Id,
                Name = city.Name,
                PolishName = city.PolishName,
                RowVersion = city.RowVersion.ToArray(),
                StateId = city.StateId
            };

            _mapperMock.Setup(x => x.Map<City, CityEntity>(It.IsAny<City>()))
                .Returns(cityEntity);
            _memoryCacheMock.Setup(x => x.Remove(It.IsAny<object>()));

            Func<Task> result = async () => await _repository.UpdateAsync(city);
            await result.Should().NotThrowAsync<Exception>();

            var updatedCity = await _dbContext.Cities.FindAsync(city.Id);
            updatedCity.Name.Should().BeEquivalentTo(city.Name);
            _memoryCacheMock.Verify(x => x.Remove(It.Is<object>(key => ReferenceEquals(key, CacheKeys.CitiesKey))));
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_City()
        {
            var city = _fixture.InsertCity("CityRepositoryTestDeleteAsync");

            _memoryCacheMock.Setup(x => x.Remove(It.IsAny<object>()));

            Func<Task> result = async () => await _repository.DeleteAsync(city);
            await result.Should().NotThrowAsync<Exception>();

            var deletedCity = await _dbContext.Cities.FindAsync(city.Id);
            deletedCity.Should().BeNull();
            _memoryCacheMock.Verify(x => x.Remove(It.Is<object>(key => ReferenceEquals(key, CacheKeys.CitiesKey))));
        }

        [Fact]
        public async Task CountAsync_Should_Return_Number_Of_Cities()
        {
            var cities = new List<City> { _city };
            object cachedCities = cities;

            _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedCities)).Returns(true);

            var result = await _repository.CountAsync();

            result.Should().Be(cities.Count);
        }

        [Fact] 
        public async Task CountAsync_Should_Return_Number_Of_Cities_For_Given_Name_PolishName_And_StateId()
        {
            var cities = new List<City> { _city };
            object cachedCities = cities;

            _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedCities)).Returns(true);

            var result = await _repository.CountAsync(_city.StateId, _city.Name, _city.PolishName);

            result.Should().Be(cities.Count);
        }
    }
}