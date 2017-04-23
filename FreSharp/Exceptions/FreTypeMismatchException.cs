using System;
using System.Runtime.Serialization;

namespace FreSharp.Exceptions {
    [Serializable]
    internal class FreTypeMismatchException : Exception {
        public FreTypeMismatchException() {
        }

        public FreTypeMismatchException(string message) : base(message) {
        }

        public FreTypeMismatchException(string message, Exception innerException) : base(message, innerException) {
        }

        protected FreTypeMismatchException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}