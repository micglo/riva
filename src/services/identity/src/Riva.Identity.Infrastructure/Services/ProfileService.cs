using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Riva.Identity.Core.Services;

namespace Riva.Identity.Infrastructure.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IAccountGetterService _accountRepository;
        private readonly IAccountClaimsCreatorService _accountClaimsCreatorService;

        public ProfileService(IAccountGetterService accountRepository, IAccountClaimsCreatorService accountClaimsCreatorService)
        {
            _accountRepository = accountRepository;
            _accountClaimsCreatorService = accountClaimsCreatorService;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var email = !string.IsNullOrWhiteSpace(context.Subject.Identity.Name)
                ? context.Subject.Identity.Name
                : context.Subject.FindFirst(x => x.Type.Equals(JwtClaimTypes.Email))?.Value;
            var getAccountResult = await _accountRepository.GetByEmailAsync(email);
            context.IssuedClaims = await _accountClaimsCreatorService.CreateAccountClaimsAsync(getAccountResult.Value);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var email = !string.IsNullOrWhiteSpace(context.Subject.Identity.Name)
                ? context.Subject.Identity.Name
                : context.Subject.FindFirst(x => x.Type.Equals(JwtClaimTypes.Email))?.Value;
            if (!string.IsNullOrWhiteSpace(email))
            {
                var getAccountResult = await _accountRepository.GetByEmailAsync(email);
                context.IsActive = getAccountResult.Success && getAccountResult.Value.Confirmed;
            }
        }
    }
}