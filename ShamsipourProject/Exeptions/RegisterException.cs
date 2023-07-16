using System.Runtime.Serialization;

namespace UniApiProject.Exeptions
{
    [Serializable]
    internal class RegisterException : Exception
    {
        public RegisterException()
        {
        }

        public RegisterException(string? message) : base(message)
        {
        }

        public RegisterException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RegisterException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}