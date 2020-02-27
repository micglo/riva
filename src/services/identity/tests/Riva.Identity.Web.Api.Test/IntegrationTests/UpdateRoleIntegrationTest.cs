using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;
using Riva.Identity.Web.Api.Models.Requests.Roles;
using Riva.Identity.Web.Api.Models.Responses.Roles;
using Riva.Identity.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.Identity.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class UpdateRoleIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public UpdateRoleIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Update_Role_When_Requesting_By_Administrator_Client()
        {
            var roleEntity = await InsertRoleEntityAsync(_fixture.AdministratorDbContext);
            var updateRoleRequest = new UpdateRoleRequest
            {
                Id = roleEntity.Id,
                Name = "UpdateRoleIntegrationTestNewName"
            };
            var updateRoleRequestString = JsonConvert.SerializeObject(updateRoleRequest);
            var requestContent = new StringContent(updateRoleRequestString, Encoding.UTF8, "application/json");
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add("api-version", "1");
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add(HeaderNames.IfMatch, $"\"{Convert.ToBase64String(roleEntity.RowVersion)}\"");

            var response = await _fixture.AdministratorHttpClient.PutAsync($"api/roles/{roleEntity.Id}", requestContent);
            var responseContentString = await response.Content.ReadAsStringAsync();
            var expectedResponse = await PrepareExpectedResponseAsync(_fixture.AdministratorDbContext, roleEntity.Id);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseContentString.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Should_Return_Forbidden_Status_Code_When_Requesting_By_User_Client()
        {
            var updateRoleRequest = new UpdateRoleRequest
            {
                Id = Guid.NewGuid(),
                Name = "UpdateRoleIntegrationTestNewName"
            };
            var updateRoleRequestString = JsonConvert.SerializeObject(updateRoleRequest);
            var requestContent = new StringContent(updateRoleRequestString, Encoding.UTF8, "application/json");
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.UserHttpClient.PutAsync($"api/roles/{updateRoleRequest.Id}", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_Fo_When_Requesting_By_Anonymous_Client()
        {
            var updateRoleRequest = new UpdateRoleRequest
            {
                Id = Guid.NewGuid(),
                Name = "UpdateRoleIntegrationTestNewName"
            };
            var updateRoleRequestString = JsonConvert.SerializeObject(updateRoleRequest);
            var requestContent = new StringContent(updateRoleRequestString, Encoding.UTF8, "application/json");
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.PutAsync($"api/roles/{updateRoleRequest.Id}", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<RoleEntity> InsertRoleEntityAsync(RivaIdentityDbContext context)
        {
            var roleEntity = new RoleEntity
            {
                Id = Guid.NewGuid(),
                Name = "UpdateRoleIntegrationTest",
                RowVersion = new byte[] { 0, 0, 0, 0, 0, 0, 70, 81 }
            };

            context.Roles.Add(roleEntity);
            await context.SaveChangesAsync();

            return roleEntity;
        }

        private static async Task<string> PrepareExpectedResponseAsync(RivaIdentityDbContext context, Guid roleId)
        {
            var roleEntity = await context.Roles.FindAsync(roleId);
            await context.Entry(roleEntity).ReloadAsync();
            var getRoleResponse = new GetRoleResponse(roleEntity.Id, roleEntity.RowVersion, roleEntity.Name);
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultTestPlatformContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };
            return JsonConvert.SerializeObject(getRoleResponse, settings);
        }
    }
}