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
    public class CreateFlatForRentAnnouncementIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public CreateFlatForRentAnnouncementIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Create_FlatForRentAnnouncement_When_Requesting_By_Administrator_Client()
        {
            var createFlatForRentAnnouncementRequest = new CreateFlatForRentAnnouncementRequest
            {
                Title = "CreateFlatForRentAnnouncement",
                SourceUrl = "http://sourceUrl",
                CityId = CityOptions.City.Id,
                Description = "Description",
                Price = 1000,
                NumberOfRooms = NumberOfRooms.One,
                CityDistricts = CityDistrictOptions.CityDistricts.Select(x => x.Id)
            };
            var createFlatForRentAnnouncementRequestString = JsonConvert.SerializeObject(createFlatForRentAnnouncementRequest);
            var requestContent = new StringContent(createFlatForRentAnnouncementRequestString, Encoding.UTF8, "application/json");
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AdministratorHttpClient.PostAsync("api/flatForRentAnnouncements", requestContent);
            var responseContentString = await response.Content.ReadAsStringAsync();
            var expectedResponse = await PrepareExpectedResponseAsync(_fixture.FlatForRentAnnouncementEntityCosmosStore, createFlatForRentAnnouncementRequest.Title);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Created);
            responseContentString.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Should_Return_Forbidden_Status_When_Requesting_By_User_Client()
        {
            var createFlatForRentAnnouncementRequest = new CreateFlatForRentAnnouncementRequest
            {
                Title = "CreateFlatForRentAnnouncement",
                SourceUrl = "http://sourceUrl",
                CityId = CityOptions.City.Id,
                Description = "Description",
                Price = 1000,
                NumberOfRooms = NumberOfRooms.One,
                CityDistricts = CityDistrictOptions.CityDistricts.Select(x => x.Id)
            };
            var createFlatForRentAnnouncementRequestString = JsonConvert.SerializeObject(createFlatForRentAnnouncementRequest);
            var requestContent = new StringContent(createFlatForRentAnnouncementRequestString, Encoding.UTF8, "application/json");
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.UserHttpClient.PostAsync("api/flatForRentAnnouncements", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            var createFlatForRentAnnouncementRequest = new CreateFlatForRentAnnouncementRequest
            {
                Title = "CreateFlatForRentAnnouncement",
                SourceUrl = "http://sourceUrl",
                CityId = CityOptions.City.Id,
                Description = "Description",
                Price = 1000,
                NumberOfRooms = NumberOfRooms.One,
                CityDistricts = CityDistrictOptions.CityDistricts.Select(x => x.Id)
            };
            var createFlatForRentAnnouncementRequestString = JsonConvert.SerializeObject(createFlatForRentAnnouncementRequest);
            var requestContent = new StringContent(createFlatForRentAnnouncementRequestString, Encoding.UTF8, "application/json");
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.PostAsync("api/flatForRentAnnouncements", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<string> PrepareExpectedResponseAsync(ICosmosStore<FlatForRentAnnouncementEntity> cosmosStore, string title)
        {
            var flatForRentAnnouncementEntity = await cosmosStore.Query().SingleOrDefaultAsync(x => x.Title.Equals(title));
            var flatForRentAnnouncementResponse = new FlatForRentAnnouncementResponse(
                flatForRentAnnouncementEntity.Id, 
                flatForRentAnnouncementEntity.Title, 
                flatForRentAnnouncementEntity.SourceUrl,
                flatForRentAnnouncementEntity.CityId, 
                flatForRentAnnouncementEntity.Created, 
                flatForRentAnnouncementEntity.Description, 
                flatForRentAnnouncementEntity.Price, 
                FlatForRentAnnouncementProfile.ConvertToNumberOfRoomsEnum(flatForRentAnnouncementEntity.NumberOfRooms.ConvertToEnumeration()),
                flatForRentAnnouncementEntity.CityDistricts);
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultTestPlatformContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Converters = new List<JsonConverter> { new IsoDateTimeConverter(), new StringEnumConverter() }
            };
            return JsonConvert.SerializeObject(flatForRentAnnouncementResponse, settings);
        }
    }
}