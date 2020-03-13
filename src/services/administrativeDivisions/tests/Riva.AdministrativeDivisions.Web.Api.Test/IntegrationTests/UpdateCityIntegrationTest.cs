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
    public class UpdateCityIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public UpdateCityIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Update_City_When_Requesting_By_Administrator_Client()
        {
            var cityEntity = await InsertCityEntityAsync(_fixture.AdministratorDbContext);
            var updateCityRequest = new UpdateCityRequest
            {
                Id = cityEntity.Id,
                Name = "UpdateCityIntegrationTestNewName",
                PolishName = "UpdateCityIntegrationTestNewPolishName",
                StateId = cityEntity.StateId
            };
            var updateCityRequestString = JsonConvert.SerializeObject(updateCityRequest);
            var requestContent = new StringContent(updateCityRequestString, Encoding.UTF8, "application/json");
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add("api-version", "1");
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add(HeaderNames.IfMatch, $"\"{Convert.ToBase64String(cityEntity.RowVersion)}\"");

            var response = await _fixture.AdministratorHttpClient.PutAsync($"api/cities/{cityEntity.Id}", requestContent);
            var responseContentString = await response.Content.ReadAsStringAsync();
            var expectedResponse = await PrepareExpectedResponseAsync(_fixture.AdministratorDbContext, cityEntity.Id);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseContentString.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Should_Return_Forbidden_Status_Code_When_Requesting_By_User_Client()
        {
            var updateCityRequest = new UpdateCityRequest
            {
                Id = Guid.NewGuid(),
                Name = "UpdateCityIntegrationTestNewName",
                PolishName = "UpdateCityIntegrationTestNewPolishName",
                StateId = Guid.NewGuid()
            };
            var updateCityRequestString = JsonConvert.SerializeObject(updateCityRequest);
            var requestContent = new StringContent(updateCityRequestString, Encoding.UTF8, "application/json");
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.UserHttpClient.PutAsync($"api/cities/{updateCityRequest.Id}", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            var updateCityRequest = new UpdateCityRequest
            {
                Id = Guid.NewGuid(),
                Name = "UpdateCityIntegrationTestNewName",
                PolishName = "UpdateCityIntegrationTestNewPolishName",
                StateId = Guid.NewGuid()
            };
            var updateCityRequestString = JsonConvert.SerializeObject(updateCityRequest);
            var requestContent = new StringContent(updateCityRequestString, Encoding.UTF8, "application/json");
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.PutAsync($"api/cities/{updateCityRequest.Id}", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<CityEntity> InsertCityEntityAsync(RivaAdministrativeDivisionsDbContext context)
        {
            var cityEntity = new CityEntity
            {
                Id = Guid.NewGuid(),
                Name = "UpdateCityIntegrationTest",
                PolishName = "UpdateCityIntegrationTest",
                RowVersion = new byte[] { 0, 0, 0, 0, 0, 0, 70, 81 },
                StateId = Guid.NewGuid()
            };
            context.Cities.Add(cityEntity);
            await context.SaveChangesAsync();
            return cityEntity;
        }

        private static async Task<string> PrepareExpectedResponseAsync(RivaAdministrativeDivisionsDbContext context, Guid cityId)
        {
            var cityEntity = await context.Cities.FindAsync(cityId);
            await context.Entry(cityEntity).ReloadAsync();
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