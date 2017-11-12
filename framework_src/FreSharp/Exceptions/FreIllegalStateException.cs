using System;
using System.Runtime.Serialization;

namespace FreSharp.Exceptions {
    [Serializable]
    internal class FreIllegalStateException : Exception {
        public FreIllegalStateException() {
        }

        public FreIllegalStateException(string message) : base(message) {
        }

        public FreIllegalStateException(string message, Exception innerException) : base(message, innerException) {
        }

        protected FreIllegalStateException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}