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
    public class RoomForRentAnnouncementPreferenceGetterService : IRoomForRentAnnouncementPreferenceGetterService
    {
        public GetResult<RoomForRentAnnouncementPreference> GetByByUserAndId(User user, Guid id)
        {
            var roomForRentAnnouncementPreference = user.RoomForRentAnnouncementPreferences.SingleOrDefault(x => x.Id == id);
            if (roomForRentAnnouncementPreference is null)
            {
                var errors = new Collection<IError>
                {
                    new Error(RoomForRentAnnouncementPreferenceErrorCode.NotFound, RoomForRentAnnouncementPreferenceErrorMessage.NotFound)
                };
                return GetResult<RoomForRentAnnouncementPreference>.Fail(errors);
            }
            return GetResult<RoomForRentAnnouncementPreference>.Ok(roomForRentAnnouncementPreference);
        }
    }
}