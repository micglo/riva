using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.SignalR.IntegrationEvents
{
    public class UserUpdateCompletedIntegrationEventFailure : IntegrationEventFailureBase
    {
        public Guid UserId { get; }

        public UserUpdateCompletedIntegrationEventFailure(Guid correlationId, DateTimeOffset creationDate, 
            string code, string reason, Guid userId) : base(correlationId, creationDate, code, reason)
        {
            UserId = userId;
        }
    }
}