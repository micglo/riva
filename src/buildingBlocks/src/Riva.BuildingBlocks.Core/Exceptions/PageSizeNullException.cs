using System;
using System.Runtime.Serialization;

namespace Riva.BuildingBlocks.Core.Exceptions
{
    [Serializable]
    public class PageSizeNullException : CoreException
    {
        private const string ExceptionMessage = "PageSize argument is required.";

        public PageSizeNullException() : base(ExceptionMessage)
        {
        }

        public PageSizeNullException(string message) : base(message)
        {
        }

        public PageSizeNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PageSizeNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}