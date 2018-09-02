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
using FREContext = System.IntPtr;

namespace TuaRua.FreSharp {
    /// <summary>
    /// Wrapper for FREContext.
    /// </summary>
    public class FreContextSharp {
        private FREContext _rawValue;

        /// <summary>
        /// Creates a C# FREObject from a C FREObject.
        /// </summary>
        /// <param name="freContext"></param>
        public FreContextSharp(FREContext freContext) {
            _rawValue = freContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        [Obsolete("SendEvent is deprecated, please use DispatchEvent instead.", true)]
        public void SendEvent(string name, string value) {
            FreSharpHelper.DispatchEvent(ref _rawValue, name, value);
        }

        /// <summary>
        /// Dispatches an event. Mimics FREDispatchStatusEventAsync.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void DispatchEvent(string name, string value) {
            FreSharpHelper.DispatchEvent(ref _rawValue, name, value);
        }


    }
}