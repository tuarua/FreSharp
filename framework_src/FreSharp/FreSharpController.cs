﻿#region License

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
//  Additional Terms
//  No part, or derivative of this Air Native Extensions's code is permitted 
//  to be sold as the basis of a commercially packaged Air Native Extension which 
//  undertakes the same purpose as this software. That is, a WebView for Windows, 
//  OSX and/or iOS and/or Android.
//  All Rights Reserved. Tua Rua Ltd.

#endregion

using System;
using System.Linq;
using FREContext = System.IntPtr;

namespace TuaRua.FreSharp {
    /// <summary>
    /// 
    /// </summary>
    public abstract class FreSharpController {
        /// <summary>
        /// 
        /// </summary>
        public FreContextSharp Context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="freContext"></param>
        public void SetFreContext(ref FREContext freContext) {
            Context = new FreContextSharp(freContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        public void Trace(params object[] values) {
            var traceStr = values.Aggregate("", (current, value) => current + value + " ");
            Context?.DispatchEvent("TRACE", traceStr);
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
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void DispatchEvent(string name, string value)
        {
            Context?.DispatchEvent(name, value);
        }
    }
}