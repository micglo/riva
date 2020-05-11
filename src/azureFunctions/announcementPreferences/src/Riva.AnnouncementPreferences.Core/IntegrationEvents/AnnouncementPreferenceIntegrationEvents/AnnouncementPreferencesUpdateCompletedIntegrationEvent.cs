using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.AnnouncementPreferences.Core.IntegrationEvents.AnnouncementPreferenceIntegrationEvents
{
    public class AnnouncementPreferencesUpdateCompletedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }

        public AnnouncementPreferencesUpdateCompletedIntegrationEvent(Guid correlationId, Guid userId) : base(correlationId)
        {
            UserId = userId;
        }
    }
}