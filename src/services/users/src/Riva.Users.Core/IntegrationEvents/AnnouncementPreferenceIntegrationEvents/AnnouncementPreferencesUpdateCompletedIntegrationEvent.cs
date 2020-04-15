using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Users.Core.IntegrationEvents.AnnouncementPreferenceIntegrationEvents
{
    public class AnnouncementPreferencesUpdateCompletedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }

        public AnnouncementPreferencesUpdateCompletedIntegrationEvent(Guid correlationId, DateTimeOffset creationDate, Guid userId) : base(correlationId, creationDate)
        {
            UserId = userId;
        }
    }
}