using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Models;
using Riva.Users.Core.Enumerations;
using Riva.Users.Core.ErrorMessages;
using Riva.Users.Core.Models;
using Riva.Users.Core.Services;
using Riva.Users.Infrastructure.Models.ApiClientResponses.RivaIdentity;
using Riva.Users.Infrastructure.Services;
using Xunit;

namespace Riva.Users.Infrastructure.Test.ServiceTests
{
    public class AccountVerificationServiceTest
    {
        private readonly Mock<IRivaIdentityApiClientService> _rivaIdentityApiClientServiceMock;
        private readonly IAccountVerificationService _service;

        public AccountVerificationServiceTest()
        {
            _rivaIdentityApiClientServiceMock = new Mock<IRivaIdentityApiClientService>();
            _service = new AccountVerificationService(_rivaIdentityApiClientServiceMock.Object);
        }

        [Fact]
        public async Task VerifyAccountExistsAsync_Should_Return_VerificationResult_With_Success_True()
        {
            var accountId = Guid.NewGuid();
            const string email = "email@email.com";
            var account = new GetAccountResponse
            {
                Id = accountId,
                Email = email
            };
            var expectedResult = VerificationResult.Ok();

            _rivaIdentityApiClientServiceMock.Setup(x => x.GetAccountAsync(It.IsAny<Guid>()))
                .ReturnsAsync(account);

            var result = await _service.VerifyAccountExistsAsync(accountId, email);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task VerifyAccountExistsAsync_Should_Return_VerificationResult_With_Success_False_When_Account_Is_Not_Found()
        {
            var accountId = Guid.NewGuid();
            const string email = "email@email.com";
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.NotFound, AccountErrorMessage.NotFound)
            };
            var expectedResult = VerificationResult.Fail(errors);

            _rivaIdentityApiClientServiceMock.Setup(x => x.GetAccountAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult<IAccount>(null));

            var result = await _service.VerifyAccountExistsAsync(accountId, email);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task VerifyAccountExistsAsync_Should_Return_VerificationResult_With_Success_False_When_Email_Do_Not_Match()
        {
            var accountId = Guid.NewGuid();
            const string email = "email@email.com";
            var account = new GetAccountResponse
            {
                Id = accountId,
                Email = "otherEmail@email.com"
            };
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.EmailMismatch, AccountErrorMessage.EmailMismatch)
            };
            var expectedResult = VerificationResult.Fail(errors);

            _rivaIdentityApiClientServiceMock.Setup(x => x.GetAccountAsync(It.IsAny<Guid>()))
                .ReturnsAsync(account);

            var result = await _service.VerifyAccountExistsAsync(accountId, email);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}