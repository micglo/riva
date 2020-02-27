using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Riva.Identity.Core.ErrorMessages;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Roles.Repositories;
using Riva.BuildingBlocks.Core.Models;
using Riva.Identity.Core.Enumerations;

namespace Riva.Identity.Infrastructure.Services
{
    public class RoleVerificationService : IRoleVerificationService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleVerificationService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<VerificationResult> VerifyNameIsNotTakenAsync(string name)
        {
            var role = await _roleRepository.GetByNameAsync(name);
            if (role != null)
            {
                var errors = new Collection<IError>
                {
                    new Error(RoleErrorCodeEnumeration.NameIsAlreadyTaken, RoleErrorMessage.NameIsAlreadyTaken)
                };
                return VerificationResult.Fail(errors);
            }
            return VerificationResult.Ok();
        }
    }
}