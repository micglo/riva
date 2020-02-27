using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Moq;
using Riva.Identity.Core.ErrorMessages;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Infrastructure.Services;
using Riva.BuildingBlocks.Core.Models;
using Riva.Identity.Core.Enumerations;
using Xunit;

namespace Riva.Identity.Infrastructure.Test.ServiceTests
{
    public class ResourceOwnerPasswordValidatorTest
    {
        private readonly Mock<IAccountGetterService> _accountGetterServiceMock;
        private readonly Mock<IAccountVerificationService> _accountVerificationServiceMock;
        private readonly Mock<IAccountClaimsCreatorService> _accountClaimsCreatorServiceMock;
        private readonly IResourceOwnerPasswordValidator _resourceOwnerPasswordValidator;

        public ResourceOwnerPasswordValidatorTest()
        {
            _accountGetterServiceMock = new Mock<IAccountGetterService>();
            _accountVerificationServiceMock = new Mock<IAccountVerificationService>();
            _accountClaimsCreatorServiceMock = new Mock<IAccountClaimsCreatorService>();
            _resourceOwnerPasswordValidator = new ResourceOwnerPasswordValidator(_accountGetterServiceMock.Object,
                _accountVerificationServiceMock.Object, _accountClaimsCreatorServiceMock.Object);
        }

        [Fact]
        public async Task ValidateAsync_Should_Set_Success_GrantValidationResult()
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
            var accountCanBeAuthenticatedResult = VerificationResult.Ok();
            var accountClaims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, account.Id.ToString()),
                new Claim(JwtClaimTypes.Email, account.Email),
                new Claim(JwtClaimTypes.EmailVerified, account.Confirmed.ToString(), ClaimValueTypes.Boolean),
                new Claim(JwtClaimTypes.Scope, "Scope"),
                new Claim(ClaimTypes.Name, account.Email),
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString())
            };
            var context = new ResourceOwnerPasswordValidationContext
            {
                Request = new ValidatedTokenRequest
                {
                    GrantType = GrantType.ResourceOwnerPassword
                },
                Password = "Password1234",
                UserName = account.Email
            };

            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(getAccountResult);
            _accountVerificationServiceMock
                .Setup(x => x.VerifyAccountCanBeAuthenticated(It.IsAny<Account>(), It.IsAny<string>()))
                .Returns(accountCanBeAuthenticatedResult);
            _accountClaimsCreatorServiceMock.Setup(x => x.CreateAccountClaimsAsync(It.IsAny<Account>())).ReturnsAsync(accountClaims);

            Func<Task> result = async () => await _resourceOwnerPasswordValidator.ValidateAsync(context);

            await result.Should().NotThrowAsync<Exception>();
            _accountGetterServiceMock.Verify(x => x.GetByEmailAsync(It.Is<string>(e => e == context.UserName)), Times.Once);
            _accountVerificationServiceMock.Verify(x =>
                x.VerifyAccountCanBeAuthenticated(It.Is<Account>(a => a == account),
                    It.Is<string>(p => p == context.Password)), Times.Once);
            _accountClaimsCreatorServiceMock.Verify(x => x.CreateAccountClaimsAsync(It.Is<Account>(a => a == account)), Times.Once);
        }

        [Fact]
        public async Task ValidateAsync_Should_Set_Fail_GrantValidationResult_When_Account_Does_Not_Exist()
        {
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.NotFound, AccountErrorMessage.NotFound)
            };
            var getAccountResult = GetResult<Account>.Fail(errors);
            var context = new ResourceOwnerPasswordValidationContext
            {
                Request = new ValidatedTokenRequest
                {
                    GrantType = GrantType.ResourceOwnerPassword
                }
            };
            var expectedResult = new GrantValidationResult(TokenRequestErrors.InvalidTarget, "Invalid credentials");

            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(getAccountResult);

            Func<Task> result = async () => await _resourceOwnerPasswordValidator.ValidateAsync(context);

            await result.Should().NotThrowAsync<Exception>();
            context.Result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task ValidateAsync_Should_Set_Fail_GrantValidationResult_When_Password_Is_Incorrect_Or_Is_Not_Set()
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
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.IncorrectPassword, AccountErrorMessage.IncorrectPassword)
            };
            var accountCanBeAuthenticatedResult = VerificationResult.Fail(errors);
            var context = new ResourceOwnerPasswordValidationContext
            {
                Request = new ValidatedTokenRequest
                {
                    GrantType = GrantType.ResourceOwnerPassword
                }
            };
            var expectedResult = new GrantValidationResult(TokenRequestErrors.InvalidTarget, "Invalid credentials");

            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(getAccountResult);
            _accountVerificationServiceMock
                .Setup(x => x.VerifyAccountCanBeAuthenticated(It.IsAny<Account>(), It.IsAny<string>()))
                .Returns(accountCanBeAuthenticatedResult);

            Func<Task> result = async () => await _resourceOwnerPasswordValidator.ValidateAsync(context);

            await result.Should().NotThrowAsync<Exception>();
            context.Result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task ValidateAsync_Should_Set_Fail_GrantValidationResult_When_Account_Email_Is_Not_Confirmed()
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
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.NotConfirmed, AccountErrorMessage.NotConfirmed)
            };
            var accountCanBeAuthenticatedResult = VerificationResult.Fail(errors);
            var context = new ResourceOwnerPasswordValidationContext
            {
                Request = new ValidatedTokenRequest
                {
                    GrantType = GrantType.ResourceOwnerPassword
                }
            };
            var expectedResult = new GrantValidationResult(TokenRequestErrors.InvalidTarget, accountCanBeAuthenticatedResult.Errors.Single().ErrorMessage);

            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(getAccountResult);
            _accountVerificationServiceMock
                .Setup(x => x.VerifyAccountCanBeAuthenticated(It.IsAny<Account>(), It.IsAny<string>()))
                .Returns(accountCanBeAuthenticatedResult);

            Func<Task> result = async () => await _resourceOwnerPasswordValidator.ValidateAsync(context);

            await result.Should().NotThrowAsync<Exception>();
            context.Result.Should().BeEquivalentTo(expectedResult);
        }
    }
}