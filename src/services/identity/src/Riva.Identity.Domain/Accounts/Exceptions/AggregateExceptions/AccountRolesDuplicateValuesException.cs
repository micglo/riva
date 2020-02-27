using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Exceptions.AggregateExceptions
{
    [Serializable]
    public class AccountRolesDuplicateValuesException : DomainException
    {
        private const string ErrorMessage = "Roles argument contains duplicate values.";

        public AccountRolesDuplicateValuesException() : base(ErrorMessage)
        {
        }

        public AccountRolesDuplicateValuesException(string message) : base(message)
        {
        }

        public AccountRolesDuplicateValuesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AccountRolesDuplicateValuesException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}