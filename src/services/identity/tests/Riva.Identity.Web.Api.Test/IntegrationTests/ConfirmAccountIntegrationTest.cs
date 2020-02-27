using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Riva.Identity.Domain.Accounts.Enumerations;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Extensions;
using Riva.Identity.Web.Api.Models.Requests.Accounts;
using Riva.Identity.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.Identity.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class ConfirmAccountIntegrationTest 
    {
        private readonly IntegrationTestFixture _fixture;

        public ConfirmAccountIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Confirm_Account_When_Requesting_By_Anonymous_Client()
        {
            const string token = "12345";
            var accountEntity = await InsertAccountEntityAsync(_fixture.AnonymousDbContext, token);
            var confirmAccountRequest = new ConfirmAccountRequest
            {
                Code = token,
                Email = accountEntity.Email
            };
            var confirmAccountRequestString = JsonConvert.SerializeObject(confirmAccountRequest);
            var requestContent = new StringContent(confirmAccountRequestString, Encoding.UTF8, "application/json");
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.PostAsync("api/accounts/confirmations", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
            await _fixture.AnonymousDbContext.Entry(accountEntity).ReloadAsync();
            accountEntity.Confirmed.Should().BeTrue();
        }

        private static async Task<AccountEntity> InsertAccountEntityAsync(RivaIdentityDbContext context, string token)
        {
            var accountEntity = new AccountEntity
            {
                Id = Guid.NewGuid(),
                Email = "confirmAccountIntegrationTest@email.com",
                Confirmed = false,
                PasswordHash = "PasswordHash",
                SecurityStamp = Guid.NewGuid(),
                Created = DateTimeOffset.UtcNow,
                Tokens = new List<TokenEntity>
                {
                    new TokenEntity
                    {
                        Value = token,
                        Id = Guid.NewGuid(),
                        Issued = DateTimeOffset.UtcNow.AddHours(-1),
                        Expires = DateTimeOffset.UtcNow.AddDays(1),
                        Type = TokenTypeEnumeration.AccountConfirmation.ConvertToEnum()
                    }
                }
            };

            context.Accounts.Add(accountEntity);
            await context.SaveChangesAsync();

            return accountEntity;
        }
    }
}