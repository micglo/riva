using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Net.Http.Headers;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;
using Riva.Identity.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.Identity.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class DeleteRoleIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public DeleteRoleIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Delete_Role_When_Requesting_By_Administrator_Client()
        {
            var roleEntity = await InsertRoleEntityAsync(_fixture.AdministratorDbContext);
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add("api-version", "1");
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add(HeaderNames.IfMatch, $"\"{Convert.ToBase64String(roleEntity.RowVersion)}\"");

            var response = await _fixture.AdministratorHttpClient.DeleteAsync($"api/roles/{roleEntity.Id}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_Return_Forbidden_Status_Code_When_Requesting_By_User_Client()
        {
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.UserHttpClient.DeleteAsync($"api/roles/{Guid.NewGuid()}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.DeleteAsync($"api/roles/{Guid.NewGuid()}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<RoleEntity> InsertRoleEntityAsync(RivaIdentityDbContext context)
        {
            var roleEntity = new RoleEntity
            {
                Id = Guid.NewGuid(),
                Name = "DeleteRoleIntegrationTest",
                RowVersion = new byte[] { 0, 0, 0, 0, 0, 0, 70, 81 }
            };

            context.Roles.Add(roleEntity);
            await context.SaveChangesAsync();

            return roleEntity;
        }
    }
}