using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.BuildingBlocks.Core.Exceptions
{
    [Serializable]
    public class ConflictException : CoreException
    {
        private const string ExceptionMessage = "The request could not be completed due to a conflict with the current state of the resource.";

        public ConflictException() : base(ExceptionMessage)
        {
        }

        public ConflictException(string message) : base(message)
        {
        }

        public ConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ConflictException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        public ConflictException(IEnumerable<IError> errors) : base(ExceptionMessage, errors)
        {
        }
    }
}