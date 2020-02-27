using System;
using System.Threading.Tasks;
using Riva.Identity.Domain.PersistedGrants.Repositories;

namespace Riva.Identity.Web.Api.Test.IntegrationTestConfigs
{
    public class PersistedGrantRepositoryStub : IPersistedGrantRepository
    {
        public Task DeleteAllBySubjectIdAsync(Guid subjectId)
        {
            return Task.CompletedTask;
        }
    }
}