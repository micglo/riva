using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Exceptions.AggregateExceptions
{
    [Serializable]
    public class AccountRoleNullException : DomainException
    {
        private const string ErrorMessage = "Role argument is required.";

        public AccountRoleNullException() : base(ErrorMessage)
        {
        }

        public AccountRoleNullException(string message) : base(message)
        {
        }

        public AccountRoleNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AccountRoleNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}