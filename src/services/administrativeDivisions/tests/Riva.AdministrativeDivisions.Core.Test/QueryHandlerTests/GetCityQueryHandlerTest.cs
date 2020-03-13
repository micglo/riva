using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.AdministrativeDivisions.Core.Enumerations;
using Riva.AdministrativeDivisions.Core.ErrorMessages;
using Riva.AdministrativeDivisions.Core.Queries;
using Riva.AdministrativeDivisions.Core.Queries.Handlers;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.Cities.Aggregates;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Core.Queries;
using Xunit;

namespace Riva.AdministrativeDivisions.Core.Test.QueryHandlerTests
{
    public class GetCityQueryHandlerTest
    {
        private readonly Mock<ICityGetterService> _cityGetterServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IQueryHandler<GetCityInputQuery, CityOutputQuery> _queryHandler;

        public GetCityQueryHandlerTest()
        {
            _cityGetterServiceMock = new Mock<ICityGetterService>();
            _mapperMock = new Mock<IMapper>();
            _queryHandler = new GetCityQueryHandler(_cityGetterServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_CityOutputQuery()
        {
            var city = City.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();
            var getCityResult = GetResult<City>.Ok(city);
            var cityOutputQuery = new CityOutputQuery(city.Id, city.RowVersion, city.Name, city.PolishName, city.StateId);

            _cityGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getCityResult);
            _mapperMock.Setup(x => x.Map<City, CityOutputQuery>(It.IsAny<City>())).Returns(cityOutputQuery);

            var result = await _queryHandler.HandleAsync(new GetCityInputQuery(city.Id));

            result.Should().BeEquivalentTo(cityOutputQuery);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_City_Is_Not_Found()
        {
            var id = Guid.NewGuid();
            var errors = new Collection<IError>
            {
                new Error(CityErrorCodeEnumeration.NotFound, CityErrorMessage.NotFound)
            };
            var getCityResult = GetResult<City>.Fail(errors);

            _cityGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getCityResult);

            Func<Task<CityOutputQuery>> result = async () => await _queryHandler.HandleAsync(new GetCityInputQuery(id));
            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();

            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}