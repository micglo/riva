using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Identity.Core.IntegrationEvents
{
    public class PasswordResetRequestedIntegrationEvent : IntegrationEventBase
    {
        public string From { get; }
        public string To { get; }
        public string Subject { get; }
        public string Body { get; }

        public PasswordResetRequestedIntegrationEvent(Guid correlationId, string to, string body) : base(correlationId)
        {
            From = "riva@riva.com";
            Subject = "Riva - Reset Your password";
            To = to;
            Body = body;
        }
    }
}