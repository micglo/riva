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
    public class GetCityIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public GetCityIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Get_City_When_Requesting_By_User_Client()
        {
            var cityEntity = await InsertCityEntityAsync(_fixture.UserDbContext);
            var expectedResponse = PrepareExpectedResponse(cityEntity);
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.UserHttpClient.GetAsync($"api/cities/{cityEntity.Id}");
            var responseContentString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);

            responseContentString.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_When_Requesting_By_Anonymous_Client()
        {
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.GetAsync($"api/cities/{Guid.NewGuid()}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<CityEntity> InsertCityEntityAsync(RivaAdministrativeDivisionsDbContext context)
        {
            var cityEntity = new CityEntity
            {
                Id = Guid.NewGuid(),
                Name = "GetCityIntegrationTest",
                PolishName = "GetCityIntegrationTest",
                RowVersion = new byte[] { 1, 2, 4, 8, 16, 32, 64 },
                StateId = Guid.NewGuid()
            };

            context.Cities.Add(cityEntity);
            await context.SaveChangesAsync();

            return cityEntity;
        }

        private static string PrepareExpectedResponse(CityEntity cityEntity)
        {
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