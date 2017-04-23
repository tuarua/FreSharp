using System;
using System.Runtime.Serialization;

namespace FreSharp.Exceptions {
    [Serializable]
    internal class FreInsufficientMemoryException : Exception {
        public FreInsufficientMemoryException() {
        }

        public FreInsufficientMemoryException(string message) : base(message) {
        }

        public FreInsufficientMemoryException(string message, Exception innerException) : base(message, innerException) {
        }

        protected FreInsufficientMemoryException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}