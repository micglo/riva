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
using Riva.AdministrativeDivisions.Domain.CityDistricts.Aggregates;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Core.Queries;
using Xunit;

namespace Riva.AdministrativeDivisions.Core.Test.QueryHandlerTests
{
    public class GetCityDistrictQueryHandlerTest
    {
        private readonly Mock<ICityDistrictGetterService> _cityDistrictGetterServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IQueryHandler<GetCityDistrictInputQuery, CityDistrictOutputQuery> _queryHandler;

        public GetCityDistrictQueryHandlerTest()
        {
            _cityDistrictGetterServiceMock = new Mock<ICityDistrictGetterService>();
            _mapperMock = new Mock<IMapper>();
            _queryHandler = new GetCityDistrictQueryHandler(_cityDistrictGetterServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_CityDistrictOutputQuery()
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .Build();
            var getCityDistrictResult = GetResult<CityDistrict>.Ok(cityDistrict);
            var cityDistrictOutputQuery = new CityDistrictOutputQuery(cityDistrict.Id, cityDistrict.RowVersion, cityDistrict.Name,
                cityDistrict.PolishName, cityDistrict.CityId, cityDistrict.ParentId, cityDistrict.NameVariants);

            _cityDistrictGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getCityDistrictResult);
            _mapperMock.Setup(x => x.Map<CityDistrict, CityDistrictOutputQuery>(It.IsAny<CityDistrict>()))
                .Returns(cityDistrictOutputQuery);

            var result = await _queryHandler.HandleAsync(new GetCityDistrictInputQuery(cityDistrict.Id));

            result.Should().BeEquivalentTo(cityDistrictOutputQuery);
        }

        [Fact]
        public async Task HandleAsync_Should_Throw_ResourceNotFoundException_When_CityDistrict_Is_Not_Found()
        {
            var errors = new Collection<IError>
            {
                new Error(CityDistrictErrorCodeEnumeration.NotFound, CityDistrictErrorMessage.NotFound)
            };
            var getCityDistrictResult = GetResult<CityDistrict>.Fail(errors);

            _cityDistrictGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getCityDistrictResult);

            Func<Task<CityDistrictOutputQuery>> result = async () => await _queryHandler.HandleAsync(new GetCityDistrictInputQuery(Guid.NewGuid()));
            var exceptionResult = await result.Should().ThrowExactlyAsync<ResourceNotFoundException>();
            exceptionResult.And.Errors.Should().BeEquivalentTo(errors);
        }
    }
}