using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Riva.BuildingBlocks.Core.Communications;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Roles.Aggregates;
using Riva.Identity.Domain.Roles.Repositories;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Repositories;
using Riva.Identity.Infrastructure.Services;
using Riva.BuildingBlocks.Domain;
using Riva.Identity.Domain.Accounts.Enumerations;
using Riva.Identity.Domain.Accounts.Events;
using Xunit;

namespace Riva.Identity.Infrastructure.Test.ServiceTests
{
    public class AccountCreatorServiceTest
    {
        private readonly Mock<IPasswordService> _passwordServiceMock;
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<ICommunicationBus> _communicationBusMock;
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly IAccountCreatorService _accountCreatorService;

        public AccountCreatorServiceTest()
        {
            _passwordServiceMock = new Mock<IPasswordService>();
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _communicationBusMock = new Mock<ICommunicationBus>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _accountCreatorService = new AccountCreatorService(_passwordServiceMock.Object,
                _roleRepositoryMock.Object, _communicationBusMock.Object, _accountRepositoryMock.Object);
        }

        [Theory]
        [InlineData("password1234", "60d13790-e547-48da-8aa4-5d22dad20e19")]
        [InlineData(null, "60d13790-e547-48da-8aa4-5d22dad20e19")]
        [InlineData("   ", "60d13790-e547-48da-8aa4-5d22dad20e19")]
        [InlineData("", "60d13790-e547-48da-8aa4-5d22dad20e19")]
        public async Task CreateAsync_Should_Create_Account(string password, string correlationIdString)
        {
            var correlationId = new Guid(correlationIdString);
            var role = new Role(Guid.NewGuid(), Array.Empty<byte>(), DefaultRoleEnumeration.User.DisplayName);
            const string hashedPassword = "hashedPassword";
            var accountId = Guid.NewGuid();
            const string email = "email@email.com";

            if (!string.IsNullOrWhiteSpace(password))
                _passwordServiceMock.Setup(x => x.HashPassword(It.IsAny<string>())).Returns(hashedPassword);
            _roleRepositoryMock.Setup(x => x.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(role);
            _communicationBusMock.Setup(x => x.DispatchDomainEventsAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _accountRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Account>())).Returns(Task.CompletedTask);


            var result = await _accountCreatorService.CreateAsync(accountId, email, password, correlationId);

            result.Id.Should().Be(accountId);
            result.Email.Should().Be(email);
            result.Roles.Should().BeEquivalentTo(new List<Guid> {role.Id});
            result.DomainEvents.Should().Contain(x => x.GetType() == typeof(AccountCreatedDomainEvent));
            result.DomainEvents.Should().Contain(x => x.GetType() == typeof(AccountRoleAddedDomainEvent));

            if (!string.IsNullOrWhiteSpace(password))
            {
                result.PasswordHash.Should().Be(hashedPassword);
                result.Confirmed.Should().BeFalse();
                result.LastLogin.Should().BeNull();
                result.Tokens.Should().Contain(x => x.Type.Equals(TokenTypeEnumeration.AccountConfirmation));
                result.DomainEvents.Should().Contain(x => x.GetType() == typeof(AccountTokenGeneratedDomainEvent));
            }
            else
            {
                result.PasswordHash.Should().BeNullOrWhiteSpace();
                result.Confirmed.Should().BeTrue();
                result.LastLogin.Should().BeNull();
            }
        }
    }
}