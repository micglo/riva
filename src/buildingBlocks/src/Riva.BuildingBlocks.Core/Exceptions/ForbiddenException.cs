using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.BuildingBlocks.Core.Exceptions
{
    [Serializable]
    public class ForbiddenException : CoreException
    {
        private const string ExceptionMessage = "Operation is forbidden.";

        public ForbiddenException() : base(ExceptionMessage)
        {
        }

        public ForbiddenException(string message) : base(message)
        {
        }

        public ForbiddenException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ForbiddenException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        public ForbiddenException(IEnumerable<IError> errors) : base(ExceptionMessage, errors)
        {
        }
    }
}