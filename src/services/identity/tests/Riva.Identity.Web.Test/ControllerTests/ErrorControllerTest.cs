using System.Threading.Tasks;
using FluentAssertions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Riva.Identity.Web.Controllers;
using Riva.Identity.Web.Models.ViewModels;
using Xunit;

namespace Riva.Identity.Web.Test.ControllerTests
{
    public class ErrorControllerTest
    {
        private readonly Mock<IIdentityServerInteractionService> _interactionMock;
        private readonly HttpContext _httpContext;
        private readonly ErrorController _controller;


        public ErrorControllerTest()
        {
            _interactionMock = new Mock<IIdentityServerInteractionService>();
            _httpContext = new DefaultHttpContext();
            _httpContext.Request.Scheme = "http";
            _httpContext.Request.Host = new HostString("localhost", 5000);
            _httpContext.Request.Path = "/auth/login";
            var controllerContext = new ControllerContext
            {
                HttpContext = _httpContext
            };
            _controller = new ErrorController(_interactionMock.Object)
            {
                ControllerContext = controllerContext
            };
        }

        [Fact]
        public async Task Index_Should_Return_ViewResult_With_500_Status_Code_And_Proper_Error_Message()
        {
            var expectedErrorViewModel = new ErrorViewModel(500, "Unexpected error has occured. Please contact with Administrator.");

            var result = await _controller.Index();
            var viewResult = result.As<ViewResult>();

            viewResult.ViewName.Should().Be("Error");
            viewResult.Model.Should().BeEquivalentTo(expectedErrorViewModel);
        }

        [Fact]
        public async Task Index_Should_Return_ViewResult_With_422_Status_Code_And_Proper_Error_Message_When_ErrorId_Is_Not_Null()
        {
            var identityServerErrorMessage = new ErrorMessage{ ErrorDescription = "ErrorDescription" };
            var expectedErrorViewModel = new ErrorViewModel(422, identityServerErrorMessage.ErrorDescription);

            _interactionMock.Setup(x => x.GetErrorContextAsync(It.IsAny<string>())).ReturnsAsync(identityServerErrorMessage);

            var result = await _controller.Index(null, null, "errorId");
            var viewResult = result.As<ViewResult>();

            viewResult.ViewName.Should().Be("Error");
            viewResult.Model.Should().BeEquivalentTo(expectedErrorViewModel);
        }

        [Fact]
        public async Task Index_Should_Return_ViewResult_With_Any_Status_Code_And_Proper_Error_Message()
        {
            const int statusCode = 412;
            var expectedErrorViewModel = new ErrorViewModel(statusCode, "Unexpected error has occured. Please contact with Administrator.");

            var result = await _controller.Index(null, statusCode);
            var viewResult = result.As<ViewResult>();

            viewResult.ViewName.Should().Be("Error");
            viewResult.Model.Should().BeEquivalentTo(expectedErrorViewModel);
        }

        [Fact]
        public async Task Index_Should_Return_ViewResult_With_500_Status_Code_And_Any_Error_Message()
        {
            const string errorMessage = "ErrorMessage";
            var expectedErrorViewModel = new ErrorViewModel(500, errorMessage);

            var result = await _controller.Index(errorMessage);
            var viewResult = result.As<ViewResult>();

            viewResult.ViewName.Should().Be("Error");
            viewResult.Model.Should().BeEquivalentTo(expectedErrorViewModel);
        }

        [Fact]
        public void StatusCodeError_Should_Return_ViewResult_With_403_Status_Code_And_Proper_Error_Message()
        {
            const int statusCode = 403;
            var expectedErrorViewModel = new ErrorViewModel(statusCode, "You are not authorized to perform this operation.");

            var result = _controller.StatusCodeError(statusCode);
            var viewResult = result.As<ViewResult>();

            viewResult.ViewName.Should().Be("Error");
            viewResult.Model.Should().BeEquivalentTo(expectedErrorViewModel);
        }

        [Fact]
        public void StatusCodeError_Should_Return_ViewResult_With_404_Status_Code_And_Proper_Error_Message()
        {
            const int statusCode = 404;
            var expectedErrorViewModel = new ErrorViewModel(statusCode, "Page was not found.");

            var result = _controller.StatusCodeError(statusCode);
            var viewResult = result.As<ViewResult>();

            viewResult.ViewName.Should().Be("Error");
            viewResult.Model.Should().BeEquivalentTo(expectedErrorViewModel);
        }

        [Fact]
        public void StatusCodeError_Should_Return_ViewResult_With_Any_Status_Code_And_Proper_Error_Message()
        {
            const int statusCode = 422;
            var expectedErrorViewModel = new ErrorViewModel(statusCode, "Unexpected error has occured. Please contact with Administrator.");

            var result = _controller.StatusCodeError(statusCode);
            var viewResult = result.As<ViewResult>();

            viewResult.ViewName.Should().Be("Error");
            viewResult.Model.Should().BeEquivalentTo(expectedErrorViewModel);
        }

        [Fact]
        public void StatusCodeError_Should_Return_RedirectToActionResult_When_Status_Code_Is_401()
        {
            const int statusCode = 401;
            var statusCodeReExecuteFeatureMock = new Mock<IStatusCodeReExecuteFeature>();
            _httpContext.Features.Set(statusCodeReExecuteFeatureMock.Object);
            const string originalRequestUrl = "http://localhost:5000/auth/login";

            statusCodeReExecuteFeatureMock.SetupGet(x => x.OriginalPathBase).Returns(_httpContext.Request.Scheme + "://" + _httpContext.Request.Host);
            statusCodeReExecuteFeatureMock.SetupGet(x => x.OriginalPath).Returns(_httpContext.Request.Path);

            var result = _controller.StatusCodeError(statusCode);
            var redirectToActionResult = result.As<RedirectToActionResult>();

            redirectToActionResult.ControllerName.Should().Be("LocalLogin");
            redirectToActionResult.ActionName.Should().Be("Login");
            redirectToActionResult.RouteValues.Keys.Should().ContainSingle(x => x.Equals("returnUrl"));
            redirectToActionResult.RouteValues.Values.Should().BeEquivalentTo(new object[] {originalRequestUrl});
        }
    }
}