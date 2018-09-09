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
using System.Linq;
using FRESharpCore;
using TuaRua.FreSharp.Display;
using TuaRua.FreSharp.Geom;
using TuaRua.FreSharp.Utils;
using FREContext = System.IntPtr;
using FREObject = System.IntPtr;
using Color = System.Drawing.Color;
using Point = System.Windows.Point;
using Rect = System.Windows.Rect;

// ReSharper disable InconsistentNaming
namespace TuaRua.FreSharp {
    internal static class FreSharpHelper {
        private static FreSharpLogger Logger => FreSharpLogger.GetInstance();
        internal static FRESharpCLR Core = new FRESharpCLR();

        internal static void DispatchEvent(ref FREContext freContext, string name, string value) {
            Core.dispatchEvent(freContext, name, value);
        }

        internal static FREObject FREObjectFromObject(object value) {
            if (value == null) return FREObject.Zero;
            var type = value.GetType();
            if (type == typeof(FREObject)) {
                return (FREObject) value;
            }

            if (type == typeof(FREArray)) {
                return ((FREArray) value).RawValue;
            }

            if (type == typeof(FreObjectSharp)) {
                return ((FreObjectSharp) value).RawValue();
            }

            if (type == typeof(int) || type == typeof(long) || type == typeof(short)) {
                return NewObject((int) value);
            }

            if (type == typeof(uint)) {
                return NewObject((uint) value);
            }

            if (type == typeof(bool)) {
                return NewObject((bool) value);
            }

            if (type == typeof(string)) {
                return NewObject((string) value);
            }

            if (type == typeof(double)) {
                return NewObject((double) value);
            }

            if (type == typeof(Rect)) {
                return ((Rect) value).ToFREObject();
            }

            if (type == typeof(Point)) {
                return ((Point) value).ToFREObject();
            }

            if (type == typeof(DateTime)) {
                return NewObject((DateTime) value);
            }

            if (type == typeof(Color)) {
                return ((Color) value).ToFREObject();
            }

            return FREObject.Zero;
        }

        internal static FREObject[] ArgsToArgv(ArrayList args) {
            var cnt = GetArgsC(args);
            var arr = new FREObject[cnt];
            if (args == null) return arr;
            var argArr = args.ToArray();

            uint i;
            for (i = 0; i < cnt; ++i) {
                if (argArr[i] is FREObject) {
                    arr.SetValue(argArr[i], i);
                    continue;
                }

                var fre = FREObjectFromObject(argArr.ElementAt((int) i));
                if (fre == FREObject.Zero) break;
                arr.SetValue(fre, i);
            }

            return arr;
        }

        internal static uint GetArgsC(ArrayList args) {
            uint cnt = 0;
            if (args != null) {
                cnt = (uint) args.Count;
            }

            return cnt;
        }

        internal static FREObject NewObject(string value) {
            uint resultPtr = 0;
            var ret = Core.getFREObject(value, ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return ret;
            Logger.Log($"cannot create FREObject from {value}", status);
            return FREObject.Zero;
        }

        internal static FREObject NewObject(DateTime value) {
            return new FREObject().Init("Date", Convert.ToDouble(new DateTimeOffset(value).ToUnixTimeMilliseconds()));
        }

        internal static FREObject NewObject(bool value) {
            uint resultPtr = 0;
            var ret = Core.getFREObject(value, ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return ret;
            Logger.Log($"cannot create FREObject from {value}", status);
            return FREObject.Zero;
        }

        internal static FREObject NewObject(int value) {
            uint resultPtr = 0;
            var ret = Core.getFREObject(value, ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return ret;
            Logger.Log($"cannot create FREObject from {value}", status);
            return FREObject.Zero;
        }

        internal static FREObject NewObject(uint value) {
            uint resultPtr = 0;
            var ret = Core.getFREObject(value, ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return ret;
            Logger.Log($"cannot create FREObject from {value}", status);
            return FREObject.Zero;
        }

        internal static FREObject NewObject(double value) {
            uint resultPtr = 0;
            var ret = Core.getFREObject(value, ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return ret;
            Logger.Log($"cannot create FREObject from {value}", status);
            return FREObject.Zero;
        }

        internal static string GetAsString(FREObject rawValue) {
            uint resultPtr = 0;
            var ret = Core.getString(rawValue, ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return StringUtils.ToUtf8(ret);
            Logger.Log($"cannot get FREObject {rawValue.toString()} as String", status);
            return null;
        }

        internal static double GetAsDouble(FREObject rawValue) {
            uint resultPtr = 0;
            var ret = Core.getDouble(rawValue, ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return ret;
            Logger.Log($"cannot get FREObject {rawValue.toString()} as Double", status);
            return 0.0;
        }

        internal static bool GetAsBool(FREObject rawValue) {
            uint resultPtr = 0;
            var ret = Core.getBool(rawValue, ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return ret;
            Logger.Log($"cannot get FREObject {rawValue.toString()} as Bool", status);
            return false;
        }

        internal static int GetAsInt(FREObject rawValue) {
            uint resultPtr = 0;
            var ret = Core.getInt32(rawValue, ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return ret;
            Logger.Log($"cannot get FREObject {rawValue.toString()} as Int", status);
            return 0;
        }

        internal static uint GetAsUInt(FREObject rawValue) {
            uint resultPtr = 0;
            var ret = Core.getUInt32(rawValue, ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return ret;
            Logger.Log($"cannot get {rawValue.toString()} as UInt", status);
            return 0;
        }

        internal static DateTime GetAsDateTime(FREObject rawValue) =>
            new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(rawValue.GetProp("time").AsDouble() / 1000);

        internal static FreObjectTypeSharp GetActionscriptType(FREObject rawValue) {
            var aneUtils = new FREObject().Init("com.tuarua.fre.ANEUtils");
            var classType = aneUtils.Call("getClassType", rawValue);
            var type = GetAsString(classType).ToLower();

            switch (type) {
                case "object":
                    return FreObjectTypeSharp.Object;
                case "int":
                    return FreObjectTypeSharp.Int;
                case "number":
                    return FreObjectTypeSharp.Number;
                case "boolean":
                    return FreObjectTypeSharp.Boolean;
                case "flash.geom::rectangle":
                    return FreObjectTypeSharp.Rectangle;
                case "flash.geom::point":
                    return FreObjectTypeSharp.Point;
                case "date":
                    return FreObjectTypeSharp.Date;
                default:
                    return FreObjectTypeSharp.Class;
            }
        }

        internal static void SetProperty(FREObject rawValue, string name, object value) {
            uint resultPtr = 0;
            var ret = Core.setProperty(rawValue, name, FREObjectFromObject(value),
                ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return;
            Logger.Log($"cannot set property {name} to {rawValue.toString()}", status, ret);
        }

        internal static void SetProperty(FREObject rawValue, string name, FREObject value) {
            uint resultPtr = 0;
            var ret = Core.setProperty(rawValue, name, value, ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return;
            Logger.Log($"cannot set property {name} to {rawValue.toString()}", status, ret);
        }

        internal static FREObject GetProperty(FREObject rawValue, string name) {
            uint resultPtr = 0;
            var ret = Core.getProperty(rawValue, name, ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return ret;
            Logger.Log($"cannot get property {name} of {rawValue.toString()}", status, ret);
            return FREObject.Zero;
        }

        internal static FreObjectTypeSharp GetType(FREObject rawValue) {
            uint resultPtr = 0;
            var type = (FreObjectTypeSharp) Core.getType(rawValue, ref resultPtr);
            return FreObjectTypeSharp.Number == type || FreObjectTypeSharp.Object == type
                ? GetActionscriptType(rawValue)
                : type;
        }

        internal static FREObject GetActionScriptData(ref FREContext freContext) {
            uint resultPtr = 0;
            var ret = Core.getActionScriptData(freContext, ref resultPtr);
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) return ret;
            Logger.Log("cannot get ActionScript data", status, ret);
            return FREObject.Zero;
        }

        internal static void SetActionScriptData(ref FREContext freContext, FREObject value) {
            uint resultPtr = 0;
            Core.setActionScriptData(freContext, value, ref resultPtr);
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) return;
            Logger.Log("cannot set ActionScript data", status);
        }

        internal static Dictionary<string, object> GetAsDictionary(FREObject rawValue) {
            var ret = new Dictionary<string, object>();
            var aneUtils = new FREObject().Init("com.tuarua.fre.ANEUtils");
            var paramsArray = new ArrayList {
                rawValue
            };
            var classProps = aneUtils.Call("getClassProps", paramsArray);
            if (classProps == null) return ret;

            var arrayLength = classProps.Length;
            for (uint i = 0; i < arrayLength; i++) {
                var elem = classProps.At(i);
                var propNameAs = elem.GetProp("name");
                var propName = GetAsString(propNameAs);

                var propVal = GetProperty(rawValue, propName);
                ret.Add(propName, GetAsObject(propVal));
            }

            return ret;
        }

        internal static object GetAsPrimitiveObject(FREObject rawValue) {
            switch (GetType(rawValue)) {
                case FreObjectTypeSharp.Class:
                case FreObjectTypeSharp.Object:
                case FreObjectTypeSharp.Bytearray:
                case FreObjectTypeSharp.Bitmapdata:
                    return rawValue;
                case FreObjectTypeSharp.Vector:
                case FreObjectTypeSharp.Array:
                    return new FREArray(rawValue);
                case FreObjectTypeSharp.Number:
                    return GetAsDouble(rawValue);
                case FreObjectTypeSharp.String:
                    return GetAsString(rawValue);
                case FreObjectTypeSharp.Boolean:
                    return GetAsBool(rawValue);
                case FreObjectTypeSharp.Null:
                    return null;
                case FreObjectTypeSharp.Int:
                    return GetAsInt(rawValue);
                case FreObjectTypeSharp.Rectangle:
                    return rawValue.AsRect();
                case FreObjectTypeSharp.Point:
                    return rawValue.AsPoint();
                case FreObjectTypeSharp.Date:
                    return GetAsDateTime(rawValue);
                default:
                    return null;
            }
        }

        internal static object GetAsObject(FREObject rawValue) {
            switch (GetType(rawValue)) {
                case FreObjectTypeSharp.Object:
                case FreObjectTypeSharp.Class:
                    return GetAsDictionary(rawValue);
                case FreObjectTypeSharp.Number:
                    return GetAsDouble(rawValue);
                case FreObjectTypeSharp.String:
                    return GetAsString(rawValue);
                case FreObjectTypeSharp.Bytearray:
                    var ba = new FreByteArraySharp(rawValue);
                    ba.Acquire();
                    var byteData = ba.Bytes;
                    ba.Release();
                    return byteData;
                case FreObjectTypeSharp.Array:
                case FreObjectTypeSharp.Vector:
                    var arrFre = new FREArray(rawValue);
                    return arrFre.AsArrayList();
                case FreObjectTypeSharp.Bitmapdata:
                    var bmdFre = new FreBitmapDataSharp(rawValue);
                    return bmdFre.AsBitmap();
                case FreObjectTypeSharp.Boolean:
                    return GetAsBool(rawValue);
                case FreObjectTypeSharp.Null:
                    return null;
                case FreObjectTypeSharp.Int:
                    return GetAsInt(rawValue);
                case FreObjectTypeSharp.Rectangle:
                    return rawValue.AsRect();
                case FreObjectTypeSharp.Point:
                    return rawValue.AsPoint();
                case FreObjectTypeSharp.Date:
                    return GetAsDateTime(rawValue);
                default:
                    return null;
            }
        }
    }
}