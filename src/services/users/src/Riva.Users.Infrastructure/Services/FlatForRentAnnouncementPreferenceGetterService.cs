using System;
using System.Collections.ObjectModel;
using System.Linq;
using Riva.BuildingBlocks.Core.Models;
using Riva.Users.Core.Enumerations;
using Riva.Users.Core.ErrorMessages;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Entities;

namespace Riva.Users.Infrastructure.Services
{
    public class FlatForRentAnnouncementPreferenceGetterService : IFlatForRentAnnouncementPreferenceGetterService
    {
        public GetResult<FlatForRentAnnouncementPreference> GetByByUserAndId(User user, Guid id)
        {
            var flatForRentAnnouncementPreference = user.FlatForRentAnnouncementPreferences.SingleOrDefault(x => x.Id == id);
            if (flatForRentAnnouncementPreference is null)
            {
                var errors = new Collection<IError>
                {
                    new Error(FlatForRentAnnouncementPreferenceErrorCode.NotFound, FlatForRentAnnouncementPreferenceErrorMessage.NotFound)
                };
                return GetResult<FlatForRentAnnouncementPreference>.Fail(errors);
            }
            return GetResult<FlatForRentAnnouncementPreference>.Ok(flatForRentAnnouncementPreference);
        }
    }
}