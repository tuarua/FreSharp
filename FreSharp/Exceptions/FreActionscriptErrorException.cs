using System;
using System.Runtime.Serialization;

namespace FreSharp.Exceptions {
    [Serializable]
    internal class FreActionscriptErrorException : Exception {
        public FreActionscriptErrorException() {
        }

        public FreActionscriptErrorException(string message) : base(message) {
        }

        public FreActionscriptErrorException(string message, Exception innerException) : base(message, innerException) {
        }

        protected FreActionscriptErrorException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}