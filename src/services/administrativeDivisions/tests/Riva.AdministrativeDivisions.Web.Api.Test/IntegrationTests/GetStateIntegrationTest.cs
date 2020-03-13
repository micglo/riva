using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Contexts;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;
using Riva.AdministrativeDivisions.Web.Api.Models.Responses;
using Riva.AdministrativeDivisions.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.AdministrativeDivisions.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class GetStateIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public GetStateIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Get_State_When_Requesting_By_User_Client()
        {
            var stateEntity = await InsertStateEntityAsync(_fixture.UserDbContext);
            var expectedResponse = PrepareExpectedResponse(stateEntity);
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.UserHttpClient.GetAsync($"api/states/{stateEntity.Id}");
            var responseContentString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseContentString.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.GetAsync($"api/states/{Guid.NewGuid()}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<StateEntity> InsertStateEntityAsync(RivaAdministrativeDivisionsDbContext context)
        {
            var stateEntity = new StateEntity
            {
                Id = Guid.NewGuid(),
                Name = "GetStateIntegrationTest",
                PolishName = "GetStateIntegrationTest",
                RowVersion = new byte[] { 1, 2, 4, 8, 16, 32, 64 }
            };

            context.States.Add(stateEntity);
            await context.SaveChangesAsync();

            return stateEntity;
        }

        private static string PrepareExpectedResponse(StateEntity stateEntity)
        {
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