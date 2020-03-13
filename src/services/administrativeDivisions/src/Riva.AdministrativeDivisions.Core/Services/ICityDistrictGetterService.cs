using System;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Aggregates;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.AdministrativeDivisions.Core.Services
{
    public interface ICityDistrictGetterService
    {
        Task<GetResult<CityDistrict>> GetByIdAsync(Guid id);
    }
}