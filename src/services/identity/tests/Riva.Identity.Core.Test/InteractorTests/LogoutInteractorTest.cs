using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.Identity.Core.Interactors.Logout;
using Riva.Identity.Core.Models;
using Riva.Identity.Core.Services;
using Xunit;

namespace Riva.Identity.Core.Test.InteractorTests
{
    public class LogoutInteractorTest
    {
        private readonly Mock<ILogoutService> _logoutServiceMock;
        private readonly Mock<IClaimsPrincipalService> _claimsPrincipalServiceMock;
        private readonly ILogoutInteractor _interactor;

        public LogoutInteractorTest()
        {
            _logoutServiceMock = new Mock<ILogoutService>();
            _claimsPrincipalServiceMock = new Mock<IClaimsPrincipalService>();
            _interactor = new LogoutInteractor(_logoutServiceMock.Object, _claimsPrincipalServiceMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Return_LogoutOutput_With_ShowLogoutPrompt_False_When_Account_Is_Not_Authenticated()
        {
            const string logoutId = "logoutId";
            var cp = new ClaimsPrincipal();
            var expectedResult = new LogoutOutput(logoutId, false);

            _claimsPrincipalServiceMock.Setup(x => x.GetClaimsPrincipal()).Returns(cp);

            var result = await _interactor.ExecuteAsync(logoutId);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Return_LogoutOutput_With_ShowLogoutPrompt_True_When_LogoutRequest_ShowSignOutPrompt_Is_True()
        {
            const string logoutId = "logoutId";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, "email@email.com")
            };
            var claimsIdentity = new ClaimsIdentity(claims, "authType");
            var cp = new ClaimsPrincipal(claimsIdentity);
            var logoutRequest = new LogoutRequest(true, string.Empty, string.Empty, Guid.NewGuid(), Guid.NewGuid());
            var expectedResult = new LogoutOutput(logoutId, logoutRequest.ShowSignOutPrompt);

            _claimsPrincipalServiceMock.Setup(x => x.GetClaimsPrincipal()).Returns(cp);
            _logoutServiceMock.Setup(x => x.GetLogoutRequestAsync(It.IsAny<string>()))
                .ReturnsAsync(logoutRequest);

            var result = await _interactor.ExecuteAsync(logoutId);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Return_LogoutOutput_With_ShowLogoutPrompt_True_When_LogoutRequest_Is_Null()
        {
            const string logoutId = "logoutId";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, "email@email.com")
            };
            var claimsIdentity = new ClaimsIdentity(claims, "authType");
            var cp = new ClaimsPrincipal(claimsIdentity);
            var expectedResult = new LogoutOutput(logoutId, true);

            _claimsPrincipalServiceMock.Setup(x => x.GetClaimsPrincipal()).Returns(cp);
            _logoutServiceMock.Setup(x => x.GetLogoutRequestAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<LogoutRequest>(null));

            var result = await _interactor.ExecuteAsync(logoutId);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}