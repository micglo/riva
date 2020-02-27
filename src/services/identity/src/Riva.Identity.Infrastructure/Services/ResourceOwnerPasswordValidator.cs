using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Riva.Identity.Core.Enumerations;
using Riva.Identity.Core.Services;

namespace Riva.Identity.Infrastructure.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IAccountGetterService _accountGetterService;
        private readonly IAccountVerificationService _accountVerificationService;
        private readonly IAccountClaimsCreatorService _accountClaimsCreatorService;

        public ResourceOwnerPasswordValidator(IAccountGetterService accountGetterService, 
            IAccountVerificationService accountVerificationService, IAccountClaimsCreatorService accountClaimsCreatorService)
        {
            _accountGetterService = accountGetterService;
            _accountVerificationService = accountVerificationService;
            _accountClaimsCreatorService = accountClaimsCreatorService;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var invalidCredentialsResult = new GrantValidationResult(TokenRequestErrors.InvalidTarget, "Invalid credentials");

            var getAccountResult = await _accountGetterService.GetByEmailAsync(context.UserName);
            if (!getAccountResult.Success)
            {
                context.Result = invalidCredentialsResult;
                return;
            }

            var accountCanBeAuthenticatedResult = _accountVerificationService.VerifyAccountCanBeAuthenticated(getAccountResult.Value, context.Password);
            if (!accountCanBeAuthenticatedResult.Success)
            {
                var error = accountCanBeAuthenticatedResult.Errors.Single();
                context.Result = error.ErrorCode.Equals(AccountErrorCodeEnumeration.PasswordIsNotSet) || error.ErrorCode.Equals(AccountErrorCodeEnumeration.IncorrectPassword)
                        ? invalidCredentialsResult
                        : new GrantValidationResult(TokenRequestErrors.InvalidTarget, error.ErrorMessage);
                return;
            }

            var claims = await _accountClaimsCreatorService.CreateAccountClaimsAsync(getAccountResult.Value);
            context.Result = new GrantValidationResult(getAccountResult.Value.Id.ToString(),
                context.Request.GrantType, claims);
        }
    }
}