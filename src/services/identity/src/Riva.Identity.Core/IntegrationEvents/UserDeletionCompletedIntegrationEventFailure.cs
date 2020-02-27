using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Identity.Core.IntegrationEvents
{
    public class UserDeletionCompletedIntegrationEventFailure : IntegrationEventFailureBase
    {
        public Guid UserId { get; }

        public UserDeletionCompletedIntegrationEventFailure(Guid correlationId, DateTimeOffset creationDate, string code,
            string reason, Guid userId) : base(correlationId, creationDate, code, reason)
        {
            UserId = userId;
        }
    }
}