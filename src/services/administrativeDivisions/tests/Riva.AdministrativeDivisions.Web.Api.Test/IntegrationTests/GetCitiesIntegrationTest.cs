using System;
using System.Linq;
using System.Net;
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
using Riva.BuildingBlocks.WebApi.Models.Responses;
using Xunit;

namespace Riva.AdministrativeDivisions.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class GetCitiesIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public GetCitiesIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Get_Cities_Collection_When_Requesting_By_User_Client()
        {
            var cityEntity = await InsertCityEntityAsync(_fixture.UserDbContext);
            var getCitiesRequest = new GetCitiesRequest
            {
                Name = cityEntity.Name,
                PolishName = cityEntity.PolishName,
                StateId = cityEntity.StateId,
                Sort = "name:asc",
                PageSize = 100,
                Page = 1
            };
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");
            var expectedResponse = await PrepareExpectedResponseAsync(_fixture.UserDbContext, getCitiesRequest);

            var response =
                await _fixture.UserHttpClient.GetAsync(
                    $"api/cities?page={getCitiesRequest.Page}&pageSize={getCitiesRequest.PageSize}&sort={getCitiesRequest.Sort}&name={getCitiesRequest.Name}&polishName={getCitiesRequest.PolishName}&stateId={getCitiesRequest.StateId}");
            var responseContentString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseContentString.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.GetAsync("api/cities");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<CityEntity> InsertCityEntityAsync(RivaAdministrativeDivisionsDbContext context)
        {
            var cityEntity = new CityEntity
            {
                Id = Guid.NewGuid(),
                Name = "GetCitiesIntegrationTest",
                PolishName = "GetCitiesIntegrationTest",
                RowVersion = new byte[] { 1, 2, 4, 8, 16, 32, 64 },
                StateId = Guid.NewGuid()
            };
            context.Cities.Add(cityEntity);
            await context.SaveChangesAsync();

            return cityEntity;
        }

        private static async Task<string> PrepareExpectedResponseAsync(RivaAdministrativeDivisionsDbContext context, GetCitiesRequest getCitiesRequest)
        {
            var cityEntities = await context.Cities
                .Where(x =>
                    x.Name.ToLower().StartsWith(getCitiesRequest.Name.ToLower()) &&
                    x.PolishName.ToLower().StartsWith(getCitiesRequest.PolishName.ToLower()) &&
                    x.StateId == getCitiesRequest.StateId)
                .OrderBy(x => x.Name)
                .Skip(getCitiesRequest.PageSize.Value * (getCitiesRequest.Page.Value - 1))
                .Take(getCitiesRequest.PageSize.Value)
                .ToListAsync();
            var cityResponses = cityEntities.Select(x => new CityResponse(x.Id, x.RowVersion, x.Name, x.PolishName, x.StateId));
            var totalCount = await context.Cities.LongCountAsync(x =>
                x.Name.ToLower().StartsWith(getCitiesRequest.Name.ToLower()) &&
                x.PolishName.ToLower().StartsWith(getCitiesRequest.PolishName.ToLower()) &&
                x.StateId == getCitiesRequest.StateId);
            var collectionResponse = new CollectionResponse<CityResponse>(totalCount, cityResponses);
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultTestPlatformContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };
            return JsonConvert.SerializeObject(collectionResponse, settings);
        }
    }
}