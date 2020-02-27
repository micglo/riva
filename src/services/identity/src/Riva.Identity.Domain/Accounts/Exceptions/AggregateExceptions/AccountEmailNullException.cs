using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Exceptions.AggregateExceptions
{
    [Serializable]
    public class AccountEmailNullException : DomainException
    {
        private const string ErrorMessage = "Email argument is required.";

        public AccountEmailNullException() : base(ErrorMessage)
        {
        }

        public AccountEmailNullException(string message) : base(message)
        {
        }

        public AccountEmailNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AccountEmailNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}