using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CreateCityDistrictIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public CreateCityDistrictIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Create_City_District_When_Requesting_By_Administrator_Client()
        {
            var city = await InsertCityEntityAsync(_fixture.AdministratorDbContext);
            var createCityDistrictRequest = new CreateCityDistrictRequest
            {
                Name = "CreateCityIntegrationTest",
                PolishName = "CreateCityIntegrationTest",
                CityId = city.Id,
                NameVariants = new List<string> { "CreateCityIntegrationTest" }
            };
            var createCityDistrictRequestString = JsonConvert.SerializeObject(createCityDistrictRequest);
            var requestContent = new StringContent(createCityDistrictRequestString, Encoding.UTF8, "application/json");
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AdministratorHttpClient.PostAsync("api/cityDistricts", requestContent);
            var responseContentString = await response.Content.ReadAsStringAsync();
            var expectedResponse = await PrepareExpectedResponseAsync(_fixture.AdministratorDbContext, createCityDistrictRequest.Name);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Created);
            responseContentString.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Should_Return_Forbidden_Status_Code_When_Requesting_By_User_Client()
        {
            var createCityDistrictRequest = new CreateCityDistrictRequest
            {
                Name = "CreateCityIntegrationTest",
                PolishName = "CreateCityIntegrationTest",
                CityId = Guid.NewGuid()
            };
            var createCityDistrictRequestString = JsonConvert.SerializeObject(createCityDistrictRequest);
            var requestContent = new StringContent(createCityDistrictRequestString, Encoding.UTF8, "application/json");
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.UserHttpClient.PostAsync("api/cityDistricts", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            var createCityDistrictRequest = new CreateCityDistrictRequest
            {
                Name = "CreateCityIntegrationTest",
                PolishName = "CreateCityIntegrationTest",
                CityId = Guid.NewGuid()
            };
            var createCityDistrictRequestString = JsonConvert.SerializeObject(createCityDistrictRequest);
            var requestContent = new StringContent(createCityDistrictRequestString, Encoding.UTF8, "application/json");
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.PostAsync("api/cityDistricts", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<CityEntity> InsertCityEntityAsync(RivaAdministrativeDivisionsDbContext context)
        {
            var cityEntity = new CityEntity
            {
                Id = Guid.NewGuid(),
                Name = "CreateCityDistrictIntegrationTest",
                PolishName = "CreateCityDistrictIntegrationTest",
                StateId = Guid.NewGuid(),
                RowVersion = new byte[] { 0, 0, 0, 0, 0, 0, 70, 81 }
            };
            context.Cities.Add(cityEntity);
            await context.SaveChangesAsync();
            return cityEntity;
        }

        private static async Task<string> PrepareExpectedResponseAsync(RivaAdministrativeDivisionsDbContext context, string cityDistrictName)
        {
            var cityDistrictEntity = await context.CityDistricts.Include(x => x.NameVariants)
                .SingleOrDefaultAsync(x => x.Name.Equals(cityDistrictName));
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