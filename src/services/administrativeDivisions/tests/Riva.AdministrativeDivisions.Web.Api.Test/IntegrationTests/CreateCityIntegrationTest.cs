using System;
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
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;
using Riva.AdministrativeDivisions.Web.Api.Models.Requests;
using Riva.AdministrativeDivisions.Web.Api.Models.Responses;
using Riva.AdministrativeDivisions.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.AdministrativeDivisions.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class CreateCityIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public CreateCityIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Create_City_When_Requesting_By_Administrator_Client()
        {
            var state = await InsertStateEntityAsync(_fixture.AdministratorDbContext);
            var createCityRequest = new CreateCityRequest
            {
                Name = "CreateCityIntegrationTest", 
                PolishName = "CreateCityIntegrationTest",
                StateId = state.Id
            };
            var createCityRequestString = JsonConvert.SerializeObject(createCityRequest);
            var requestContent = new StringContent(createCityRequestString, Encoding.UTF8, "application/json");
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AdministratorHttpClient.PostAsync("api/cities", requestContent);
            var responseContentString = await response.Content.ReadAsStringAsync();
            var expectedResponse = await PrepareExpectedResponseAsync(_fixture.AdministratorDbContext, createCityRequest.Name);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Created);
            responseContentString.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Should_Return_Forbidden_Status_Code_When_Requesting_By_User_Client()
        {
            var createCityRequest = new CreateCityRequest
            {
                Name = "CreateCityIntegrationTest",
                PolishName = "CreateCityIntegrationTest",
                StateId = Guid.NewGuid()
            };
            var createCityRequestString = JsonConvert.SerializeObject(createCityRequest);
            var requestContent = new StringContent(createCityRequestString, Encoding.UTF8, "application/json");
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.UserHttpClient.PostAsync("api/cities", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            var createCityRequest = new CreateCityRequest
            {
                Name = "CreateCityIntegrationTest",
                PolishName = "CreateCityIntegrationTest",
                StateId = Guid.NewGuid()
            };
            var createCityRequestString = JsonConvert.SerializeObject(createCityRequest);
            var requestContent = new StringContent(createCityRequestString, Encoding.UTF8, "application/json");
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.PostAsync("api/cities", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<StateEntity> InsertStateEntityAsync(RivaAdministrativeDivisionsDbContext context)
        {
            var stateEntity = new StateEntity
            {
                Id = Guid.NewGuid(),
                Name = "CreateCityIntegrationTest",
                PolishName = "CreateCityIntegrationTest",
                RowVersion = new byte[] {0, 0, 0, 0, 0, 0, 70, 81}
            };
            context.States.Add(stateEntity);
            await context.SaveChangesAsync();
            return stateEntity;
        }

        private static async Task<string> PrepareExpectedResponseAsync(RivaAdministrativeDivisionsDbContext context, string cityName)
        {
            var cityEntity = await context.Cities.SingleOrDefaultAsync(x => x.Name.Equals(cityName));
            var cityResponse = new CityResponse(cityEntity.Id, cityEntity.RowVersion, cityEntity.Name,
                cityEntity.PolishName, cityEntity.StateId);
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultTestPlatformContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };
            return JsonConvert.SerializeObject(cityResponse, settings);
        }
    }
}