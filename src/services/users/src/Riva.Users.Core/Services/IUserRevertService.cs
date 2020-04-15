using System;
using System.Threading.Tasks;

namespace Riva.Users.Core.Services
{
    public interface IUserRevertService
    {
        Task RevertUserAsync(Guid userId, Guid correlationId);
    }
}