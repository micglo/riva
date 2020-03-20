using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.Announcements.Core.Enumerations;
using Riva.Announcements.Core.ErrorMessages;
using Riva.Announcements.Core.Models;
using Riva.Announcements.Core.Services;
using Riva.Announcements.Infrastructure.Models.ApiClientResponses.RivaAdministrativeDivisions;
using Riva.Announcements.Infrastructure.Services;
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.Announcements.Infrastructure.Test.ServiceTests
{
    public class CityDistrictVerificationServiceTest
    {
        private readonly Mock<IRivaAdministrativeDivisionsApiClientService> _rivaAdministrativeDivisionApiClientServiceMock;
        private readonly ICityDistrictVerificationService _service;

        public CityDistrictVerificationServiceTest()
        {
            _rivaAdministrativeDivisionApiClientServiceMock = new Mock<IRivaAdministrativeDivisionsApiClientService>();
            _service = new CityDistrictVerificationService(_rivaAdministrativeDivisionApiClientServiceMock.Object);
        }

        [Fact]
        public async Task VerifyCityDistrictsExistAsync_Should_Return_Verification_Ok()
        {
            var cityId = Guid.NewGuid();
            var cityDistricts = new List<ICityDistrict>
            {
                new CityDistrict
                {
                    Id = Guid.NewGuid(),
                    Name = "Name",
                    PolishName = "PolishName",
                    CityId = cityId
                }
            };
            var expectedResult = VerificationResult.Ok();

            _rivaAdministrativeDivisionApiClientServiceMock.Setup(x => x.GetCityDistrictsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(cityDistricts);

            var result = await _service.VerifyCityDistrictsExistAsync(cityId, cityDistricts.Select(x => x.Id));

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task VerifyCityDistrictsExistAsync_Should_Return_Verification_Fail()
        {
            var cityId = Guid.NewGuid();
            var cityDistricts = new List<ICityDistrict>
            {
                new CityDistrict
                {
                    Id = Guid.NewGuid(),
                    Name = "Name",
                    PolishName = "PolishName",
                    CityId = Guid.NewGuid()
                }
            };
            var cityDistrictIds = new List<Guid> {Guid.NewGuid()};
            var errors = new Collection<IError>
            {
                new Error(CityDistrictErrorCodeEnumeration.NotFound, CityDistrictErrorMessage.NotFound)
            };
            var expectedResult = VerificationResult.Fail(errors);

            _rivaAdministrativeDivisionApiClientServiceMock.Setup(x => x.GetCityDistrictsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(cityDistricts);

            var result = await _service.VerifyCityDistrictsExistAsync(cityId, cityDistrictIds);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}