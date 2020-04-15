using System;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.Users.Core.Commands
{
    public class DeleteRoomForRentAnnouncementPreferenceCommand : ICommand
    {
        public Guid RoomForRentAnnouncementPreferenceId { get; }
        public Guid UserId { get; }
        public Guid CorrelationId { get; }

        public DeleteRoomForRentAnnouncementPreferenceCommand(Guid roomForRentAnnouncementPreferenceId, Guid userId)
        {
            RoomForRentAnnouncementPreferenceId = roomForRentAnnouncementPreferenceId;
            UserId = userId;
            CorrelationId = Guid.NewGuid();
        }
    }
}