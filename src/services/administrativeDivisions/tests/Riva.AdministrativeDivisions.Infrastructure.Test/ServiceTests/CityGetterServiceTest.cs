using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.AdministrativeDivisions.Core.Enumerations;
using Riva.AdministrativeDivisions.Core.ErrorMessages;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.Cities.Aggregates;
using Riva.AdministrativeDivisions.Domain.Cities.Repositories;
using Riva.AdministrativeDivisions.Infrastructure.Services;
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.AdministrativeDivisions.Infrastructure.Test.ServiceTests
{
    public class CityGetterServiceTest
    {
        private readonly Mock<ICityRepository> _cityRepositoryMock;
        private readonly ICityGetterService _cityGetterService;

        public CityGetterServiceTest()
        {
            _cityRepositoryMock = new Mock<ICityRepository>();
            _cityGetterService = new CityGetterService(_cityRepositoryMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_GetResult_With_City()
        {
            var id = Guid.NewGuid();
            var city = City.Builder()
                .SetId(id)
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetStateId(Guid.NewGuid())
                .Build();

            var expectedResult = GetResult<City>.Ok(city);

            _cityRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(city);

            var result = await _cityGetterService.GetByIdAsync(id);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_GetResult_With_Errors_When_City_Is_Not_Found()
        {
            var id = Guid.NewGuid();
            var errors = new Collection<IError>
            {
                new Error(CityErrorCodeEnumeration.NotFound, CityErrorMessage.NotFound)
            };
            var expectedResult = GetResult<City>.Fail(errors);

            _cityRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<City>(null));

            var result = await _cityGetterService.GetByIdAsync(id);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}