using System;
using FREObject = System.IntPtr;

namespace TuaRua.FreSharp {
    /// <summary>
    /// 
    /// </summary>
    public class FreSharpLogger {
        private static volatile FreSharpLogger _instance;

        // Lock synchronization object
        private static readonly object SyncLock = new object();

        // Constructor (protected)
        /// <summary>
        /// 
        /// </summary>
        protected FreSharpLogger() { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static FreSharpLogger GetInstance() {
            if (_instance != null) return _instance;
            lock (SyncLock) {
                if (_instance == null) {
                    _instance = new FreSharpLogger();
                }
            }

            return _instance;
        }

        /// <summary>
        /// 
        /// </summary>
        public FreContextSharp Context { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public void Log(string message, FreResultSharp type) {
            Context?.DispatchEvent("TRACE", $"[FreSharp] {type} {message}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        /// <param name="freException"></param>
        public void Log(string message, FreResultSharp type, FREObject freException) {
            Context?.DispatchEvent("TRACE", $"[FreSharp] {type} {message}");
            if (FreObjectTypeSharp.Class != freException.Type()) return;
            try {
                if (!freException.hasOwnProperty("getStackTrace")) return;
                var asStackTrace = freException.Call("getStackTrace");
                if (FreObjectTypeSharp.String == asStackTrace.Type()) {
                    Context?.DispatchEvent("TRACE", $"[FreSharp] {FreSharpHelper.GetAsString(asStackTrace)}");
                }
            }
            catch (Exception) {
                //ignored
            }
        }
    }
}