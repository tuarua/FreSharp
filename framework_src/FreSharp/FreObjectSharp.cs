/* Copyright 2017 Tua Rua Ltd.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.*/

using System;
using FREObject = System.IntPtr;

namespace TuaRua.FreSharp {
    /// <summary>
    /// FreObjectSharp wraps a C FREObject with helper methods.
    /// </summary>
    public class FreObjectSharp {
        /// <summary>
        /// Returns the associated C FREObject of the C# FREObject.
        /// </summary>
        /// <returns></returns>
        public FREObject RawValue { get; set; } = FREObject.Zero;

        /// <summary>
        /// Returns the C# FREObject as an object.
        /// </summary>
        public object Value => FreSharpHelper.GetAsObject(RawValue);

        /// <summary>
        /// Creates an empty C# FREObject
        /// </summary>
        public FreObjectSharp() { }

        /// <summary>
        /// Creates a C# FREObject from a C FREObject
        /// </summary>
        /// <param name="freObject"></param>
        public FreObjectSharp(FREObject freObject) {
            RawValue = freObject;
        }

        /// <summary>
        /// Creates a C# FREObject from a string
        /// </summary>
        /// <param name="value"></param>
        public FreObjectSharp(string value) {
            uint resultPtr = 0;
            RawValue = FreSharpHelper.Core.getFREObject(value, ref resultPtr);
        }

        /// <summary>
        /// Creates a C# FREObject from a bool
        /// </summary>
        /// <param name="value"></param>
        public FreObjectSharp(bool value) {
            uint resultPtr = 0;
            RawValue = FreSharpHelper.Core.getFREObject(value, ref resultPtr);
        }

        /// <summary>
        /// Creates a C# FREObject from a double
        /// </summary>
        /// <param name="value"></param>
        public FreObjectSharp(double value) {
            uint resultPtr = 0;
            RawValue = FreSharpHelper.Core.getFREObject(value, ref resultPtr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public FreObjectSharp(float value)
        {
            uint resultPtr = 0;
            RawValue = FreSharpHelper.Core.getFREObject(value, ref resultPtr);
        }

        /// <summary>
        /// Creates a C# FREObject from an int
        /// </summary>
        /// <param name="value"></param>
        public FreObjectSharp(int value) {
            uint resultPtr = 0;
            RawValue = FreSharpHelper.Core.getFREObject(value, ref resultPtr);
        }

        /// <summary>
        /// Creates a C# FREObject from a uint
        /// </summary>
        /// <param name="value"></param>
        public FreObjectSharp(uint value) {
            uint resultPtr = 0;
            RawValue = FreSharpHelper.Core.getFREObject(value, ref resultPtr);
        }

        /// <summary>
        /// Creates a C# FREObject from a DateTime
        /// </summary>
        /// <param name="value"></param>
        public FreObjectSharp(DateTime value) {
            RawValue = new FREObject().Init("Date", Convert.ToDouble(new DateTimeOffset(value).ToUnixTimeMilliseconds()));
        }
    }
}