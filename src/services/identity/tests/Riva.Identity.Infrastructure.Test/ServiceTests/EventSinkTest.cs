using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Moq;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Models;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Repositories;
using Riva.Identity.Infrastructure.Services;
using Xunit;

namespace Riva.Identity.Infrastructure.Test.ServiceTests
{
    public class EventSinkTest
    {
        private readonly Mock<IAccountGetterService> _accountGetterServiceMock;
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<ICommunicationBus> _communicationBusMock;
        private readonly IEventSink _service;

        public EventSinkTest()
        {
            _accountGetterServiceMock = new Mock<IAccountGetterService>();
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _communicationBusMock = new Mock<ICommunicationBus>();
            _service = new IdentityServerEventSink(_accountGetterServiceMock.Object, _accountRepositoryMock.Object,
                _communicationBusMock.Object);
        }

        [Fact]
        public async Task PersistAsync_Should_Update_Account_Login_On_TokenIssuedSuccessEvent()
        {
            var account = Account.Builder()
                .SetId(Guid.NewGuid())
                .SetEmail("email@email.com")
                .SetConfirmed(true)
                .SetPasswordHash(string.Empty)
                .SetSecurityStamp(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .Build();
            var getAccountResult = GetResult<Account>.Ok(account);
            var claims = new[] {
                new Claim(JwtClaimTypes.Subject, account.Id.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims);
            var authorizeResponse = new AuthorizeResponse
            {
                Request = new ValidatedAuthorizeRequest
                {
                    Subject = new ClaimsPrincipal(claimsIdentity),
                    ClientId = "clientId",
                    Client = new Client
                    {
                        ClientName = "ClientName"
                    },
                    RedirectUri = "RedirectUri",
                    GrantType = "GrantType"
                }
            };
            var evt = new TokenIssuedSuccessEvent(authorizeResponse);

            _accountGetterServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(getAccountResult);
            _communicationBusMock
                .Setup(x => x.DispatchDomainEventsAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
            _accountRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Account>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            Func<Task> result = async () => await _service.PersistAsync(evt);

            await result.Should().NotThrowAsync<Exception>();
            _communicationBusMock.Verify(
                x => x.DispatchDomainEventsAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()), Times.Exactly(1));
            _accountRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Account>(a => a.LastLogin.HasValue)));
        }
    }
}