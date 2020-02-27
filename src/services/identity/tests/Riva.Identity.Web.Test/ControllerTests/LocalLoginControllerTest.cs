using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Moq;
using Riva.BuildingBlocks.Core.Models;
using Riva.Identity.Core.Enumerations;
using Riva.Identity.Core.ErrorMessages;
using Riva.Identity.Core.Interactors.LocalLogin;
using Riva.Identity.Web.Controllers;
using Riva.Identity.Web.Models.AppSettings;
using Riva.Identity.Web.Models.ErrorMessages;
using Riva.Identity.Web.Models.Requests;
using Riva.Identity.Web.Models.ViewModels;
using Riva.Identity.Web.ServiceCollectionExtensions;
using Xunit;

namespace Riva.Identity.Web.Test.ControllerTests
{
    public class LocalLoginControllerTest
    {
        private readonly Mock<ILocalLoginInteractor> _localLoginInteractorMock;
        private readonly LocalLoginController _controller;

        public LocalLoginControllerTest()
        {
            _localLoginInteractorMock = new Mock<ILocalLoginInteractor>();
            var applicationUrlsOptionsMock = new Mock<IOptions<ApplicationUrlsAppSettings>>();
            var applicationUrlsAppSettings = new ApplicationUrlsAppSettings
            {
                RivaWebRegistrationUrl = string.Empty,
                RivaWebRequestRegistrationConfirmationEmailUrl = string.Empty,
                RivaWebRequestPasswordResetEmailUrl = string.Empty,
            };
            applicationUrlsOptionsMock.SetupGet(x => x.Value).Returns(applicationUrlsAppSettings);
            _controller = new LocalLoginController(_localLoginInteractorMock.Object, applicationUrlsOptionsMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Fact]
        public async Task Login_Should_Return_ViewResult_After_Get_Request()
        {
            const string returnUrl = "http://returnUrl.com";
            var externalProviders = new List<LocalLoginExternalProviderOutput>
            {
                new LocalLoginExternalProviderOutput("google", AuthenticationExtension.GoogleAuthScheme),
                new LocalLoginExternalProviderOutput("facebook", AuthenticationExtension.FacebookAuthScheme)
            };
            var localLoginOutput = new LocalLoginOutput(true, externalProviders);
            var expectedLocalLoginViewModel =
                new LocalLoginViewModel(true, true, true, string.Empty, string.Empty, string.Empty)
                {
                    ReturnUrl = returnUrl
                };

            _localLoginInteractorMock.Setup(x => x.ExecuteAsync(It.IsAny<string>())).ReturnsAsync(localLoginOutput);

            var result = await _controller.Login(returnUrl);
            var viewResult = result.As<ViewResult>();

            viewResult.Model.Should().BeEquivalentTo(expectedLocalLoginViewModel);
        }

        [Fact]
        public async Task Login_Should_Return_RedirectToActionResult_After_Get_Request()
        {
            const string returnUrl = "http://returnUrl.com";
            var externalProviders = new List<LocalLoginExternalProviderOutput>
            {
                new LocalLoginExternalProviderOutput(AuthenticationExtension.GoogleAuthScheme, AuthenticationExtension.GoogleAuthScheme)
            };
            var localLoginOutput = new LocalLoginOutput(false, externalProviders);
            var expectedRouteValues = new RouteValueDictionary(new { scheme = localLoginOutput.ExternalLoginScheme, returnUrl });

            _localLoginInteractorMock.Setup(x => x.ExecuteAsync(It.IsAny<string>())).ReturnsAsync(localLoginOutput);

            var result = await _controller.Login(returnUrl);
            var redirectToActionResult = result.As<RedirectToActionResult>();

            redirectToActionResult.ControllerName.Should().BeEquivalentTo("ExternalLogin");
            redirectToActionResult.ActionName.Should().BeEquivalentTo("Challenge");
            redirectToActionResult.RouteValues.Should().BeEquivalentTo(expectedRouteValues);
        }

        [Fact]
        public async Task Login_Should_Return_ViewResult_With_Redirect_When_IsInTheContextOfAuthorizationRequest_And_IsNativeClient_Are_True()
        {
            var localLoginRequest = new LocalLoginRequest
            {
                Email = "email@email.com",
                Password = "Password1234",
                ReturnUrl = "http://returnUrl.com",
                RememberLogin = false
            };
            var localLoginResultOutput = LocalLoginResultOutput.Ok(true, true);
            var expectedRedirectViewModel = new RedirectViewModel(localLoginRequest.ReturnUrl);

            _localLoginInteractorMock
                .Setup(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<string>())).ReturnsAsync(localLoginResultOutput);

            var result = await _controller.Login(localLoginRequest);
            var viewResult = result.As<ViewResult>();

            viewResult.ViewName.Should().BeEquivalentTo("Redirect");
            viewResult.Model.Should().BeEquivalentTo(expectedRedirectViewModel);
        }

        [Fact]
        public async Task Login_Should_Return_RedirectResult_When_IsInTheContextOfAuthorizationRequest_Is_True_And_IsNativeClient_Is_False()
        {
            var localLoginRequest = new LocalLoginRequest
            {
                Email = "email@email.com",
                Password = "Password1234",
                ReturnUrl = "http://returnUrl.com",
                RememberLogin = false
            };
            var localLoginResultOutput = LocalLoginResultOutput.Ok(true, false);

            _localLoginInteractorMock
                .Setup(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<string>())).ReturnsAsync(localLoginResultOutput);

            var result = await _controller.Login(localLoginRequest);
            var redirectResult = result.As<RedirectResult>();

            redirectResult.Url.Should().BeEquivalentTo(localLoginRequest.ReturnUrl);
        }

        [Fact]
        public async Task Login_Should_Return_RedirectResult_When_IsInTheContextOfAuthorizationRequest_And_IsNativeClient_Are_False_And_ReturnUrl_Is_Local()
        {
            var localLoginRequest = new LocalLoginRequest
            {
                Email = "email@email.com",
                Password = "Password1234",
                ReturnUrl = "http://returnUrl.com",
                RememberLogin = false
            };
            var localLoginResultOutput = LocalLoginResultOutput.Ok(false, false);
            var urlHelperMock = new Mock<IUrlHelper>(MockBehavior.Strict);
            _controller.Url = urlHelperMock.Object;

            _localLoginInteractorMock
                .Setup(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<string>())).ReturnsAsync(localLoginResultOutput);
            urlHelperMock.Setup(x => x.IsLocalUrl(It.IsAny<string>())).Returns(true);

            var result = await _controller.Login(localLoginRequest);
            var redirectResult = result.As<RedirectResult>();

            redirectResult.Url.Should().Be(localLoginRequest.ReturnUrl);
            redirectResult.Permanent.Should().BeFalse();
        }

        [Fact]
        public async Task Login_Should_Return_RedirectResult_When_IsInTheContextOfAuthorizationRequest_And_IsNativeClient_Are_False_And_ReturnUrl_Is_Not_Defined()
        {
            var localLoginRequest = new LocalLoginRequest
            {
                Email = "email@email.com",
                Password = "Password1234",
                RememberLogin = false
            };
            var localLoginResultOutput = LocalLoginResultOutput.Ok(false, false);
            var urlHelperMock = new Mock<IUrlHelper>(MockBehavior.Strict);
            _controller.Url = urlHelperMock.Object;

            _localLoginInteractorMock
                .Setup(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<string>())).ReturnsAsync(localLoginResultOutput);
            urlHelperMock.Setup(x => x.IsLocalUrl(It.IsAny<string>())).Returns(false);

            var result = await _controller.Login(localLoginRequest);
            var redirectResult = result.As<RedirectResult>();

            redirectResult.Url.Should().Be("~/");
            redirectResult.Permanent.Should().BeFalse();
        }

        [Fact]
        public async Task Login_Should_Return_ViewResult_With_Errors_When_IsInTheContextOfAuthorizationRequest_And_IsNativeClient_Are_False_And_ReturnUrl_Is_Invalid()
        {
            var localLoginRequest = new LocalLoginRequest
            {
                Email = "email@email.com",
                Password = "Password1234",
                RememberLogin = false,
                ReturnUrl = "http://nonEmptyNonLocalUrl.com"
            };
            var localLoginResultOutput = LocalLoginResultOutput.Ok(false, false);
            var urlHelperMock = new Mock<IUrlHelper>(MockBehavior.Strict);
            _controller.Url = urlHelperMock.Object;
            var externalProviders = new List<LocalLoginExternalProviderOutput>
                {
                    new LocalLoginExternalProviderOutput("google", AuthenticationExtension.GoogleAuthScheme),
                    new LocalLoginExternalProviderOutput("facebook", AuthenticationExtension.FacebookAuthScheme)
                };
            var localLoginOutput = new LocalLoginOutput(true, externalProviders);
            var expectedLocalLoginViewModel =
                new LocalLoginViewModel(true, true, true, string.Empty, string.Empty, string.Empty)
                {
                    ReturnUrl = localLoginRequest.ReturnUrl,
                    Email = localLoginRequest.Email,
                    Password = localLoginRequest.Password,
                    RememberLogin = localLoginRequest.RememberLogin
                };
            var localLoginViewModelErrors = new List<string> { LocalLoginErrorMessage.InvalidReturnUrl };
            expectedLocalLoginViewModel.SetErrors(localLoginViewModelErrors);

            _localLoginInteractorMock
                .Setup(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<string>())).ReturnsAsync(localLoginResultOutput);
            urlHelperMock.Setup(x => x.IsLocalUrl(It.IsAny<string>())).Returns(false);
            _localLoginInteractorMock.Setup(x => x.ExecuteAsync(It.IsAny<string>())).ReturnsAsync(localLoginOutput);

            var result = await _controller.Login(localLoginRequest);
            var viewResult = result.As<ViewResult>();

            viewResult.Model.Should().BeEquivalentTo(expectedLocalLoginViewModel);
        }

        [Fact]
        public async Task Login_Should_Return_ViewResult_With_Errors_After_Unsuccessful_Credentials_Verification_When_Account_Is_Not_Found()
        {
            var localLoginRequest = new LocalLoginRequest
            {
                Email = "email@email.com",
                Password = "Password1234",
                RememberLogin = false,
                ReturnUrl = "~/"
            };
            var localLoginResultOutputErrors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.NotFound, AccountErrorMessage.NotFound)
            };
            var localLoginResultOutput = LocalLoginResultOutput.Fail(false, localLoginResultOutputErrors);
            var externalProviders = new List<LocalLoginExternalProviderOutput>
            {
                new LocalLoginExternalProviderOutput("google", AuthenticationExtension.GoogleAuthScheme),
                new LocalLoginExternalProviderOutput("facebook", AuthenticationExtension.FacebookAuthScheme)
            };
            var localLoginOutput = new LocalLoginOutput(true, externalProviders);
            var expectedLocalLoginViewModel =
                new LocalLoginViewModel(true, true, true, string.Empty, string.Empty, string.Empty)
                {
                    ReturnUrl = localLoginRequest.ReturnUrl,
                    Email = localLoginRequest.Email,
                    Password = localLoginRequest.Password,
                    RememberLogin = localLoginRequest.RememberLogin
                };
            var localLoginViewModelErrors = new List<string> { LocalLoginErrorMessage.InvalidCredentials };
            expectedLocalLoginViewModel.SetErrors(localLoginViewModelErrors);

            _localLoginInteractorMock
                .Setup(x => x.ExecuteAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<string>())).ReturnsAsync(localLoginResultOutput);
            _localLoginInteractorMock.Setup(x => x.ExecuteAsync(It.IsAny<string>())).ReturnsAsync(localLoginOutput);

            var result = await _controller.Login(localLoginRequest);
            var viewResult = result.As<ViewResult>();

            viewResult.Model.Should().BeEquivalentTo(expectedLocalLoginViewModel);
        }
    }
}