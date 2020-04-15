using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.Users.Domain.Cities.Aggregates;
using Riva.Users.Domain.Cities.Repositories;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Contexts;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Entities;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly DbSet<CityEntity> _dbSet;
        private readonly IMapper _mapper;

        public CityRepository(RivaUsersDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _dbSet = context.Set<CityEntity>();
        }

        public async Task<City> GetByIdAsync(Guid id)
        {
            var entity = await _dbSet.Include(x => x.CityDistricts).SingleOrDefaultAsync(x => x.Id == id);
            return entity != null ? _mapper.Map<CityEntity, City>(entity) : null;
        }
    }
}