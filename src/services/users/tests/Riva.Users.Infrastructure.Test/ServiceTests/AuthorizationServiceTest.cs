using System;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Riva.BuildingBlocks.Domain;
using Riva.Users.Core.Services;
using Riva.Users.Infrastructure.Services;
using Xunit;

namespace Riva.Users.Infrastructure.Test.ServiceTests
{
    public class AuthorizationServiceTest
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly IAuthorizationService _service;

        public AuthorizationServiceTest()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _service = new AuthorizationService(_httpContextAccessorMock.Object);
        }

        [Fact]
        public void IsAdministrator_Should_Return_True()
        {
            var claims = new[] {
                new Claim(ClaimTypes.Name, "email@email.com"),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, DefaultRoleEnumeration.Administrator.DisplayName)
            };
            var claimsIdentity = new ClaimsIdentity(claims);
            var cp = new ClaimsPrincipal(claimsIdentity);
            var httpContext = new DefaultHttpContext { User = cp };

            _httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(httpContext);

            var result = _service.IsAdministrator();

            result.Should().BeTrue();
        }

        [Fact]
        public void IsAdministrator_Should_Return_False()
        {
            var claims = new[] {
                new Claim(ClaimTypes.Name, "email@email.com"),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, DefaultRoleEnumeration.User.DisplayName)
            };
            var claimsIdentity = new ClaimsIdentity(claims);
            var cp = new ClaimsPrincipal(claimsIdentity);
            var httpContext = new DefaultHttpContext { User = cp };

            _httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(httpContext);

            var result = _service.IsAdministrator();

            result.Should().BeFalse();
        }
    }
}