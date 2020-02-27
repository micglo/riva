using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;
using Riva.Identity.Web.Api.Models.Requests.Accounts;
using Riva.Identity.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.Identity.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class RequestAccountConfirmationTokenIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public RequestAccountConfirmationTokenIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Request_Account_Confirmation_Token_When_Requesting_By_Anonymous_Client()
        {
            var accountEntity = await InsertAccountEntityAsync(_fixture.AnonymousDbContext);
            var requestAccountConfirmationTokenRequest = new RequestAccountConfirmationTokenRequest { Email = accountEntity.Email };
            var requestAccountConfirmationTokenRequestString = JsonConvert.SerializeObject(requestAccountConfirmationTokenRequest);
            var requestContent = new StringContent(requestAccountConfirmationTokenRequestString, Encoding.UTF8, "application/json");
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.PostAsync("api/accounts/confirmations/tokens", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }

        private static async Task<AccountEntity> InsertAccountEntityAsync(RivaIdentityDbContext context)
        {
            var accountEntity = new AccountEntity
            {
                Id = Guid.NewGuid(),
                Email = "RequestAccountConfirmationTokenIntegrationTest@email.com",
                Confirmed = false,
                PasswordHash = "PasswordHash",
                SecurityStamp = Guid.NewGuid(),
                Created = DateTimeOffset.UtcNow
            };

            context.Accounts.Add(accountEntity);
            await context.SaveChangesAsync();

            return accountEntity;
        }
    }
}