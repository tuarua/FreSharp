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
using System.Drawing;
using System.Linq;
using TuaRua.FreSharp.Display;
using FREObject = System.IntPtr;
// ReSharper disable UnusedParameter.Global

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace TuaRua.FreSharp {
    /// <summary>
    /// 
    /// </summary>
    public static class FlashRuntimeExtensions {
        private static FreSharpLogger Logger => FreSharpLogger.GetInstance();

        /// <summary>
        /// Converts a C# string to a FREObject
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this string value) => FreSharpHelper.NewObject(value);

        /// <summary>
        /// Converts a FREObject to a C# string
        /// </summary>
        /// <param name="freObject"></param>
        /// <returns></returns>
        public static string AsString(this FREObject freObject) => FreSharpHelper.GetAsString(freObject);

        /// <summary>
        /// Calls toString() on a FREObject
        /// </summary>
        /// <param name="freObject"></param>
        /// <returns></returns>
        public static string toString(this FREObject freObject) {
            if (freObject.Type() == FreObjectTypeSharp.String
                || freObject.Type() == FreObjectTypeSharp.Null) return "";
            return freObject.Call("toString").AsString();
        }

        /// <summary>
        /// Indicates whether an object has a specified property defined.
        /// </summary>
        /// <param name="freObject"></param>
        /// <param name="name">The property of the FREObject. </param>
        /// <returns></returns>
        public static bool hasOwnProperty(this FREObject freObject, string name) =>
            freObject.Call("hasOwnProperty", name).AsBool();

        /// <summary>
        /// Returns the className of the FREObject
        /// </summary>
        /// <param name="freObject"></param>
        /// <returns></returns>
        public static string ClassName(this FREObject freObject) =>
            new FREObject().Init("com.tuarua.fre.ANEUtils").Call("getClassType", freObject).AsString();

        /// <summary>
        /// Converts a FREObject to a DateTime.
        /// </summary>
        /// <param name="freObject"></param>
        /// <returns></returns>
        public static DateTime AsDateTime(this FREObject freObject) => FreSharpHelper.GetAsDateTime(freObject);

        /// <summary>
        /// Converts a C# DateTime to a FREObject.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this DateTime value) => FreSharpHelper.NewObject(value);

        /// <summary>
        /// Converts a FREObject to a C# bool.
        /// </summary>
        /// <param name="freObject"></param>
        /// <returns></returns>
        public static bool AsBool(this FREObject freObject) => FreSharpHelper.GetAsBool(freObject);

        /// <summary>
        /// Converts a C# bool to a FREObject.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this bool value) => FreSharpHelper.NewObject(value);

        /// <summary>
        /// Gets the property of a FREObject.
        /// </summary>
        /// <param name="freObject"></param>
        /// <param name="name">Name of the property</param>
        /// <returns></returns>
        public static FREObject GetProp(this FREObject freObject, string name) {
            return FreSharpHelper.GetProperty(freObject, name);
        }

        /// <summary>
        /// Sets the property of a FREObject.
        /// </summary>
        /// <param name="freObject"></param>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value of the property.</param>
        public static void SetProp(this FREObject freObject, string name, object value) {
            FreSharpHelper.SetProperty(freObject, name, value);
        }

        /// <summary>
        /// Sets the property of a FREObject.
        /// </summary>
        /// <param name="freObject"></param>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value of the property.</param>
        public static void SetProp(this FREObject freObject, string name, FREObject value) {
            FreSharpHelper.SetProperty(freObject, name, value);
        }

        /// <summary>
        /// Returns the type of the FREObject.
        /// </summary>
        /// <param name="freObject"></param>
        /// <returns></returns>
        public static FreObjectTypeSharp Type(this FREObject freObject) => FreSharpHelper.GetType(freObject);

        /// <summary>
        /// Calls a method on a FREObject.
        /// </summary>
        /// <param name="freObject"></param>
        /// <param name="method">The method name.</param>
        /// <param name="args">Arguments to pass to the method.</param>
        /// <returns></returns>
        public static FREObject Call(this FREObject freObject, string method, params object[] args) {
            uint resultPtr = 0;
            var argsArr = new ArrayList();
            if (args != null) {
                for (var i = 0; i < args.Length; i++) {
                    argsArr.Add(args.ElementAt(i));
                }
            }

            var ret = FreSharpHelper.Core.callMethod(freObject, method, FreSharpHelper.ArgsToArgv(argsArr),
                FreSharpHelper.GetArgsC(argsArr), ref resultPtr);

            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return ret;
            Logger.Error($"cannot call method {method} of {freObject.toString()}", status, ret);
            return FREObject.Zero;
        }

        /// <summary>
        /// Calls a method on a FREArray.
        /// </summary>
        /// <param name="freObject"></param>
        /// <param name="methodName">The method name.</param>
        /// <param name="args">Arguments to pass to the method.</param>
        /// <returns></returns>
        public static FREArray Call(this FREObject freObject, string methodName, ArrayList args) {
            uint resultPtr = 0;
            var fre = FreSharpHelper.Core.callMethod(freObject, methodName,
                FreSharpHelper.ArgsToArgv(args), FreSharpHelper.GetArgsC(args), ref resultPtr);
            var ret = new FREArray(fre);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return ret;
            Logger.Error($"cannot call method {methodName} of {freObject.toString()}", status, fre);
            return null;
        }

        /// <summary>
        /// Converts a C# int[] to a FREObject.
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this IEnumerable<int> arr) => new FREArray(arr).RawValue;

        /// <summary>
        /// Converts a C# bool[] to a FREObject.
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this IEnumerable<bool> arr) => new FREArray(arr).RawValue;

        /// <summary>
        /// Converts a C# double[] to a FREObject.
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this IEnumerable<double> arr) => new FREArray(arr).RawValue;

        /// <summary>
        /// Converts a C# string[] to a FREObject.
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this IEnumerable<string> arr) => new FREArray(arr).RawValue;

        /// <summary>
        /// Converts a FREObject to an ArrayList.
        /// </summary>
        /// <param name="freObject"></param>
        /// <returns></returns>
        public static ArrayList ToArrayList(this FREObject freObject) => new FREArray(freObject).AsArrayList();

        /// <summary>
        /// Converts a FREObject to a C# int.
        /// </summary>
        /// <param name="freObject"></param>
        /// <returns></returns>
        public static int AsInt(this FREObject freObject) => FreSharpHelper.GetAsInt(freObject);

        /// <summary>
        /// Converts a FREObject to a C# short.
        /// </summary>
        /// <param name="freObject"></param>
        /// <returns></returns>
        public static short AsShort(this FREObject freObject) => (short) FreSharpHelper.GetAsInt(freObject);

        /// <summary>
        /// Initialise a System.Drawing.Color from a FREObject.
        /// </summary>
        /// <param name="freObject"></param>
        /// <param name="hasAlpha">Set to true when the AS3 uint is in ARGB format.</param>
        /// <returns></returns>
        public static Color AsColor(this FREObject freObject, bool hasAlpha = true) {
            var rgb = freObject.AsUInt();
            if (hasAlpha) {
                return Color.FromArgb(
                    Convert.ToByte((rgb >> 24) & 0xff),
                    Convert.ToByte((rgb >> 16) & 0xff),
                    Convert.ToByte((rgb >> 8) & 0xff),
                    Convert.ToByte(rgb & 0xff));
            }

            return Color.FromArgb(
                Convert.ToByte((rgb >> 16) & 0xff),
                Convert.ToByte((rgb >> 8) & 0xff),
                Convert.ToByte(rgb & 0xff));
        }


        /// <summary>
        /// Converts a C# System.Drawing.Color to a FREObject.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this Color c) => FreSharpHelper.NewObject((uint) ((c.A << 24)
                                                                                              | (c.R << 16)
                                                                                              | (c.G << 8)
                                                                                              | c.B));

        /// <summary>
        /// Converts a FREObject to a C# Dictionary&lt;string, object&gt;.
        /// </summary>
        /// <param name="freObject"></param>
        /// <returns></returns>
        public static Dictionary<string, object> AsDictionary(this FREObject freObject) =>
            FreSharpHelper.GetAsDictionary(freObject);

        /// <summary>
        /// Converts a C# int to a FREObject.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this int value) => FreSharpHelper.NewObject(value);

        /// <summary>
        /// Converts a FREObject to a C# uint.
        /// </summary>
        /// <param name="freObject"></param>
        /// <returns></returns>
        public static uint AsUInt(this FREObject freObject) => FreSharpHelper.GetAsUInt(freObject);

        /// <summary>
        /// Converts a C# uint to a FREObject.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this uint value) => FreSharpHelper.NewObject(value);

        /// <summary>
        /// Converts a FREObject to a C# double.
        /// </summary>
        /// <param name="freObject"></param>
        /// <returns></returns>
        public static double AsDouble(this FREObject freObject) => FreSharpHelper.GetAsDouble(freObject);

        /// <summary>
        /// Converts a C# double to a FREObject.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this double value) => FreSharpHelper.NewObject(value);

        /// <summary>
        /// Converts a C# float to a FREObject.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this float value) => FreSharpHelper.NewObject(value);

        /// <summary>
        /// Converts a C# short to a FREObject.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this short value) => FreSharpHelper.NewObject(value);

        /// <summary>
        /// Converts a C# long to a FREObject.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this long value) => FreSharpHelper.NewObject((int) value);

        /// <summary>
        /// Converts a FREObject to a C# Bitmap.
        /// </summary>
        /// <param name="freObject"></param>
        /// <returns></returns>
        public static Bitmap AsBitmap(this FREObject freObject) => new FreBitmapDataSharp(freObject).AsBitmap();

        /// <summary>
        /// Converts a C# Bitmap to a FREObject.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this Bitmap bitmap) => new FreBitmapDataSharp(bitmap).RawValue;

        /// <summary>
        /// Creates a FREObject.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="className">Name of the Class.</param>
        /// <param name="args">Arguments to pass to the Class.</param>
        /// <returns></returns>
        public static FREObject Init(this FREObject value, string className, params object[] args) {
            uint resultPtr = 0;
            var argsArr = new ArrayList();
            if (args != null) {
                for (var i = 0; i < args.Length; i++) {
                    argsArr.Add(args.ElementAt(i));
                }
            }

            var ret = FreSharpHelper.Core.getFREObject(className, FreSharpHelper.ArgsToArgv(argsArr),
                FreSharpHelper.GetArgsC(argsArr), ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return ret;
            Logger.Error($"cannot create class {className}", status, ret);
            return FREObject.Zero;
        }

        /// <summary>
        /// Creates a FREObject.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name">Name of the Class.</param>
        /// <returns></returns>
        public static FREObject Init(this FREObject value, string name) {
            uint resultPtr = 0;
            var argsArr = new ArrayList();
            var ret = FreSharpHelper.Core.getFREObject(name, FreSharpHelper.ArgsToArgv(argsArr),
                FreSharpHelper.GetArgsC(argsArr), ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return ret;
            Logger.Error($"cannot create class {name}", status, ret);
            return FREObject.Zero;
        }
    }
}