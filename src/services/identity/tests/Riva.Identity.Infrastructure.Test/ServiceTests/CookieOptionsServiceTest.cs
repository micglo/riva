using FluentAssertions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Riva.Identity.Infrastructure.Services;
using Xunit;

namespace Riva.Identity.Infrastructure.Test.ServiceTests
{
    public class CookieOptionsServiceTest
    {
        private readonly IConfigureNamedOptions<CookieAuthenticationOptions> _service;

        public CookieOptionsServiceTest()
        {
            _service = new CookieOptionsService();
        }

        [Fact]
        public void Configure_Should_Set_Options()
        {
            const string accessDeniedPath = "/error/403";
            const string loginPath = "/login";
            const string returnUrlParameter = "returnUrl";

            var options = new CookieAuthenticationOptions();

            _service.Configure("name", options);

            options.AccessDeniedPath.Value.Should().Be(accessDeniedPath);
            options.LoginPath.Value.Should().Be(loginPath);
            options.ReturnUrlParameter.Should().Be(returnUrlParameter);
        }
    }
}