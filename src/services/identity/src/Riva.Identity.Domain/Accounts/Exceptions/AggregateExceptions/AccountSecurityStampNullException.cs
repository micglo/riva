using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Exceptions.AggregateExceptions
{
    [Serializable]
    public class AccountSecurityStampNullException : DomainException
    {
        private const string ErrorMessage = "SecurityStamp argument is required.";

        public AccountSecurityStampNullException() : base(ErrorMessage)
        {
        }

        public AccountSecurityStampNullException(string message) : base(message)
        {
        }

        public AccountSecurityStampNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AccountSecurityStampNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}