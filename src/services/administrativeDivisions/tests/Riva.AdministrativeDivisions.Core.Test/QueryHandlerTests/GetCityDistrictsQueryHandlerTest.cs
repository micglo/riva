using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.AdministrativeDivisions.Core.Queries;
using Riva.AdministrativeDivisions.Core.Queries.Handlers;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Aggregates;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Repositories;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Xunit;

namespace Riva.AdministrativeDivisions.Core.Test.QueryHandlerTests
{
    public class GetCityDistrictsQueryHandlerTest
    {
        private readonly Mock<ICityDistrictRepository> _cityDistrictRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IQueryHandler<GetCityDistrictsInputQuery, CollectionOutputQuery<CityDistrictOutputQuery>> _queryHandler;

        public GetCityDistrictsQueryHandlerTest()
        {
            _cityDistrictRepositoryMock = new Mock<ICityDistrictRepository>();
            _mapperMock = new Mock<IMapper>();
            _queryHandler = new GetCityDistrictsQueryHandler(_cityDistrictRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_CollectionOutputQuery_With_CityDistrictOutputQuery_When_Input_Is_Null()
        {
            var cityDistricts = new List<CityDistrict>
            {
                CityDistrict.Builder()
                    .SetId(Guid.NewGuid())
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName("Name")
                    .SetPolishName("PolishName")
                    .SetCityId(Guid.NewGuid())
                    .Build()
            };
            var cityDistrictOutputQueries = cityDistricts.Select(x =>
                new CityDistrictOutputQuery(x.Id, x.RowVersion, x.Name, x.PolishName, x.CityId, x.ParentId, x.NameVariants)).ToList();
            var collectionOutputQuery = new CollectionOutputQuery<CityDistrictOutputQuery>(cityDistricts.Count, cityDistrictOutputQueries);

            _cityDistrictRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(cityDistricts);
            _cityDistrictRepositoryMock.Setup(x => x.CountAsync()).ReturnsAsync(cityDistricts.Count);
            _mapperMock
                .Setup(x => x.Map<List<CityDistrict>, IEnumerable<CityDistrictOutputQuery>>(It.IsAny<List<CityDistrict>>()))
                .Returns(cityDistrictOutputQueries);

            var result = await _queryHandler.HandleAsync(null);

            result.Should().BeEquivalentTo(collectionOutputQuery);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_CollectionOutputQuery_With_CityDistrictOutputQuery_When_Input_Is_Not_Null()
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetParentId(Guid.NewGuid())
                .Build();
            var cityDistrictsInputQuery = new GetCityDistrictsInputQuery(1, 100, "name:asc", cityDistrict.Name,
                cityDistrict.PolishName, cityDistrict.CityId, cityDistrict.ParentId, new List<Guid>());
            
            var cityDistricts = new List<CityDistrict> { cityDistrict };
            var cityDistrictOutputQueries = cityDistricts.Select(x =>
                new CityDistrictOutputQuery(x.Id, x.RowVersion, x.Name, x.PolishName, x.CityId, x.ParentId, x.NameVariants)).ToList();
            var collectionOutputQuery = new CollectionOutputQuery<CityDistrictOutputQuery>(cityDistricts.Count, cityDistrictOutputQueries);

            _cityDistrictRepositoryMock
                .Setup(x => x.FindAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid?>(), It.IsAny<Guid?>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityDistricts);
            _cityDistrictRepositoryMock
                .Setup(x => x.CountAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid?>(), It.IsAny<Guid?>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(cityDistricts.Count);
            _mapperMock.Setup(x =>
                    x.Map<List<CityDistrict>, IEnumerable<CityDistrictOutputQuery>>(It.IsAny<List<CityDistrict>>()))
                .Returns(cityDistrictOutputQueries);

            var result = await _queryHandler.HandleAsync(cityDistrictsInputQuery);

            result.Should().BeEquivalentTo(collectionOutputQuery);
        }
    }
}