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
using FREObject = System.IntPtr;
// ReSharper disable UnusedMember.Global

namespace TuaRua.FreSharp {
    /// <summary>
    /// Wrapper for FREContext.
    /// </summary>
    public class FreContextSharp {
        private FREContext _rawValue;

        /// <summary>
        /// Creates a FreContextSharp.
        /// </summary>
        /// <param name="freContext">FREContext</param>
        public FreContextSharp(FREContext freContext) {
            _rawValue = freContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        [Obsolete("SendEvent is obsoleted, please use DispatchEvent instead.", true)]
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

        /// <summary>
        /// Call this function to get an extension context’s ActionScript data.
        /// </summary>
        /// <returns></returns>
        public FREObject GetActionScriptData() {
            return _rawValue == FREObject.Zero
                ? FREObject.Zero
                : FreSharpHelper.GetActionScriptData(ref _rawValue);
        }

        /// <summary>
        /// Call this function to set an extension context’s ActionScript data.
        /// </summary>
        /// <param name="value">FREObject to set</param>
        public void SetActionScriptData(FREObject value) {
            FreSharpHelper.SetActionScriptData(ref _rawValue, value);
        }
    }
}