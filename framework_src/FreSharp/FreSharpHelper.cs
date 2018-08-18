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

namespace TuaRua.FreSharp {
    /// <summary>
    /// Creates a new FreSharp Helper
    /// </summary>
    public static class FreSharpHelper {
        private static FreSharpLogger Logger => FreSharpLogger.GetInstance();

        /// <summary>
        /// 
        /// </summary>
        public static FRESharpCLR Core = new FRESharpCLR();

        /// <summary>
        /// Dispatches an event. Mimics FREDispatchStatusEventAsync
        /// </summary>
        /// <param name="freContext"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void DispatchEvent(ref FREContext freContext, string name, string value) {
            Core.dispatchEvent(freContext, name, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FreObjectSharp FreObjectSharpFromObject(object value) {
            if (value == null) return null;
            var t = value.GetType();
            if (t == typeof(FREObject)) {
                return new FreObjectSharp((FREObject) value);
            }

            if (t == typeof(FreObjectSharp)) {
                return value as FreObjectSharp;
            }

            if (t == typeof(int) || t == typeof(long) || t == typeof(short)) {
                return new FreObjectSharp((int) value);
            }

            if (t == typeof(uint)) {
                return new FreObjectSharp((uint) value);
            }

            if (t == typeof(bool)) {
                return new FreObjectSharp((bool) value);
            }

            if (t == typeof(string)) {
                return new FreObjectSharp((string) value);
            }

            if (t == typeof(double)) {
                return new FreObjectSharp((double) value);
            }

            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static FREObject[] ArgsToArgv(ArrayList args) {
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

                var fre = FreObjectSharpFromObject(argArr.ElementAt((int) i));
                if (fre == null) break;
                arr.SetValue(fre.RawValue, i);
            }

            return arr;
        }


        /// <summary>
        /// Gets length of ArrayList as argc which can be passed to library calls
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static uint GetArgsC(ArrayList args) {
            uint cnt = 0;
            if (args != null) {
                cnt = (uint) args.Count;
            }

            return cnt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawValue"></param>
        /// <returns></returns>
        public static string GetAsString(FREObject rawValue) {
            uint resultPtr = 0;
            var ret = Core.getString(rawValue, ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return StringUtils.ToUtf8(ret);
            Logger.Log($"cannot get FREObject {rawValue.toString()} as String", status);
            return null;
        }

        /// <summary>
        /// Returns the C# FREObject as a double.
        /// </summary>
        /// <returns></returns>
        public static double GetAsDouble(FREObject rawValue) {
            uint resultPtr = 0;
            var ret = Core.getDouble(rawValue, ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return ret;
            Logger.Log($"cannot get FREObject {rawValue.toString()} as Double", status);
            return 0.0;
        }

        /// <summary>
        /// Returns the C# FREObject as a bool.
        /// </summary>
        /// <returns></returns>
        public static bool GetAsBool(FREObject rawValue) {
            uint resultPtr = 0;
            var ret = Core.getBool(rawValue, ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return ret;
            Logger.Log($"cannot get FREObject {rawValue.toString()} as Bool", status);
            return false;
        }

        /// <summary>
        /// Returns the C# FREObject as an int.
        /// </summary>
        /// <returns></returns>
        public static int GetAsInt(FREObject rawValue) {
            uint resultPtr = 0;
            var ret = Core.getInt32(rawValue, ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return ret;
            Logger.Log($"cannot get FREObject {rawValue.toString()} as Int", status);
            return 0;
        }

        /// <summary>
        /// Returns the C# FREObject as a uint.
        /// </summary>
        /// <returns></returns>
        public static uint GetAsUInt(FREObject rawValue) {
            uint resultPtr = 0;
            var ret = Core.getUInt32(rawValue, ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) return ret;
            Logger.Log($"cannot get {rawValue.toString()} as UInt", status);
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawValue"></param>
        /// <returns></returns>
        public static DateTime GetAsDateTime(FREObject rawValue) =>
            new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(rawValue.GetProp("time").AsDouble() / 1000);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawValue"></param>
        /// <returns></returns>
        public static FreObjectTypeSharp GetActionscriptType(FREObject rawValue) {
            var aneUtils = new FREObject().Init("com.tuarua.fre.ANEUtils", null);
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
            var ret = Core.setProperty(rawValue, name, FreObjectSharpFromObject(value).RawValue,
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


        /// <summary>
        /// Returns the Actionscript type of the C# FREObject. !Important - your ane must include ANEUtils.as in com.tuarua.fre
        /// </summary>
        /// <returns></returns>
        public static FreObjectTypeSharp GetType(FREObject rawValue) {
            uint resultPtr = 0;
            var type = (FreObjectTypeSharp) Core.getType(rawValue, ref resultPtr);
            return FreObjectTypeSharp.Number == type || FreObjectTypeSharp.Object == type
                ? GetActionscriptType(rawValue)
                : type;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawValue"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetAsDictionary(FREObject rawValue) {
            var ret = new Dictionary<string, object>();
            var aneUtils = new FREObject().Init("com.tuarua.fre.ANEUtils", null);
            var paramsArray = new ArrayList {
                new FreObjectSharp(rawValue)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static object GetAsObject(FREObject rawValue) {
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
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}