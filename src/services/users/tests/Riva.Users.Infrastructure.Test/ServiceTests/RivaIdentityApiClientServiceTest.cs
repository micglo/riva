using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using Riva.BuildingBlocks.Core.Enumerations;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Logger;
using Riva.BuildingBlocks.Core.Services;
using Riva.Users.Core.Services;
using Riva.Users.Infrastructure.Models.ApiClientResponses.RivaIdentity;
using Riva.Users.Infrastructure.Models.AppSettings;
using Riva.Users.Infrastructure.Services;
using Xunit;

namespace Riva.Users.Infrastructure.Test.ServiceTests
{
    public class RivaIdentityApiClientServiceTest
    {
        private readonly Mock<IHttpClientService> _httpClientServiceMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly IRivaIdentityApiClientService _service;

        public RivaIdentityApiClientServiceTest()
        {
            const string uri = "http://localhost";
            _httpClientServiceMock = new Mock<IHttpClientService>();
            _httpClientServiceMock.SetupGet(x => x.BaseAddress).Returns(new Uri(uri));
            _httpClientServiceMock.SetupGet(x => x.DefaultRequestHeaders).Returns(new HttpClient().DefaultRequestHeaders);
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(_ => _.GetService(typeof(IAuthenticationService))).Returns(authenticationServiceMock.Object);
            httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(new DefaultHttpContext { RequestServices = serviceProviderMock.Object });
            _loggerMock = new Mock<ILogger>();
            var apiClientsOptionsMock = new Mock<IOptions<ApiClientsAppSettings>>();
            apiClientsOptionsMock.SetupGet(x => x.Value)
                .Returns(new ApiClientsAppSettings { RivaIdentityApiUrl = uri });
            _service = new RivaIdentityApiClientService(_httpClientServiceMock.Object, httpContextAccessorMock.Object,
                _loggerMock.Object, apiClientsOptionsMock.Object);
        }

        [Fact]
        public async Task GetAccountAsync_Should_Return_Account()
        {
            var accountId = Guid.NewGuid();
            var getAccountResponse = new GetAccountResponse
            {
                Id = accountId,
                Email = "email@email.com"
            };
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(getAccountResponse))
            };

            _httpClientServiceMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(httpResponseMessage);

            var result = await _service.GetAccountAsync(accountId);

            result.Should().BeEquivalentTo(getAccountResponse);
        }

        [Fact]
        public async Task GetAccountAsync_Should_Return_Null()
        {
            var accountId = Guid.NewGuid();
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent("")
            };

            _httpClientServiceMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(httpResponseMessage);

            var result = await _service.GetAccountAsync(accountId);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAccountAsync_Should_Throw_UnprocessableException()
        {
            var accountId = Guid.NewGuid();
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadGateway)
            {
                Content = new StringContent("")
            };
            var message = $"Requesting account data failed. Http status code: {httpResponseMessage.StatusCode}. Response content: .";

            _httpClientServiceMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(httpResponseMessage);
            _loggerMock.Setup(x =>
                    x.LogError(It.IsAny<ServiceComponentEnumeration>(), It.IsAny<string>(), It.IsAny<object[]>()))
                .Verifiable();

            Func<Task> result = async () => await _service.GetAccountAsync(accountId);
            var exceptionResult = await result.Should().ThrowAsync<UnprocessableException>();

            exceptionResult.And.Message.Should().Be(message);
            _loggerMock.Verify(x =>
                x.LogError(It.Is<ServiceComponentEnumeration>(s => Equals(s, ServiceComponentEnumeration.RivaUsers)),
                    It.Is<string>(s => s == "Message={message}"), It.Is<object[]>(s => s.Contains(message))));
        }
    }
}