using System;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.Users.Core.Commands
{
    public class DeleteFlatForRentAnnouncementPreferenceCommand : ICommand
    {
        public Guid FlatForRentAnnouncementPreferenceId { get; }
        public Guid UserId { get; }
        public Guid CorrelationId { get; }

        public DeleteFlatForRentAnnouncementPreferenceCommand(Guid flatForRentAnnouncementPreferenceId, Guid userId)
        {
            FlatForRentAnnouncementPreferenceId = flatForRentAnnouncementPreferenceId;
            UserId = userId;
            CorrelationId = Guid.NewGuid();
        }
    }
}