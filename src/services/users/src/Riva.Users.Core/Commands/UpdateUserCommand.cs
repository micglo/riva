using System;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.Users.Core.Models;
using Riva.Users.Domain.Users.Enumerations;

namespace Riva.Users.Core.Commands
{
    public class UpdateUserCommand : ICommand
    {
        public Guid UserId { get; }
        public bool ServiceActive { get; }
        public int AnnouncementPreferenceLimit { get; }
        public AnnouncementSendingFrequencyEnumeration AnnouncementSendingFrequency { get; }
        public Guid CorrelationId { get; }
        public PictureDto Picture { get; }

        public UpdateUserCommand(Guid userId, bool serviceActive, int announcementPreferenceLimit, 
            AnnouncementSendingFrequencyEnumeration announcementSendingFrequency, PictureDto picture)
        {
            UserId = userId;
            ServiceActive = serviceActive;
            AnnouncementPreferenceLimit = announcementPreferenceLimit;
            AnnouncementSendingFrequency = announcementSendingFrequency;
            Picture = picture;
            CorrelationId = Guid.NewGuid();
        }
    }
}