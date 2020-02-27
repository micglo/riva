using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;
using Riva.Identity.Web.Api.Models.Responses.Roles;
using Riva.Identity.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.Identity.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class GetRoleIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public GetRoleIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Get_Role_When_Requesting_By_Administrator_Client()
        {
            var roleEntity = await InsertRoleEntityAsync(_fixture.AdministratorDbContext);
            var expectedResponse = PrepareExpectedResponse(roleEntity);
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AdministratorHttpClient.GetAsync($"api/roles/{roleEntity.Id}");
            var responseContentString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseContentString.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Should_Return_Forbidden_Status_Code_When_Requesting_By_User_Client()
        {
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.UserHttpClient.GetAsync($"api/roles/{Guid.NewGuid()}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.GetAsync($"api/roles/{Guid.NewGuid()}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<RoleEntity> InsertRoleEntityAsync(RivaIdentityDbContext context)
        {
            var roleEntity = new RoleEntity
            {
                Id = Guid.NewGuid(),
                Name = "GetRoleIntegrationTest",
                RowVersion = new byte[] { 1, 2, 4, 8, 16, 64 }
            };

            context.Roles.Add(roleEntity);
            await context.SaveChangesAsync();

            return roleEntity;
        }

        private static string PrepareExpectedResponse(RoleEntity roleEntity)
        {
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