using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Riva.Identity.Core.ErrorMessages;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Entities;
using Riva.BuildingBlocks.Core.Models;
using Riva.Identity.Core.Enumerations;

namespace Riva.Identity.Infrastructure.Services
{
    public class AccountVerificationService : IAccountVerificationService
    {
        private readonly IAccountGetterService _accountGetterService;
        private readonly IPasswordService _passwordService;

        public AccountVerificationService(IAccountGetterService accountGetterService, IPasswordService passwordService)
        {
            _accountGetterService = accountGetterService;
            _passwordService = passwordService;
        }

        public async Task<VerificationResult> VerifyEmailIsNotTakenAsync(string email)
        {
            var getAccountResult = await _accountGetterService.GetByEmailAsync(email);
            if (getAccountResult.Success)
            {
                var errors = new Collection<IError>
                {
                    new Error(AccountErrorCodeEnumeration.EmailIsAlreadyTaken, AccountErrorMessage.EmailIsAlreadyTaken)
                };
                return VerificationResult.Fail(errors);
            }

            return VerificationResult.Ok();
        }

        public VerificationResult VerifyAccountIsNotConfirmed(bool confirmed)
        {
            if (confirmed)
            {
                var errors = new Collection<IError>
                {
                    new Error(AccountErrorCodeEnumeration.AlreadyConfirmed, AccountErrorMessage.AlreadyConfirmed)
                };
                return VerificationResult.Fail(errors);
            }

            return VerificationResult.Ok();
        }

        public VerificationResult VerifyAccountIsConfirmed(bool confirmed)
        {
            if (!confirmed)
            {
                var errors = new Collection<IError>
                {
                    new Error(AccountErrorCodeEnumeration.NotConfirmed, AccountErrorMessage.NotConfirmed)
                };
                return VerificationResult.Fail(errors);
            }

            return VerificationResult.Ok();
        }

        public VerificationResult VerifyPassword(string passwordHash, string password)
        {
            if (!_passwordService.VerifyHashedPassword(passwordHash, password))
            {
                var errors = new Collection<IError>
                {
                    new Error(AccountErrorCodeEnumeration.IncorrectPassword, AccountErrorMessage.IncorrectPassword)
                };
                return VerificationResult.Fail(errors);
            }

            return VerificationResult.Ok();
        }

        public VerificationResult VerifyPasswordIsSet(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                var errors = new Collection<IError>
                {
                    new Error(AccountErrorCodeEnumeration.PasswordIsNotSet, AccountErrorMessage.PasswordIsNotSet)
                };
                return VerificationResult.Fail(errors);
            }

            return VerificationResult.Ok();
        }

        public VerificationResult VerifyPasswordIsNotSet(string passwordHash)
        {
            if (!string.IsNullOrWhiteSpace(passwordHash))
            {
                var errors = new Collection<IError>
                {
                    new Error(AccountErrorCodeEnumeration.PasswordAlreadySet, AccountErrorMessage.PasswordAlreadySet)
                };
                return VerificationResult.Fail(errors);
            }

            return VerificationResult.Ok();
        }

        public VerificationResult VerifyAccountCanBeAuthenticated(Account account, string password)
        {
            var emailIsConfirmedVerificationResult = VerifyAccountIsConfirmed(account.Confirmed);
            if (!emailIsConfirmedVerificationResult.Success)
                return emailIsConfirmedVerificationResult;

            var passwordIsSetVerificationResult = VerifyPasswordIsSet(account.PasswordHash);
            if (!passwordIsSetVerificationResult.Success)
                return passwordIsSetVerificationResult;

            var passwordVerificationResult = VerifyPassword(account.PasswordHash, password);
            if (!passwordVerificationResult.Success)
                return passwordVerificationResult;

            return VerificationResult.Ok();
        }

        public VerificationResult VerifyConfirmationCode(Token token, string confirmationCode)
        {
            if (token is null)
            {
                var errors = new Collection<IError>
                {
                    new Error(AccountErrorCodeEnumeration.ConfirmationCodeWasNotGenerated, AccountErrorMessage.ConfirmationCodeWasNotGenerated)
                };
                return VerificationResult.Fail(errors);
            }

            if (!token.Value.Equals(confirmationCode))
            {
                var errors = new Collection<IError>
                {
                    new Error(AccountErrorCodeEnumeration.IncorrectConfirmationCode, AccountErrorMessage.IncorrectConfirmationCode)
                };
                return VerificationResult.Fail(errors);
            }

            if (DateTimeOffset.UtcNow > token.Expires)
            {
                var errors = new Collection<IError>
                {
                    new Error(AccountErrorCodeEnumeration.ConfirmationCodeExpired, AccountErrorMessage.ConfirmationCodeExpired)
                };
                return VerificationResult.Fail(errors);
            }

            return VerificationResult.Ok();
        }
    }
}