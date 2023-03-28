using System;
using System.Runtime.Serialization;

namespace EventHub.Listener.Exceptions
{
    public class UnableToProcessEventException : Exception
    {
        public UnableToProcessEventException()
        {
        }

        public UnableToProcessEventException(string message) : base(message)
        {
        }

        public UnableToProcessEventException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnableToProcessEventException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
