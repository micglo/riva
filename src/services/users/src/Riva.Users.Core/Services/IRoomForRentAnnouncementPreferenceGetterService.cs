using System;
using Riva.BuildingBlocks.Core.Models;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Entities;

namespace Riva.Users.Core.Services
{
    public interface IRoomForRentAnnouncementPreferenceGetterService
    {
        GetResult<RoomForRentAnnouncementPreference> GetByByUserAndId(User user,
            Guid id);
    }
}