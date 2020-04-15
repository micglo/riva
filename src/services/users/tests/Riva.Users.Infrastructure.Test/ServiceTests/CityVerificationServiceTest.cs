using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Models;
using Riva.Users.Core.Enumerations;
using Riva.Users.Core.ErrorMessages;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Cities.Aggregates;
using Riva.Users.Infrastructure.Services;
using Xunit;

namespace Riva.Users.Infrastructure.Test.ServiceTests
{
    public class CityVerificationServiceTest
    {
        private readonly Mock<ICityGetterService> _cityGetterServiceMock;
        private readonly ICityVerificationService _service;

        public CityVerificationServiceTest()
        {
            _cityGetterServiceMock = new Mock<ICityGetterService>();
            _service = new CityVerificationService(_cityGetterServiceMock.Object);
        }

        [Fact]
        public async Task VerifyCityAndCityDistrictsAsync_Should_Return_VerificationResult_With_Success()
        {
            var cityId = Guid.NewGuid();
            var cityDistricts = new List<Guid> { Guid.NewGuid() };
            var city = new City(cityId, cityDistricts);
            var getCityResult = GetResult<City>.Ok(city);
            var expectedResult = VerificationResult.Ok();

            _cityGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getCityResult);

            var result = await _service.VerifyCityAndCityDistrictsAsync(cityId, cityDistricts);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task VerifyCityAndCityDistrictsAsync_Should_Return_VerificationResult_With_Fail_When_City_Do_Not_Exist()
        {
            var cityId = Guid.NewGuid();
            var cityDistricts = new List<Guid> { Guid.NewGuid() };
            var errors = new Collection<IError>
            {
                new Error(CityErrorCodeEnumeration.NotFound, CityErrorMessage.NotFound)
            };
            var getCityResult = GetResult<City>.Fail(errors);
            var expectedResult = VerificationResult.Fail(errors);

            _cityGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getCityResult);

            var result = await _service.VerifyCityAndCityDistrictsAsync(cityId, cityDistricts);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task VerifyCityAndCityDistrictsAsync_Should_Return_VerificationResult_With_Fail_When_CityDistricts_Are_Incorrect()
        {
            var cityId = Guid.NewGuid();
            var cityDistricts = new List<Guid> { Guid.NewGuid() };
            var city = new City(cityId, new List<Guid> { Guid.NewGuid() });
            var getCityResult = GetResult<City>.Ok(city);
            var errors = new Collection<IError>
            {
                new Error(CityErrorCodeEnumeration.IncorrectCityDistricts, CityErrorMessage.IncorrectCityDistricts)
            };
            var expectedResult = VerificationResult.Fail(errors);

            _cityGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(getCityResult);

            var result = await _service.VerifyCityAndCityDistrictsAsync(cityId, cityDistricts);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}