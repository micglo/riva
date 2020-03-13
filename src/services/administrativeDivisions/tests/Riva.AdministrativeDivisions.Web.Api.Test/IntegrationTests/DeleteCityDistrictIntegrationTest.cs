using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Net.Http.Headers;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Contexts;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;
using Riva.AdministrativeDivisions.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.AdministrativeDivisions.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class DeleteCityDistrictIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public DeleteCityDistrictIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Delete_City_District_When_Requesting_By_Administrator_Client()
        {
            var cityDistrictEntity = await InsertCityDistrictEntityAsync(_fixture.AdministratorDbContext);
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add("api-version", "1");
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add(HeaderNames.IfMatch, $"\"{Convert.ToBase64String(cityDistrictEntity.RowVersion)}\"");

            var response = await _fixture.AdministratorHttpClient.DeleteAsync($"api/cityDistricts/{cityDistrictEntity.Id}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_Return_Forbidden_Status_Code_When_Requesting_By_User_Client()
        {
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.UserHttpClient.DeleteAsync($"api/cityDistricts/{Guid.NewGuid()}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.DeleteAsync($"api/cityDistricts/{Guid.NewGuid()}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<CityDistrictEntity> InsertCityDistrictEntityAsync(RivaAdministrativeDivisionsDbContext context)
        {
            var cityDistrictEntity = new CityDistrictEntity
            {
                Id = Guid.NewGuid(),
                Name = "DeleteCityDistrictIntegrationTest",
                PolishName = "DeleteCityDistrictIntegrationTest",
                CityId = Guid.NewGuid(),
                RowVersion = new byte[] { 0, 0, 0, 0, 0, 0, 70, 81 }
            };
            context.CityDistricts.Add(cityDistrictEntity);
            await context.SaveChangesAsync();
            return cityDistrictEntity;
        }
    }
}