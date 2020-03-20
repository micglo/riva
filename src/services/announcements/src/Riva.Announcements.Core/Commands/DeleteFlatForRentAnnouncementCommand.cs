using System;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.Announcements.Core.Commands
{
    public class DeleteFlatForRentAnnouncementCommand : ICommand
    {
        public Guid FlatForRentAnnouncementId { get; }

        public DeleteFlatForRentAnnouncementCommand(Guid flatForRentAnnouncementId)
        {
            FlatForRentAnnouncementId = flatForRentAnnouncementId;
        }
    }
}