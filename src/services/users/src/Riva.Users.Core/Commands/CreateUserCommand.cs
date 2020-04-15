using System;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.Users.Domain.Users.Enumerations;

namespace Riva.Users.Core.Commands
{
    public class CreateUserCommand : ICommand
    {
        public Guid UserId { get; }
        public string Email { get; }
        public bool ServiceActive { get; }
        public int AnnouncementPreferenceLimit { get; }
        public AnnouncementSendingFrequencyEnumeration AnnouncementSendingFrequency { get; }

        public CreateUserCommand(Guid userId, string email, bool serviceActive, int announcementPreferenceLimit, AnnouncementSendingFrequencyEnumeration announcementSendingFrequency)
        {
            UserId = userId;
            Email = email;
            ServiceActive = serviceActive;
            AnnouncementPreferenceLimit = announcementPreferenceLimit;
            AnnouncementSendingFrequency = announcementSendingFrequency;
        }
    }
}