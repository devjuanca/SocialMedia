using System;
using System.Runtime.Serialization;

namespace SocialMedia.UI.Controllers
{
    [Serializable]
    internal class CustonException : Exception
    {
        public CustonException()
        {
        }

        public CustonException(string message) : base(message)
        {
        }

        public CustonException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CustonException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}