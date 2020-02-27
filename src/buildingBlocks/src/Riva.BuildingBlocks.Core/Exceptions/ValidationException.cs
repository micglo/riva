using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.BuildingBlocks.Core.Exceptions
{
    [Serializable]
    public class ValidationException : CoreException
    {
        private const string ExceptionMessage = "Validation failed.";

        public ValidationException() : base(ExceptionMessage)
        {
        }

        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ValidationException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        public ValidationException(IEnumerable<IError> errors) : base(ExceptionMessage, errors)
        {
        }
    }
}