using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Clients.Exceptions
{
    [Serializable]
    public class ClientIdentityProviderRestrictionsInvalidValuesException : DomainException
    {
        private const string ErrorMessage = "IdentityProviderRestrictions argument is invalid.";

        public ClientIdentityProviderRestrictionsInvalidValuesException() : base(ErrorMessage)
        {
        }

        public ClientIdentityProviderRestrictionsInvalidValuesException(string message) : base(message)
        {
        }

        public ClientIdentityProviderRestrictionsInvalidValuesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ClientIdentityProviderRestrictionsInvalidValuesException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}