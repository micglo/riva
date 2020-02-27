using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Exceptions.AggregateExceptions
{
    [Serializable]
    public class AccountCorrelationIdNullException : DomainException
    {
        private const string ErrorMessage = "CorrelationId argument is required.";

        public AccountCorrelationIdNullException() : base(ErrorMessage)
        {
        }

        public AccountCorrelationIdNullException(string message) : base(message)
        {
        }

        public AccountCorrelationIdNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AccountCorrelationIdNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}