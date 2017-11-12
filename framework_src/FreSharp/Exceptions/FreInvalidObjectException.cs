using System;
using System.Runtime.Serialization;

namespace FreSharp.Exceptions {
    [Serializable]
    internal class FreInvalidObjectException : Exception {
        public FreInvalidObjectException() {
        }

        public FreInvalidObjectException(string message) : base(message) {
        }

        public FreInvalidObjectException(string message, Exception innerException) : base(message, innerException) {
        }

        protected FreInvalidObjectException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}