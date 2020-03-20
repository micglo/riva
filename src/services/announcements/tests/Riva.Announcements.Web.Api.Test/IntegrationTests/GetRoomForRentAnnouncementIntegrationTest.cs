using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Cosmonaut;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Entities;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Enums;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Extensions;
using Riva.Announcements.Web.Api.AutoMapperProfiles;
using Riva.Announcements.Web.Api.Models.Responses;
using Riva.Announcements.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.Announcements.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class GetRoomForRentAnnouncementIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public GetRoomForRentAnnouncementIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Get_RoomForRentAnnouncement_When_Requesting_By_Administrator_Client()
        {
            var roomForRentAnnouncementEntity =
                await InsertRoomForRentAnnouncementEntityAsync(_fixture.RoomForRentAnnouncementEntityCosmosStore);
            var expectedResponse = await PrepareExpectedResponseAsync(roomForRentAnnouncementEntity.Id,
                _fixture.RoomForRentAnnouncementEntityCosmosStore);
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response =
                await _fixture.AdministratorHttpClient.GetAsync(
                    $"api/roomForRentAnnouncements/{roomForRentAnnouncementEntity.Id}");
            var responseContentString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseContentString.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Should_Return_Forbidden_Status_Code_When_Requesting_By_User_Client()
        {
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.UserHttpClient.GetAsync($"api/roomForRentAnnouncements/{Guid.NewGuid()}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.GetAsync($"api/roomForRentAnnouncements/{Guid.NewGuid()}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<RoomForRentAnnouncementEntity> InsertRoomForRentAnnouncementEntityAsync(ICosmosStore<RoomForRentAnnouncementEntity> cosmosStore)
        {
            var roomForRentAnnouncementEntity = new RoomForRentAnnouncementEntity
            {
                Id = Guid.NewGuid(),
                Title = "GetRoomForRentAnnouncement",
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

        private static async Task<string> PrepareExpectedResponseAsync(Guid roomForRentAnnouncementId, ICosmosStore<RoomForRentAnnouncementEntity> cosmosStore)
        {
            var roomForRentAnnouncementEntity = await cosmosStore.FindAsync(roomForRentAnnouncementId.ToString());
            var roomForRentAnnouncementResponse = new RoomForRentAnnouncementResponse(
                roomForRentAnnouncementEntity.Id,
                roomForRentAnnouncementEntity.Title,
                roomForRentAnnouncementEntity.SourceUrl,
                roomForRentAnnouncementEntity.CityId,
                roomForRentAnnouncementEntity.Created,
                roomForRentAnnouncementEntity.Description,
                roomForRentAnnouncementEntity.Price,
                roomForRentAnnouncementEntity.RoomTypes.Select(x => RoomForRentAnnouncementProfile.ConvertToRoomTypeEnum(x.ConvertToEnumeration())),
                roomForRentAnnouncementEntity.CityDistricts);
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultTestPlatformContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Converters = new List<JsonConverter> { new IsoDateTimeConverter(), new StringEnumConverter() }
            };
            return JsonConvert.SerializeObject(roomForRentAnnouncementResponse, settings);
        }
    }
}