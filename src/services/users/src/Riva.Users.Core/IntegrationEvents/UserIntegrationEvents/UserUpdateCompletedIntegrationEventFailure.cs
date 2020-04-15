using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Users.Core.IntegrationEvents.UserIntegrationEvents
{
    public class UserUpdateCompletedIntegrationEventFailure : IntegrationEventFailureBase
    {
        public Guid UserId { get; }

        public UserUpdateCompletedIntegrationEventFailure(Guid correlationId, string code, string reason, Guid userId) : base(correlationId, code, reason)
        {
            UserId = userId;
        }
    }
}