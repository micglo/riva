using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Extensions;
using Riva.Identity.Domain.Roles.Aggregates;
using Riva.Identity.Domain.Roles.Repositories;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;

namespace Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RivaIdentityDbContext _context;
        private readonly DbSet<RoleEntity> _dbSet;
        private readonly IMapper _mapper;

        public RoleRepository(RivaIdentityDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _dbSet = context.Set<RoleEntity>();
        }

        public async Task<List<Role>> GetAllAsync()
        {
            var entities = await _dbSet.ToListAsync();
            return _mapper.Map<List<RoleEntity>, List<Role>>(entities);
        }

        public async Task<List<Role>> FindByIdsAsync(IEnumerable<Guid> ids)
        {
            var entities = await _dbSet.Where(x => ids.Any(i => i == x.Id)).ToListAsync();
            return _mapper.Map<List<RoleEntity>, List<Role>>(entities);
        }

        public async Task<Role> GetByIdAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity != null ? _mapper.Map<RoleEntity, Role>(entity) : null;
        }

        public async Task<Role> GetByNameAsync(string name)
        {
            var entity = await _dbSet.SingleOrDefaultAsync(x => x.Name.ToLower().Equals(name.ToLower()));
            return entity != null ? _mapper.Map<RoleEntity, Role>(entity) : null;
        }

        public async Task AddAsync(Role role)
        {
            var entity = _mapper.Map<Role, RoleEntity>(role);
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            role.SetRowVersion(entity.RowVersion);
        }

        public async Task UpdateAsync(Role role)
        {
            var entity = _mapper.Map<Role, RoleEntity>(role);
            _context.AttachEntity(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            role.SetRowVersion(entity.RowVersion);
        }

        public Task DeleteAsync(Role role)
        {
            var entity = _mapper.Map<Role, RoleEntity>(role);
            _context.AttachEntity(entity);
            _context.Entry(entity).State = EntityState.Deleted;
            return _context.SaveChangesAsync();
        }
    }
}