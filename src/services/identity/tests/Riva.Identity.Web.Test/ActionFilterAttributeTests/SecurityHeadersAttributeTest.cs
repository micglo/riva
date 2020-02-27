using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Moq;
using Riva.Identity.Core.Interactors.LocalLogin;
using Riva.Identity.Web.ActionFilterAttributes;
using Riva.Identity.Web.Controllers;
using Riva.Identity.Web.Models.AppSettings;
using Xunit;

namespace Riva.Identity.Web.Test.ActionFilterAttributeTests
{
    public class SecurityHeadersAttributeTest
    {
        private readonly SecurityHeadersAttribute _securityHeadersAttribute;
        private readonly HttpContext _httpContext;
        private readonly ActionContext _actionContext;
        private readonly LocalLoginController _localLoginController;

        public SecurityHeadersAttributeTest()
        {
            _httpContext = new DefaultHttpContext();
            _actionContext = new ActionContext(_httpContext, new RouteData(), new ActionDescriptor());
            var localLoginInteractor = new Mock<ILocalLoginInteractor>();
            var applicationUrlsOptionsMock = new Mock<IOptions<ApplicationUrlsAppSettings>>();
            _localLoginController = new LocalLoginController(localLoginInteractor.Object, applicationUrlsOptionsMock.Object);
            _securityHeadersAttribute = new SecurityHeadersAttribute();
        }

        [Fact]
        public void OnResultExecuting_Should_Add_Headers_To_Response_When_result_Is_ViewResult()
        {
            var context = new ResultExecutingContext(_actionContext, new List<IFilterMetadata>(), new ViewResult(), _localLoginController);
            var expectedHeaderKeys = new List<string>
            {
                "X-Content-Type-Options",
                "X-Frame-Options",
                "Content-Security-Policy",
                "X-Content-Security-Policy",
                "Referrer-Policy",
            };

            _securityHeadersAttribute.OnResultExecuting(context);

            _httpContext.Response.Headers.Keys.Should().BeEquivalentTo(expectedHeaderKeys);
        }
    }
}