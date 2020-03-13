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
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Contexts;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;
using Riva.AdministrativeDivisions.Web.Api.Models.Requests;
using Riva.AdministrativeDivisions.Web.Api.Models.Responses;
using Riva.AdministrativeDivisions.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.AdministrativeDivisions.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class UpdateStateIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public UpdateStateIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Update_State_When_Requesting_By_Administrator_Client()
        {
            var stateEntity = await InsertStateEntityAsync(_fixture.AdministratorDbContext);
            var updateStateRequest = new UpdateStateRequest
            {
                Id = stateEntity.Id,
                Name = "UpdateStateIntegrationTestNewName",
                PolishName = "UpdateStateIntegrationTestNewPolishName"
            };
            var updateStateRequestString = JsonConvert.SerializeObject(updateStateRequest);
            var requestContent = new StringContent(updateStateRequestString, Encoding.UTF8, "application/json");
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add("api-version", "1");
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add(HeaderNames.IfMatch, $"\"{Convert.ToBase64String(stateEntity.RowVersion)}\"");

            var response = await _fixture.AdministratorHttpClient.PutAsync($"api/states/{stateEntity.Id}", requestContent);
            var responseContentString = await response.Content.ReadAsStringAsync();
            var expectedResponse = await PrepareExpectedResponseAsync(_fixture.AdministratorDbContext, stateEntity.Id);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseContentString.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Should_Return_Forbidden_Status_Code_When_Requesting_By_User_Client()
        {
            var updateStateRequest = new UpdateStateRequest
            {
                Id = Guid.NewGuid(),
                Name = "UpdateStateIntegrationTestNewName",
                PolishName = "UpdateStateIntegrationTestNewPolishName"
            };
            var updateStateRequestString = JsonConvert.SerializeObject(updateStateRequest);
            var requestContent = new StringContent(updateStateRequestString, Encoding.UTF8, "application/json");
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.UserHttpClient.PutAsync($"api/states/{updateStateRequest.Id}", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            var updateStateRequest = new UpdateStateRequest
            {
                Id = Guid.NewGuid(),
                Name = "UpdateStateIntegrationTestNewName",
                PolishName = "UpdateStateIntegrationTestNewPolishName"
            };
            var updateStateRequestString = JsonConvert.SerializeObject(updateStateRequest);
            var requestContent = new StringContent(updateStateRequestString, Encoding.UTF8, "application/json");
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.PutAsync($"api/states/{updateStateRequest.Id}", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<StateEntity> InsertStateEntityAsync(RivaAdministrativeDivisionsDbContext context)
        {
            var stateEntity = new StateEntity
            {
                Id = Guid.NewGuid(),
                Name = "UpdateStateIntegrationTest",
                PolishName = "UpdateStateIntegrationTest",
                RowVersion = new byte[] { 0, 0, 0, 0, 0, 0, 70, 81 },
                Cities = Array.Empty<CityEntity>()
            };

            context.States.Add(stateEntity);
            await context.SaveChangesAsync();

            return stateEntity;
        }

        private static async Task<string> PrepareExpectedResponseAsync(RivaAdministrativeDivisionsDbContext context, Guid stateId)
        {
            var stateEntity = await context.States.FindAsync(stateId);
            await context.Entry(stateEntity).ReloadAsync();
            var stateResponse = new StateResponse(stateEntity.Id, stateEntity.RowVersion, stateEntity.Name,
                stateEntity.PolishName);
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