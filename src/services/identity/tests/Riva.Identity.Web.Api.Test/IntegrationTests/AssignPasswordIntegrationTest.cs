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
using Riva.Identity.Web.Api.Models.Requests.Accounts;
using Riva.Identity.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.Identity.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class AssignPasswordIntegrationTest 
    {
        private readonly IntegrationTestFixture _fixture;

        public AssignPasswordIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Assign_Password_When_Requesting_By_Administrator_Client()
        {
            var factory = new AdministratorWebApplicationFactory("AdministratorShouldSetPasswordIntegrationTest");
            var client = factory.WithWebHostBuilder(builder => builder.ConfigureWebHostBuilderForIntegrationTest())
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
            var accountEntity = await InsertAccountEntityAsync(factory.DbContext);
            var assignPasswordRequest = new AssignPasswordRequest
            {
                Password = "Password1234",
                ConfirmPassword = "Password1234"
            };
            var assignPasswordRequestString = JsonConvert.SerializeObject(assignPasswordRequest);
            var requestContent = new StringContent(assignPasswordRequestString, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Add("api-version", "1");

            var response = await client.PostAsync($"api/accounts/{accountEntity.Id}/passwords/assignments", requestContent);

            await factory.DbContext.Entry(accountEntity).ReloadAsync();
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
            accountEntity.PasswordHash.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Should_Assign_Password_When_Requesting_By_User_Client()
        {
            var factory = new UserWebApplicationFactory("UserShouldSetPasswordIntegrationTest");
            var client = factory.WithWebHostBuilder(builder => builder.ConfigureWebHostBuilderForIntegrationTest())
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
            var accountEntity = await InsertAccountEntityAsync(factory.DbContext);
            var assignPasswordRequest = new AssignPasswordRequest
            {
                Password = "Password1234",
                ConfirmPassword = "Password1234"
            };
            var setPasswordRequestString = JsonConvert.SerializeObject(assignPasswordRequest);
            var requestContent = new StringContent(setPasswordRequestString, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Add("api-version", "1");

            var response = await client.PostAsync($"api/accounts/{accountEntity.Id}/passwords/assignments", requestContent);
            await factory.DbContext.Entry(accountEntity).ReloadAsync();

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
            accountEntity.PasswordHash.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            var assignPasswordRequest = new AssignPasswordRequest
            {
                Password = "Password1234",
                ConfirmPassword = "Password1234"
            };
            var setPasswordRequestString = JsonConvert.SerializeObject(assignPasswordRequest);
            var requestContent = new StringContent(setPasswordRequestString, Encoding.UTF8, "application/json");
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.PostAsync($"api/accounts/{Guid.NewGuid()}/passwords/assignments", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<AccountEntity> InsertAccountEntityAsync(RivaIdentityDbContext context)
        {
            var accountEntity = new AccountEntity
            {
                Id = AuthUserOptions.UserId,
                Email = AuthUserOptions.Email,
                Confirmed = true,
                SecurityStamp = Guid.NewGuid(),
                Created = DateTimeOffset.UtcNow
            };

            context.Accounts.Add(accountEntity);
            await context.SaveChangesAsync();

            return accountEntity;
        }
    }
}