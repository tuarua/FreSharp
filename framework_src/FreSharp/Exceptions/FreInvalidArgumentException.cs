using System;
using System.Runtime.Serialization;

namespace FreSharp.Exceptions {
    [Serializable]
    internal class FreInvalidArgumentException : Exception {
        public FreInvalidArgumentException() {
        }

        public FreInvalidArgumentException(string message) : base(message) {
        }

        public FreInvalidArgumentException(string message, Exception innerException) : base(message, innerException) {
        }

        protected FreInvalidArgumentException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}