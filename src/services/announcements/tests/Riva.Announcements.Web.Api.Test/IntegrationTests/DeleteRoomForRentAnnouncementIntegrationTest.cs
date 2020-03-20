using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Cosmonaut;
using FluentAssertions;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Entities;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Enums;
using Riva.Announcements.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.Announcements.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class DeleteRoomForRentAnnouncementIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public DeleteRoomForRentAnnouncementIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Delete_RoomForRentAnnouncement_When_Requesting_By_Administrator_Client()
        {
            var roomForRentAnnouncementEntity =
                await InsertForRentAnnouncementEntityAsync(_fixture.RoomForRentAnnouncementEntityCosmosStore);
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response =
                await _fixture.AdministratorHttpClient.DeleteAsync(
                    $"api/roomForRentAnnouncements/{roomForRentAnnouncementEntity.Id}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_Return_Forbidden_Status_Code_For_When_Requesting_By_User_Client()
        {
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.UserHttpClient.DeleteAsync($"api/roomForRentAnnouncements/{Guid.NewGuid()}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.DeleteAsync($"api/roomForRentAnnouncements/{Guid.NewGuid()}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<RoomForRentAnnouncementEntity> InsertForRentAnnouncementEntityAsync(ICosmosStore<RoomForRentAnnouncementEntity> cosmosStore)
        {
            var roomForRentAnnouncementEntity = new RoomForRentAnnouncementEntity
            {
                Id = Guid.NewGuid(),
                Title = "DeleteRoomForRentAnnouncement",
                SourceUrl = "http://sourceUrl",
                CityId = CityOptions.City.Id,
                Created = DateTimeOffset.UtcNow,
                Description = "Description",
                Price = 1000,
                CityDistricts = CityDistrictOptions.CityDistricts.Select(x => x.Id),
                RoomTypes = new List<RoomType>{ RoomType.Single },
                CosmosEntityName = nameof(RoomForRentAnnouncement)
            };

            await cosmosStore.AddAsync(roomForRentAnnouncementEntity);

            return roomForRentAnnouncementEntity;
        }
    }
}