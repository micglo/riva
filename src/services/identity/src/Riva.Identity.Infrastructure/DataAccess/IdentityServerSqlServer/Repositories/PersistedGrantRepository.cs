using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Riva.Identity.Domain.PersistedGrants.Repositories;

namespace Riva.Identity.Infrastructure.DataAccess.IdentityServerSqlServer.Repositories
{
    public class PersistedGrantRepository : IPersistedGrantRepository
    {
        private readonly PersistedGrantDbContext _persistedGrantDbContext;

        public PersistedGrantRepository(PersistedGrantDbContext persistedGrantDbContext)
        {
            _persistedGrantDbContext = persistedGrantDbContext;
        }

        public async Task DeleteAllBySubjectIdAsync(Guid subjectId)
        {
            var entities = await _persistedGrantDbContext.PersistedGrants
                .Where(x => x.SubjectId.Equals(subjectId.ToString()))
                .ToListAsync();
            _persistedGrantDbContext.RemoveRange(entities);
            await _persistedGrantDbContext.SaveChangesAsync();
        }
    }
}