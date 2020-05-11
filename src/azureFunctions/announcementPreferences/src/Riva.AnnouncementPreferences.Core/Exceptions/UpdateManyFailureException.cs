using System;
using System.Runtime.Serialization;

namespace Riva.AnnouncementPreferences.Core.Exceptions
{
    [Serializable]
    public class UpdateManyFailureException : Exception
    {
        private const string ExceptionMessage = "Update failed.";

        public UpdateManyFailureException() : base(ExceptionMessage)
        {
        }

        public UpdateManyFailureException(string message) : base(message)
        {
        }

        public UpdateManyFailureException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UpdateManyFailureException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}