using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FreSharp.Exceptions;
using FRESharpCore;
using TuaRua.FreSharp.Display;
using TuaRua.FreSharp.Geom;
using FREContext = System.IntPtr;
using FREObject = System.IntPtr;
namespace TuaRua.FreSharp {
    /// <summary>
    /// Creates a new FreSharp Helper
    /// </summary>
    public static class FreSharpHelper {
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

        private static FreObjectSharp FreObjectSharpFromObject(object value) {
            var t = value.GetType();

            if (t == typeof(FreObjectSharp)) {
                return value as FreObjectSharp;
            }

            //TODO others Bitmap, arrayList

            if (t == typeof(int) || t == typeof(long)) {
                return new FreObjectSharp((int)value);
            }

            if (t == typeof(uint)) {
                return new FreObjectSharp((uint)value);
            }

            if (t == typeof(bool)) {
                return new FreObjectSharp((bool)value);
            }

            if (t == typeof(string)) {
                return new FreObjectSharp((string)value);
            }

            if (t == typeof(double)) {
                return new FreObjectSharp((double)value);
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
                var fre = FreObjectSharpFromObject(argArr.ElementAt((int)i));
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
                cnt = (uint)args.Count;
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
            var status = (FreResultSharp)resultPtr;

            if (status == FreResultSharp.Ok) {
                return ret;
            }
            ThrowFreException(status, "cannot get FREObject as String", null);
            return "";
        }

        /// <summary>
        /// Returns the C# FREObject as a double.
        /// </summary>
        /// <returns></returns>
        public static double GetAsDouble(FREObject rawValue) {
            uint resultPtr = 0;
            var ret = Core.getDouble(rawValue, ref resultPtr);
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) {
                return ret;
            }
            ThrowFreException(status, "cannot get FREObject as Double", null);
            return 0.0;
        }

        /// <summary>
        /// Returns the C# FREObject as a bool.
        /// </summary>
        /// <returns></returns>
        public static bool GetAsBool(FREObject rawValue) {
            uint resultPtr = 0;
            var ret = Core.getBool(rawValue, ref resultPtr);
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) {
                return ret;
            }
            ThrowFreException(status, "cannot get FREObject as Bool", null);
            return false;
        }


        /// <summary>
        /// Returns the C# FREObject as an int.
        /// </summary>
        /// <returns></returns>
        public static int GetAsInt(FREObject rawValue) {
            uint resultPtr = 0;
            var ret = Core.getInt32(rawValue, ref resultPtr);
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) {
                return ret;
            }

            ThrowFreException(status, "cannot get FREObject as Int", null);
            return 0;
        }


        /// <summary>
        /// Returns the C# FREObject as a uint.
        /// </summary>
        /// <returns></returns>
        public static uint GetAsUInt(FREObject rawValue) {
            uint resultPtr = 0;
            var ret = Core.getUInt32(rawValue, ref resultPtr);
            var status = (FreResultSharp)resultPtr;

            if (status == FreResultSharp.Ok) {
                return ret;
            }
            ThrowFreException(status, "cannot get FREObject as Uint", null);
            return 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawValue"></param>
        /// <returns></returns>
        public static FreObjectTypeSharp GetActionscriptType(FREObject rawValue) {
            var aneUtils = new FreObjectSharp("com.tuarua.fre.ANEUtils", null);
            var classType = aneUtils.CallMethod("getClassType", rawValue);
            var type = GetAsString(classType.RawValue).ToLower();

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
                default:
                    return FreObjectTypeSharp.Class;
            }
        }

        internal static void SetProperty(FREObject rawValue, string name, object value) {
            uint resultPtr = 0;
            var ret = new FreObjectSharp(Core.setProperty(rawValue, name, FreObjectSharpFromObject(value).RawValue, ref resultPtr));
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) {
                return;
            }
            ThrowFreException(status, "cannot set property " + name, ret);
        }


        /// <summary>
        /// Returns the Actionscript type of the C# FREObject. !Important - your ane must include ANEUtils.as in com.tuarua.fre
        /// </summary>
        /// <returns></returns>
        public static FreObjectTypeSharp GetType(FREObject rawValue) {
            uint resultPtr = 0;
            var type = (FreObjectTypeSharp)Core.getType(rawValue, ref resultPtr);
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
            var aneUtils = new FreObjectSharp("com.tuarua.fre.ANEUtils", null);
            var paramsArray = new ArrayList {
                new FreObjectSharp(rawValue)
            };
            var classProps = aneUtils.CallMethod("getClassProps", paramsArray);
            if (classProps == null) return ret;
            var arrayLength = classProps.Length;
            for (uint i = 0; i < arrayLength; i++) {
                var elem = classProps.GetObjectAt(i);
                var propNameAs = elem.GetProperty("name");
                var propName = GetAsString(propNameAs.RawValue);

                var propVal = GetProperty(rawValue, propName);
                ret.Add(propName, GetAsObject(propVal.RawValue));
            }
            return ret;
        }

        internal static FreObjectSharp GetProperty(FREObject rawValue, string name) {
            uint resultPtr = 0;
            var ret = new FreObjectSharp(Core.getProperty(rawValue, name, ref resultPtr));
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) {
                return ret;
            }

            ThrowFreException(status, "cannot get property " + name, ret);
            return null;
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
                    var arrFre = new FreArraySharp(rawValue);
                    return arrFre.GetAsArrayList();
                case FreObjectTypeSharp.Bitmapdata:
                    var bmdFre = new FreBitmapDataSharp(rawValue);
                    return bmdFre.GetAsBitmap();
                case FreObjectTypeSharp.Boolean:
                    return GetAsBool(rawValue);
                case FreObjectTypeSharp.Null:
                    return null;
                case FreObjectTypeSharp.Int:
                    return GetAsInt(rawValue);
                case FreObjectTypeSharp.Rectangle:
                    return new FreRectangleSharp(rawValue).Value;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <param name="freObject"></param>
        /// <exception cref="FreActionscriptErrorException"></exception>
        /// <exception cref="NoSuchNameException"></exception>
        /// <exception cref="FreInvalidObjectException"></exception>
        /// <exception cref="FreTypeMismatchException"></exception>
        /// <exception cref="FreInvalidArgumentException"></exception>
        /// <exception cref="FreReadOnlyException"></exception>
        /// <exception cref="FreWrongThreadException"></exception>
        /// <exception cref="FreIllegalStateException"></exception>
        /// <exception cref="FreInsufficientMemoryException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ThrowFreException(FreResultSharp status, string message, FreObjectSharp freObject) {
            if (FreObjectTypeSharp.Class == freObject?.GetType()) {
                try {
                    var hasStackTrace = GetAsBool(freObject.CallMethod("hasOwnProperty", "getStackTrace").RawValue);

                    if (hasStackTrace) {
                        var asStackTrace = freObject.CallMethod("getStackTrace");
                        if (FreObjectTypeSharp.String == asStackTrace.GetType()) {
                            message = GetAsString(asStackTrace.RawValue);
                        }
                    }
                }
                catch (Exception) {
                    //ignored
                }
            }
            switch (status) {
                case FreResultSharp.FreActionscriptError:
                    throw new FreActionscriptErrorException(message);
                case FreResultSharp.NoSuchName:
                    throw new NoSuchNameException(message);
                case FreResultSharp.FreInvalidObject:
                    throw new FreInvalidObjectException(message);
                case FreResultSharp.FreTypeMismatch:
                    throw new FreTypeMismatchException(message);
                case FreResultSharp.FreInvalidArgument:
                    throw new FreInvalidArgumentException(message);
                case FreResultSharp.FreReadOnly:
                    throw new FreReadOnlyException(message);
                case FreResultSharp.FreWrongThread:
                    throw new FreWrongThreadException(message);
                case FreResultSharp.FreIllegalState:
                    throw new FreIllegalStateException(message);
                case FreResultSharp.FreInsufficientMemory:
                    throw new FreInsufficientMemoryException(message);
                case FreResultSharp.Ok:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }


    }
}
