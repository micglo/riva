using System;

namespace Riva.BuildingBlocks.Core.Communications.IntegrationEvents
{
    public abstract class IntegrationEventFailureBase : IntegrationEventBase, IIntegrationEventFailure
    {
        public string Reason { get; }
        public string Code { get; }

        protected IntegrationEventFailureBase(Guid correlationId, string code, string reason) : base(correlationId)
        {
            Code = code;
            Reason = reason;
        }

        protected IntegrationEventFailureBase(Guid correlationId, DateTimeOffset creationDate, string code, string reason) : base(correlationId, creationDate)
        {
            Code = code;
            Reason = reason;
        }
    }
}