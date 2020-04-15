using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Users.Core.IntegrationEvents.AnnouncementPreferenceIntegrationEvents
{
    public class AnnouncementPreferencesDeletionCompletedIntegrationEventFailure : IntegrationEventFailureBase
    {
        public Guid UserId { get; }

        public AnnouncementPreferencesDeletionCompletedIntegrationEventFailure(Guid correlationId, DateTimeOffset creationDate,
            string code, string reason, Guid userId) : base(correlationId, creationDate, code, reason)
        {
            UserId = userId;
        }
    }
}