using System;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Riva.Identity.Domain.PersistedGrants.Repositories;
using Riva.Identity.Infrastructure.DataAccess.IdentityServerSqlServer.Repositories;
using Xunit;

namespace Riva.Identity.Infrastructure.Test.DataAccessTests.IdentityServerSqlServerTests
{
    public class PersistedGrantRepositoryTest
    {
        private readonly PersistedGrantDbContext _context;
        private readonly IPersistedGrantRepository _persistedGrantRepository;

        public PersistedGrantRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<PersistedGrantDbContext>()
                .UseInMemoryDatabase("RivaIdentityIdentityServerIntegrationTestsDb").Options;
            _context = new PersistedGrantDbContext(options, new OperationalStoreOptions());
            _persistedGrantRepository = new PersistedGrantRepository(_context);
        }

        [Fact]
        public async Task DeleteAllBySubjectIdAsync_Should_Delete_All_PersistedGrants_For_Particular_SubjectId()
        {
            var subjectId = Guid.NewGuid();
            var persistedGrant = new PersistedGrant
            {
                ClientId = "clientId",
                Type = "refresh_token",
                Data =
                    "{\"CreationTime\":\"2020-02-17T18:39:53Z\",\"Lifetime\":2592000,\"AccessToken\":{\"Audiences\":[\"Riva.Identity\"],\"Issuer\":\"http://localhost:5000\",\"CreationTime\":\"2020-02-17T18:39:53Z\",\"Lifetime\":3600,\"Type\":\"access_token\",\"ClientId\":\"0cf7dfaa-d296-4a8d-8f3c-4da799ddc526\",\"AccessTokenType\":0,\"Claims\":[{\"Type\":\"client_id\",\"Value\":\"0cf7dfaa-d296-4a8d-8f3c-4da799ddc526\",\"ValueType\":\"http://www.w3.org/2001/XMLSchema#string\"},{\"Type\":\"scope\",\"Value\":\"openid\",\"ValueType\":\"http://www.w3.org/2001/XMLSchema#string\"},{\"Type\":\"scope\",\"Value\":\"profile\",\"ValueType\":\"http://www.w3.org/2001/XMLSchema#string\"},{\"Type\":\"scope\",\"Value\":\"Riva.Identity\",\"ValueType\":\"http://www.w3.org/2001/XMLSchema#string\"},{\"Type\":\"scope\",\"Value\":\"offline_access\",\"ValueType\":\"http://www.w3.org/2001/XMLSchema#string\"},{\"Type\":\"sub\",\"Value\":\"5eb19001-41ef-4819-9a96-e7f777fe3dcc\",\"ValueType\":\"http://www.w3.org/2001/XMLSchema#string\"},{\"Type\":\"auth_time\",\"Value\":\"1581964793\",\"ValueType\":\"http://www.w3.org/2001/XMLSchema#integer64\"},{\"Type\":\"idp\",\"Value\":\"local\",\"ValueType\":\"http://www.w3.org/2001/XMLSchema#string\"},{\"Type\":\"amr\",\"Value\":\"pwd\",\"ValueType\":\"http://www.w3.org/2001/XMLSchema#string\"},{\"Type\":\"role\",\"Value\":\"Administrator\",\"ValueType\":\"http://www.w3.org/2001/XMLSchema#string\"},{\"Type\":\"role\",\"Value\":\"Account\",\"ValueType\":\"http://www.w3.org/2001/XMLSchema#string\"},{\"Type\":\"email\",\"Value\":\"michalglowaczewski@gmail.com\",\"ValueType\":\"http://www.w3.org/2001/XMLSchema#string\"},{\"Type\":\"email_verified\",\"Value\":\"True\",\"ValueType\":\"http://www.w3.org/2001/XMLSchema#boolean\"},{\"Type\":\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name\",\"Value\":\"michalglowaczewski@gmail.com\",\"ValueType\":\"http://www.w3.org/2001/XMLSchema#string\"},{\"Type\":\"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier\",\"Value\":\"5eb19001-41ef-4819-9a96-e7f777fe3dcc\",\"ValueType\":\"http://www.w3.org/2001/XMLSchema#string\"}],\"Version\":4},\"Version\":4}",
                CreationTime = DateTime.UtcNow,
                Key = "LUzQkz9A1uljITj7GRk4FkwFXA7z6S8VN8Cx1EnWxg8=",
                SubjectId = subjectId.ToString(),
                Expiration = DateTime.UtcNow.AddDays(1)
            };
            _context.PersistedGrants.Add(persistedGrant);
            await _context.SaveChangesAsync();

            Func<Task> result = async () => await _persistedGrantRepository.DeleteAllBySubjectIdAsync(subjectId);
            
            await result.Should().NotThrowAsync<Exception>();
            var deletedEntity = await _context.PersistedGrants.FindAsync(persistedGrant.Key);
            deletedEntity.Should().BeNull();
        }
    }
}