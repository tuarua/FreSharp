using System;
using System.Collections;
using System.Collections.Generic;
using FreSharp.Geom;
using TuaRua.FreSharp.Display;
using TuaRua.FreSharp.Geom;
using FREObject = System.IntPtr;

namespace TuaRua.FreSharp {
    /// <summary></summary>
    // ReSharper disable once InconsistentNaming
    // : FreObjectSharp
    public class FREArray  {
        /// <inheritdoc />
        /// <summary>
        /// Creates an Empty C# FreArray.
        /// </summary>
        public FREArray() { }

        /// <summary>
        /// Returns the associated C FREObject of the C# FREObject.
        /// </summary>
        /// <returns></returns>
        public FREObject RawValue { get; set; } = FREObject.Zero;

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
        public FREArray(string className) {
            RawValue = new FREObject().Init(className);
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates a FREArray from a C# int[]
        /// </summary>
        /// <param name="intArray"></param>
        public FREArray(IReadOnlyList<int> intArray) {
            RawValue = new FREObject().Init("Array");
            var count = intArray.Count;
            for (var i = 0; i < count; i++) {
                Set((uint) i, intArray[i]);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates a FREArray from a C# double[]
        /// </summary>
        /// <param name="doubleArray"></param>
        public FREArray(IReadOnlyList<double> doubleArray) {
            RawValue = new FREObject().Init("Array");
            var count = doubleArray.Count;
            for (var i = 0; i < count; i++) {
                Set((uint) i, doubleArray[i]);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates a FREArray from a C# bool[]
        /// </summary>
        /// <param name="boolArray"></param>
        public FREArray(IReadOnlyList<bool> boolArray) {
            RawValue = new FREObject().Init("Array");
            var count = boolArray.Count;
            for (var i = 0; i < count; i++) {
                Set((uint) i, boolArray[i]);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates a FREArray from a C# string[]
        /// </summary>
        /// <param name="stringArray"></param>
        public FREArray(IReadOnlyList<string> stringArray) {
            RawValue = new FREObject().Init("Array");
            var count = stringArray.Count;
            for (var i = 0; i < count; i++) {
                Set((uint) i, stringArray[i]);
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
                if (status == FreResultSharp.Ok) {
                    return ret;
                }

                FreSharpHelper.ThrowFreException(status, "cannot get length of array", FREObject.Zero);
                return 0;
            }
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
                FreSharpHelper.FreObjectSharpFromObject(value).RawValue, ref resultPtr);
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
                        var rectFre = new FreRectangleSharp(itm);
                        al.Add(rectFre.Value);
                        break;
                    case FreObjectTypeSharp.Point:
                        var pointFre = new FrePointSharp(itm);
                        al.Add(pointFre.Value);
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
    }
}