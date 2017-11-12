using System;
using System.Runtime.Serialization;

namespace FreSharp.Exceptions {
    [Serializable]
    internal class FreReadOnlyException : Exception {
        public FreReadOnlyException() {
        }

        public FreReadOnlyException(string message) : base(message) {
        }

        public FreReadOnlyException(string message, Exception innerException) : base(message, innerException) {
        }

        protected FreReadOnlyException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}