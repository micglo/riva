using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.Identity.Core.Interactors.ExternalLogin;
using Riva.Identity.Core.Models;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Aggregates;
using Xunit;

namespace Riva.Identity.Core.Test.InteractorTests
{
    public class ExternalLoginInteractorTest
    {
        private readonly Mock<IAuthorizationService> _authorizationServiceMock;
        private readonly Mock<IAccountProviderService> _accountProviderServiceMock;
        private readonly Mock<IIntegrationEventBus> _integrationEventBusMock;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;
        private readonly Mock<IAccountClaimsCreatorService> _accountClaimsCreatorServiceMock;
        private readonly Mock<ISignInService> _signInServiceMock;
        private readonly Mock<ISignOutService> _signOutServiceMock;
        private readonly IExternalLoginInteractor _externalLoginInteractor;

        public ExternalLoginInteractorTest()
        {
            _authorizationServiceMock = new Mock<IAuthorizationService>();
            _accountProviderServiceMock = new Mock<IAccountProviderService>();
            _integrationEventBusMock = new Mock<IIntegrationEventBus>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _accountClaimsCreatorServiceMock = new Mock<IAccountClaimsCreatorService>();
            _signInServiceMock = new Mock<ISignInService>();
            _signOutServiceMock = new Mock<ISignOutService>();
            _externalLoginInteractor = new ExternalLoginInteractor(_authorizationServiceMock.Object,
                _accountProviderServiceMock.Object, _integrationEventBusMock.Object, _authenticationServiceMock.Object, 
                _accountClaimsCreatorServiceMock.Object, _signInServiceMock.Object, _signOutServiceMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Throw_Authentication_Result_Exception()
        {
            var exception = new Exception("Invalid scheme.");
            var authResult = new AuthenticateResult(false, exception, null, null);

            _authenticationServiceMock.Setup(x => x.AuthenticateAsync(It.IsAny<string>())).ReturnsAsync(authResult);

            Func<Task<ExternalLoginResultOutput>> result = async () => await _externalLoginInteractor.ExecuteAsync("Scheme");

            var exceptionResult = await result.Should().ThrowExactlyAsync<Exception>();
            exceptionResult.WithMessage(exception.Message);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Return_ExternalLoginResultOutput()
        {
            const string email = "email@email.com";
            const string returnUrl = "http://localhost";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email)
            };
            var claimsIdentities = new List<ClaimsIdentity>
            {
                new ClaimsIdentity(claims)
            };
            var cp = new ClaimsPrincipal(claimsIdentities);
            var authResult = new AuthenticateResult(true, null, cp, new Dictionary<string, string> { { "returnUrl", returnUrl } });
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail(email)
                .SetConfirmed(true)
                .SetPasswordHash(string.Empty)
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();
            account.AddCreatedEvent(Guid.NewGuid());
            var authRequest = new AuthorizationRequest("idP", "clientId", returnUrl);
            var expectedResult = new ExternalLoginResultOutput(returnUrl, false);

            _authenticationServiceMock.Setup(x => x.AuthenticateAsync(It.IsAny<string>()))
                .ReturnsAsync(authResult);
            _accountProviderServiceMock.Setup(x => x.ProvideAccountForExternalLoginAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(account);
            _integrationEventBusMock.Setup(x => x.PublishIntegrationEventAsync(It.IsAny<IIntegrationEvent>()))
                .Returns(Task.CompletedTask);
            _accountClaimsCreatorServiceMock.Setup(x => x.CreateAccountClaimsAsync(It.IsAny<Account>())).ReturnsAsync(claims);
            _signInServiceMock
                .Setup(x => x.ExternalSignInAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<IEnumerable<Claim>>())).Returns(Task.CompletedTask);
            _signOutServiceMock.Setup(x => x.SignOutAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            _authorizationServiceMock.Setup(x => x.GetAuthorizationRequestAsync(It.IsAny<string>()))
                .ReturnsAsync(authRequest);

            var result = await _externalLoginInteractor.ExecuteAsync("Scheme");

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}