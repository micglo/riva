using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Messages
{
    public class MessageIntegrationEvent : IntegrationEventBase
    {
        public string From { get; }
        public string To { get; }
        public string Subject { get; }
        public string Body { get; }

        public MessageIntegrationEvent(Guid correlationId, DateTimeOffset creationDate, string from, string to, string subject, string body) : base(correlationId, creationDate)
        {
            From = from;
            To = to;
            Subject = subject;
            Body = body;
        }
    }
}