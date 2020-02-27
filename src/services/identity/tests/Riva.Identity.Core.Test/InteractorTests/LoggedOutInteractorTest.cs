using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.Identity.Core.Interactors.Logout;
using Riva.Identity.Core.Models;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.PersistedGrants.Repositories;
using Xunit;

namespace Riva.Identity.Core.Test.InteractorTests
{
    public class LoggedOutInteractorTest
    {
        private readonly Mock<ILogoutService> _logoutServiceMock;
        private readonly Mock<IPersistedGrantRepository> _persistedGrantRepositoryMock;
        private readonly Mock<IClaimsPrincipalService> _claimsPrincipalServiceMock;
        private readonly Mock<ISignOutService> _signOutServiceMock;
        private readonly Mock<ISchemeService> _schemeServiceMock;
        private readonly ILoggedOutInteractor _interactor;

        public LoggedOutInteractorTest()
        {
            _logoutServiceMock = new Mock<ILogoutService>();
            _persistedGrantRepositoryMock = new Mock<IPersistedGrantRepository>();
            _claimsPrincipalServiceMock = new Mock<IClaimsPrincipalService>();
            _signOutServiceMock = new Mock<ISignOutService>();
            _schemeServiceMock = new Mock<ISchemeService>();
            _interactor = new LoggedOutInteractor(_logoutServiceMock.Object, _persistedGrantRepositoryMock.Object, 
                _claimsPrincipalServiceMock.Object, _signOutServiceMock.Object, _schemeServiceMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Return_LoggedOutOutput_When_Account_Is_Not_Authenticated()
        {
            const string logoutId = "logoutId";
            var cp = new ClaimsPrincipal();
            var logoutRequest = new LogoutRequest(true, string.Empty, string.Empty, Guid.NewGuid(), Guid.NewGuid());
            var expectedResult = new LoggedOutOutput(logoutId, logoutRequest.PostLogoutRedirectUri,
                logoutRequest.SignOutIFrameUrl, logoutRequest.ClientId, null);

            _logoutServiceMock.Setup(x => x.GetLogoutRequestAsync(It.IsAny<string>()))
                .ReturnsAsync(logoutRequest);
            _claimsPrincipalServiceMock.Setup(x => x.GetClaimsPrincipal()).Returns(cp);

            var result = await _interactor.ExecuteAsync(logoutId);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Return_LoggedOutOutput_When_Identity_Provider_Is_Not_Local_And_Scheme_Supports_Sign_Out()
        {
            var logoutId = string.Empty;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, "email@email.com")
            };
            var claimsIdentity = new ClaimsIdentity(claims, "authType");
            var cp = new ClaimsPrincipal(claimsIdentity);
            var logoutRequest = new LogoutRequest(true, string.Empty, string.Empty, Guid.NewGuid(), Guid.NewGuid());
            const string idp = "Google";
            var logoutContext = Guid.NewGuid().ToString();
            var expectedResult = new LoggedOutOutput(logoutContext, logoutRequest.PostLogoutRedirectUri,
                logoutRequest.SignOutIFrameUrl, logoutRequest.ClientId, idp);

            _logoutServiceMock.Setup(x => x.GetLogoutRequestAsync(It.IsAny<string>()))
                .ReturnsAsync(logoutRequest);
            _claimsPrincipalServiceMock.Setup(x => x.GetClaimsPrincipal()).Returns(cp);
            _signOutServiceMock.Setup(x => x.SignOutAsync()).Returns(Task.CompletedTask);
            _persistedGrantRepositoryMock.Setup(x => x.DeleteAllBySubjectIdAsync(It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);
            _claimsPrincipalServiceMock.Setup(x => x.GetNonLocalIdentityProvider(It.IsAny<ClaimsPrincipal>()))
                .Returns(idp);
            _schemeServiceMock.Setup(x => x.SchemeSupportsSignOutAsync(It.IsAny<string>())).ReturnsAsync(true);
            _logoutServiceMock.Setup(x => x.CreateLogoutContextAsync()).ReturnsAsync(logoutContext);

            var result = await _interactor.ExecuteAsync(logoutId);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}