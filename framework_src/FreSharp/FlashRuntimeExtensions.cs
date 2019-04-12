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
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static string AsString(this FREObject inFre) => FreSharpHelper.GetAsString(inFre);

        /// <summary>
        /// Calls toString() on a FREObject
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static string toString(this FREObject inFre) {
            if (inFre.Type() == FreObjectTypeSharp.String
                || inFre.Type() == FreObjectTypeSharp.Null) return "";
            return inFre.Call("toString").AsString();
        }

        /// <summary>
        /// Indicates whether an object has a specified property defined.
        /// </summary>
        /// <param name="inFre"></param>
        /// <param name="name">The property of the FREObject. </param>
        /// <returns></returns>
        public static bool hasOwnProperty(this FREObject inFre, string name) =>
            inFre.Call("hasOwnProperty", name).AsBool();

        /// <summary>
        /// Returns the className of the FREObject
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static string ClassName(this FREObject inFre) =>
            new FREObject().Init("com.tuarua.fre.ANEUtils").Call("getClassType", inFre).AsString();

        /// <summary>
        /// Converts a FREObject to a DateTime.
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static DateTime AsDateTime(this FREObject inFre) => FreSharpHelper.GetAsDateTime(inFre);

        /// <summary>
        /// Converts a C# DateTime to a FREObject.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this DateTime value) => FreSharpHelper.NewObject(value);

        /// <summary>
        /// Converts a FREObject to a C# bool.
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static bool AsBool(this FREObject inFre) => FreSharpHelper.GetAsBool(inFre);

        /// <summary>
        /// Converts a C# bool to a FREObject.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this bool value) => FreSharpHelper.NewObject(value);

        /// <summary>
        /// Gets the property of a FREObject.
        /// </summary>
        /// <param name="inFre"></param>
        /// <param name="name">Name of the property</param>
        /// <returns></returns>
        public static FREObject GetProp(this FREObject inFre, string name) {
            return FreSharpHelper.GetProperty(inFre, name);
        }

        /// <summary>
        /// Sets the property of a FREObject.
        /// </summary>
        /// <param name="inFre"></param>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value of the property.</param>
        public static void SetProp(this FREObject inFre, string name, object value) {
            FreSharpHelper.SetProperty(inFre, name, value);
        }

        /// <summary>
        /// Sets the property of a FREObject.
        /// </summary>
        /// <param name="inFre"></param>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value of the property.</param>
        public static void SetProp(this FREObject inFre, string name, FREObject value) {
            FreSharpHelper.SetProperty(inFre, name, value);
        }

        /// <summary>
        /// Returns the type of the FREObject.
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static FreObjectTypeSharp Type(this FREObject inFre) => FreSharpHelper.GetType(inFre);

        /// <summary>
        /// Calls a method on a FREObject.
        /// </summary>
        /// <param name="inFre"></param>
        /// <param name="method">The method name.</param>
        /// <param name="args">Arguments to pass to the method.</param>
        /// <returns></returns>
        public static FREObject Call(this FREObject inFre, string method, params object[] args) {
            uint resultPtr = 0;
            var argsArr = new ArrayList();
            if (args != null) {
                for (var i = 0; i < args.Length; i++) {
                    argsArr.Add(args.ElementAt(i));
                }
            }

            var ret = FreSharpHelper.Core.callMethod(inFre, method, FreSharpHelper.ArgsToArgv(argsArr),
                FreSharpHelper.GetArgsC(argsArr), ref resultPtr);

            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return ret;
            Logger.Log($"cannot call method {method} of {inFre.toString()}", status, ret);
            return FREObject.Zero;
        }

        /// <summary>
        /// Calls a method on a FREArray.
        /// </summary>
        /// <param name="inFre"></param>
        /// <param name="methodName">The method name.</param>
        /// <param name="args">Arguments to pass to the method.</param>
        /// <returns></returns>
        public static FREArray Call(this FREObject inFre, string methodName, ArrayList args) {
            uint resultPtr = 0;
            var fre = FreSharpHelper.Core.callMethod(inFre, methodName,
                FreSharpHelper.ArgsToArgv(args), FreSharpHelper.GetArgsC(args), ref resultPtr);
            var ret = new FREArray(fre);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return ret;
            Logger.Log($"cannot call method {methodName} of {inFre.toString()}", status, fre);
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
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static ArrayList ToArrayList(this FREObject inFre) => new FREArray(inFre).AsArrayList();

        /// <summary>
        /// Converts a FREArray to an ArrayList.
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static ArrayList ToArrayList(this FREArray inFre) => inFre.AsArrayList(); //TODO can I delete this

        /// <summary>
        /// Converts a FREObject to a C# int.
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static int AsInt(this FREObject inFre) => FreSharpHelper.GetAsInt(inFre);

        /// <summary>
        /// Converts a FREObject to a C# short.
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static short AsShort(this FREObject inFre) => (short) FreSharpHelper.GetAsInt(inFre);

        /// <summary>
        /// Initialise a System.Drawing.Color from a FREObject.
        /// </summary>
        /// <param name="inFre"></param>
        /// <param name="hasAlpha">Set to true when the AS3 uint is in ARGB format.</param>
        /// <returns></returns>
        public static Color AsColor(this FREObject inFre, bool hasAlpha = true) {
            var rgb = inFre.AsUInt();
            if (hasAlpha) {
                return Color.FromArgb(
                    Convert.ToByte((rgb >> 24) & 0xff),
                    Convert.ToByte((rgb >> 16) & 0xff),
                    Convert.ToByte((rgb >> 8) & 0xff),
                    Convert.ToByte((rgb >> 0) & 0xff));
            }

            return Color.FromArgb(
                Convert.ToByte((rgb >> 16) & 0xff),
                Convert.ToByte((rgb >> 8) & 0xff),
                Convert.ToByte((rgb >> 0) & 0xff));
        }


        /// <summary>
        /// Converts a C# System.Drawing.Color to a FREObject.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this Color c) => FreSharpHelper.NewObject((uint) ((c.A << 24)
                                                                                              | (c.R << 16)
                                                                                              | (c.G << 8)
                                                                                              | (c.B << 0)));

        /// <summary>
        /// Converts a FREObject to a C# Dictionary&lt;string, object&gt;.
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static Dictionary<string, object> AsDictionary(this FREObject inFre) =>
            FreSharpHelper.GetAsDictionary(inFre);

        /// <summary>
        /// Converts a C# int to a FREObject.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this int value) => FreSharpHelper.NewObject(value);

        /// <summary>
        /// Converts a FREObject to a C# uint.
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static uint AsUInt(this FREObject inFre) => FreSharpHelper.GetAsUInt(inFre);

        /// <summary>
        /// Converts a C# uint to a FREObject.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this uint value) => FreSharpHelper.NewObject(value);

        /// <summary>
        /// Converts a FREObject to a C# double.
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static double AsDouble(this FREObject inFre) => FreSharpHelper.GetAsDouble(inFre);

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
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static Bitmap AsBitmap(this FREObject inFre) => new FreBitmapDataSharp(inFre).AsBitmap();

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
            Logger.Log($"cannot create class {className}", status, ret);
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
            Logger.Log($"cannot create class {name}", status, ret);
            return FREObject.Zero;
        }
    }
}