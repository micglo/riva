using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Domain.Cities.Aggregates;

namespace Riva.AdministrativeDivisions.Domain.Cities.Repositories
{
    public interface ICityRepository
    {
        Task<List<City>> GetAllAsync();
        Task<List<City>> FindAsync(int? skip, int? take, string sort, Guid? stateId, string name, string polishName);
        Task<City> GetByIdAsync(Guid id);
        Task<City> GetByNameAndStateIdAsync(string name, Guid stateId);
        Task<City> GetByPolishNameAndStateIdAsync(string polishName, Guid stateId);
        Task AddAsync(City city);
        Task UpdateAsync(City city);
        Task DeleteAsync(City city);
        Task<long> CountAsync();
        Task<long> CountAsync(Guid? stateId, string name, string polishName);
    }
}