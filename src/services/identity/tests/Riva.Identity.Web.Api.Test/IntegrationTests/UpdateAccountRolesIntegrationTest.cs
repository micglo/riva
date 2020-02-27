using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Riva.BuildingBlocks.Domain;
using Riva.BuildingBlocks.WebApiTest.IntegrationTests;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;
using Riva.Identity.Web.Api.Models.Requests.Accounts;
using Riva.Identity.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.Identity.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class UpdateAccountRolesIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public UpdateAccountRolesIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Update_Account_Roles_When_Requesting_By_Administrator_Client()
        {
            var factory = new AdministratorWebApplicationFactory("AdministratorShouldUpdateAccountRolesIntegrationTest");
            var client = factory.WithWebHostBuilder(builder => builder.ConfigureWebHostBuilderForIntegrationTest())
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
            var accountRoleEntity = await InsertUserRoleEntityIfNotExistsAsync(factory.DbContext);
            var accountEntity = await InsertAccountEntityAsync(factory.DbContext, accountRoleEntity.Id);
            var roleToUpdate = await InsertRoleEntityToUpdateAsync(factory.DbContext);
            var updateAccountRolesRequest = new UpdateAccountRolesRequest
            {
                Roles = new List<Guid> { accountRoleEntity.Id, roleToUpdate.Id }
            };
            var updateAccountRolesRequestString = JsonConvert.SerializeObject(updateAccountRolesRequest);
            var requestContent = new StringContent(updateAccountRolesRequestString, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Add("api-version", "1");

            var response = await client.PutAsync($"api/accounts/{accountEntity.Id}/roles", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
            var updatedAccountRoleEntities = await factory.DbContext.AccountRoles.Where(x => x.AccountId == accountEntity.Id)
                .ToListAsync();
            var updatedAccountRoleIds = updatedAccountRoleEntities.Select(x => x.RoleId);
            updatedAccountRoleIds.Should().Contain(roleToUpdate.Id);
        }

        [Fact]
        public async Task Should_Return_Forbidden_Status_Code_When_Requesting_By_User_Client()
        {
            var factory = new UserWebApplicationFactory("UserShouldNotUpdateAccountRolesIntegrationTest");
            var client = factory.WithWebHostBuilder(builder => builder.ConfigureWebHostBuilderForIntegrationTest())
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
            var accountRoleEntity = await InsertUserRoleEntityIfNotExistsAsync(factory.DbContext);
            var accountEntity = await InsertAccountEntityAsync(factory.DbContext, accountRoleEntity.Id);
            var updateAccountRolesRequest = new UpdateAccountRolesRequest
            {
                Roles = new List<Guid> { accountRoleEntity.Id }
            };
            var updateAccountRolesRequestString = JsonConvert.SerializeObject(updateAccountRolesRequest);
            var requestContent = new StringContent(updateAccountRolesRequestString, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Add("api-version", "1");

            var response = await client.PutAsync($"api/accounts/{accountEntity.Id}/roles", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            var updateAccountRolesRequest = new UpdateAccountRolesRequest
            {
                Roles = new List<Guid> { Guid.NewGuid() }
            };
            var updateAccountRequestString = JsonConvert.SerializeObject(updateAccountRolesRequest);
            var requestContent = new StringContent(updateAccountRequestString, Encoding.UTF8, "application/json");
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.PutAsync($"api/accounts/{Guid.NewGuid()}/roles", requestContent);

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<RoleEntity> InsertUserRoleEntityIfNotExistsAsync(RivaIdentityDbContext context)
        {
            var roleEntity = await context.Roles.SingleOrDefaultAsync(x => x.Name.Equals(DefaultRoleEnumeration.User.DisplayName));
            if (roleEntity is null)
            {
                roleEntity = new RoleEntity
                {
                    Id = Guid.NewGuid(),
                    Name = DefaultRoleEnumeration.User.DisplayName,
                    RowVersion = new byte[] { 1, 2, 4, 8, 16, 32, 64 }
                };
                context.Roles.Add(roleEntity);
                await context.SaveChangesAsync();
            }
            return roleEntity;
        }

        private static async Task<RoleEntity> InsertRoleEntityToUpdateAsync(RivaIdentityDbContext context)
        {
            var roleEntity = new RoleEntity
            {
                Id = Guid.NewGuid(),
                Name = "UpdateAccountIntegrationTestRoleToUpdate",
                RowVersion = new byte[] { 1, 2, 4, 8, 16, 32, 64, 128 }
            };
            context.Roles.Add(roleEntity);
            await context.SaveChangesAsync();
            return roleEntity;
        }

        private static async Task<AccountEntity> InsertAccountEntityAsync(RivaIdentityDbContext context, Guid initialRoleId)
        {
            var accountId = AuthUserOptions.UserId;
            var accountEntity = new AccountEntity
            {
                Id = accountId,
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
                        RoleId = initialRoleId
                    }
                }
            };

            context.Accounts.Add(accountEntity);
            await context.SaveChangesAsync();

            return accountEntity;
        }
    }
}