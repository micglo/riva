using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.AdministrativeDivisions.Core.Queries;
using Riva.AdministrativeDivisions.Core.Queries.Handlers;
using Riva.AdministrativeDivisions.Domain.Cities.Aggregates;
using Riva.AdministrativeDivisions.Domain.Cities.Repositories;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Xunit;

namespace Riva.AdministrativeDivisions.Core.Test.QueryHandlerTests
{
    public class GetCitiesQueryHandlerTest
    {
        private readonly Mock<ICityRepository> _cityRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IQueryHandler<GetCitiesInputQuery, CollectionOutputQuery<CityOutputQuery>> _queryHandler;

        public GetCitiesQueryHandlerTest()
        {
            _cityRepositoryMock = new Mock<ICityRepository>();
            _mapperMock = new Mock<IMapper>();
            _queryHandler = new GetCitiesQueryHandler(_cityRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_CollectionOutputQuery_With_CityOutputQuery_When_Input_Is_Null()
        {
            var cities = new List<City>
            {
                City.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build()
            };
            var cityOutputQueries = cities.Select(x => new CityOutputQuery(x.Id, x.RowVersion, x.Name, x.PolishName, x.StateId)).ToList();
            var collectionOutputQuery = new CollectionOutputQuery<CityOutputQuery>(cities.Count, cityOutputQueries);

            _cityRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(cities);
            _cityRepositoryMock.Setup(x => x.CountAsync()).ReturnsAsync(cities.Count);
            _mapperMock
                .Setup(x => x.Map<List<City>, IEnumerable<CityOutputQuery>>(It.IsAny<List<City>>()))
                .Returns(cityOutputQueries);

            var result = await _queryHandler.HandleAsync(null);

            result.Should().BeEquivalentTo(collectionOutputQuery);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_CollectionOutputQuery_With_CityOutputQuery_When_Input_Is_Not_Null()
        {
            var city = City.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();
            var getCitiesInputQuery = new GetCitiesInputQuery(1, 100, "name:asc", city.StateId, city.Name, city.PolishName);
            var cities = new List<City> { city };
            var cityOutputQueries = cities.Select(x => new CityOutputQuery(x.Id, x.RowVersion, x.Name, x.PolishName, x.StateId)).ToList();
            var collectionOutputQuery = new CollectionOutputQuery<CityOutputQuery>(cities.Count, cityOutputQueries);

            _cityRepositoryMock.Setup(x => x.FindAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<string>(),
                It.IsAny<Guid?>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(cities);
            _cityRepositoryMock.Setup(x => x.CountAsync(It.IsAny<Guid?>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(cities.Count);
            _mapperMock
                .Setup(x => x.Map<List<City>, IEnumerable<CityOutputQuery>>(It.IsAny<List<City>>()))
                .Returns(cityOutputQueries);

            var result = await _queryHandler.HandleAsync(getCitiesInputQuery);

            result.Should().BeEquivalentTo(collectionOutputQuery);
        }
    }
}