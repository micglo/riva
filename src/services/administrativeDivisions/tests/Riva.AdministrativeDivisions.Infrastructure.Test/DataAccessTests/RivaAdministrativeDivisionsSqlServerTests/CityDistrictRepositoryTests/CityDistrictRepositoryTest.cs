using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Aggregates;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Repositories;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Contexts;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;
using Riva.AdministrativeDivisions.Infrastructure.Models;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Services;
using Xunit;

namespace Riva.AdministrativeDivisions.Infrastructure.Test.DataAccessTests.RivaAdministrativeDivisionsSqlServerTests.CityDistrictRepositoryTests
{
    [Collection("RivaAdministrativeDivisionsSqlServer tests collection")]
    public class CityDistrictRepositoryTest : IClassFixture<CityDistrictRepositoryTestFixture>
    {
        private readonly RivaAdministrativeDivisionsDbContext _dbContext;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IOrderByExpressionCreator<CityDistrict>> _orderByExpressionCreatorMock;
        private readonly Mock<IMemoryCache> _memoryCacheMock;
        private readonly ICityDistrictRepository _repository;
        private readonly CityDistrict _cityDistrict;
        private readonly CityEntity _cityEntity;
        private readonly CityDistrictRepositoryTestFixture _fixture;

        public CityDistrictRepositoryTest(CityDistrictRepositoryTestFixture fixture)
        {
            _dbContext = fixture.DbContext;
            _mapperMock = fixture.MapperMock;
            _orderByExpressionCreatorMock = fixture.OrderByExpressionCreatorMock;
            _memoryCacheMock = fixture.MemoryCacheMock;
            _repository = fixture.Repository;
            _cityDistrict = fixture.CityDistrict;
            _cityEntity = fixture.CityEntity;
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_CityDistricts_Collection_From_Cache()
        {
            var cityDistricts = new List<CityDistrict> { _cityDistrict };
            object cachedStates = cityDistricts;

            _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedStates)).Returns(true);

            var result = await _repository.GetAllAsync();

            result.Should().BeEquivalentTo(cityDistricts);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_CityDistricts_Collection()
        {
            var cityDistrictEntities = await _dbContext.CityDistricts
                .Include(x => x.NameVariants)
                .ToListAsync();
            var cityDistricts = cityDistrictEntities.Select(x =>
                CityDistrict.Builder()
                    .SetId(x.Id)
                    .SetRowVersion(x.RowVersion)
                    .SetName(x.Name)
                    .SetPolishName(x.PolishName)
                    .SetCityId(x.CityId)
                    .SetParentId(x.ParentId)
                    .SetNameVariants(x.NameVariants.Select(nv => nv.Value).ToArray())
                    .Build()
            ).ToList();
            object cachedCityDistricts = null;
            var cacheEntryMock = new Mock<ICacheEntry>();

            _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedCityDistricts)).Returns(false);
            _memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock.Object);
            _mapperMock.Setup(x => x.Map<List<CityDistrictEntity>, List<CityDistrict>>(It.IsAny<List<CityDistrictEntity>>()))
                .Returns(cityDistricts);

            var result = await _repository.GetAllAsync();

            result.Should().BeEquivalentTo(cityDistricts);
        }

        [Fact]
        public async Task FindAsync_Should_Return_CityDistricts_Collection()
        {
            var cityDistricts = new List<CityDistrict> { _cityDistrict };
            object cachedCityDistricts = cityDistricts;
            static IOrderedQueryable<CityDistrict> OrderByExpression(IQueryable<CityDistrict> o) => o.OrderByDescending(x => x.Name);

            _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedCityDistricts)).Returns(true);
            _orderByExpressionCreatorMock.Setup(x => x.CreateExpression(It.IsAny<string>())).Returns(OrderByExpression);

            var result = await _repository.FindAsync(1, 100, "name:asc", _cityDistrict.Name, _cityDistrict.PolishName,
                _cityDistrict.CityId, _cityDistrict.ParentId, new List<Guid> { _cityDistrict.CityId });

            result.Should().BeEquivalentTo(cityDistricts);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_CityDistrict()
        {
            _mapperMock.Setup(x => x.Map<CityDistrictEntity, CityDistrict>(It.IsAny<CityDistrictEntity>()))
                .Returns(_cityDistrict);

            var result = await _repository.GetByIdAsync(_cityDistrict.Id);

            result.Should().BeEquivalentTo(_cityDistrict);
        }

        [Fact]
        public async Task GetByNameAndCityIdAsync_Should_Return_CityDistrict()
        {
            _mapperMock.Setup(x => x.Map<CityDistrictEntity, CityDistrict>(It.IsAny<CityDistrictEntity>()))
                .Returns(_cityDistrict);

            var result = await _repository.GetByNameAndCityIdAsync(_cityDistrict.Name, _cityDistrict.CityId);

            result.Should().BeEquivalentTo(_cityDistrict);
        }

        [Fact]
        public async Task GetByPolishNameAndCityIdAsync_Should_Return_CityDistrict()
        {
            _mapperMock.Setup(x => x.Map<CityDistrictEntity, CityDistrict>(It.IsAny<CityDistrictEntity>()))
                .Returns(_cityDistrict);

            var result = await _repository.GetByPolishNameAndCityIdAsync(_cityDistrict.PolishName, _cityDistrict.CityId);

            result.Should().BeEquivalentTo(_cityDistrict);
        }

        [Fact]
        public async Task AddAsync_Should_Add_CityDistrict()
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("CityDistrictRepositoryTestAddAsync")
                .SetPolishName("CityDistrictRepositoryTestAddAsync")
                .SetCityId(_cityEntity.Id)
                .Build();
            var cityDistrictEntity = new CityDistrictEntity
            {
                Id = cityDistrict.Id,
                Name = cityDistrict.Name,
                PolishName = cityDistrict.PolishName,
                RowVersion = cityDistrict.RowVersion.ToArray(),
                CityId = cityDistrict.CityId
            };

            _mapperMock.Setup(x => x.Map<CityDistrict, CityDistrictEntity>(It.IsAny<CityDistrict>()))
                .Returns(cityDistrictEntity);
            _memoryCacheMock.Setup(x => x.Remove(It.IsAny<object>()));

            Func<Task> result = async () => await _repository.AddAsync(cityDistrict);
            await result.Should().NotThrowAsync<Exception>();

            var addedCityDistrict = await _dbContext.CityDistricts.FindAsync(cityDistrict.Id);
            addedCityDistrict.Should().NotBeNull();
            _memoryCacheMock.Verify(x => x.Remove(It.Is<object>(key => ReferenceEquals(key, CacheKeys.CityDistrictsKey))));
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_CityDistrict()
        {
            const string nameVariant = "NewCityDistrictNameVariant";
            var cityDistrict = _fixture.InsertCityDistrict("CityDistrictRepositoryTestUpdateAsync");
            cityDistrict.ChangeName("CityDistrictRepositoryTestUpdateAsyncNewName");
            cityDistrict.AddNameVariant(nameVariant);

            _memoryCacheMock.Setup(x => x.Remove(It.IsAny<object>()));

            Func<Task> result = async () => await _repository.UpdateAsync(cityDistrict);
            await result.Should().NotThrowAsync<Exception>();

            var updatedCityDistrict = await _dbContext.CityDistricts.FindAsync(cityDistrict.Id);
            updatedCityDistrict.Name.Should().BeEquivalentTo(cityDistrict.Name);
            updatedCityDistrict.NameVariants.Select(x => x.Value).Should().Contain(nameVariant);
            _memoryCacheMock.Verify(x => x.Remove(It.Is<object>(key => ReferenceEquals(key, CacheKeys.CityDistrictsKey))));
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_CityDistrict()
        {
            var cityDistrict = _fixture.InsertCityDistrict("CityDistrictRepositoryTestDeleteAsync");

            _memoryCacheMock.Setup(x => x.Remove(It.IsAny<object>()));

            Func<Task> result = async () => await _repository.DeleteAsync(cityDistrict);
            await result.Should().NotThrowAsync<Exception>();

            var deletedCityDistrict = await _dbContext.CityDistricts.FindAsync(cityDistrict.Id);
            deletedCityDistrict.Should().BeNull();
            _memoryCacheMock.Verify(x => x.Remove(It.Is<object>(key => ReferenceEquals(key, CacheKeys.CityDistrictsKey))));
        }

        [Fact]
        public async Task CountAsync_Should_Return_Number_Of_CityDistricts()
        {
            var cityDistricts = new List<CityDistrict> { _cityDistrict };
            object cachedCityDistricts = cityDistricts;

            _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedCityDistricts)).Returns(true);

            var result = await _repository.CountAsync();

            result.Should().Be(cityDistricts.Count);
        }

        [Fact] 
        public async Task CountAsync_Should_Return_Number_Of_CityDistricts_For_Given_Name_PolishName_CityId_And_ParentId()
        {
            var cityDistricts = new List<CityDistrict> { _cityDistrict };
            object cachedCityDistricts = cityDistricts;

            _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedCityDistricts)).Returns(true);

            var result = await _repository.CountAsync(_cityDistrict.Name, _cityDistrict.PolishName,
                _cityDistrict.CityId, _cityDistrict.ParentId, new List<Guid>{ _cityDistrict.CityId });

            result.Should().Be(cityDistricts.Count);
        }
    }
}