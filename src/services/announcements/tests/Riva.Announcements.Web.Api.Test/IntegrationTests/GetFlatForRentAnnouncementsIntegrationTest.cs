using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Entities;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Enums;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Extensions;
using Riva.Announcements.Web.Api.AutoMapperProfiles;
using Riva.Announcements.Web.Api.Models.Responses;
using Riva.Announcements.Web.Api.Test.IntegrationTestConfigs;
using Riva.BuildingBlocks.WebApi.Models.Responses;
using Xunit;

namespace Riva.Announcements.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class GetFlatForRentAnnouncementsIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public GetFlatForRentAnnouncementsIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Get_FlatForRentAnnouncements_Collection_When_Requesting_By_Administrator_Client()
        {
            await InsertFlatForRentAnnouncementEntityAsync(_fixture.FlatForRentAnnouncementEntityCosmosStore);
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add("api-version", "1");
            var expectedResponse = await PrepareExpectedResponseAsync(_fixture.FlatForRentAnnouncementEntityCosmosStore);

            var response = await _fixture.AdministratorHttpClient.GetAsync("api/flatForRentAnnouncements");
            var responseContentString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseContentString.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Should_Return_Forbidden_Status_Code_When_Requesting_By_User_Client()
        {
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.UserHttpClient.GetAsync("api/flatForRentAnnouncements");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.GetAsync("api/flatForRentAnnouncements");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task InsertFlatForRentAnnouncementEntityAsync(ICosmosStore<FlatForRentAnnouncementEntity> cosmosStore)
        {
            var flatForRentAnnouncementEntity = new FlatForRentAnnouncementEntity
            {
                Id = Guid.NewGuid(),
                Title = "GetFlatForRentAnnouncements",
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
        }

        private static async Task<string> PrepareExpectedResponseAsync(ICosmosStore<FlatForRentAnnouncementEntity> cosmosStore)
        {
            var flatForRentAnnouncementEntities = await cosmosStore.Query().ToListAsync();
            var getFlatForRentAnnouncementsCollectionItemResponses = flatForRentAnnouncementEntities.Select(
                flatForRentAnnouncementEntity => new FlatForRentAnnouncementResponse(
                    flatForRentAnnouncementEntity.Id,
                    flatForRentAnnouncementEntity.Title,
                    flatForRentAnnouncementEntity.SourceUrl,
                    flatForRentAnnouncementEntity.CityId,
                    flatForRentAnnouncementEntity.Created,
                    flatForRentAnnouncementEntity.Description,
                    flatForRentAnnouncementEntity.Price,
                    FlatForRentAnnouncementProfile.ConvertToNumberOfRoomsEnum(flatForRentAnnouncementEntity
                        .NumberOfRooms.ConvertToEnumeration()),
                    flatForRentAnnouncementEntity.CityDistricts));
            var collectionResponse =
                new CollectionResponse<FlatForRentAnnouncementResponse>(flatForRentAnnouncementEntities.Count, getFlatForRentAnnouncementsCollectionItemResponses);
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultTestPlatformContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Converters = new List<JsonConverter> { new IsoDateTimeConverter(), new StringEnumConverter() }
            };
            return JsonConvert.SerializeObject(collectionResponse, settings);
        }
    }
}