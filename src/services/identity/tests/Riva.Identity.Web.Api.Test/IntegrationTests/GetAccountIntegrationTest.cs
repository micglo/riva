using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Riva.BuildingBlocks.WebApiTest.IntegrationTests;
using Riva.Identity.Domain.Accounts.Enumerations;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Extensions;
using Riva.Identity.Web.Api.AutoMapperProfiles;
using Riva.Identity.Web.Api.Models.Responses.Accounts;
using Riva.Identity.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.Identity.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class GetAccountIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public GetAccountIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Get_Account_When_Requesting_By_Administrator_Client()
        {
            var factory = new AdministratorWebApplicationFactory("AdministratorShouldGetAccountIntegrationTest");
            var client = factory.WithWebHostBuilder(builder => builder.ConfigureWebHostBuilderForIntegrationTest())
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
            var accountEntity = await InsertAccountEntityAsync(factory.DbContext);
            var expectedResponse = PrepareExpectedResponse(accountEntity);
            client.DefaultRequestHeaders.Add("api-version", "1");

            var response = await client.GetAsync($"api/accounts/{accountEntity.Id}");
            var responseContentString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseContentString.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Should_Return_Account_When_Requesting_By_User_Client()
        {
            var factory = new UserWebApplicationFactory("UserShouldGetAccountIntegrationTest");
            var client = factory.WithWebHostBuilder(builder => builder.ConfigureWebHostBuilderForIntegrationTest())
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
            var accountEntity = await InsertAccountEntityAsync(factory.DbContext);
            var expectedResponse = PrepareExpectedResponse(accountEntity);
            client.DefaultRequestHeaders.Add("api-version", "1");

            var response = await client.GetAsync($"api/accounts/{accountEntity.Id}");
            var responseContentString = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseContentString.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.GetAsync($"api/accounts/{Guid.NewGuid()}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<AccountEntity> InsertAccountEntityAsync(RivaIdentityDbContext context)
        {
            var accountId = AuthUserOptions.UserId;
            var accountEntity = new AccountEntity
            {
                Id = AuthUserOptions.UserId,
                Email = AuthUserOptions.Email,
                Confirmed = true,
                PasswordHash = "PasswordHash",
                SecurityStamp = Guid.NewGuid(),
                Created = DateTimeOffset.UtcNow,
                Roles = new List<AccountRoleEntity>
                {
                    new AccountRoleEntity
                    {
                        AccountId = accountId,
                        RoleId = Guid.NewGuid()
                    }
                },
                Tokens = new List<TokenEntity>
                {
                    new TokenEntity
                    {
                        Id = Guid.NewGuid(),
                        Issued = DateTimeOffset.UtcNow,
                        Expires = DateTimeOffset.UtcNow.AddDays(1),
                        Type = TokenTypeEnumeration.AccountConfirmation.ConvertToEnum(),
                        Value = "123456",
                        AccountId = accountId
                    }
                }
            };

            context.Accounts.Add(accountEntity);
            await context.SaveChangesAsync();

            return accountEntity;
        }

        private static string PrepareExpectedResponse(AccountEntity accountEntity)
        {
            var accountTokens = accountEntity.Tokens.Select(x => new AccountToken(x.Issued, x.Expires,
                TokenProfile.ConvertToAccountTokenTypeEnum(x.Type.ConvertToEnumeration()), x.Value));
            var getAccountResponse = new GetAccountResponse(accountEntity.Id, accountEntity.Email,
                accountEntity.Confirmed, accountEntity.Created, !string.IsNullOrWhiteSpace(accountEntity.PasswordHash),
                accountEntity.LastLogin, accountEntity.Roles.Select(x => x.RoleId), accountTokens);
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultTestPlatformContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };
            return JsonConvert.SerializeObject(getAccountResponse, settings);
        }
    }
}