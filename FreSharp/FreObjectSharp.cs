using System;
using System.Collections;
using System.Collections.Generic;
using FreSharp.Exceptions;

namespace FreSharp {
    /// <summary>
    /// FreObjectSharp wraps a C FREObject with helper methods.
    /// </summary>
    public class FreObjectSharp {

        private readonly IntPtr _freObject = IntPtr.Zero;

        /// <summary>
        /// Creates an empty C# FREObject
        /// </summary>
        public FreObjectSharp() { }

        /// <summary>
        /// Creates a C# FREObject from a C FREObject
        /// </summary>
        /// <param name="freObject"></param>
        public FreObjectSharp(IntPtr freObject) {
            _freObject = freObject;
        }
        /// <summary>
        /// Creates a C# FREObject from a string
        /// </summary>
        /// <param name="value"></param>
        public FreObjectSharp(string value) {
            uint resultPtr = 0;
            _freObject = FreSharpHelper.Core.getFREObject(value, ref resultPtr);
        }
        /// <summary>
        /// Creates a C# FREObject from a bool
        /// </summary>
        /// <param name="value"></param>
        public FreObjectSharp(bool value) {
            uint resultPtr = 0;
            _freObject = FreSharpHelper.Core.getFREObject(value, ref resultPtr);
        }
        /// <summary>
        /// Creates a C# FREObject from a double
        /// </summary>
        /// <param name="value"></param>
        public FreObjectSharp(double value) {
            uint resultPtr = 0;
            _freObject = FreSharpHelper.Core.getFREObject(value, ref resultPtr);
        }
        /// <summary>
        /// Creates a C# FREObject from an int
        /// </summary>
        /// <param name="value"></param>
        public FreObjectSharp(int value) {
            uint resultPtr = 0;
            _freObject = FreSharpHelper.Core.getFREObject(value, ref resultPtr);
        }
        /// <summary>
        /// Creates a C# FREObject from a uint
        /// </summary>
        /// <param name="value"></param>
        public FreObjectSharp(uint value) {
            uint resultPtr = 0;
            _freObject = FreSharpHelper.Core.getFREObject(value, ref resultPtr);
        }

        /// <summary>
        /// Creates a C# FREObject with given class name
        /// </summary>
        /// <param name="className"></param>
        /// <param name="args"></param>
        public FreObjectSharp(string className, ArrayList args) {
            uint resultPtr = 0;
            _freObject = FreSharpHelper.Core.getFREObject(className, FreSharpHelper.ArgsToArgv(args),
                FreSharpHelper.GetArgsC(args), ref resultPtr);
        }

        /// <summary>
        /// Calls a method on a C# FREObject.
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public FreObjectSharp CallMethod(string methodName, ArrayList args) {
            uint resultPtr = 0;
            var ret = new FreObjectSharp(FreSharpHelper.Core.callMethod(_freObject, methodName,
                FreSharpHelper.ArgsToArgv(args), FreSharpHelper.GetArgsC(args), ref resultPtr));
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) {
                return ret;
            }
            ThrowFreException(status, "cannot call method " + methodName);
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <param name="returnArray"></param>
        /// <returns></returns>
        public FreArraySharp CallMethod(string methodName, ArrayList args, bool returnArray) {
            uint resultPtr = 0;
            var ret = new FreArraySharp(FreSharpHelper.Core.callMethod(_freObject, methodName,
                FreSharpHelper.ArgsToArgv(args), FreSharpHelper.GetArgsC(args), ref resultPtr));
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) {
                return ret;
            }
            ThrowFreException(status, "cannot call method " + methodName);
            return null;
        }

        /// <summary>
        /// Returns the C# FREObject as a string.
        /// </summary>
        /// <returns></returns>
        public string GetAsString() {
            uint resultPtr = 0;
            var ret = FreSharpHelper.Core.getString(_freObject, ref resultPtr);
            var status = (FreResultSharp)resultPtr;

            if (status == FreResultSharp.Ok) {
                return ret;
            }
            ThrowFreException(status, "cannot get FREObject as String");
            return "";
        }

        internal void ThrowFreException(FreResultSharp status, string message) {
            switch (status) {
                case FreResultSharp.FreActionscriptError:
                    throw new FreActionscriptErrorException(message);
                case FreResultSharp.Ok:
                    break;
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }

        /// <summary>
        /// Returns the C# FREObject as a uint.
        /// </summary>
        /// <returns></returns>
        public uint GetAsUInt() {
            uint resultPtr = 0;
            var ret = FreSharpHelper.Core.getUInt32(_freObject, ref resultPtr);
            var status = (FreResultSharp)resultPtr;

            if (status == FreResultSharp.Ok) {
                return ret;
            }
            ThrowFreException(status, "cannot get FREObject as Uint");
            return 0;
        }

        /// <summary>
        /// Returns the C# FREObject as an int.
        /// </summary>
        /// <returns></returns>
        public int GetAsInt() {
            uint resultPtr = 0;
            var ret = FreSharpHelper.Core.getInt32(_freObject, ref resultPtr);
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) {
                return ret;
            }
            ThrowFreException(status, "cannot get FREObject as Int");
            return 0;
        }

        /// <summary>
        /// Returns the C# FREObject as a bool.
        /// </summary>
        /// <returns></returns>
        public bool GetAsBool() {
            uint resultPtr = 0;
            var ret = FreSharpHelper.Core.getBool(_freObject, ref resultPtr);
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) {
                return ret;
            }
            ThrowFreException(status, "cannot get FREObject as Bool");
            return false;
        }

        /// <summary>
        /// Returns the C# FREObject as a double.
        /// </summary>
        /// <returns></returns>
        public double GetAsDouble() {
            uint resultPtr = 0;
            var ret = FreSharpHelper.Core.getDouble(_freObject, ref resultPtr);
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) {
                return ret;
            }
            ThrowFreException(status, "cannot get FREObject as Double");
            return 0.0;
        }

        /// <summary>
        /// Returns the property of the C# FREObject of the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FreObjectSharp GetProperty(string name) {
            uint resultPtr = 0;
            var ret = new FreObjectSharp(FreSharpHelper.Core.getProperty(_freObject, name, ref resultPtr));
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) {
                return ret;
            }
            ThrowFreException(status, "cannot get property " + name);
            return null;
        }

        /// <summary>
        /// Sets the property of the C# FREObject as a C# FREObject value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetProperty(string name, FreObjectSharp value) {
            uint resultPtr = 0;
            FreSharpHelper.Core.setProperty(_freObject, name, value.Get(), ref resultPtr);
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) {
                return;
            }
            ThrowFreException(status, "cannot set property " + name);
        }

        /// <summary>
        /// Sets the property of the C# FREObject as a string.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetProperty(string name, string value) {
            uint resultPtr = 0;
            FreSharpHelper.Core.setProperty(_freObject, name, value, ref resultPtr);
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) {
                return;
            }
            ThrowFreException(status, "cannot set property " + name);
        }

        /// <summary>
        /// Sets the property of the C# FREObject as a double.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetProperty(string name, double value) {
            uint resultPtr = 0;
            FreSharpHelper.Core.setProperty(_freObject, name, value, ref resultPtr);
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) {
                return;
            }
            ThrowFreException(status, "cannot set property " + name);
        }

        /// <summary>
        /// Sets the property of the C# FREObject as a bool.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetProperty(string name, bool value) {
            uint resultPtr = 0;
            FreSharpHelper.Core.setProperty(_freObject, name, value, ref resultPtr);
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) {
                return;
            }
            ThrowFreException(status, "cannot set property " + name);
        }

        /// <summary>
        /// Sets the property of the C# FREObject as an int.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetProperty(string name, int value) {
            uint resultPtr = 0;
            FreSharpHelper.Core.setProperty(_freObject, name, value, ref resultPtr);
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) {
                return;
            }
            ThrowFreException(status, "cannot set property " + name);
        }

        /// <summary>
        /// Sets the property of the C# FREObject as a uint.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetProperty(string name, uint value) {
            uint resultPtr = 0;
            FreSharpHelper.Core.setProperty(_freObject, name, value, ref resultPtr);
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) {
                return;
            }
            ThrowFreException(status, "cannot set property " + name);
        }

        /// <summary>
        /// Returns the Actionscript type of the C# FREObject. !Important - your ane must include ANEUtils.as in com.tuarua
        /// </summary>
        /// <returns></returns>
        public new FreObjectTypeSharp GetType() {
            uint resultPtr = 0;
            var type = (FreObjectTypeSharp)FreSharpHelper.Core.getType(_freObject, ref resultPtr);
            return FreObjectTypeSharp.Number == type || FreObjectTypeSharp.Object == type
                ? GetActionscriptType()
                : type;
        }

        private FreObjectTypeSharp GetActionscriptType() {
            var aneUtils = new FreObjectSharp("com.tuarua.ANEUtils", null);
            var args = new ArrayList {
                new FreObjectSharp(_freObject)
            };
            var classType = aneUtils.CallMethod("getClassType", args);
            var type = classType.GetAsString().ToLower();
            switch (type) {
                case "object":
                    return FreObjectTypeSharp.Object;
                case "int":
                    return FreObjectTypeSharp.Int;
                case "number":
                    return FreObjectTypeSharp.Number;
                case "boolean":
                    return FreObjectTypeSharp.Boolean;
                default:
                    return FreObjectTypeSharp.Custom;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetAsDictionary() {
            var ret = new Dictionary<string, object>();
            var aneUtils = new FreObjectSharp("com.tuarua.ANEUtils", null);
            var paramsArray = new ArrayList {
                this
            };
            var classProps = aneUtils.CallMethod("getClassProps", paramsArray, true);
            if (classProps == null) return ret;
            var arrayLength = classProps.GetLength();
            for (uint i = 0; i < arrayLength; i++) {
                var elem = classProps.GetObjectAt(i);
                var propNameAs = elem.GetProperty("name");
                var propName = propNameAs.GetAsString();
                var propVal = GetProperty(propName);
                ret.Add(propName, propVal.GetAsObject());
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object GetAsObject() {
            var objectType = GetType();
            switch (objectType) {
                case FreObjectTypeSharp.Object:
                case FreObjectTypeSharp.Custom:
                    return GetAsDictionary();
                case FreObjectTypeSharp.Number:
                    return GetAsDouble();
                case FreObjectTypeSharp.String:
                    return GetAsString();
                case FreObjectTypeSharp.Bytearray: //TODO
                    break;
                case FreObjectTypeSharp.Array:
                case FreObjectTypeSharp.Vector:  //TODO
                    break;
                case FreObjectTypeSharp.Bitmapdata: //TODO
                    break;
                case FreObjectTypeSharp.Boolean:
                    return GetAsBool();
                case FreObjectTypeSharp.Null:
                    return null;
                case FreObjectTypeSharp.Int:
                    return GetAsInt();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return true;
        }

        /// <summary>
        /// Returns the associated C FREObject of the C# FREObject.
        /// </summary>
        /// <returns></returns>
        public IntPtr Get() {
            return _freObject;
        }

    }
}