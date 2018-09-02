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
using System.Runtime.InteropServices;
using FRESharpCore;
using FREObject = System.IntPtr;
namespace TuaRua.FreSharp {
    /// <summary>
    /// FreByteArraySharp wraps a C FREByteArray with helper methods.
    /// </summary>
    public class FreByteArraySharp {
        /// <summary>
        /// The number of bytes in the bytes array.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// The Byte Array 
        /// </summary>
        public byte[] Bytes { get; set; }

        private readonly FREByteArrayCLR _byteArray = new FREByteArrayCLR();

        /// <summary>
        /// Returns the associated C FREByteArray of the C# FREByteArray.
        /// </summary>
        /// <returns></returns>
        public FREObject RawValue { get; }

        /// <summary>
        /// Creates a C# FREByteArray from a C FREByteArray.
        /// </summary>
        /// <param name="freByteArray"></param>
        public FreByteArraySharp(FREObject freByteArray) {
            RawValue = freByteArray;
        }

        /// <summary>
        /// Calls FREAcquireByteArray on the C FREByteArray.
        /// </summary>
        public void Acquire() {
            FreSharpHelper.Core.acquireByteArrayData(RawValue, _byteArray);
            Length = (int) _byteArray.length;
            Bytes = new byte[Length];
            Marshal.Copy(_byteArray.bytes, Bytes, 0, Length);
        }

        /// <summary>
        /// Calls FREReleaseByteArray on the C FREByteArray.
        /// </summary>
        public void Release() {
            FreSharpHelper.Core.releaseByteArrayData(RawValue);
        }
    }
}