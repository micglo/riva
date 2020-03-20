using System;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.Announcements.Core.Services
{
    public interface ICityVerificationService
    {
        Task<VerificationResult> VerifyCityExistsAsync(Guid cityId);
    }
}