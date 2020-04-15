using System;
using System.Threading.Tasks;
using Riva.Users.Domain.Cities.Aggregates;

namespace Riva.Users.Domain.Cities.Repositories
{
    public interface ICityRepository
    {
        Task<City> GetByIdAsync(Guid id);
    }
}