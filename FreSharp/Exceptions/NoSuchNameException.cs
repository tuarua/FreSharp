using System;
using System.Runtime.Serialization;

namespace FreSharp.Exceptions {
    [Serializable]
    internal class NoSuchNameException : Exception {
        public NoSuchNameException() {
        }

        public NoSuchNameException(string message) : base(message) {
        }

        public NoSuchNameException(string message, Exception innerException) : base(message, innerException) {
        }

        protected NoSuchNameException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}