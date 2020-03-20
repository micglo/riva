using System;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.Announcements.Core.Queries
{
    public class GetRoomForRentAnnouncementInputQuery : IInputQuery
    {
        public Guid RoomForRentAnnouncementId { get; }

        public GetRoomForRentAnnouncementInputQuery(Guid roomForRentAnnouncementId)
        {
            RoomForRentAnnouncementId = roomForRentAnnouncementId;
        }
    }
}