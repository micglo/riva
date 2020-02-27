using System;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Moq;
using Riva.BuildingBlocks.Core.Mapper;
using Xunit;

namespace Riva.Identity.Infrastructure.Test.ServiceTests
{
    public class AuthenticationServiceTest
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Core.Services.IAuthenticationService _service;

        public AuthenticationServiceTest()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _mapperMock = new Mock<IMapper>();
            _service = new Services.AuthenticationService(_httpContextAccessorMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task AuthenticateAsync_Should_Return_AuthenticateResult()
        {
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            var cp = new ClaimsPrincipal();
            const string scheme = "scheme";
            var aspNetCoreAuthenticateResult = AuthenticateResult.Success(new AuthenticationTicket(cp, scheme));
            var authenticateResult = new Core.Models.AuthenticateResult(true, null, cp, null);

            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProviderMock.Object
            };

            _httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(httpContext);
            serviceProviderMock.Setup(sp => sp.GetService(typeof(IAuthenticationService)))
                .Returns(authenticationServiceMock.Object);
            authenticationServiceMock.Setup(x => x.AuthenticateAsync(It.IsAny<HttpContext>(), It.IsAny<string>()))
                .ReturnsAsync(aspNetCoreAuthenticateResult);
            _mapperMock.Setup(x =>
                    x.Map<AuthenticateResult, Core.Models.AuthenticateResult>(It.IsAny<AuthenticateResult>()))
                .Returns(authenticateResult);

            var result = await _service.AuthenticateAsync(scheme);

            result.Should().BeEquivalentTo(authenticateResult);
        }
    }
}