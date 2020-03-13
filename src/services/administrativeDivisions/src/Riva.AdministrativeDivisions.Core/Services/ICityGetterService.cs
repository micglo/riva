using System;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Domain.Cities.Aggregates;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.AdministrativeDivisions.Core.Services
{
    public interface ICityGetterService
    {
        Task<GetResult<City>> GetByIdAsync(Guid id);
    }
}