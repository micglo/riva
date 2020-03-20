using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Cosmonaut;
using FluentAssertions;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Entities;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Enums;
using Riva.Announcements.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.Announcements.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class DeleteFlatForRentAnnouncementIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public DeleteFlatForRentAnnouncementIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Delete_FlatForRentAnnouncement_When_Requesting_By_Administrator_Client()
        {
            var flatForRentAnnouncementEntity =
                await InsertForRentAnnouncementEntityAsync(_fixture.FlatForRentAnnouncementEntityCosmosStore);
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response =
                await _fixture.AdministratorHttpClient.DeleteAsync(
                    $"api/flatForRentAnnouncements/{flatForRentAnnouncementEntity.Id}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_Return_Forbidden_Status_Code_When_Requesting_By_User_Client()
        {
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.UserHttpClient.DeleteAsync($"api/flatForRentAnnouncements/{Guid.NewGuid()}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.DeleteAsync($"api/flatForRentAnnouncements/{Guid.NewGuid()}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<FlatForRentAnnouncementEntity> InsertForRentAnnouncementEntityAsync(ICosmosStore<FlatForRentAnnouncementEntity> cosmosStore)
        {
            var flatForRentAnnouncementEntity = new FlatForRentAnnouncementEntity
            {
                Id = Guid.NewGuid(),
                Title = "DeleteFlatForRentAnnouncement",
                SourceUrl = "http://sourceUrl",
                CityId = CityOptions.City.Id,
                Created = DateTimeOffset.UtcNow,
                Description = "Description",
                Price = 1000,
                NumberOfRooms = NumberOfRooms.One,
                CityDistricts = CityDistrictOptions.CityDistricts.Select(x => x.Id),
                CosmosEntityName = nameof(FlatForRentAnnouncement)
            };

            await cosmosStore.AddAsync(flatForRentAnnouncementEntity);

            return flatForRentAnnouncementEntity;
        }
    }
}