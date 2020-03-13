using System;
using System.Linq;
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
    public class GetCityDistrictIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public GetCityDistrictIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Get_City_District_When_Requesting_By_User_Client()
        {
            var cityDistrictEntity = await InsertCityDistrictEntityAsync(_fixture.UserDbContext);
            var expectedResponse = PrepareExpectedResponse(cityDistrictEntity);
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.UserHttpClient.GetAsync($"api/cityDistricts/{cityDistrictEntity.Id}");
            var responseContentString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseContentString.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.GetAsync($"api/cityDistricts/{Guid.NewGuid()}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<CityDistrictEntity> InsertCityDistrictEntityAsync(RivaAdministrativeDivisionsDbContext context)
        {
            var cityDistrictEntity = new CityDistrictEntity
            {
                Id = Guid.NewGuid(),
                Name = "GetCityDistrictIntegrationTest",
                PolishName = "GetCityDistrictIntegrationTest",
                CityId = Guid.NewGuid(),
                RowVersion = new byte[] { 0, 0, 0, 0, 0, 0, 70, 81 }
            };
            context.CityDistricts.Add(cityDistrictEntity);
            await context.SaveChangesAsync();
            return cityDistrictEntity;
        }

        private static string PrepareExpectedResponse(CityDistrictEntity cityDistrictEntity)
        {
            var cityDistrictResponse = new CityDistrictResponse(cityDistrictEntity.Id,
                cityDistrictEntity.RowVersion, 
                cityDistrictEntity.Name,
                cityDistrictEntity.PolishName, 
                cityDistrictEntity.CityId, 
                cityDistrictEntity.ParentId,
                cityDistrictEntity.NameVariants.Select(x => x.Value));
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