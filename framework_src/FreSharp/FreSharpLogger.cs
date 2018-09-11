#region License

// Copyright 2017 Tua Rua Ltd.
// 
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
// 
//  http://www.apache.org/licenses/LICENSE-2.0
// 
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
// 
//  All Rights Reserved. Tua Rua Ltd.

#endregion
using System;
using FREObject = System.IntPtr;

namespace TuaRua.FreSharp {
    /// <summary>
    /// Logger utility for logging any FRE errors which occur
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
        /// Returns the shared instance
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
        /// The FreContextSharp
        /// </summary>
        public FreContextSharp Context { set; get; }

        /// <summary>
        /// Traces the message to the console.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="type">Type of error</param>
        public void Log(string message, FreResultSharp type) {
            Context?.DispatchEvent("TRACE", $"[FreSharp] ‼ {type} {message}");
        }

        /// <summary>
        /// Traces the message to the console.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="type">Type of error</param>
        /// <param name="freException">The Exception</param>
        public void Log(string message, FreResultSharp type, FREObject freException) {
            Context?.DispatchEvent("TRACE", $"[FreSharp] ‼ {type} {message}");
            if (FreObjectTypeSharp.Class != freException.Type()) return;
            try {
                if (!freException.hasOwnProperty("getStackTrace")) return;
                var asStackTrace = freException.Call("getStackTrace");
                if (FreObjectTypeSharp.String == asStackTrace.Type()) {
                    Context?.DispatchEvent("TRACE", $"[FreSharp] ‼ {FreSharpHelper.GetAsString(asStackTrace)}");
                }
            }
            catch (Exception) {
                //ignored
            }
        }
    }
}