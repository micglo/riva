using System;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.Users.Core.Services
{
    public interface IUserVerificationService
    {
        Task<VerificationResult> VerifyUserDoesNotExistAsync(Guid userId);
        VerificationResult VerifyAnnouncementPreferenceLimitCanBeChanged();
        VerificationResult VerifyAnnouncementSendingFrequencyCanBeChanged();
        VerificationResult VerifyAnnouncementPreferenceLimitIsNotExceeded(int announcementPreferenceLimit, int currentAnnouncementPreferencesCount);
    }
}