using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Moq;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.Identity.Core.Services;
using Riva.Identity.Infrastructure.Services;
using Xunit;

namespace Riva.Identity.Infrastructure.Test.ServiceTests
{
    public class SchemeServiceTest
    {
        private readonly Mock<IAuthenticationSchemeProvider> _schemeProviderMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly ISchemeService _service;

        public SchemeServiceTest()
        {
            _schemeProviderMock = new Mock<IAuthenticationSchemeProvider>();
            _mapperMock = new Mock<IMapper>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _service = new SchemeService(_schemeProviderMock.Object, _mapperMock.Object, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task GetAllSchemesAsync_Should_Return_AuthenticationScheme_Collection()
        {
            var microsoftAuthenticationSchemes = new List<AuthenticationScheme>
            {
                new AuthenticationScheme("name", "displayName", typeof(CookieAuthenticationHandler))
            };
            var authenticationSchemes = microsoftAuthenticationSchemes
                .Select(x => new Core.Models.AuthenticationScheme(x.Name, x.DisplayName)).ToList();

            _schemeProviderMock.Setup(x => x.GetAllSchemesAsync()).ReturnsAsync(microsoftAuthenticationSchemes);
            _mapperMock.Setup(x =>
                    x.Map<IEnumerable<AuthenticationScheme>,
                        IEnumerable<Core.Models.AuthenticationScheme>>(
                        It.IsAny<IEnumerable<AuthenticationScheme>>()))
                .Returns(authenticationSchemes);

            var result = await _service.GetAllSchemesAsync();

            result.Should().BeEquivalentTo(authenticationSchemes);
        }

        [Fact]
        public async Task GetSchemesAsync_Should_Return_AuthenticationScheme()
        {
            const string schemeName = "name";
            var microsoftAuthenticationScheme =
                new AuthenticationScheme(schemeName, "displayName", typeof(CookieAuthenticationHandler));
            var authenticationScheme = new Core.Models.AuthenticationScheme(microsoftAuthenticationScheme.Name,
                microsoftAuthenticationScheme.DisplayName);

            _schemeProviderMock.Setup(x => x.GetSchemeAsync(It.IsAny<string>()))
                .ReturnsAsync(microsoftAuthenticationScheme);
            _mapperMock.Setup(x => x.Map<AuthenticationScheme, Core.Models.AuthenticationScheme>(It.IsAny<AuthenticationScheme>()))
                .Returns(authenticationScheme);

            var result = await _service.GetSchemeAsync(schemeName);

            result.Should().BeEquivalentTo(authenticationScheme);
        }

        [Fact]
        public async Task SchemeSupportsSignOutAsync_Should_Return_True()
        {
            const string scheme = "scheme";
            var authenticationHandlerProviderMock = new Mock<IAuthenticationHandlerProvider>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(sp => sp.GetService(typeof(IAuthenticationHandlerProvider)))
                .Returns(authenticationHandlerProviderMock.Object);
            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProviderMock.Object
            };
            var authenticationSignOutHandlerMock = new Mock<IAuthenticationSignOutHandler>();

            _httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(httpContext);
            authenticationHandlerProviderMock.Setup(x => x.GetHandlerAsync(It.IsAny<HttpContext>(), It.IsAny<string>()))
                .ReturnsAsync(authenticationSignOutHandlerMock.Object);


            var result = await _service.SchemeSupportsSignOutAsync(scheme);
            
            result.Should().BeTrue();
        }

        [Fact]
        public async Task SchemeSupportsSignOutAsync_Should_Return_False()
        {
            const string scheme = "scheme";
            var authenticationHandlerProviderMock = new Mock<IAuthenticationHandlerProvider>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(sp => sp.GetService(typeof(IAuthenticationHandlerProvider)))
                .Returns(authenticationHandlerProviderMock.Object);
            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProviderMock.Object
            };

            _httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(httpContext);
            authenticationHandlerProviderMock.Setup(x => x.GetHandlerAsync(It.IsAny<HttpContext>(), It.IsAny<string>()))
                .Returns(Task.FromResult<IAuthenticationHandler>(null));


            var result = await _service.SchemeSupportsSignOutAsync(scheme);

            result.Should().BeFalse();
        }
    }
}