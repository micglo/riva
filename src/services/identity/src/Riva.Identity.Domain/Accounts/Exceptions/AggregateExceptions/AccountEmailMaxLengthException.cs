using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Exceptions.AggregateExceptions
{
    [Serializable]
    public class AccountEmailMaxLengthException : DomainException
    {
        private const string ErrorMessage = "Email argument max length is invalid.";

        public AccountEmailMaxLengthException() : base(ErrorMessage)
        {
        }

        public AccountEmailMaxLengthException(int emailMaxLength) : base($"Email argument max length is {emailMaxLength}.")
        {
        }

        public AccountEmailMaxLengthException(string message) : base(message)
        {
        }

        public AccountEmailMaxLengthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AccountEmailMaxLengthException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}