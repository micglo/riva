using System;
using System.Runtime.Serialization;

namespace Riva.BuildingBlocks.Core.Exceptions
{
    [Serializable]
    public class PageNullException : CoreException
    {
        private const string ExceptionMessage = "Page argument is required.";

        public PageNullException() : base(ExceptionMessage)
        {
        }

        public PageNullException(string message) : base(message)
        {
        }

        public PageNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PageNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}