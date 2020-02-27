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
using Riva.Identity.Domain.Accounts.Repositories;
using Riva.Identity.Infrastructure.Services;
using Riva.BuildingBlocks.Core.Models;
using Xunit;

namespace Riva.Identity.Infrastructure.Test.ServiceTests
{
    public class AccountGetterServiceTest
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly IAccountGetterService _accountGetterService;

        public AccountGetterServiceTest()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _accountGetterService = new AccountGetterService(_accountRepositoryMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return__GetResult_With_Account()
        {
            var id = Guid.NewGuid();
            var account = Account.Builder()
                .SetId(id)
                .SetEmail("email@email.com")
                .SetConfirmed(true)
                .SetPasswordHash("PasswordHash")
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .Build();
            var expectedResult = GetResult<Account>.Ok(account);

            _accountRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(account);

            var result = await _accountGetterService.GetByIdAsync(id);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_GetResult_With_Errors_When_Account_Is_Not_Found()
        {
            var id = Guid.NewGuid();
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.NotFound, AccountErrorMessage.NotFound)
            };
            var expectedResult = GetResult<Account>.Fail(errors);

            _accountRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Account>(null));

            var result = await _accountGetterService.GetByIdAsync(id);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetByEmailAsync_Should_Return_GetResult_With_Account()
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
            var expectedResult = GetResult<Account>.Ok(account);

            _accountRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(account);

            var result = await _accountGetterService.GetByEmailAsync(email);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetByEmailAsync_Should_Return_GetResult_With_Errors_When_Account_Is_Not_Found()
        {
            const string email = "email@email.com";
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.NotFound, AccountErrorMessage.NotFound)
            };
            var expectedResult = GetResult<Account>.Fail(errors);

            _accountRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult<Account>(null));

            var result = await _accountGetterService.GetByEmailAsync(email);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}