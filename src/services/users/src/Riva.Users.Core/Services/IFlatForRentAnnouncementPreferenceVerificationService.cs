using System.Collections.Generic;
using Riva.BuildingBlocks.Core.Models;
using Riva.Users.Domain.Users.Entities;

namespace Riva.Users.Core.Services
{
    public interface IFlatForRentAnnouncementPreferenceVerificationService
    {
        VerificationResult VerifyFlatForRentAnnouncementPreferences(IEnumerable<FlatForRentAnnouncementPreference> flatForRentAnnouncementPreferences);
    }
}