using System;
using System.Threading.Tasks;
using Riva.Identity.Domain.Roles.Aggregates;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.Identity.Core.Services
{
    public interface IRoleGetterService
    {
        Task<GetResult<Role>> GetByIdAsync(Guid id);
    }
}