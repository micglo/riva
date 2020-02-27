using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Identity.Core.IntegrationEvents
{
    public class AccountConfirmationRequestedIntegrationEvent : IntegrationEventBase
    {
        public string From { get; }
        public string To { get; }
        public string Subject { get; }
        public string Body { get; }

        public AccountConfirmationRequestedIntegrationEvent(Guid correlationId, string to, string body) : base(correlationId)
        {
            From = "riva@riva.com";
            Subject = "Riva - Confirm Your account";
            To = to;
            Body = body;
        }
    }
}