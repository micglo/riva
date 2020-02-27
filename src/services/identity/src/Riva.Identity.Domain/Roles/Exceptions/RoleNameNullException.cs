using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Roles.Exceptions
{
    [Serializable]
    public class RoleNameNullException : DomainException
    {
        private const string ErrorMessage = "Name argument is required.";

        public RoleNameNullException() : base(ErrorMessage)
        {
        }

        public RoleNameNullException(string message) : base(message)
        {
        }

        public RoleNameNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RoleNameNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}