using System;
using System.Collections.ObjectModel;
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
    public class CityVerificationServiceTest
    {
        private readonly Mock<IRivaAdministrativeDivisionsApiClientService> _rivaAdministrativeDivisionApiClientServiceMock;
        private readonly ICityVerificationService _service;

        public CityVerificationServiceTest()
        {
            _rivaAdministrativeDivisionApiClientServiceMock = new Mock<IRivaAdministrativeDivisionsApiClientService>();
            _service = new CityVerificationService(_rivaAdministrativeDivisionApiClientServiceMock.Object);
        }

        [Fact]
        public async Task VVerifyCityExistsAsync_Should_Return_Verification_Ok()
        {
            var cityId = Guid.NewGuid();
            var city = new GetCityResponse
            {
                Id = cityId,
                Name = "Name",
                PolishName = "PolishName"
            };
            var expectedResult = VerificationResult.Ok();

            _rivaAdministrativeDivisionApiClientServiceMock.Setup(x => x.GetCityAsync(It.IsAny<Guid>()))
                .ReturnsAsync(city);

            var result = await _service.VerifyCityExistsAsync(cityId);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task VerifyCityExistsAsync_Should_Return_Verification_Fail()
        {
            var cityId = Guid.NewGuid();
            var errors = new Collection<IError>
            {
                new Error(CityErrorCodeEnumeration.NotFound, CityErrorMessage.NotFound)
            };
            var expectedResult = VerificationResult.Fail(errors);

            _rivaAdministrativeDivisionApiClientServiceMock.Setup(x => x.GetCityAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult<ICity>(null));

            var result = await _service.VerifyCityExistsAsync(cityId);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}