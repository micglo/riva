using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Roles.Exceptions
{
    [Serializable]
    public class RoleNameMaxLengthException : DomainException
    {
        private const string ErrorMessage = "Name argument max length is invalid.";

        public RoleNameMaxLengthException() : base(ErrorMessage)
        {
        }

        public RoleNameMaxLengthException(int nameMaxLength) : base($"Name argument max length is {nameMaxLength}.")
        {
        }

        public RoleNameMaxLengthException(string message) : base(message)
        {
        }

        public RoleNameMaxLengthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RoleNameMaxLengthException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}