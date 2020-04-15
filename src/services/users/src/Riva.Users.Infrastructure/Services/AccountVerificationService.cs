using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Models;
using Riva.Users.Core.Enumerations;
using Riva.Users.Core.ErrorMessages;
using Riva.Users.Core.Services;

namespace Riva.Users.Infrastructure.Services
{
    public class AccountVerificationService : IAccountVerificationService
    {

        private readonly IRivaIdentityApiClientService _rivaIdentityApiClientService;

        public AccountVerificationService(IRivaIdentityApiClientService rivaIdentityApiClientService)
        {
            _rivaIdentityApiClientService = rivaIdentityApiClientService;
        }

        public async Task<VerificationResult> VerifyAccountExistsAsync(Guid accountId, string email)
        {
            var account = await _rivaIdentityApiClientService.GetAccountAsync(accountId);

            if (account is null)
            {
                var errors = new Collection<IError>
                {
                    new Error(AccountErrorCodeEnumeration.NotFound, AccountErrorMessage.NotFound)
                };
                return VerificationResult.Fail(errors);
            }

            if (!account.Email.Equals(email))
            {
                var errors = new Collection<IError>
                {
                    new Error(AccountErrorCodeEnumeration.EmailMismatch, AccountErrorMessage.EmailMismatch)
                };
                return VerificationResult.Fail(errors);
            }

            return VerificationResult.Ok();
        }
    }
}