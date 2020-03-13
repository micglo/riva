using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Contexts;
using Riva.AdministrativeDivisions.Web.Api.Models.Requests;
using Riva.AdministrativeDivisions.Web.Api.Models.Responses;
using Riva.AdministrativeDivisions.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.AdministrativeDivisions.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class CreateStateIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public CreateStateIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Create_State_When_Requesting_By_Administrator_Client()
        {
            var createStateRequest = new CreateStateRequest { Name = "CreateStateIntegrationTest", PolishName = "CreateStateIntegrationTest" };
            var createStateRequestString = JsonConvert.SerializeObject(createStateRequest);
            var requestContent = new StringContent(createStateRequestString, Encoding.UTF8, "application/json");
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AdministratorHttpClient.PostAsync("api/states", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Created);
            var responseContentString = await response.Content.ReadAsStringAsync();
            var expectedResponse = await PrepareExpectedResponseAsync(_fixture.AdministratorDbContext, createStateRequest.Name);

            responseContentString.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Should_Return_Forbidden_Status_Code_When_Requesting_By_User_Client()
        {
            var createStateRequest = new CreateStateRequest { Name = "CreateStateIntegrationTest", PolishName = "CreateStateIntegrationTest" };
            var createStateRequestString = JsonConvert.SerializeObject(createStateRequest);
            var requestContent = new StringContent(createStateRequestString, Encoding.UTF8, "application/json");
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.UserHttpClient.PostAsync("api/states", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            var createStateRequest = new CreateStateRequest { Name = "CreateStateIntegrationTest", PolishName = "CreateStateIntegrationTest" };
            var createStateRequestString = JsonConvert.SerializeObject(createStateRequest);
            var requestContent = new StringContent(createStateRequestString, Encoding.UTF8, "application/json");
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.PostAsync("api/states", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<string> PrepareExpectedResponseAsync(RivaAdministrativeDivisionsDbContext context, string stateName)
        {
            var stateEntity = await context.States.SingleOrDefaultAsync(x => x.Name.Equals(stateName));
            var stateResponse = new StateResponse(stateEntity.Id, stateEntity.RowVersion, stateEntity.Name, stateEntity.PolishName);
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultTestPlatformContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };
            return JsonConvert.SerializeObject(stateResponse, settings);
        }
    }
}