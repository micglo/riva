using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.BuildingBlocks.Core.Exceptions
{
    [Serializable]
    public class ResourceNotFoundException : CoreException
    {
        private const string ExceptionMessage = "Resource not found.";

        public ResourceNotFoundException() : base(ExceptionMessage)
        {
        }

        public ResourceNotFoundException(string message) : base(message)
        {
        }

        public ResourceNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ResourceNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        public ResourceNotFoundException(IEnumerable<IError> errors) : base(ExceptionMessage, errors)
        {
        }
    }
}