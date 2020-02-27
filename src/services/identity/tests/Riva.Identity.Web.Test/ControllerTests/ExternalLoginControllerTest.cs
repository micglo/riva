using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Riva.Identity.Core.Interactors.ExternalLogin;
using Riva.Identity.Web.Controllers;
using Riva.Identity.Web.Models.ErrorMessages;
using Riva.Identity.Web.Models.ViewModels;
using Xunit;

namespace Riva.Identity.Web.Test.ControllerTests
{
    public class ExternalLoginControllerTest
    {
        private readonly Mock<IExternalLoginInteractor> _externalLoginInteractorMock;
        private readonly Mock<IUrlHelper> _urlHelperMock;
        private readonly ExternalLoginController _controller;

        public ExternalLoginControllerTest()
        {
            _externalLoginInteractorMock = new Mock<IExternalLoginInteractor>();
            _urlHelperMock = new Mock<IUrlHelper>(MockBehavior.Strict);
            _controller = new ExternalLoginController(_externalLoginInteractorMock.Object)
            {
                Url = _urlHelperMock.Object,
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Fact]
        public void Challenge_Should_Return_ChallengeResult_When_ReturnUrl_Is_Null()
        {
            const string provider = "local";
            const string returnUrl = null;
            const string redirectUri = "http://localhost:5000/ExternalLogin/Callback";
            var expectedAuthenticationProperties = new AuthenticationProperties
            {
                RedirectUri = redirectUri,
                Items =
                {
                    { "returnUrl", "~/" },
                    { "scheme", provider }
                }
            };

            _urlHelperMock.Setup(x => x.IsLocalUrl(It.IsAny<string>())).Returns(true);
            _urlHelperMock.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns(redirectUri);

            var result = _controller.Challenge(provider, returnUrl);
            var challengeResult = result.As<ChallengeResult>();

            challengeResult.Properties.Should().BeEquivalentTo(expectedAuthenticationProperties);
            challengeResult.AuthenticationSchemes.Should().ContainSingle(provider);
        }

        [Fact]
        public void Challenge_Should_Return_ChallengeResult_When_ReturnUrl_Is_Local()
        {
            const string provider = "local";
            const string returnUrl = "/home";
            const string redirectUri = "http://localhost:5000/ExternalLogin/Callback";
            var expectedAuthenticationProperties = new AuthenticationProperties
            {
                RedirectUri = redirectUri,
                Items =
                {
                    { "returnUrl", returnUrl },
                    { "scheme", provider }
                }
            };

            _urlHelperMock.Setup(x => x.IsLocalUrl(It.IsAny<string>())).Returns(true);
            _urlHelperMock.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns(redirectUri);

            var result = _controller.Challenge(provider, returnUrl);
            var challengeResult = result.As<ChallengeResult>();

            challengeResult.Properties.Should().BeEquivalentTo(expectedAuthenticationProperties);
            challengeResult.AuthenticationSchemes.Should().ContainSingle(provider);
        }

        [Fact]
        public void Challenge_Should_Return_RedirectToActionResult_When_ReturnUrl_Is_Not_Local()
        {
            const string provider = "local";
            const string returnUrl = "http://anyurl.com";

            _urlHelperMock.Setup(x => x.IsLocalUrl(It.IsAny<string>())).Returns(false);

            var result = _controller.Challenge(provider, returnUrl);
            var redirectToActionResult = result.As<RedirectToActionResult>();

            redirectToActionResult.ControllerName.Should().Be("Error");
            redirectToActionResult.ActionName.Should().Be("Index");
            redirectToActionResult.RouteValues.Keys.Should().Contain("message").And.Contain("statusCode");
            redirectToActionResult.RouteValues.Values.Should().BeEquivalentTo(LocalLoginErrorMessage.InvalidReturnUrl, 422);
        }

        [Fact]
        public async Task Callback_Should_Return_ViewResult_When_IsNativeClient_Is_True()
        {
            var externalLoginResultOutput = new ExternalLoginResultOutput("https://localhost:500/home", true);
            var expectedRedirectViewModel = new RedirectViewModel(externalLoginResultOutput.ReturnUrl);

            _externalLoginInteractorMock.Setup(x => x.ExecuteAsync(It.IsAny<string>())).ReturnsAsync(externalLoginResultOutput);

            var result = await _controller.Callback();
            var viewResult = result.As<ViewResult>();

            viewResult.ViewName.Should().Be("Redirect");
            viewResult.Model.Should().BeEquivalentTo(expectedRedirectViewModel);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(null)]
        public async Task Callback_Should_Return_RedirectResult_When_IsNativeClient_Is_False_Or_Null(bool? isNativeClient)
        {
            const string returnUrl = "https://localhost:500/home";
            var externalLoginResultOutput = new ExternalLoginResultOutput(returnUrl, isNativeClient);

            _externalLoginInteractorMock.Setup(x => x.ExecuteAsync(It.IsAny<string>())).ReturnsAsync(externalLoginResultOutput);

            var result = await _controller.Callback();
            var redirectResult = result.As<RedirectResult>();

            redirectResult.Url.Should().Be(returnUrl);
            redirectResult.Permanent.Should().BeFalse();
        }
    }
}