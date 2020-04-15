using System;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Models;
using Riva.Users.Domain.Cities.Aggregates;

namespace Riva.Users.Core.Services
{
    public interface ICityGetterService
    {
        Task<GetResult<City>> GetByIdAsync(Guid id);
    }
}