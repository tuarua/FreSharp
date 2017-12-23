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
//  Additional Terms
//  No part, or derivative of this Air Native Extensions's code is permitted 
//  to be sold as the basis of a commercially packaged Air Native Extension which 
//  undertakes the same purpose as this software. That is, a WebView for Windows, 
//  OSX and/or iOS and/or Android.
//  All Rights Reserved. Tua Rua Ltd.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Color = System.Windows.Media.Color;
using FreSharp.Geom;
using TuaRua.FreSharp.Geom;
using FREObject = System.IntPtr;

namespace TuaRua.FreSharp {
    /// <summary>
    /// 
    /// </summary>
    public static class FreSharpExtensions {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this string str) => new FreObjectSharp(str).RawValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static string AsString(this FREObject inFre) => FreSharpHelper.GetAsString(inFre);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static string AsString(this FreObjectSharp inFre) => Convert.ToString(inFre.Value);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static bool AsBool(this FREObject inFre) => FreSharpHelper.GetAsBool(inFre);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static bool AsBool(this FreObjectSharp inFre) => (bool) inFre.Value;

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this bool i) => new FreObjectSharp(i).RawValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static FREObject GetProp(this FREObject inFre, string name) {
            //throws? //TODO
            return FreSharpHelper.GetProperty(inFre, name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetProp(this FREObject inFre, string name, object value) {
            FreSharpHelper.SetProperty(inFre, name, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetProp(this FREObject inFre, string name, FREObject value){
            FreSharpHelper.SetProperty(inFre, name, value);
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static FreObjectTypeSharp GetType(this FREObject inFre) {
            return FreSharpHelper.GetType(inFre);
        }*/


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static FreObjectTypeSharp Type(this FREObject inFre) => FreSharpHelper.GetType(inFre);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <param name="method"></param>
        /// <param name="args"></param>
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

            if (status == FreResultSharp.Ok) {
                return ret;
            }

            FreSharpHelper.ThrowFreException(status, "cannot call method " + method, ret);
            return FREObject.Zero;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static FREArray Call(this FREObject inFre, string methodName, ArrayList args) {
            uint resultPtr = 0;
            var ret = new FREArray(FreSharpHelper.Core.callMethod(inFre, methodName,
                FreSharpHelper.ArgsToArgv(args), FreSharpHelper.GetArgsC(args), ref resultPtr));
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) {
                return ret;
            }

            FreSharpHelper.ThrowFreException(status, "cannot call method " + methodName, ret.RawValue);
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static ArrayList ToArrayList(this FREObject inFre) => new FREArray(inFre).GetAsArrayList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static ArrayList ToArrayList(this FREArray inFre) => inFre.GetAsArrayList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static int AsInt(this FREObject inFre) => FreSharpHelper.GetAsInt(inFre);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <param name="hasAlpha"></param>
        /// <returns></returns>
        public static Color AsColor(this FREObject inFre, bool hasAlpha = false) {
            var rgb = FreSharpHelper.GetAsUInt(new FreObjectSharp(inFre).RawValue);
            if (hasAlpha) {
                return Color.FromArgb(
                    Convert.ToByte((rgb >> 24) & 0xff),
                    Convert.ToByte((rgb >> 16) & 0xff),
                    Convert.ToByte((rgb >> 8) & 0xff),
                    Convert.ToByte((rgb >> 0) & 0xff));
                
            }
            return Color.FromRgb(
                Convert.ToByte((rgb >> 16) & 0xff),
                Convert.ToByte((rgb >> 8) & 0xff),
                Convert.ToByte((rgb >> 0) & 0xff));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static Dictionary<string, object> AsDictionary(this FREObject inFre) =>
            FreSharpHelper.GetAsDictionary(inFre);

        /*/// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static int AsInt(this FreObjectSharp inFre) => Convert.ToInt32(inFre.Value);*/

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this int i) => new FreObjectSharp(i).RawValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static uint AsUInt(this FREObject inFre) => FreSharpHelper.GetAsUInt(inFre);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static uint AsUInt(this FreObjectSharp inFre) => Convert.ToUInt32(inFre.Value);


        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this uint i) => new FreObjectSharp(i).RawValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static double AsDouble(this FREObject inFre) => FreSharpHelper.GetAsDouble(inFre);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static double AsDouble(this FreObjectSharp inFre) => Convert.ToDouble(inFre.Value);

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this double i) => new FreObjectSharp(i).RawValue;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static Rect AsRect(this FREObject inFre) => new FreRectangleSharp(inFre).Value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static Rect AsRect(this FreObjectSharp inFre) => new FreRectangleSharp(inFre).Value;

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this Rect rect) => new FreRectangleSharp(rect).RawValue;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static Point AsPoint(this FREObject inFre) => new FrePointSharp(inFre).Value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static Point AsPoint(this FrePointSharp inFre) => inFre.Value;

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this Point point) => new FrePointSharp(point).RawValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="className"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static FREObject Init(this FREObject value, string className, params object[] args) {
            uint resultPtr = 0;
            var argsArr = new ArrayList();
            if (args != null) {
                for (var i = 0; i < args.Length; i++) {
                    argsArr.Add(args.ElementAt(i));
                }
            }
            var rawValue = FreSharpHelper.Core.getFREObject(className, FreSharpHelper.ArgsToArgv(argsArr),
                FreSharpHelper.GetArgsC(argsArr), ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) {
                return rawValue;
            }
            FreSharpHelper.ThrowFreException(status, "cannot create class " + className, rawValue);
            return FREObject.Zero;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static FREObject Init(this FREObject value, string name) {
            uint resultPtr = 0;
            var argsArr = new ArrayList();
            var rawValue = FreSharpHelper.Core.getFREObject(name, FreSharpHelper.ArgsToArgv(argsArr),
                FreSharpHelper.GetArgsC(argsArr), ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) {
                return rawValue;
            }
            FreSharpHelper.ThrowFreException(status, "cannot create class " + name, rawValue);
            return FREObject.Zero;
        }
    }
}