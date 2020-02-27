using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;
using Riva.Identity.Web.Api.Models.Requests.Roles;
using Riva.Identity.Web.Api.Models.Responses.Roles;
using Riva.Identity.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.Identity.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class CreateRoleIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public CreateRoleIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Create_Role_When_Requesting_By_Administrator_Client()
        {
            var createRoleRequest = new CreateRoleRequest { Name = "CreateRoleIntegrationTest" };
            var createRoleRequestString = JsonConvert.SerializeObject(createRoleRequest);
            var requestContent = new StringContent(createRoleRequestString, Encoding.UTF8, "application/json");
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AdministratorHttpClient.PostAsync("api/roles", requestContent);
            var responseContentString = await response.Content.ReadAsStringAsync();
            var expectedResponse = await PrepareExpectedResponseAsync(_fixture.AdministratorDbContext, createRoleRequest.Name);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Created);
            responseContentString.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Should_Return_Forbidden_Status_Code_When_Requesting_By_User_Client()
        {
            var createRoleRequest = new CreateRoleRequest { Name = "CreateRoleIntegrationTest" };
            var createRoleRequestString = JsonConvert.SerializeObject(createRoleRequest);
            var requestContent = new StringContent(createRoleRequestString, Encoding.UTF8, "application/json");
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.UserHttpClient.PostAsync("api/roles", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            var createRoleRequest = new CreateRoleRequest { Name = "CreateRoleIntegrationTest" };
            var createRoleRequestString = JsonConvert.SerializeObject(createRoleRequest);
            var requestContent = new StringContent(createRoleRequestString, Encoding.UTF8, "application/json");
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.PostAsync("api/roles", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<string> PrepareExpectedResponseAsync(RivaIdentityDbContext context, string roleName)
        {
            var roleEntity = await context.Roles.SingleOrDefaultAsync(x => x.Name.Equals(roleName));
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