using System;
using System.Runtime.Serialization;

namespace Riva.AnnouncementPreferences.Core.Exceptions
{
    [Serializable]
    public class DeleteManyFailureException : Exception
    {
        private const string ExceptionMessage = "Delete failed.";

        public DeleteManyFailureException() : base(ExceptionMessage)
        {
        }

        public DeleteManyFailureException(string message) : base(message)
        {
        }

        public DeleteManyFailureException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DeleteManyFailureException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}