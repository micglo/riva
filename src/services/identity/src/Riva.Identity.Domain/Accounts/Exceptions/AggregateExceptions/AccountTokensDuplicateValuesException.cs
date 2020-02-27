using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Exceptions.AggregateExceptions
{
    [Serializable]
    public class AccountTokensDuplicateValuesException : DomainException
    {
        private const string ErrorMessage = "Tokens argument contains duplicate values.";

        public AccountTokensDuplicateValuesException() : base(ErrorMessage)
        {
        }

        public AccountTokensDuplicateValuesException(string message) : base(message)
        {
        }

        public AccountTokensDuplicateValuesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AccountTokensDuplicateValuesException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}