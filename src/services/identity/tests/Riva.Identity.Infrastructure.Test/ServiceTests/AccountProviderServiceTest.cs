using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Communications;
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
    public class AccountProviderServiceTest
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<IAccountGetterService> _accountGetterServiceMock;
        private readonly Mock<IAccountCreatorService> _accountCreatorServiceMock;
        private readonly Mock<ICommunicationBus> _communicationBusMock;
        private readonly IAccountProviderService _accountProviderService;

        public AccountProviderServiceTest()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _accountGetterServiceMock = new Mock<IAccountGetterService>();
            _accountCreatorServiceMock = new Mock<IAccountCreatorService>();
            _communicationBusMock = new Mock<ICommunicationBus>();
            _accountProviderService = new AccountProviderService(_accountRepositoryMock.Object, _accountGetterServiceMock.Object,
                _accountCreatorServiceMock.Object, _communicationBusMock.Object);
        }

        [Fact]
        public async Task ProvideAccountForExternalLoginAsync_Should_Return_Created_Account()
        {
            var errors = new Collection<IError>
            {
                new Error(AccountErrorCodeEnumeration.NotFound, AccountErrorMessage.NotFound)
            };
            var getAccountResult = GetResult<Account>.Fail(errors);
            const string email = "email@email.com";
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail(email)
                .SetConfirmed(true)
                .SetPasswordHash(string.Empty)
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid>{ Guid.NewGuid() })
                .Build();

            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(getAccountResult);
            _accountCreatorServiceMock.Setup(x => x.CreateAsync(It.IsAny<Guid>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(account);

            var result = await _accountProviderService.ProvideAccountForExternalLoginAsync(email, Guid.NewGuid());

            result.Email.Should().Be(email);
            result.Confirmed.Should().BeTrue();
        }

        [Fact]
        public async Task ProvideAccountForExternalLoginAsync_Should_Return_Existing_Confirmed_Account()
        {
            const string email = "email@email.com";
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail(email)
                .SetConfirmed(false)
                .SetPasswordHash(string.Empty)
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetRoles(new List<Guid> { Guid.NewGuid() })
                .SetLastLogin(DateTimeOffset.UtcNow.AddDays(-1))
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);

            _accountGetterServiceMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(getAccountResult);
            _communicationBusMock.Setup(x => x.DispatchDomainEventsAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _accountRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Account>())).Returns(Task.CompletedTask);

            var result = await _accountProviderService.ProvideAccountForExternalLoginAsync(email, Guid.NewGuid());

            result.Email.Should().Be(email);
            result.Confirmed.Should().BeTrue();
        }
    }
}