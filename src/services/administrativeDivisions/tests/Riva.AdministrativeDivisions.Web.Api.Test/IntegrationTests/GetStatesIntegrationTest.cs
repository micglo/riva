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
    public class GetStatesIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public GetStatesIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Get_States_Collection_When_Requesting_By_User_Client()
        {
            var stateEntity = await InsertStateEntityAsync(_fixture.UserDbContext);
            var getStatesRequest = new GetStatesRequest
            {
                Page = 1,
                PageSize = 100,
                Name = stateEntity.Name,
                PolishName = stateEntity.PolishName,
                Sort = "name:asc"
            };
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response =
                await _fixture.UserHttpClient.GetAsync(
                    $"api/states?page={getStatesRequest.Page}&pageSize={getStatesRequest.PageSize}&sort={getStatesRequest.Sort}&name={getStatesRequest.Name}&polishName={getStatesRequest.PolishName}");
            var responseContentString = await response.Content.ReadAsStringAsync();
            var expectedResponse = await PrepareExpectedResponseAsync(_fixture.UserDbContext, getStatesRequest);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseContentString.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.GetAsync("api/states");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<StateEntity> InsertStateEntityAsync(RivaAdministrativeDivisionsDbContext context)
        {
            var stateEntity = new StateEntity
            {
                Id = Guid.NewGuid(),
                Name = "GetStatesIntegrationTest",
                PolishName = "GetStatesIntegrationTest",
                RowVersion = new byte[] { 1, 2, 4, 8, 16, 32, 64 }
            };

            context.States.Add(stateEntity);
            await context.SaveChangesAsync();

            return stateEntity;
        }

        private static async Task<string> PrepareExpectedResponseAsync(RivaAdministrativeDivisionsDbContext context, GetStatesRequest getStatesRequest)
        {
            var stateEntities = await context.States
                .Where(x =>
                    x.Name.ToLower().StartsWith(getStatesRequest.Name.ToLower()) &&
                    x.PolishName.ToLower().StartsWith(getStatesRequest.PolishName.ToLower()))
                .OrderBy(x => x.Name)
                .Skip(getStatesRequest.PageSize.Value * (getStatesRequest.Page.Value - 1))
                .Take(getStatesRequest.PageSize.Value)
                .ToListAsync();
            var stateResponses = stateEntities.Select(x => new StateResponse(x.Id, x.RowVersion, x.Name, x.PolishName));
            var totalCount = await context.States.LongCountAsync(x =>
                x.Name.ToLower().StartsWith(getStatesRequest.Name.ToLower()) &&
                x.PolishName.ToLower().StartsWith(getStatesRequest.PolishName.ToLower()));
            var collectionResponse = new CollectionResponse<StateResponse>(totalCount, stateResponses);
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