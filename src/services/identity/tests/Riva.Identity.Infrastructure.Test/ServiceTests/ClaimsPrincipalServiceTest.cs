using System;
using System.Collections.Generic;
using System.Security.Claims;
using FluentAssertions;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Moq;
using Riva.Identity.Core.Services;
using Riva.Identity.Infrastructure.Services;
using Xunit;

namespace Riva.Identity.Infrastructure.Test.ServiceTests
{
    public class ClaimsPrincipalServiceTest
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly IClaimsPrincipalService _service;

        public ClaimsPrincipalServiceTest()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _service = new ClaimsPrincipalService(_httpContextAccessorMock.Object);
        }

        [Fact]
        public void GetClaimsPrincipal_Should_Return_User_Principal()
        {
            var claims = new[] {
                new Claim(ClaimTypes.Name, "email@email.com"),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims);
            var cp = new ClaimsPrincipal(claimsIdentity);
            var httpContext = new DefaultHttpContext { User = cp };

            _httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(httpContext);

            var result = _service.GetClaimsPrincipal();

            result.Should().BeEquivalentTo(cp);
        }

        [Fact]
        public void GetNonLocalIdentityProvider_Should_Return_IdentityProvider()
        {
            const string identityProvider = "Google";
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.IdentityProvider, identityProvider)
            };
            var claimIdentities = new List<ClaimsIdentity>
            {
                new ClaimsIdentity(claims)
            };
            var cp = new ClaimsPrincipal(claimIdentities);

            var result = _service.GetNonLocalIdentityProvider(cp);

            result.Should().Be(identityProvider);
        }
    }
}