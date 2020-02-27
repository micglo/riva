using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Riva.BuildingBlocks.Infrastructure.Models.Constants;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Roles.Repositories;

namespace Riva.Identity.Infrastructure.Services
{
    public class AccountClaimsCreatorService : IAccountClaimsCreatorService
    {
        private readonly IRoleRepository _roleRepository;

        public AccountClaimsCreatorService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<Claim>> CreateAccountClaimsAsync(Account account)
        {
            if (!account.Roles.Any())
                throw new ArgumentNullException(nameof(account.Roles));

            var roles = await _roleRepository.FindByIdsAsync(account.Roles);
            var roleJwtClaims = roles.Select(x => new Claim(JwtClaimTypes.Role, x.Name));
            var roleSecurityClaims = roles.Select(x => new Claim(ClaimTypes.Role, x.Name));
            var roleClaims = new List<Claim>(roleJwtClaims);
            roleClaims.AddRange(roleSecurityClaims);

            return new List<Claim>(roleClaims)
            {
                new Claim(JwtClaimTypes.Subject, account.Id.ToString()),
                new Claim(JwtClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(JwtClaimTypes.EmailVerified, account.Confirmed.ToString(), ClaimValueTypes.Boolean),
                new Claim(JwtClaimTypes.Scope, ApiResourcesConstants.RivaIdentityApiResource.Name),
                new Claim(ClaimTypes.Name, account.Email),
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString())
            };
        }
    }
}