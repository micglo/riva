using System;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.Announcements.Core.Commands
{
    public class DeleteRoomForRentAnnouncementCommand : ICommand
    {
        public Guid FlatForRentAnnouncementId { get; }

        public DeleteRoomForRentAnnouncementCommand(Guid flatForRentAnnouncementId)
        {
            FlatForRentAnnouncementId = flatForRentAnnouncementId;
        }
    }
}