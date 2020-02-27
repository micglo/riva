using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Moq;
using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Domain;
using Riva.Identity.Core.Enumerations;
using Riva.Identity.Core.ErrorMessages;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Infrastructure.Services;
using Xunit;

namespace Riva.Identity.Infrastructure.Test.ServiceTests
{
    public class ProfileServiceTest
    {
        private readonly Mock<IAccountGetterService> _accountGetterServiceMock;
        private readonly Mock<IAccountClaimsCreatorService> _accountClaimsCreatorServiceMock;
        private readonly IProfileService _profileService;

        public ProfileServiceTest()
        {
            _accountGetterServiceMock = new Mock<IAccountGetterService>();
            _accountClaimsCreatorServiceMock = new Mock<IAccountClaimsCreatorService>();
            _profileService = new ProfileService(_accountGetterServiceMock.Object, _accountClaimsCreatorServiceMock.Object);
        }

        [Fact]
        public async Task GetProfileDataAsync_Should_Set_Issued_Claims_When_Identity_Is_Set()
        {
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
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Email),
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString())
            };
            var claimsIdentities = new List<ClaimsIdentity>
            {
                new ClaimsIdentity(claims)
            };
            var cp = new ClaimsPrincipal(claimsIdentities);
            var context = new ProfileDataRequestContext(cp, new Client(), "caller", new List<string>());
            var accountClaims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, account.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(JwtClaimTypes.Email, account.Email),
                new Claim(JwtClaimTypes.EmailVerified, account.Confirmed.ToString(), ClaimValueTypes.Boolean),
                new Claim(JwtClaimTypes.Scope, "Scope"),
                new Claim(JwtClaimTypes.Role, DefaultRoleEnumeration.User.DisplayName)
            };

            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(getAccountResult);
            _accountClaimsCreatorServiceMock.Setup(x => x.CreateAccountClaimsAsync(It.IsAny<Account>())).ReturnsAsync(accountClaims);

            Func<Task> result = async () => await _profileService.GetProfileDataAsync(context);

            await result.Should().NotThrowAsync<Exception>();
            context.IssuedClaims.Should().BeEquivalentTo(accountClaims);
        }

        [Fact]
        public async Task GetProfileDataAsync_Should_Set_Issued_Claims_When_Identity_Is_Not_Set()
        {
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
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Email, account.Email)
            };
            var claimsIdentities = new List<ClaimsIdentity>
            {
                new ClaimsIdentity(claims)
            };
            var cp = new ClaimsPrincipal(claimsIdentities);
            var context = new ProfileDataRequestContext(cp, new Client(), "caller", new List<string>());

            var accountClaims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, account.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(JwtClaimTypes.Email, account.Email),
                new Claim(JwtClaimTypes.EmailVerified, account.Confirmed.ToString(), ClaimValueTypes.Boolean),
                new Claim(JwtClaimTypes.Scope, "Scope"),
                new Claim(JwtClaimTypes.Role, DefaultRoleEnumeration.User.DisplayName)
            };

            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(getAccountResult);
            _accountClaimsCreatorServiceMock.Setup(x => x.CreateAccountClaimsAsync(It.IsAny<Account>())).ReturnsAsync(accountClaims);

            Func<Task> result = async () => await _profileService.GetProfileDataAsync(context);

            await result.Should().NotThrowAsync<Exception>();
            context.IssuedClaims.Should().BeEquivalentTo(accountClaims);
        }

        [Fact]
        public async Task IsActiveAsync_Should_Set_IsActive_To_True_When_Identity_Is_Set()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(true)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid>{ Guid.NewGuid() })
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Email),
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString())
            };
            var claimsIdentities = new List<ClaimsIdentity>
            {
                new ClaimsIdentity(claims)
            };
            var cp = new ClaimsPrincipal(claimsIdentities);
            var context = new IsActiveContext(cp, new Client(), "caller");

            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(getAccountResult);

            Func<Task> result = async () => await _profileService.IsActiveAsync(context);

            await result.Should().NotThrowAsync<Exception>();
            context.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task IsActiveAsync_Should_Set_IsActive_To_True_When_Identity_Is_Not_Set()
        {
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
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Email, account.Email)
            };
            var claimsIdentities = new List<ClaimsIdentity>
            {
                new ClaimsIdentity(claims)
            };
            var cp = new ClaimsPrincipal(claimsIdentities);
            var context = new IsActiveContext(cp, new Client(), "caller");

            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(getAccountResult);

            Func<Task> result = async () => await _profileService.IsActiveAsync(context);

            await result.Should().NotThrowAsync<Exception>();
            context.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task IsActiveAsync_Should_Set_IsActive_To_False_When_Account_Is_Not_Confirmed()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Email),
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString())
            };
            var claimsIdentities = new List<ClaimsIdentity>
            {
                new ClaimsIdentity(claims)
            };
            var cp = new ClaimsPrincipal(claimsIdentities);
            var context = new IsActiveContext(cp, new Client(), "caller");

            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(getAccountResult);

            Func<Task> result = async () => await _profileService.IsActiveAsync(context);

            await result.Should().NotThrowAsync<Exception>();
            context.IsActive.Should().BeFalse();
        }

        [Fact]
        public async Task IsActiveAsync_Should_Set_IsActive_To_False_When_Account_Is_Not_Found()
        {
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.NotFound, AccountErrorMessage.NotFound)
            };
            var getAccountResult = GetResult<Account>.Fail(errors);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "email@email.com"),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            };
            var claimsIdentities = new List<ClaimsIdentity>
            {
                new ClaimsIdentity(claims)
            };
            var cp = new ClaimsPrincipal(claimsIdentities);
            var context = new IsActiveContext(cp, new Client(), "caller");

            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(getAccountResult);

            Func<Task> result = async () => await _profileService.IsActiveAsync(context);

            await result.Should().NotThrowAsync<Exception>();
            context.IsActive.Should().BeFalse();
        }
    }
}