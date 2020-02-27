using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Models;
using Riva.Identity.Core.Enumerations;
using Riva.Identity.Core.ErrorMessages;
using Riva.Identity.Core.Interactors.LocalLogin;
using Riva.Identity.Core.Models;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Clients.Aggregates;
using Riva.Identity.Domain.Clients.Repositories;
using Riva.Identity.Domain.Accounts.Aggregates;
using Xunit;

namespace Riva.Identity.Core.Test.InteractorTests
{
    public class LocalLoginInteractorTest
    {
        private readonly Mock<IAuthorizationService> _authorizationServiceMock;
        private readonly Mock<IClientRepository> _clientRepositoryMock;
        private readonly Mock<IAccountGetterService> _accountGetterServiceMock;
        private readonly Mock<IAccountVerificationService> _accountVerificationServiceMock;
        private readonly Mock<ISchemeService> _schemeServiceMock;
        private readonly Mock<IAccountClaimsCreatorService> _accountClaimsCreatorServiceMock;
        private readonly Mock<ISignInService> _signInServiceMock;
        private readonly ILocalLoginInteractor _localLoginInteractor;

        public LocalLoginInteractorTest()
        {
            _authorizationServiceMock = new Mock<IAuthorizationService>();
            _clientRepositoryMock = new Mock<IClientRepository>();
            _accountGetterServiceMock = new Mock<IAccountGetterService>();
            _accountVerificationServiceMock = new Mock<IAccountVerificationService>();
            _schemeServiceMock = new Mock<ISchemeService>();
            _accountClaimsCreatorServiceMock = new Mock<IAccountClaimsCreatorService>();
            _signInServiceMock = new Mock<ISignInService>();
            _localLoginInteractor = new LocalLoginInteractor(_authorizationServiceMock.Object,
                _clientRepositoryMock.Object, _accountGetterServiceMock.Object, _accountVerificationServiceMock.Object,
                _schemeServiceMock.Object, _accountClaimsCreatorServiceMock.Object, _signInServiceMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Return_LocalLoginOutput_With_LocalLoginEnabled_True_And_Empty_External_Providers_When_Idenity_Provider_Exists_And_Is_Local_()
        {
            const string redirectUri = "http://localhost";
            var authRequest = new AuthorizationRequest(AuthConstants.LocalIdentityProvider, "clientId", redirectUri);
            var scheme = new AuthenticationScheme("Google", "Google");
            var expectedResult = new LocalLoginOutput(true, new List<LocalLoginExternalProviderOutput>());

            
            _authorizationServiceMock.Setup(x => x.GetAuthorizationRequestAsync(It.IsAny<string>()))
                .ReturnsAsync(authRequest);
            _schemeServiceMock.Setup(x => x.GetSchemeAsync(It.IsAny<string>())).ReturnsAsync(scheme);

            var result = await _localLoginInteractor.ExecuteAsync(redirectUri);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Return_LocalLoginOutput_With_LocalLoginEnabled_False_And_External_Providers_When_Idenity_Provider_Exists_And_Is_Not_Local_()
        {
            const string redirectUri = "http://localhost";
            var scheme = new AuthenticationScheme("Google", "Google");
            var authRequest = new AuthorizationRequest(scheme.Name, "clientId", redirectUri);
            var externalProviders = new List<LocalLoginExternalProviderOutput> {
                new LocalLoginExternalProviderOutput(string.Empty, authRequest.IdP)
            };
            var expectedResult = new LocalLoginOutput(false, externalProviders);


            _authorizationServiceMock.Setup(x => x.GetAuthorizationRequestAsync(It.IsAny<string>()))
                .ReturnsAsync(authRequest);
            _schemeServiceMock.Setup(x => x.GetSchemeAsync(It.IsAny<string>())).ReturnsAsync(scheme);

            var result = await _localLoginInteractor.ExecuteAsync(redirectUri);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Return_LocalLoginOutput_With_LocalLoginEnabled_False_When_When_Identity_Provider_And_ClientId_Are_Provided_But_Scheme_Is_Null()
        {
            const string redirectUri = "http://localhost";
            var identityProviderRestrictions = new List<string> { "Google" };
            var client = Client.Builder()
                .SetId(Guid.NewGuid())
                .SetEnabled(true)
                .SetEnableLocalLogin(false)
                .SetRequirePkce(false)
                .SetIdentityProviderRestrictions(identityProviderRestrictions)
                .Build();
            var authRequest = new AuthorizationRequest("idP", client.Id.ToString(), redirectUri);
            var schemes = new List<AuthenticationScheme>
            {
                new AuthenticationScheme("Google", "Google"),
                new AuthenticationScheme("Facebook", "Facebook")
            };
            var externalProviders = schemes
                .Where(scheme => client.IdentityProviderRestrictions.Contains(scheme.DisplayName)).Select(scheme =>
                    new LocalLoginExternalProviderOutput(scheme.DisplayName, scheme.Name));
            var expectedResult = new LocalLoginOutput(client.EnableLocalLogin, externalProviders);

            _authorizationServiceMock.Setup(x => x.GetAuthorizationRequestAsync(It.IsAny<string>()))
                .ReturnsAsync(authRequest);
            _schemeServiceMock.Setup(x => x.GetSchemeAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<AuthenticationScheme>(null));
            _schemeServiceMock.Setup(x => x.GetAllSchemesAsync()).ReturnsAsync(schemes);
            _clientRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(client);

            var result = await _localLoginInteractor.ExecuteAsync("http://localhost");

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Return_LocalLoginOutput_With_LocalLoginEnabled_False_When_When_Identity_Provider_And_ClientId_Are_Not_Provided()
        {
            const string redirectUri = "http://localhost";
            var authRequest = new AuthorizationRequest(null, null, redirectUri);
            var schemes = new List<AuthenticationScheme>
            {
                new AuthenticationScheme("Google", "Google"),
                new AuthenticationScheme("Facebook", "Facebook")
            };
            var externalProviders = schemes.Select(scheme =>
                new LocalLoginExternalProviderOutput(scheme.DisplayName ?? scheme.Name, scheme.Name));
            var expectedResult = new LocalLoginOutput(true, externalProviders);

            _authorizationServiceMock.Setup(x => x.GetAuthorizationRequestAsync(It.IsAny<string>()))
                .ReturnsAsync(authRequest);
            _schemeServiceMock.Setup(x => x.GetAllSchemesAsync()).ReturnsAsync(schemes);
            _clientRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Client>(null));

            var result = await _localLoginInteractor.ExecuteAsync("http://localhost");

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Return_LocalLoginResultOutput_With_Fail_When_Account_Does_Not_Exist()
        {
            const string redirectUri = "http://localhost";
            var authRequest = new AuthorizationRequest("idP", "clientId", redirectUri);
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.NotFound, AccountErrorMessage.NotFound)
            };
            var getAccountResult = GetResult<Account>.Fail(errors);
            var expectedResult = LocalLoginResultOutput.Fail(true, errors);

            _authorizationServiceMock.Setup(x => x.GetAuthorizationRequestAsync(It.IsAny<string>()))
                .ReturnsAsync(authRequest);
            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(getAccountResult);

            var result = await _localLoginInteractor.ExecuteAsync("email@email.com", "Password", true, redirectUri);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Return_LocalLoginResultOutput_With_Fail_When_Account_Cannot_Be_Authenticated()
        {
            const string redirectUri = "http://localhost";
            var authRequest = new AuthorizationRequest("idP", "clientId", redirectUri);
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(true)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.NotConfirmed, AccountErrorMessage.NotConfirmed)
            };
            var accountCanBeAuthenticatedVerificationResult = VerificationResult.Fail(errors);
            var expectedResult = LocalLoginResultOutput.Fail(true, errors);

            _authorizationServiceMock.Setup(x => x.GetAuthorizationRequestAsync(It.IsAny<string>()))
                .ReturnsAsync(authRequest);
            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(getAccountResult);
            _accountVerificationServiceMock
                .Setup(x => x.VerifyAccountCanBeAuthenticated(It.IsAny<Account>(), It.IsAny<string>()))
                .Returns(accountCanBeAuthenticatedVerificationResult);

            var result = await _localLoginInteractor.ExecuteAsync("email@email.com", "Password", true, redirectUri);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task ExecuteAsync_Should_Return_LocalLoginResultOutput_With_Success_Re_When_ClientId_Is_Not_Provided_For_Given_ReturnUrl()
        {
            const string redirectUri = "http://localhost";
            var authRequest = new AuthorizationRequest("idP", "clientId", redirectUri);
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(true)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);
            var accountCanBeAuthenticatedVerificationResult = VerificationResult.Ok();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, account.Email)
            };
            var expectedResult = LocalLoginResultOutput.Ok(true, false);

            _authorizationServiceMock.Setup(x => x.GetAuthorizationRequestAsync(It.IsAny<string>()))
                .ReturnsAsync(authRequest);
            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(getAccountResult);
            _accountVerificationServiceMock
                .Setup(x => x.VerifyAccountCanBeAuthenticated(It.IsAny<Account>(), It.IsAny<string>()))
                .Returns(accountCanBeAuthenticatedVerificationResult);
            _accountClaimsCreatorServiceMock.Setup(x => x.CreateAccountClaimsAsync(It.IsAny<Account>()))
                .ReturnsAsync(claims);
            _signInServiceMock
                .Setup(x => x.SignInAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<IEnumerable<Claim>>())).Returns(Task.CompletedTask);

            var result = await _localLoginInteractor.ExecuteAsync(account.Email, "Password", true, redirectUri);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}