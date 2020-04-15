using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.Users.Domain.Cities.Aggregates;
using Riva.Users.Domain.Cities.Repositories;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Entities;
using Xunit;

namespace Riva.Users.Infrastructure.Test.DataAccessTests.CityRepositoryTests
{
    [Collection("RivaUsersSqlServer tests collection")]
    public class CityRepositoryTest : IClassFixture<CityRepositoryTestFixture>
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly ICityRepository _repository;
        private readonly City _city;

        public CityRepositoryTest(CityRepositoryTestFixture fixture)
        {
            _mapperMock = fixture.MapperMock;
            _repository = fixture.Repository;
            _city = fixture.City;
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_City_With_CityDistricts()
        {
            _mapperMock.Setup(x => x.Map<CityEntity, City>(It.IsAny<CityEntity>()))
                .Returns(_city);

            var result = await _repository.GetByIdAsync(_city.Id);

            result.Should().BeEquivalentTo(_city);
        }
    }
}