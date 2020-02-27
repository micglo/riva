using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.BuildingBlocks.Core.Exceptions
{
    [Serializable]
    public abstract class CoreException : Exception
    {
        public IReadOnlyCollection<IError> Errors { get; }

        protected CoreException()
        {
            Errors = new List<IError>();
        }

        protected CoreException(string message) : base(message)
        {
            Errors = new List<IError>();
        }

        protected CoreException(string message, Exception innerException) : base(message, innerException)
        {
           Errors = new List<IError>();
        }

        protected CoreException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
            Errors = new List<IError>();
        }

        protected CoreException(string message, IEnumerable<IError> errors) : base(message)
        {
            Errors = errors.ToList().AsReadOnly();
        }
    }
}