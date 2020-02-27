using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Riva.Identity.Core.ErrorMessages;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Roles.Aggregates;
using Riva.Identity.Domain.Roles.Repositories;
using Riva.BuildingBlocks.Core.Models;
using Riva.Identity.Core.Enumerations;

namespace Riva.Identity.Infrastructure.Services
{
    public class RoleGetterService : IRoleGetterService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleGetterService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<GetResult<Role>> GetByIdAsync(Guid id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role is null)
            {
                var errors = new Collection<IError>
                {
                    new Error(RoleErrorCodeEnumeration.NotFound, RoleErrorMessage.NotFound)
                };
                return GetResult<Role>.Fail(errors);
            }

            return GetResult<Role>.Ok(role);
        }
    }
}