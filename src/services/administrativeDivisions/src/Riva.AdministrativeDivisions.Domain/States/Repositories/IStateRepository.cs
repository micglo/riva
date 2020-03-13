using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Domain.States.Aggregates;

namespace Riva.AdministrativeDivisions.Domain.States.Repositories
{
    public interface IStateRepository
    {
        Task<List<State>> GetAllAsync();
        Task<List<State>> FindAsync(int? skip, int? take, string sort, string name, string polishName);
        Task<State> GetByIdAsync(Guid id);
        Task<State> GetByNameAsync(string name);
        Task<State> GetByPolishNameAsync(string polishName);
        Task AddAsync(State state);
        Task UpdateAsync(State state);
        Task DeleteAsync(State state);
        Task<long> CountAsync();
        Task<long> CountAsync(string name, string polishName);
    }
}