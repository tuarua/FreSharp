using System;
using System.Runtime.Serialization;

namespace FreSharp.Exceptions {
    [Serializable]
    internal class FreWrongThreadException : Exception {
        public FreWrongThreadException() {
        }

        public FreWrongThreadException(string message) : base(message) {
        }

        public FreWrongThreadException(string message, Exception innerException) : base(message, innerException) {
        }

        protected FreWrongThreadException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}