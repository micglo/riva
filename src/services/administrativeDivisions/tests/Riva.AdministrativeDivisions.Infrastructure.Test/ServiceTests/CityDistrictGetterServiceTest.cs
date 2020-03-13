using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.AdministrativeDivisions.Core.Enumerations;
using Riva.AdministrativeDivisions.Core.ErrorMessages;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Aggregates;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Repositories;
using Riva.AdministrativeDivisions.Infrastructure.Services;
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.AdministrativeDivisions.Infrastructure.Test.ServiceTests
{
    public class CityDistrictGetterServiceTest
    {
        private readonly Mock<ICityDistrictRepository> _cityDistrictRepositoryMock;
        private readonly ICityDistrictGetterService _service;

        public CityDistrictGetterServiceTest()
        {
            _cityDistrictRepositoryMock = new Mock<ICityDistrictRepository>();
            _service = new CityDistrictGetterService(_cityDistrictRepositoryMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_GetResult_With_CityDistrict()
        {
            var cityDistrict = CityDistrict.Builder()
                .SetId(Guid.NewGuid())
                .SetRowVersion(Array.Empty<byte>())
                .SetName("Name")
                .SetPolishName("PolishName")
                .SetCityId(Guid.NewGuid())
                .SetParentId(Guid.NewGuid())
                .SetNameVariants(new List<string>{ "NameVariant" })
                .Build();
            var expectedResult = GetResult<CityDistrict>.Ok(cityDistrict);

            _cityDistrictRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(cityDistrict);

            var result = await _service.GetByIdAsync(cityDistrict.Id);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_GetResult_With_Errors_When_CityDistrict_Is_Not_Found()
        {
            var errors = new Collection<IError>
            {
                new Error(CityDistrictErrorCodeEnumeration.NotFound, CityDistrictErrorMessage.NotFound)
            };
            var expectedResult = GetResult<CityDistrict>.Fail(errors);

            _cityDistrictRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult<CityDistrict>(null));

            var result = await _service.GetByIdAsync(Guid.NewGuid());

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}