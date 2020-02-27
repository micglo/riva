using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Riva.BuildingBlocks.WebApiTest.IntegrationTests;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;
using Riva.Identity.Infrastructure.Services;
using Riva.Identity.Web.Api.Models.Requests.Accounts;
using Riva.Identity.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.Identity.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class ChangePasswordIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public ChangePasswordIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Change_Password_When_Requesting_By_Administrator_Client()
        {
            var factory = new AdministratorWebApplicationFactory("AdministratorShouldChangePasswordIntegrationTest");
            var client = factory.WithWebHostBuilder(builder => builder.ConfigureWebHostBuilderForIntegrationTest())
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
            const string oldPassword = "OldPassword";
            var accountEntity = await InsertAccountEntityAsync(factory.DbContext, oldPassword);
            var oldPasswordHash = accountEntity.PasswordHash;
            var changePasswordRequest = new ChangePasswordRequest
            {
                OldPassword = oldPassword,
                NewPassword = "NewPassword",
                ConfirmNewPassword = "NewPassword"
            };
            var changePasswordRequestString = JsonConvert.SerializeObject(changePasswordRequest);
            var requestContent = new StringContent(changePasswordRequestString, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Add("api-version", "1");

            var response = await client.PostAsync($"api/accounts/{accountEntity.Id}/passwords/changes", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
            await factory.DbContext.Entry(accountEntity).ReloadAsync();
            accountEntity.PasswordHash.Should().NotBe(oldPasswordHash);
        }

        [Fact]
        public async Task Should_Change_Password_When_Requesting_By_User_Client()
        {
            var factory = new UserWebApplicationFactory("UserShouldChangePasswordIntegrationTest");
            var client = factory.WithWebHostBuilder(builder => builder.ConfigureWebHostBuilderForIntegrationTest())
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
            const string oldPassword = "OldPassword";
            var accountEntity = await InsertAccountEntityAsync(factory.DbContext, oldPassword);
            var oldPasswordHash = accountEntity.PasswordHash;
            var changePasswordRequest = new ChangePasswordRequest
            {
                OldPassword = oldPassword,
                NewPassword = "NewPassword",
                ConfirmNewPassword = "NewPassword"
            };
            var changePasswordRequestString = JsonConvert.SerializeObject(changePasswordRequest);
            var requestContent = new StringContent(changePasswordRequestString, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Add("api-version", "1");

            var response = await client.PostAsync($"api/accounts/{accountEntity.Id}/passwords/changes", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
            await factory.DbContext.Entry(accountEntity).ReloadAsync();

            accountEntity.PasswordHash.Should().NotBe(oldPasswordHash);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            var changePasswordRequest = new ChangePasswordRequest
            {
                OldPassword = "OldPassword",
                NewPassword = "NewPassword",
                ConfirmNewPassword = "NewPassword"
            };
            var changePasswordRequestString = JsonConvert.SerializeObject(changePasswordRequest);
            var requestContent = new StringContent(changePasswordRequestString, Encoding.UTF8, "application/json");
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.PostAsync($"api/accounts/{Guid.NewGuid()}/passwords/changes", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<AccountEntity> InsertAccountEntityAsync(RivaIdentityDbContext context, string password)
        {
            var passwordService = new PasswordService();
            var accountEntity = new AccountEntity
            {
                Id = AuthUserOptions.UserId,
                Email = AuthUserOptions.Email,
                Confirmed = true,
                PasswordHash = passwordService.HashPassword(password),
                SecurityStamp = Guid.NewGuid(),
                Created = DateTimeOffset.UtcNow
            };

            context.Accounts.Add(accountEntity);
            await context.SaveChangesAsync();

            return accountEntity;
        }
    }
}