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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TuaRua.FreSharp.Display;
using TuaRua.FreSharp.Geom;
using FREObject = System.IntPtr;
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable InconsistentNaming

namespace TuaRua.FreSharp {
    /// <summary>
    /// 
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class FREArray : IEnumerable<FREObject> {
        private static FreSharpLogger Logger => FreSharpLogger.GetInstance();

        /// <summary>
        /// Returns the associated C FREObject of the C# FREObject.
        /// </summary>
        /// <returns></returns>
        public FREObject RawValue { get; }

        /// <inheritdoc />
        /// <summary>
        /// Creates a C# FreArray from a C FREObject.
        /// </summary>
        /// <param name="freObject"></param>
        public FREArray(FREObject freObject) {
            RawValue = freObject;
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates a C# FreArray with a given class name.
        /// </summary>
        /// <param name="className"></param>
        [Obsolete("FREArray is obsoleted, please use FREArray(className, length, fixedSize) instead.", true)]
        public FREArray(string className) {
            RawValue = new FREObject().Init(className);
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates a C# FREArray with a given class name. Do not specify the &lt;Vector. prefix.
        /// </summary>
        /// <param name="className">name of AS3 class to create.</param>
        /// <param name="length">number of elements in the array.</param>
        /// <param name="fixedSize">whether the array is fixed.</param>
        public FREArray(string className, int length = 0, bool fixedSize = false) {
            RawValue = new FREObject().Init("Vector.<" + className + ">", length, fixedSize);
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates a FREArray from a C# int[].
        /// </summary>
        /// <param name="intArray"></param>
        public FREArray(IEnumerable<int> intArray) {
            RawValue = new FREObject().Init("Vector.<int>");
            foreach (var v in intArray) {
                Push(v);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates a FREArray from a C# uint[].
        /// </summary>
        /// <param name="uintArray"></param>
        public FREArray(IEnumerable<uint> uintArray) {
            RawValue = new FREObject().Init("Vector.<uint>");
            foreach (var v in uintArray) {
                Push(v);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates a FREArray from a C# DateTime[].
        /// </summary>
        /// <param name="dateArray"></param>
        public FREArray(IEnumerable<DateTime> dateArray) {
            RawValue = new FREObject().Init("Vector.<Date>");
            foreach (var v in dateArray) {
                Push(v);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates a FREArray from a C# double[].
        /// </summary>
        /// <param name="doubleArray"></param>
        public FREArray(IEnumerable<double> doubleArray) {
            RawValue = new FREObject().Init("Vector.<Number>");
            foreach (var v in doubleArray) {
                Push(v);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates a FREArray from a C# bool[].
        /// </summary>
        /// <param name="boolArray"></param>
        public FREArray(IEnumerable<bool> boolArray) {
            RawValue = new FREObject().Init("Vector.<Boolean>");
            foreach (var v in boolArray) {
                Push(v);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates a FREArray from a C# string[].
        /// </summary>
        /// <param name="stringArray"></param>
        public FREArray(IEnumerable<string> stringArray) {
            RawValue = new FREObject().Init("Vector.<String>");
            foreach (var v in stringArray) {
                Push(v);
            }
        }

        /// <summary>
        /// Returns the length of the C# FreArray.
        /// </summary>
        public uint Length {
            get {
                uint resultPtr = 0;
                var ret = FreSharpHelper.Core.getArrayLength(RawValue, ref resultPtr);
                var status = (FreResultSharp) resultPtr;
                if (status == FreResultSharp.Ok) return ret;
                Logger.Log("cannot get length of array", status);
                return 0;
            }
        }

        /// <summary>
        /// A Boolean value indicating whether the FREArray is empty.
        /// </summary>
        public bool IsEmpty => Length == 0;

        /// <summary>
        /// Adds one or more elements to the end of an array and returns the new length of the array.
        /// </summary>
        /// <param name="args">One or more values to append to the array.</param>
        public void Push(params object[] args) {
            uint resultPtr = 0;
            var argsArr = new ArrayList();
            if (args != null) {
                for (var i = 0; i < args.Length; i++) {
                    argsArr.Add(args.ElementAt(i));
                }
            }
            var ret = FreSharpHelper.Core.callMethod(RawValue, "push", FreSharpHelper.ArgsToArgv(argsArr),
                FreSharpHelper.GetArgsC(argsArr), ref resultPtr);
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) return;
            Logger.Log("cannot call method push on FREArray", status, ret);
        }

        /// <summary>
        /// Insert a single element into the FREArray.
        /// </summary>
        /// <param name="at">An uint that specifies the position in the Vector where the element is to be inserted.
        /// You can use a negative Int to specify a position relative to the end of the
        /// FREArray(for example, -1 for the last element of the FREArray)</param>
        /// <param name="value"></param>
        public void Insert(FREObject value, int at) {
            RawValue.Call("insertAt", at, value);
        }

        /// <summary>
        /// Remove a single element from the Vector. This method modifies the FREArray without making a copy.
        /// </summary>
        /// <param name="at">An Int that specifies the index of the element in the FREArray that is to be deleted.
        /// You can use a negative Int to specify a position relative to the end of the FREArray
        /// (for example, -1 for the last element of the Vector).</param>
        /// <returns>The element that was removed from the original FREArray</returns>
        public FREObject Remove(int at) {
            return RawValue.Call("removeAt", at);
        }

        /// <summary>
        /// Returns the C# FreObject from the C# FreArray at i.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public FREObject At(uint i) {
            uint resultPtr = 0;
            return FreSharpHelper.Core.getObjectAt(RawValue, i, ref resultPtr);
        }

        /// <summary>
        /// Sets the C# FreObject in the C# FreArray at i.
        /// </summary>
        public void Set(uint index, object value) {
            uint resultPtr = 0;
            FreSharpHelper.Core.setObjectAt(RawValue, index,
                FreSharpHelper.FREObjectFromObject(value), ref resultPtr);
        }

        /// <summary>
        /// Sets the C# FreObject in the C# FreArray at i.
        /// </summary>
        public void Set(uint index, FREObject value) {
            uint resultPtr = 0;
            FreSharpHelper.Core.setObjectAt(RawValue, index, value, ref resultPtr);
        }

        /// <summary>
        /// Returns the FREArray as C# int[].
        /// </summary>
        /// <returns></returns>
        public int[] AsIntArray() {
            var arr = new int[Length];
            var len = Length;
            if (len <= 0) return arr;
            for (uint i = 0; i < len; i++) {
                var itm = At(i);
                if (itm.Type() != FreObjectTypeSharp.Int) {
                    return arr;
                }

                arr[i] = FreSharpHelper.GetAsInt(itm);
            }

            return arr;
        }

        /// <summary>
        /// Returns the FREArray as C# uint[].
        /// </summary>
        /// <returns></returns>
        public uint[] AsUIntArray() {
            var arr = new uint[Length];
            var len = Length;
            if (len <= 0) return arr;
            for (uint i = 0; i < len; i++) {
                var itm = At(i);
                if (itm.Type() != FreObjectTypeSharp.Int) {
                    return arr;
                }

                arr[i] = FreSharpHelper.GetAsUInt(itm);
            }

            return arr;
        }

        /// <summary>
        /// Returns the FREArray as a C# double[].
        /// </summary>
        /// <returns></returns>
        public double[] AsDoubleArray() {
            var arr = new double[Length];
            var len = Length;
            if (len <= 0) return arr;
            for (uint i = 0; i < len; i++) {
                var itm = At(i);
                if (itm.Type() != FreObjectTypeSharp.Number && itm.Type() != FreObjectTypeSharp.Int) {
                    return arr;
                }

                arr[i] = FreSharpHelper.GetAsDouble(itm);
            }

            return arr;
        }

        /// <summary>
        /// Returns the FREArray as a C# string[].
        /// </summary>
        /// <returns></returns>
        public string[] AsStringArray() {
            var arr = new string[Length];
            var len = Length;
            if (len <= 0) return arr;
            for (uint i = 0; i < len; i++) {
                var itm = At(i);
                if (itm.Type() != FreObjectTypeSharp.String) {
                    return arr;
                }

                arr[i] = FreSharpHelper.GetAsString(itm);
            }

            return arr;
        }

        /// <summary>
        /// Returns the FREArray as a C# bool[].
        /// </summary>
        /// <returns></returns>
        public bool[] AsBoolArray() {
            var arr = new bool[Length];
            var len = Length;
            if (len <= 0) return arr;
            for (uint i = 0; i < len; i++) {
                var itm = At(i);
                if (itm.Type() != FreObjectTypeSharp.Boolean) {
                    return arr;
                }

                arr.SetValue(FreSharpHelper.GetAsBool(itm), i);
            }

            return arr;
        }

        /// <summary>
        /// Returns the FREArray as a C# DateTime[].
        /// </summary>
        /// <returns></returns>
        public DateTime[] AsDateArray()
        {
            var arr = new DateTime[Length];
            var len = Length;
            if (len <= 0) return arr;
            for (uint i = 0; i < len; i++) {
                var itm = At(i);
                if (itm.Type() != FreObjectTypeSharp.Date) {
                    return arr;
                }
                arr.SetValue(FreSharpHelper.GetAsDateTime(itm), i);
            }

            return arr;
        }

        /// <summary>
        /// Returns the FREArray as a C# ArrayList.
        /// </summary>
        /// <returns></returns>
        public ArrayList AsArrayList() {
            var al = new ArrayList();
            var len = Length;
            if (len <= 0) return al;
            for (uint i = 0; i < len; i++) {
                var itm = At(i);
                var type = itm.Type();
                switch (type) {
                    case FreObjectTypeSharp.String:
                        al.Add(FreSharpHelper.GetAsString(itm));
                        break;
                    case FreObjectTypeSharp.Bytearray:
                        var ba = new FreByteArraySharp(itm);
                        ba.Acquire();
                        var baTarget = new byte[ba.Length];
                        ba.Bytes.CopyTo(baTarget, 0);
                        al.Add(baTarget);
                        ba.Release();
                        break;
                    case FreObjectTypeSharp.Array:
                    case FreObjectTypeSharp.Vector:
                        var arrFre = new FREArray(itm);
                        al.Add(arrFre.AsArrayList());
                        break;
                    case FreObjectTypeSharp.Bitmapdata:
                        var bmdFre = new FreBitmapDataSharp(itm);
                        al.Add(bmdFre.AsBitmap());
                        break;
                    case FreObjectTypeSharp.Boolean:
                        al.Add(FreSharpHelper.GetAsBool(itm));
                        break;
                    case FreObjectTypeSharp.Null:
                        break;
                    case FreObjectTypeSharp.Int:
                        al.Add(FreSharpHelper.GetAsInt(itm));
                        break;
                    case FreObjectTypeSharp.Object:
                    case FreObjectTypeSharp.Class:
                        al.Add(FreSharpHelper.GetAsDictionary(itm));
                        break;
                    case FreObjectTypeSharp.Number:
                        al.Add(FreSharpHelper.GetAsDouble(itm));
                        break;
                    case FreObjectTypeSharp.Rectangle:
                        al.Add(itm.AsRect());
                        break;
                    case FreObjectTypeSharp.Point:
                        al.Add(itm.AsPoint());
                        break;
                    case FreObjectTypeSharp.Date:
                        al.Add(FreSharpHelper.GetAsDateTime(itm));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return al;
        }

        /// <summary>
        /// Provides bracket access to <see cref="FREArray"/>.
        /// </summary>
        /// <param name="i"></param>
        public FREObject this[uint i] {
            get => At(i);
            set {
                uint resultPtr = 0;
                FreSharpHelper.Core.setObjectAt(RawValue, i, value, ref resultPtr);
            }
        }

        /// <inheritdoc />
        public IEnumerator<FREObject> GetEnumerator() {
            var list = new List<FREObject>((int) Length);
            for (uint i = 0; i < Length; i++) {
                list.Add(At(i));
            }

            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}