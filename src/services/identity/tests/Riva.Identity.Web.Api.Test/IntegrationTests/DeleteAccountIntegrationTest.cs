using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Riva.BuildingBlocks.WebApiTest.IntegrationTests;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;
using Riva.Identity.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.Identity.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class DeleteAccountIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public DeleteAccountIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Delete_Account_When_Requesting_By_Administrator_Client()
        {
            var factory = new AdministratorWebApplicationFactory("AdministratorShouldDeleteAccountIntegrationTest");
            var client = factory.WithWebHostBuilder(builder => builder.ConfigureWebHostBuilderForIntegrationTest())
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
            var accountEntity = await InsertAccountEntityAsync(factory.DbContext);
            client.DefaultRequestHeaders.Add("api-version", "1");

            var response = await client.DeleteAsync($"api/accounts/{accountEntity.Id}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Accepted);
        }

        [Fact]
        public async Task Should_Delete_Account_When_Requesting_By_User_Client()
        {
            var factory = new UserWebApplicationFactory("UserShouldDeleteAccountIntegrationTest");
            var client = factory.WithWebHostBuilder(builder => builder.ConfigureWebHostBuilderForIntegrationTest())
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
            var accountEntity = await InsertAccountEntityAsync(factory.DbContext);
            client.DefaultRequestHeaders.Add("api-version", "1");

            var response = await client.DeleteAsync($"api/accounts/{accountEntity.Id}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Accepted);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.DeleteAsync($"api/accounts/{Guid.NewGuid()}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<AccountEntity> InsertAccountEntityAsync(RivaIdentityDbContext context)
        {
            var accountEntity = new AccountEntity
            {
                Id = AuthUserOptions.UserId,
                Email = AuthUserOptions.Email,
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