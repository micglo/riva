using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Exceptions.AggregateExceptions
{
    [Serializable]
    public class AccountRolesNullException : DomainException
    {
        private const string ErrorMessage = "Roles argument is required.";

        public AccountRolesNullException() : base(ErrorMessage)
        {
        }

        public AccountRolesNullException(string message) : base(message)
        {
        }

        public AccountRolesNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AccountRolesNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}