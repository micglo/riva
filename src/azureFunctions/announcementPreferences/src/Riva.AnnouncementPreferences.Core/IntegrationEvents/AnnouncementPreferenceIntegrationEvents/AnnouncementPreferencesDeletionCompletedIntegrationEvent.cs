using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.AnnouncementPreferences.Core.IntegrationEvents.AnnouncementPreferenceIntegrationEvents
{
    public class AnnouncementPreferencesDeletionCompletedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }

        public AnnouncementPreferencesDeletionCompletedIntegrationEvent(Guid correlationId, Guid userId) : base(correlationId)
        {
            UserId = userId;
        }
    }
}