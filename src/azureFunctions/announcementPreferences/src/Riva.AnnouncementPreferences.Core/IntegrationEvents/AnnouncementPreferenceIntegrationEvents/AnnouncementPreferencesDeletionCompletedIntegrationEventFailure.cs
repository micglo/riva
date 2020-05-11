using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.AnnouncementPreferences.Core.IntegrationEvents.AnnouncementPreferenceIntegrationEvents
{
    public class AnnouncementPreferencesDeletionCompletedIntegrationEventFailure : IntegrationEventFailureBase
    {
        public Guid UserId { get; }

        public AnnouncementPreferencesDeletionCompletedIntegrationEventFailure(Guid correlationId, string code, string reason, Guid userId)
            : base(correlationId, code, reason)
        {
            UserId = userId;
        }
    }
}