using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.BuildingBlocks.Core.Exceptions
{
    [Serializable]
    public class UnprocessableException : CoreException
    {
        public UnprocessableException()
        {
        }

        public UnprocessableException(string message) : base(message)
        {
        }

        public UnprocessableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UnprocessableException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        public UnprocessableException(string message, IEnumerable<IError> errors) : base(message, errors)
        {
        }
    }
}