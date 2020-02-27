using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.Identity.Core.Enumerations;
using Riva.Identity.Core.ErrorMessages;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Entities;
using Riva.Identity.Domain.Accounts.Enumerations;
using Riva.Identity.Infrastructure.Services;
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.Identity.Infrastructure.Test.ServiceTests
{
    public class AccountVerificationServiceTest
    {
        private readonly Mock<IAccountGetterService> _accountGetterServiceMock;
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly IAccountVerificationService _accountVerificationService;

        public AccountVerificationServiceTest()
        {
            _accountGetterServiceMock = new Mock<IAccountGetterService>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _accountVerificationService = new AccountVerificationService(_accountGetterServiceMock.Object,
                _passwordServiceMock.Object);
        }

        [Fact]
        public async Task VerifyEmailIsNotTakenAsync_Should_Return_VerificationResult_With_Success_True()
        {
            const string email = "email@email.com";
            var getAccountResult = GetResult<Account>.Fail(new List<IError>());
            var expectedResult = VerificationResult.Ok();

            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(getAccountResult);

            var result = await _accountVerificationService.VerifyEmailIsNotTakenAsync(email);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task VerifyEmailIsNotTakenAsync_Should_Return_VerificationResult_With_Success_False_When_Email_Is_Already_Taken()
        {
            const string email = "email@email.com";
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail(email)
                .SetConfirmed(true)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.EmailIsAlreadyTaken, AccountErrorMessage.EmailIsAlreadyTaken)
            };
            var expectedResult = VerificationResult.Fail(errors);

            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(getAccountResult);

            var result = await _accountVerificationService.VerifyEmailIsNotTakenAsync(email);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyAccountIsNotConfirmed_Should_Return_VerificationResult_With_Success_True()
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
            var expectedResult = VerificationResult.Ok();

            var result = _accountVerificationService.VerifyAccountIsNotConfirmed(account.Confirmed);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyAccountIsNotConfirmed_Should_Return_VerificationResult_With_Success_False_When_Account_Is_Already_Confirmed()
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
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.AlreadyConfirmed, AccountErrorMessage.AlreadyConfirmed)
            };
            var expectedResult = VerificationResult.Fail(errors);

            var result = _accountVerificationService.VerifyAccountIsNotConfirmed(account.Confirmed);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyAccountIsConfirmed_Should_Return_VerificationResult_With_Success_True()
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
            var expectedResult = VerificationResult.Ok();

            var result = _accountVerificationService.VerifyAccountIsConfirmed(account.Confirmed);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyAccountIsConfirmed_Should_Return_VerificationResult_With_Success_False_When_Account_Is_Not_Confirmed()
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
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.NotConfirmed, AccountErrorMessage.NotConfirmed)
            };
            var expectedResult = VerificationResult.Fail(errors);

            var result = _accountVerificationService.VerifyAccountIsConfirmed(account.Confirmed);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyPassword_Should_Return_VerificationResult_With_Success_True()
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
            var expectedResult = VerificationResult.Ok();

            _passwordServiceMock.Setup(x => x.VerifyHashedPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            var result = _accountVerificationService.VerifyPassword(account.PasswordHash, "password");

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyPassword_Should_Return_VerificationResult_With_Success_False_When_Password_Is_Incorrect()
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
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.IncorrectPassword, AccountErrorMessage.IncorrectPassword)
            };
            var expectedResult = VerificationResult.Fail(errors);

            _passwordServiceMock.Setup(x => x.VerifyHashedPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            var result = _accountVerificationService.VerifyPassword(account.PasswordHash, "password");

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyPasswordIsSet_Should_Return_VerificationResult_With_Success_True()
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
            var expectedResult = VerificationResult.Ok();

            var result = _accountVerificationService.VerifyPasswordIsSet(account.PasswordHash);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyPasswordIsSet_Should_Return_VerificationResult_With_Success_False_When_Password_Is_Not_Set()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(false)
                .SetPasswordHash(string.Empty)
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.PasswordIsNotSet, AccountErrorMessage.PasswordIsNotSet)
            };
            var expectedResult = VerificationResult.Fail(errors);

            var result = _accountVerificationService.VerifyPasswordIsSet(account.PasswordHash);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyPasswordIsNotSet_Should_Return_VerificationResult_With_Success_True()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(true)
                .SetPasswordHash(string.Empty)
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();
            var expectedResult = VerificationResult.Ok();

            var result = _accountVerificationService.VerifyPasswordIsNotSet(account.PasswordHash);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyPasswordIsNotSet_Should_Return_VerificationResult_With_Success_False_When_Password_Is_Set()
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
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.PasswordAlreadySet, AccountErrorMessage.PasswordAlreadySet)
            };
            var expectedResult = VerificationResult.Fail(errors);

            var result = _accountVerificationService.VerifyPasswordIsNotSet(account.PasswordHash);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyAccountCanBeAuthenticated_Should_Return_VerificationResult_With_Success_True()
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
            var expectedResult = VerificationResult.Ok();

            _passwordServiceMock.Setup(x => x.VerifyHashedPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            var result = _accountVerificationService.VerifyAccountCanBeAuthenticated(account, "password");

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyAccountCanBeAuthenticated_Should_Return_VerificationResult_With_Success_False_When_Account_Is_Not_Confirmed()
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
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.NotConfirmed, AccountErrorMessage.NotConfirmed)
            };
            var expectedResult = VerificationResult.Fail(errors);

            var result = _accountVerificationService.VerifyAccountCanBeAuthenticated(account, "password");

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyAccountCanBeAuthenticated_Should_Return_VerificationResult_With_Success_False_When_Password_Is_Not_Set()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(true)
                .SetPasswordHash(string.Empty)
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.PasswordIsNotSet, AccountErrorMessage.PasswordIsNotSet)
            };
            var expectedResult = VerificationResult.Fail(errors);

            var result = _accountVerificationService.VerifyAccountCanBeAuthenticated(account, "password");

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyAccountCanBeAuthenticated_Should_Return_VerificationResult_With_Success_False_When_Password_Is_Incorrect()
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
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.IncorrectPassword, AccountErrorMessage.IncorrectPassword)
            };
            var expectedResult = VerificationResult.Fail(errors);

            _passwordServiceMock.Setup(x => x.VerifyHashedPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            var result = _accountVerificationService.VerifyAccountCanBeAuthenticated(account, "password");

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyAccountToken_Should_Return_VerificationResult_With_Success_True()
        {
            const string tokenValue = "12345";
            var token = Token.Builder()
                .SetIssued(DateTimeOffset.UtcNow)
                .SetExpires(DateTimeOffset.UtcNow.AddDays(1))
                .SetType(TokenTypeEnumeration.AccountConfirmation)
                .SetValue(tokenValue)
                .Build();
            var expectedResult = VerificationResult.Ok();

            var result = _accountVerificationService.VerifyConfirmationCode(token, tokenValue);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyAccountToken_Should_Return_VerificationResult_With_Success_False_When_Token_Is_Null()
        {
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.ConfirmationCodeWasNotGenerated, AccountErrorMessage.ConfirmationCodeWasNotGenerated)
            };
            var expectedResult = VerificationResult.Fail(errors);

            var result = _accountVerificationService.VerifyConfirmationCode(null, "12345");

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyAccountToken_Should_Return_VerificationResult_With_Success_False_When_Confirmation_Code_Is_Incorrect()
        {
            const string tokenValue = "12345";
            const string wrongConfirmationCode = "54321";
            var token = Token.Builder()
                .SetIssued(DateTimeOffset.UtcNow)
                .SetExpires(DateTimeOffset.UtcNow.AddDays(1))
                .SetType(TokenTypeEnumeration.AccountConfirmation)
                .SetValue(tokenValue)
                .Build();
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.IncorrectConfirmationCode, AccountErrorMessage.IncorrectConfirmationCode)
            };
            var expectedResult = VerificationResult.Fail(errors);

            var result = _accountVerificationService.VerifyConfirmationCode(token, wrongConfirmationCode);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void VerifyAccountToken_Should_Return_VerificationResult_With_Success_False_When_Token_Is_Expired()
        {
            const string tokenValue = "12345";
            var token = Token.Builder()
                .SetIssued(DateTimeOffset.UtcNow.AddDays(-2))
                .SetExpires(DateTimeOffset.UtcNow.AddDays(-1))
                .SetType(TokenTypeEnumeration.AccountConfirmation)
                .SetValue(tokenValue)
                .Build();
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.ConfirmationCodeExpired, AccountErrorMessage.ConfirmationCodeExpired)
            };
            var expectedResult = VerificationResult.Fail(errors);

            var result = _accountVerificationService.VerifyConfirmationCode(token, tokenValue);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}