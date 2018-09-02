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
//  All Rights Reserved. Tua Rua Ltd.

#endregion
using System;
using System.Diagnostics.CodeAnalysis;
using FREObject = System.IntPtr;

namespace TuaRua.FreSharp.Exceptions {
    /// <summary>
    /// 
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class FreArgException {
        /// <summary>
        /// Returns a ANEError as a FREObject
        /// </summary>
        public FREObject RawValue { get; }

        /// <summary>
        /// Creates a FreException from a C# Exception
        /// </summary>
        /// <param name="e"></param>
        public FreArgException(Exception e) {
            RawValue = new FREObject().Init("com.tuarua.fre.ANEError", e.Message, 0, e.GetType().ToString(), e.Source,
                e.StackTrace);
        }

        /// <summary>
        /// Creates a FreException from a C# message string
        /// </summary>
        /// <param name="message"></param>
        public FreArgException(string message) {
            RawValue = new FREObject().Init("com.tuarua.fre.ANEError", $"{message} - incorrect arguments", 0,
                "FreSharp.Exceptions.FreInvalidArgumentException", "", "");
        }
    }
}