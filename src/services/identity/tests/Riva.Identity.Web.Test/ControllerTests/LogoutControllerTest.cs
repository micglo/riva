using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.Identity.Core.Interactors.Logout;
using Riva.Identity.Web.Controllers;
using Riva.Identity.Web.Models.ViewModels;
using Xunit;

namespace Riva.Identity.Web.Test.ControllerTests
{
    public class LogoutControllerTest
    {
        private readonly Mock<ILogoutInteractor> _logoutInteractorMock;
        private readonly Mock<ILoggedOutInteractor> _loggedOutInteractorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly LogoutController _controller;

        public LogoutControllerTest()
        {
            _logoutInteractorMock = new Mock<ILogoutInteractor>();
            _loggedOutInteractorMock = new Mock<ILoggedOutInteractor>();
            _mapperMock = new Mock<IMapper>();
            _controller = new LogoutController(_logoutInteractorMock.Object, _loggedOutInteractorMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Logout_Should_Return_ViewResult()
        {
            const string logoutId = "logoutId";
            var logoutOutput = new LogoutOutput(logoutId, true);
            var logoutViewModel = new LogoutViewModel(logoutOutput.LogoutId, logoutOutput.ShowLogoutPrompt);

            _logoutInteractorMock.Setup(x => x.ExecuteAsync(It.IsAny<string>())).ReturnsAsync(logoutOutput);
            _mapperMock.Setup(x => x.Map<LogoutOutput, LogoutViewModel>(It.IsAny<LogoutOutput>()))
                .Returns(logoutViewModel);

            var result = await _controller.Logout(logoutId);
            var viewResult = result.As<ViewResult>();

            viewResult.Should().NotBeNull();
            viewResult.ViewData.Model.Should().BeEquivalentTo(logoutViewModel);
        }

        [Fact]
        public async Task Logout_Should_Return_RedirectToActionResult()
        {
            const string logoutId = "logoutId";

            var result = await _controller.Logout(logoutId, "false");
            var redirectToActionResult = result.As<RedirectToActionResult>();

            redirectToActionResult.ControllerName.Should().Be("Home");
            redirectToActionResult.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task Logout_Should_Return_SignOutResult_When_TriggerExternalSignOut_Is_True()
        {
            const string logoutId = "logoutId";
            const string authenticationScheme = "Google";
            var loggedOutOutput = new LoggedOutOutput(logoutId, "postLogoutRedirectUri", "signOutIframeUrl",
                Guid.NewGuid(), authenticationScheme);
            var loggedOutViewModel = new LoggedOutViewModel(loggedOutOutput.LogoutId,
                loggedOutOutput.PostLogoutRedirectUri, loggedOutOutput.SignOutIframeUrl, loggedOutOutput.ClientId,
                loggedOutOutput.ExternalAuthenticationScheme);
            var urlHelperMock = new Mock<IUrlHelper>(MockBehavior.Strict);
            const string redirectUrl = "http://localhost:5000/auth/logout";
            var expectedAuthProperties = new AuthenticationProperties { RedirectUri = redirectUrl };

            _mapperMock.Setup(x => x.Map<LoggedOutOutput, LoggedOutViewModel>(It.IsAny<LoggedOutOutput>()))
                .Returns(loggedOutViewModel);
            _loggedOutInteractorMock.Setup(x => x.ExecuteAsync(It.IsAny<string>())).ReturnsAsync(loggedOutOutput);
            urlHelperMock.Setup(x => x.Action(It.IsAny<UrlActionContext>())).Returns(redirectUrl);
            _controller.Url = urlHelperMock.Object;

            var result = await _controller.Logout(logoutId, "true");
            var signOutResult = result.As<SignOutResult>();

            signOutResult.Properties.Should().BeEquivalentTo(expectedAuthProperties);
            signOutResult.AuthenticationSchemes.Should().Contain(authenticationScheme);
        }

        [Fact]
        public async Task Logout_Should_Return_ViewResult_When_TriggerExternalSignOut_Is_False()
        {
            const string logoutId = "logoutId";
            const string authenticationScheme = null;
            var loggedOutOutput = new LoggedOutOutput(logoutId, "postLogoutRedirectUri", "signOutIframeUrl",
                Guid.NewGuid(), authenticationScheme);
            var loggedOutViewModel = new LoggedOutViewModel(loggedOutOutput.LogoutId,
                loggedOutOutput.PostLogoutRedirectUri, loggedOutOutput.SignOutIframeUrl, loggedOutOutput.ClientId,
                loggedOutOutput.ExternalAuthenticationScheme);

            _mapperMock.Setup(x => x.Map<LoggedOutOutput, LoggedOutViewModel>(It.IsAny<LoggedOutOutput>()))
                .Returns(loggedOutViewModel);
            _loggedOutInteractorMock.Setup(x => x.ExecuteAsync(It.IsAny<string>())).ReturnsAsync(loggedOutOutput);

            var result = await _controller.Logout(logoutId, "true");
            var viewResult = result.As<ViewResult>();

            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().BeEquivalentTo("LoggedOut");
            viewResult.ViewData.Model.Should().BeEquivalentTo(loggedOutViewModel);
        }
    }
}