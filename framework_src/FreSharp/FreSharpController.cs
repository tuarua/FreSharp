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
using System.Linq;
using FREContext = System.IntPtr;
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace TuaRua.FreSharp {
    /// <summary>
    /// 
    /// </summary>
    public abstract class FreSharpController {
        private const string TRACE = "TRACE";

        /// <summary>
        /// Tag used when tracing logs
        /// </summary>
        public abstract string TAG { get; }

        /// <summary>
        /// 
        /// </summary>
        public FreContextSharp Context;

        /// <summary>
        /// Sets the FREContext.
        /// </summary>
        /// <param name="freContext"></param>
        public void SetFreContext(ref FREContext freContext) {
            Context = new FreContextSharp(freContext);
        }

        /// <summary>
        /// Sends StatusEvent to our swc with a level of "TRACE".
        /// </summary>
        /// <param name="values">value to trace to console</param>
        public void Trace(params object[] values) {
            var traceStr = values.Aggregate("", (current, value) => current + value + " ");
            Context?.DispatchEvent(TRACE, traceStr);
        }

        /// <summary>
        /// Sends StatusEvent to our swc with a level of "TRACE".
        /// The output string is prefixed with ⚠️ WARNING:
        /// </summary>
        /// <param name="values">value to trace to console</param>
        public void Warning(params object[] values) {
            var traceStr = values.Aggregate("", (current, value) => current + value + " ");
            Context?.DispatchEvent(TRACE, $"⚠️WARNING: {traceStr}");
        }

        /// <summary>
        /// Sends StatusEvent to our swc with a level of "TRACE".
        /// The output string is prefixed with ℹ️ INFO:
        /// </summary>
        /// <param name="values">value to trace to console.</param>
        public void Info(params object[] values) {
            var traceStr = values.Aggregate("", (current, value) => current + value + " ");
            Context?.DispatchEvent(TRACE, $"ℹ️INFO: {traceStr}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        [Obsolete("SendEvent is deprecated, please use DispatchEvent instead.", true)]
        public void SendEvent(string name, string value) {
            Context?.DispatchEvent(name, value);
        }

        /// <summary>
        /// Sends StatusEvent to our swc with a level of name and code of value.
        /// Replaces DispatchStatusEventAsync.
        /// </summary>
        /// <param name="name">name of event.</param>
        /// <param name="value">value passed with event.</param>
        public void DispatchEvent(string name, string value) {
            Context?.DispatchEvent(name, value);
        }
    }
}