using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Users.Core.IntegrationEvents.UserIntegrationEvents
{
    public class UserDeletionCompletedIntegrationEventFailure : IntegrationEventFailureBase
    {
        public Guid UserId { get; }

        public UserDeletionCompletedIntegrationEventFailure(Guid correlationId, string code, string reason, Guid userId) : base(correlationId, code, reason)
        {
            UserId = userId;
        }
    }
}