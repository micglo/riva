using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Riva.BuildingBlocks.WebApi.Models.Responses;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;
using Riva.Identity.Web.Api.Models.Requests.Accounts;
using Riva.Identity.Web.Api.Models.Responses.Accounts;
using Riva.Identity.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.Identity.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class GetAccountsIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public GetAccountsIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Get_Accounts_Collection_When_Requesting_By_Administrator_Client()
        {
            var accountEntity = await InsertAccountEntityAsync(_fixture.AdministratorDbContext);
            var getAccountsRequest = new GetAccountsRequest
            {
                Email = accountEntity.Email,
                Confirmed = accountEntity.Confirmed,
                Sort = "email:asc",
                PageSize = 100,
                Page = 1
            };
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add("api-version", "1");
            var expectedResponse = await PrepareExpectedResponseAsync(getAccountsRequest, _fixture.AdministratorDbContext);

            var response =
                await _fixture.AdministratorHttpClient.GetAsync(
                    $"api/accounts?page={getAccountsRequest.Page}&pageSize={getAccountsRequest.PageSize}&sort={getAccountsRequest.Sort}&email={getAccountsRequest.Email}&confirmed={getAccountsRequest.Confirmed}");
            var responseContentString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseContentString.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Should_Return_Forbidden_Status_Code_When_Requesting_By_User_Client()
        {
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.UserHttpClient.GetAsync("api/accounts");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Adnonymous_Client()
        {
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.GetAsync("api/accounts");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<AccountEntity> InsertAccountEntityAsync(RivaIdentityDbContext context)
        {
            var accountId = Guid.NewGuid();
            var accountEntity = new AccountEntity
            {
                Id = accountId,
                Email = "getAccountsIntegrationTest@email.com",
                Confirmed = true,
                PasswordHash = "PasswordHash",
                SecurityStamp = Guid.NewGuid(),
                Created = DateTimeOffset.UtcNow
            };

            context.Accounts.Add(accountEntity);
            await context.SaveChangesAsync();

            return accountEntity;
        }

        private static async Task<string> PrepareExpectedResponseAsync(GetAccountsRequest getAccountsRequest, RivaIdentityDbContext context)
        {
            var accountEntities = await context.Accounts
                .Where(x => x.Email.ToLower().StartsWith(getAccountsRequest.Email.ToLower()) && x.Confirmed == getAccountsRequest.Confirmed)
                .OrderBy(x => x.Email)
                .Skip(getAccountsRequest.PageSize.Value * (getAccountsRequest.Page.Value - 1))
                .Take(getAccountsRequest.PageSize.Value)
                .ToListAsync();
            var totalCount = await context.Accounts
                .LongCountAsync(x => x.Email.ToLower().StartsWith(getAccountsRequest.Email.ToLower()) && x.Confirmed == getAccountsRequest.Confirmed);
            var getAccountsCollectionItemResponses = accountEntities.Select(x =>
                new GetAccountsCollectionItemResponse(x.Id, x.Email, x.Confirmed, x.Created,
                    !string.IsNullOrWhiteSpace(x.PasswordHash), x.LastLogin));
            var collectionResponse = new CollectionResponse<GetAccountsCollectionItemResponse>(totalCount, getAccountsCollectionItemResponses);
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultTestPlatformContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };
            return JsonConvert.SerializeObject(collectionResponse, settings);
        }
    }
}