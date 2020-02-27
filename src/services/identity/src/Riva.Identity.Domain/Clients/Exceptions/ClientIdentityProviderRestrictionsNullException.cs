using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Clients.Exceptions
{
    [Serializable]
    public class ClientIdentityProviderRestrictionsNullException : DomainException
    {
        private const string ErrorMessage = "IdentityProviderRestrictions argument is required.";

        public ClientIdentityProviderRestrictionsNullException() : base(ErrorMessage)
        {
        }

        public ClientIdentityProviderRestrictionsNullException(string message) : base(message)
        {
        }

        public ClientIdentityProviderRestrictionsNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ClientIdentityProviderRestrictionsNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}