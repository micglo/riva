using System;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.Users.Core.Queries
{
    public class GetRoomForRentAnnouncementPreferenceInputQuery : IInputQuery
    {
        public Guid UserId { get; }
        public Guid RoomForRentAnnouncementPreferenceId { get; }

        public GetRoomForRentAnnouncementPreferenceInputQuery(Guid userId, Guid roomForRentAnnouncementPreferenceId)
        {
            UserId = userId;
            RoomForRentAnnouncementPreferenceId = roomForRentAnnouncementPreferenceId;
        }
    }
}