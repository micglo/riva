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
    public class RequestPasswordResetTokenIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public RequestPasswordResetTokenIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Request_Password_Reset_Token_When_Requesting_By_Anonymous_Client()
        {
            var accountEntity = await InsertAccountEntityAsync(_fixture.AnonymousDbContext);
            var requestPasswordResetTokenRequest = new RequestPasswordResetTokenRequest { Email = accountEntity.Email };
            var requestPasswordResetTokenRequestString = JsonConvert.SerializeObject(requestPasswordResetTokenRequest);
            var requestContent = new StringContent(requestPasswordResetTokenRequestString, Encoding.UTF8, "application/json");
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.PostAsync("api/accounts/passwords/tokens", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }

        private static async Task<AccountEntity> InsertAccountEntityAsync(RivaIdentityDbContext context)
        {
            var accountEntity = new AccountEntity
            {
                Id = Guid.NewGuid(),
                Email = "RequestPasswordResetTokenIntegrationTest@email.com",
                Confirmed = true,
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