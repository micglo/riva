using System;
using System.Threading.Tasks;

namespace Riva.Identity.Domain.PersistedGrants.Repositories
{
    public interface IPersistedGrantRepository
    {
        Task DeleteAllBySubjectIdAsync(Guid subjectId);
    }
}