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
using System.Collections.Generic;
using System.Linq;
using FREObject = System.IntPtr;
using FREContext = System.IntPtr;
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace TuaRua.FreSharp {
    /// <summary>
    /// 
    /// </summary>
    public abstract class FreSharpMainController {
        private const string TRACE = "TRACE";

        /// <summary>
        /// Called by C++ ContextFinalizer.
        /// </summary>
        public abstract void OnFinalize();

        /// <summary>
        /// Returns functions which connect C++ to C#.
        /// </summary>
        public Dictionary<string, Func<FREContext, uint, FREObject[], FREObject>> FunctionsDict;

        /// <summary>
        /// Tag used when tracing logs
        /// </summary>
        public abstract string TAG { get; }

        /// <summary>
        /// 
        /// </summary>
        public FreContextSharp Context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ctx"></param>
        /// <param name="argc"></param>
        /// <param name="argv"></param>
        /// <returns></returns>
        public FREObject CallSharpFunction(string name, ref FREContext ctx, uint argc, FREObject[] argv) {
            return FunctionsDict[name].Invoke(ctx, argc, argv);
        }

        /// <summary>
        /// Sets the FREContext
        /// </summary>
        /// <param name="freContext"></param>
        public void SetFreContext(ref FREContext freContext) {
            Context = new FreContextSharp(freContext);
        }

        /// <summary>
        /// Sends StatusEvent to our swc with a level of "TRACE".
        /// </summary>
        /// <param name="values">value to trace to console.</param>
        public void Trace(params object[] values) {
            var traceStr = values.Aggregate("", (current, value) => current + value + " ");
            Context?.DispatchEvent(TRACE, $"{TAG}: {traceStr}");
        }

        /// <summary>
        /// Sends StatusEvent to our swc with a level of "TRACE".
        /// The output string is prefixed with ⚠️ WARNING:
        /// </summary>
        /// <param name="values">value to trace to console</param>
        public void Warning(params object[] values) {
            var traceStr = values.Aggregate("", (current, value) => current + value + " ");
            Context?.DispatchEvent(TRACE, $"{TAG}: ⚠️WARNING: {traceStr}");
        }

        /// <summary>
        /// Sends StatusEvent to our swc with a level of "TRACE".
        /// The output string is prefixed with ℹ️ INFO:
        /// </summary>
        /// <param name="values">value to trace to console.</param>
        public void Info(params object[] values) {
            var traceStr = values.Aggregate("", (current, value) => current + value + " ");
            Context?.DispatchEvent(TRACE, $"{TAG}: ℹ️INFO: {traceStr}");
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