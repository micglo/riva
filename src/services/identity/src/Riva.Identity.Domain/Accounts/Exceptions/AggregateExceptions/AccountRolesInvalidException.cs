using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Exceptions.AggregateExceptions
{
    [Serializable]
    public class AccountRolesInvalidException : DomainException
    {
        private const string ErrorMessage = "Roles argument is invalid.";

        public AccountRolesInvalidException() : base(ErrorMessage)
        {
        }

        public AccountRolesInvalidException(string message) : base(message)
        {
        }

        public AccountRolesInvalidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AccountRolesInvalidException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}