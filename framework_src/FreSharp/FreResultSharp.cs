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
// ReSharper disable UnusedMember.Global
namespace TuaRua.FreSharp {
    /// <summary>C# implementation of FREResult.</summary>
    public enum FreResultSharp {
        /// <summary>The function succeeded. </summary>
        Ok = 0,

        /// <summary>
        /// The name of a class, property, or method passed as
        /// a parameter does not match an ActionScript class name,
        /// property, or method.
        /// </summary>
        NoSuchName = 1,

        /// <summary>
        /// An FREObject parameter is invalid. For examples of invalid
        /// FREObject variables, see FREObject validity .
        /// </summary>
        FreInvalidObject = 2,

        /// <summary>
        /// An FREObject parameter does not represent an object of the
        /// ActionScript class expected by the called function. </summary>
        FreTypeMismatch = 3,

        /// <summary>
        /// An ActionScript error occurred, and an exception was thrown.
        /// The C API functions that can result in this error allow you to specify
        /// an FREObject to receive information about the exception.
        /// </summary>
        FreActionscriptError = 4,

        /// <summary>A pointer parameter is NULL .</summary>
        FreInvalidArgument = 5,

        /// <summary>The function attempted to modify a read-only property of an
        /// ActionScript object. </summary>
        FreReadOnly = 6,

        /// <summary>
        /// The method was called from a thread other than the one on which the runtime
        /// has an outstanding call to a native extension function.
        /// </summary>
        FreWrongThread = 7,

        /// <summary>
        /// A call was made to a native extensions C API function when the extension
        /// context was in an illegal state for that call. This return value occurs
        /// in the following situation. The context has acquired access to an
        /// ActionScript BitmapData or ByteArray class object. With one exception,
        /// the context can call no other C API functions until it releases the
        /// BitmapData or ByteArray object. The one exception is that the context
        /// can call FREInvalidateBitmapDataRect() after calling
        /// FREAcquireBitmapData() or FREAcquireBitmapData2() .
        /// </summary>
        FreIllegalState = 8,

        /// <summary>
        /// The runtime could not allocate enough memory to change the size of an
        /// Array or Vector object.
        /// </summary>
        FreInsufficientMemory = 9,
    }
}