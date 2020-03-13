using System;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Domain.States.Aggregates;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.AdministrativeDivisions.Core.Services
{
    public interface IStateGetterService
    {
        Task<GetResult<State>> GetByIdAsync(Guid id);
    }
}