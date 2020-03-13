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
    public class GetCityDistrictsIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public GetCityDistrictsIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Get_City_Districts_Collection_When_Requesting_By_User_Client()
        {
            var cityDistrictEntity = await InsertCityDistrictEntityAsync(_fixture.UserDbContext);
            var getCityDistrictsRequest = new GetCityDistrictsRequest
            {
                Name = cityDistrictEntity.Name,
                PolishName = cityDistrictEntity.PolishName,
                CityId = cityDistrictEntity.CityId,
                Sort = "name:asc",
                PageSize = 100,
                Page = 1,
                ParentId = cityDistrictEntity.ParentId
            };
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");
            var expectedResponse = await PrepareExpectedResponseAsync(_fixture.UserDbContext, getCityDistrictsRequest);

            var response =
                await _fixture.UserHttpClient.GetAsync(
                    $"api/cityDistricts?page={getCityDistrictsRequest.Page}&pageSize={getCityDistrictsRequest.PageSize}&sort={getCityDistrictsRequest.Sort}&name={getCityDistrictsRequest.Name}&polishName={getCityDistrictsRequest.PolishName}&cityId={getCityDistrictsRequest.CityId}");
            var responseContentString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseContentString.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.GetAsync("api/cityDistricts");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<CityDistrictEntity> InsertCityDistrictEntityAsync(RivaAdministrativeDivisionsDbContext context)
        {
            var cityDistrictEntity = new CityDistrictEntity
            {
                Id = Guid.NewGuid(),
                Name = "GetCityDistrictsIntegrationTest",
                PolishName = "GetCityDistrictsIntegrationTest",
                RowVersion = new byte[] { 1, 2, 4, 8, 16, 32, 64 },
                CityId = Guid.NewGuid(),
                ParentId = Guid.NewGuid()
            };
            context.CityDistricts.Add(cityDistrictEntity);
            await context.SaveChangesAsync();

            return cityDistrictEntity;
        }

        private static async Task<string> PrepareExpectedResponseAsync(RivaAdministrativeDivisionsDbContext context, GetCityDistrictsRequest getCityDistrictsRequest)
        {
            var cityDistrictEntities = await context.CityDistricts
                .Where(x =>
                    x.Name.ToLower().StartsWith(getCityDistrictsRequest.Name.ToLower()) &&
                    x.PolishName.ToLower().StartsWith(getCityDistrictsRequest.PolishName.ToLower()) &&
                    x.CityId == getCityDistrictsRequest.CityId &&
                    x.ParentId == getCityDistrictsRequest.ParentId)
                .OrderBy(x => x.Name)
                .Skip(getCityDistrictsRequest.PageSize.Value * (getCityDistrictsRequest.Page.Value - 1))
                .Take(getCityDistrictsRequest.PageSize.Value)
                .ToListAsync();
            var cityDistrictResponses = cityDistrictEntities.Select(x => new CityDistrictResponse(x.Id, x.RowVersion,
                x.Name, x.PolishName, x.CityId, x.ParentId, x.NameVariants.Select(nv => nv.Value)));
            var totalCount = await context.CityDistricts.LongCountAsync(x =>
                x.Name.ToLower().StartsWith(getCityDistrictsRequest.Name.ToLower()) &&
                x.PolishName.ToLower().StartsWith(getCityDistrictsRequest.PolishName.ToLower()) &&
                x.CityId == getCityDistrictsRequest.CityId &&
                x.ParentId == getCityDistrictsRequest.ParentId);
            var collectionResponse = new CollectionResponse<CityDistrictResponse>(totalCount, cityDistrictResponses);
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