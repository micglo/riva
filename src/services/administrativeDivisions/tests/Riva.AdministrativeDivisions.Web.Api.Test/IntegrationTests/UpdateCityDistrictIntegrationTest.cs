using System;
using System.Linq;
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
    public class UpdateCityDistrictIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public UpdateCityDistrictIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Update_City_District_When_Requesting_By_Administrator_Client()
        {
            var cityDistrictEntity = await InsertCityDistrictEntityAsync(_fixture.AdministratorDbContext);
            var updateCityDistrictRequest = new UpdateCityDistrictRequest
            {
                Id = cityDistrictEntity.Id,
                Name = "UpdateCityDistrictIntegrationTestNewName",
                PolishName = "UpdateCityDistrictIntegrationTestNewPolishName",
                CityId = cityDistrictEntity.CityId
            };
            var updateCityDistrictRequestString = JsonConvert.SerializeObject(updateCityDistrictRequest);
            var requestContent = new StringContent(updateCityDistrictRequestString, Encoding.UTF8, "application/json");
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add("api-version", "1");
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add(HeaderNames.IfMatch, $"\"{Convert.ToBase64String(cityDistrictEntity.RowVersion)}\"");

            var response = await _fixture.AdministratorHttpClient.PutAsync($"api/cityDistricts/{cityDistrictEntity.Id}", requestContent);
            var responseContentString = await response.Content.ReadAsStringAsync();
            var expectedResponse = await PrepareExpectedResponseAsync(_fixture.AdministratorDbContext, cityDistrictEntity.Id);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseContentString.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Should_Return_Forbidden_Status_Code_When_Requesting_By_User_Client()
        {
            var updateCityRequest = new UpdateCityRequest
            {
                Id = Guid.NewGuid(),
                Name = "UpdateCityDistrictIntegrationTestNewName",
                PolishName = "UpdateCityDistrictIntegrationTestNewPolishName",
                StateId = Guid.NewGuid()
            };
            var updateCityRequestString = JsonConvert.SerializeObject(updateCityRequest);
            var requestContent = new StringContent(updateCityRequestString, Encoding.UTF8, "application/json");
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.UserHttpClient.PutAsync($"api/cityDistricts/{updateCityRequest.Id}", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            var updateCityRequest = new UpdateCityRequest
            {
                Id = Guid.NewGuid(),
                Name = "UpdateCityDistrictIntegrationTestNewName",
                PolishName = "UpdateCityDistrictIntegrationTestNewPolishName",
                StateId = Guid.NewGuid()
            };
            var updateCityRequestString = JsonConvert.SerializeObject(updateCityRequest);
            var requestContent = new StringContent(updateCityRequestString, Encoding.UTF8, "application/json");
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.PutAsync($"api/cityDistricts/{updateCityRequest.Id}", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<CityDistrictEntity> InsertCityDistrictEntityAsync(RivaAdministrativeDivisionsDbContext context)
        {
            var cityDistrictEntity = new CityDistrictEntity
            {
                Id = Guid.NewGuid(),
                Name = "UpdateCityDistrictIntegrationTest",
                PolishName = "UpdateCityDistrictIntegrationTest",
                RowVersion = new byte[] { 0, 0, 0, 0, 0, 0, 70, 81 },
                CityId = Guid.NewGuid()
            };
            context.CityDistricts.Add(cityDistrictEntity);
            await context.SaveChangesAsync();
            return cityDistrictEntity;
        }

        private static async Task<string> PrepareExpectedResponseAsync(RivaAdministrativeDivisionsDbContext context, Guid cityDistrictId)
        {
            var cityDistrictEntity = await context.CityDistricts.FindAsync(cityDistrictId);
            await context.Entry(cityDistrictEntity).ReloadAsync();
            var cityDistrictResponse = new CityDistrictResponse(cityDistrictEntity.Id, cityDistrictEntity.RowVersion,
                cityDistrictEntity.Name,
                cityDistrictEntity.PolishName, 
                cityDistrictEntity.CityId,
                cityDistrictEntity.ParentId,
                cityDistrictEntity.NameVariants.Select(x => x.Value)
                );
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultTestPlatformContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };
            return JsonConvert.SerializeObject(cityDistrictResponse, settings);
        }
    }
}