using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.AnnouncementPreferences.Core.IntegrationEvents.AnnouncementPreferenceIntegrationEvents
{
    public class AnnouncementPreferencesUpdateCompletedIntegrationEventFailure : IntegrationEventFailureBase
    {
        public Guid UserId { get; }

        public AnnouncementPreferencesUpdateCompletedIntegrationEventFailure(Guid correlationId, string code, string reason, Guid userId)
            : base(correlationId, code, reason)
        {
            UserId = userId;
        }
    }
}