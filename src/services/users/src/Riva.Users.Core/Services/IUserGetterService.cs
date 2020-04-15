using System;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Models;
using Riva.Users.Domain.Users.Aggregates;

namespace Riva.Users.Core.Services
{
    public interface IUserGetterService
    {
        Task<GetResult<User>> GetByIdAsync(Guid id);
    }
}