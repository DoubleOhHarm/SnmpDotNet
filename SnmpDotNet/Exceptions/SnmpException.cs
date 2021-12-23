using System.Runtime.Serialization;

namespace SnmpDotNet.Exceptions
{
    [Serializable]
    public class SnmpException : Exception
    {
        public SnmpException()
        {
        }

        public SnmpException(string? message) : base(message)
        {
        }

        public SnmpException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected SnmpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
