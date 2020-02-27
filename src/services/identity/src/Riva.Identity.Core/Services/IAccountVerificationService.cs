using System.Threading.Tasks;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Entities;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.Identity.Core.Services
{
    public interface IAccountVerificationService
    {
        Task<VerificationResult> VerifyEmailIsNotTakenAsync(string email);
        VerificationResult VerifyAccountIsNotConfirmed(bool confirmed);
        VerificationResult VerifyAccountIsConfirmed(bool confirmed);
        VerificationResult VerifyPassword(string passwordHash, string password);
        VerificationResult VerifyPasswordIsSet(string passwordHash);
        VerificationResult VerifyPasswordIsNotSet(string passwordHash);
        VerificationResult VerifyAccountCanBeAuthenticated(Account account, string password);
        VerificationResult VerifyConfirmationCode(Token token, string confirmationCode);
    }
}