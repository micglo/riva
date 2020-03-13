using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Aggregates;

namespace Riva.AdministrativeDivisions.Domain.CityDistricts.Repositories
{
    public interface ICityDistrictRepository
    {
        Task<List<CityDistrict>> GetAllAsync();
        Task<List<CityDistrict>> FindAsync(int? skip, int? take, string sort, string name, string polishName,
            Guid? cityId, Guid? parentId, IEnumerable<Guid> cityIds);
        Task<CityDistrict> GetByIdAsync(Guid id);
        Task<CityDistrict> GetByNameAndCityIdAsync(string name, Guid cityId);
        Task<CityDistrict> GetByPolishNameAndCityIdAsync(string polishName, Guid cityId);
        Task AddAsync(CityDistrict cityDistrict);
        Task UpdateAsync(CityDistrict cityDistrict);
        Task DeleteAsync(CityDistrict cityDistrict);
        Task<long> CountAsync();
        Task<long> CountAsync(string name, string polishName, Guid? cityId, Guid? parentId, IEnumerable<Guid> cityIds);
    }
}