using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Entities;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Extensions;
using Riva.Announcements.Web.Api.AutoMapperProfiles;
using Riva.Announcements.Web.Api.Models.Enums;
using Riva.Announcements.Web.Api.Models.Requests;
using Riva.Announcements.Web.Api.Models.Responses;
using Riva.Announcements.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.Announcements.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class CreateRoomForRentAnnouncementIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public CreateRoomForRentAnnouncementIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Create_RoomForRentAnnouncement_When_Requesting_By_Administrator_Client()
        {
            var createRoomForRentAnnouncementRequest = new CreateRoomForRentAnnouncementRequest
            {
                Title = "CreateRoomForRentAnnouncement",
                SourceUrl = "http://sourceUrl",
                CityId = CityOptions.City.Id,
                Description = "Description",
                Price = 1000,
                RoomTypes = new List<RoomType> { RoomType.Single },
                CityDistricts = CityDistrictOptions.CityDistricts.Select(x => x.Id)
            };
            var createRoomForRentAnnouncementRequestString = JsonConvert.SerializeObject(createRoomForRentAnnouncementRequest);
            var requestContent = new StringContent(createRoomForRentAnnouncementRequestString, Encoding.UTF8, "application/json");
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AdministratorHttpClient.PostAsync("api/roomForRentAnnouncements", requestContent);
            var responseContentString = await response.Content.ReadAsStringAsync();
            var expectedResponse = await PrepareExpectedResponseAsync(_fixture.RoomForRentAnnouncementEntityCosmosStore, createRoomForRentAnnouncementRequest.Title);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Created);
            responseContentString.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Should_Return_Forbidden_Status_Code_When_Requesting_By_User_Client()
        {
            var createRoomForRentAnnouncementRequest = new CreateRoomForRentAnnouncementRequest
            {
                Title = "CreateRoomForRentAnnouncement",
                SourceUrl = "http://sourceUrl",
                CityId = CityOptions.City.Id,
                Description = "Description",
                Price = 1000,
                RoomTypes = new List<RoomType> { RoomType.Single },
                CityDistricts = CityDistrictOptions.CityDistricts.Select(x => x.Id)
            };
            var createRoomForRentAnnouncementRequestString = JsonConvert.SerializeObject(createRoomForRentAnnouncementRequest);
            var requestContent = new StringContent(createRoomForRentAnnouncementRequestString, Encoding.UTF8, "application/json");
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.UserHttpClient.PostAsync("api/roomForRentAnnouncements", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            var createRoomForRentAnnouncementRequest = new CreateRoomForRentAnnouncementRequest
            {
                Title = "CreateRoomForRentAnnouncement",
                SourceUrl = "http://sourceUrl",
                CityId = CityOptions.City.Id,
                Description = "Description",
                Price = 1000,
                RoomTypes = new List<RoomType> { RoomType.Single },
                CityDistricts = CityDistrictOptions.CityDistricts.Select(x => x.Id)
            };
            var createRoomForRentAnnouncementRequestString = JsonConvert.SerializeObject(createRoomForRentAnnouncementRequest);
            var requestContent = new StringContent(createRoomForRentAnnouncementRequestString, Encoding.UTF8, "application/json");
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.PostAsync("api/roomForRentAnnouncements", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<string> PrepareExpectedResponseAsync(ICosmosStore<RoomForRentAnnouncementEntity> cosmosStore, string title)
        {
            var roomForRentAnnouncementEntity = await cosmosStore.Query().SingleOrDefaultAsync(x => x.Title.Equals(title));
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