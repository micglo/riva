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
    public class ResetPasswordIntegrationTest 
    {
        private readonly IntegrationTestFixture _fixture;

        public ResetPasswordIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Reset_Password_When_Requesting_By_Anonymous_Client()
        {
            const string token = "12345";
            var accountEntity = await InsertAccountEntityAsync(_fixture.AnonymousDbContext, token);
            var oldPasswordHash = accountEntity.PasswordHash;
            var resetPasswordRequest = new ResetPasswordRequest
            {
                Code = token,
                Email = accountEntity.Email,
                Password = "NewPassword1234",
                ConfirmPassword = "NewPassword1234"
            };
            var resetPasswordRequestString = JsonConvert.SerializeObject(resetPasswordRequest);
            var requestContent = new StringContent(resetPasswordRequestString, Encoding.UTF8, "application/json");
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.PostAsync("api/accounts/passwords/resets", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
            await _fixture.AnonymousDbContext.Entry(accountEntity).ReloadAsync();
            accountEntity.PasswordHash.Should().NotBe(oldPasswordHash);
        }

        private static async Task<AccountEntity> InsertAccountEntityAsync(RivaIdentityDbContext context, string token)
        {
            var accountEntity = new AccountEntity
            {
                Id = Guid.NewGuid(),
                Email = "ResetPasswordIntegrationTest@email.com",
                Confirmed = true,
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
                        Type = TokenTypeEnumeration.PasswordReset.ConvertToEnum()
                    }
                }
            };

            context.Accounts.Add(accountEntity);
            await context.SaveChangesAsync();

            return accountEntity;
        }
    }
}