using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using Riva.Announcements.Core.Models;
using Riva.Announcements.Core.Services;
using Riva.Announcements.Infrastructure.Models.ApiClientResponses.RivaAdministrativeDivisions;
using Riva.Announcements.Infrastructure.Models.AppSettings;
using Riva.Announcements.Infrastructure.Services;
using Riva.BuildingBlocks.Core.Enumerations;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Logger;
using Riva.BuildingBlocks.Core.Services;
using Xunit;

namespace Riva.Announcements.Infrastructure.Test.ServiceTests
{
    public class RivaAdministrativeDivisionsApiClientServiceTest
    {
        private readonly Mock<IHttpClientService> _httpClientMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly IRivaAdministrativeDivisionsApiClientService _rivaAdministrativeDivisionsApiClientService;

        public RivaAdministrativeDivisionsApiClientServiceTest()
        {
            const string uri = "http://localhost";
            _httpClientMock = new Mock<IHttpClientService>();
            _httpClientMock.SetupGet(x => x.BaseAddress).Returns(new Uri(uri));
            _httpClientMock.SetupGet(x => x.DefaultRequestHeaders).Returns(new HttpClient().DefaultRequestHeaders);
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(_ => _.GetService(typeof(IAuthenticationService))).Returns(authenticationServiceMock.Object);
            httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(new DefaultHttpContext{RequestServices = serviceProviderMock.Object});
            _loggerMock = new Mock<ILogger>();
            var optionsMock = new Mock<IOptions<ApiClientsAppSettings>>();
            optionsMock.SetupGet(x => x.Value)
                .Returns(new ApiClientsAppSettings {RivaAdministrativeDivisionsApiUrl = uri});
            _rivaAdministrativeDivisionsApiClientService = new RivaAdministrativeDivisionsApiClientService(
                _httpClientMock.Object, httpContextAccessorMock.Object, _loggerMock.Object, optionsMock.Object);
        }

        [Fact]
        public async Task GetCityAsync_Should_Return_ICity()
        {
            var cityId = Guid.NewGuid();
            var getCityResponse = new GetCityResponse
            {
                Id = cityId,
                Name = "Name",
                PolishName = "PolishName",
                RowVersion = Array.Empty<byte>(),
                StateId = Guid.NewGuid()
            };
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(getCityResponse))
            };

            _httpClientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(httpResponseMessage);

            var result = await _rivaAdministrativeDivisionsApiClientService.GetCityAsync(cityId);

            result.Should().BeEquivalentTo(getCityResponse);
        }

        [Fact]
        public async Task GetCityAsync_Should_Throw_UnprocessableException_When_Error_Occures()
        {
            var cityId = Guid.NewGuid();
            const HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            var httpResponseMessage = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(string.Empty)
            };
            var expectedErrorMessage = $"Requesting city data failed. Http status code: {statusCode.ToString()}. Response content: .";

            _httpClientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(httpResponseMessage);
            _loggerMock.Setup(x => x.LogError(It.IsAny<ServiceComponentEnumeration>(), It.IsAny<string>(), It.IsAny<object[]>()));

            Func<Task<ICity>> result = async () => await _rivaAdministrativeDivisionsApiClientService.GetCityAsync(cityId);
            
            var exceptionResult = await result.Should().ThrowExactlyAsync<UnprocessableException>();
            exceptionResult.And.Message.Should().Be(expectedErrorMessage);
        }

        [Fact]
        public async Task GetCityDistrictsAsync_Should_Return_ICity()
        {
            var cityId = Guid.NewGuid();
            var getCityResponse = new GetCityDistrictsResponse
            {
                Results = new List<CityDistrict>
                {
                    new CityDistrict
                    {
                        Id = cityId,
                        Name = "Name",
                        PolishName = "PolishName",
                        RowVersion = Array.Empty<byte>(),
                        CityId = cityId,
                        NameVariants = new List<string>()
                    }
                },
                TotalCount = 1
            };
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(getCityResponse))
            };

            _httpClientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(httpResponseMessage);

            var result = await _rivaAdministrativeDivisionsApiClientService.GetCityDistrictsAsync(cityId);

            result.Should().BeEquivalentTo(getCityResponse.Results);
        }

        [Fact]
        public async Task GetCityDistrictsAsync_Should_Throw_UnprocessableException_When_Error_Occures()
        {
            var cityId = Guid.NewGuid();
            const HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            var httpResponseMessage = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(string.Empty)
            };
            var expectedErrorMessage = $"Requesting cityDistricts data failed. Http status code: {statusCode.ToString()}. Response content: .";

            _httpClientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(httpResponseMessage);
            _loggerMock.Setup(x => x.LogError(It.IsAny<ServiceComponentEnumeration>(), It.IsAny<string>(), It.IsAny<object[]>()));

            Func<Task<IEnumerable<ICityDistrict>>> result = async () => await _rivaAdministrativeDivisionsApiClientService.GetCityDistrictsAsync(cityId);

            var exceptionResult = await result.Should().ThrowExactlyAsync<UnprocessableException>();
            exceptionResult.And.Message.Should().Be(expectedErrorMessage);
        }
    }
}