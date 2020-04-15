using System;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.Users.Core.Services
{
    public interface IAccountVerificationService
    {
        Task<VerificationResult> VerifyAccountExistsAsync(Guid accountId, string email);
    }
}