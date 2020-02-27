using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.BuildingBlocks.Core.Exceptions
{
    [Serializable]
    public class PreconditionFailedException : CoreException
    {
        private const string ExceptionMessage = "The condition was not met.";

        public PreconditionFailedException() : base(ExceptionMessage)
        {
        }

        public PreconditionFailedException(string message) : base(message)
        {
        }

        public PreconditionFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PreconditionFailedException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        public PreconditionFailedException(IEnumerable<IError> errors) : base(ExceptionMessage, errors)
        {
        }
    }
}