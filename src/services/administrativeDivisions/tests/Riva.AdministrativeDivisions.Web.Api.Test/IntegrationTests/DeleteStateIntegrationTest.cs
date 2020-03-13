using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Net.Http.Headers;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Contexts;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;
using Riva.AdministrativeDivisions.Web.Api.Test.IntegrationTestConfigs;
using Xunit;

namespace Riva.AdministrativeDivisions.Web.Api.Test.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class DeleteStateIntegrationTest
    {
        private readonly IntegrationTestFixture _fixture;

        public DeleteStateIntegrationTest(IntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Delete_State_When_Requesting_By_Administrator_Client()
        {
            var stateEntity = await InsertStateEntityAsync(_fixture.AdministratorDbContext);
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add("api-version", "1");
            _fixture.AdministratorHttpClient.DefaultRequestHeaders.Add(HeaderNames.IfMatch, $"\"{Convert.ToBase64String(stateEntity.RowVersion)}\"");

            var response = await _fixture.AdministratorHttpClient.DeleteAsync($"api/states/{stateEntity.Id}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Should_Return_Forbidden_Status_Code_When_Requesting_By_User_Client()
        {
            _fixture.UserHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.UserHttpClient.DeleteAsync($"api/states/{Guid.NewGuid()}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_Return_Unauthorized_Status_Code_When_Requesting_By_Anonymous_Client()
        {
            _fixture.AnonymousHttpClient.DefaultRequestHeaders.Add("api-version", "1");

            var response = await _fixture.AnonymousHttpClient.DeleteAsync($"api/states/{Guid.NewGuid()}");

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        private static async Task<StateEntity> InsertStateEntityAsync(RivaAdministrativeDivisionsDbContext context)
        {
            var stateEntity = new StateEntity
            {
                Id = Guid.NewGuid(),
                Name = "DeleteStateIntegrationTest",
                PolishName = "DeleteStateIntegrationTest",
                RowVersion = new byte[] { 0, 0, 0, 0, 0, 0, 70, 81 }
            };

            context.States.Add(stateEntity);
            await context.SaveChangesAsync();

            return stateEntity;
        }
    }
}