using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Riva.BuildingBlocks.Domain;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;
using Riva.Identity.Web.Api.Models.Requests.Accounts;
using Riva.Identity.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.Identity.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class CreateAccountIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public CreateAccountIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Create_Account_When_Requesting_By_Anonymous_Client()
        {
            await InsertUserRoleEntityIfNotExistsAsync(_fixture.AnonymousDbContext);
            var createAccountRequest = new CreateAccountRequest
            {
                Email = "createAccountIntegrationTest@email.com",
                Password = "Password1234",
                ConfirmPassword = "Password1234"
            };
            var createAccountRequestString = JsonConvert.SerializeObject(createAccountRequest);
            var requestContent = new StringContent(createAccountRequestString, Encoding.UTF8, "application/json");
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.PostAsync("api/accounts", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Accepted);
        }

        private static async Task InsertUserRoleEntityIfNotExistsAsync(RivaIdentityDbContext context)
        {
            var roleEntity = await context.Roles.SingleOrDefaultAsync(x => x.Name.Equals(DefaultRoleEnumeration.User.DisplayName));
            if (roleEntity is null)
            {
                roleEntity = new RoleEntity
                {
                    Id = Guid.NewGuid(),
                    Name = DefaultRoleEnumeration.User.DisplayName,
                    RowVersion = new byte[] { 1, 2, 4, 8, 16, 32, 64 }
                };
                context.Roles.Add(roleEntity);
                await context.SaveChangesAsync();
            }
        }
    }
}