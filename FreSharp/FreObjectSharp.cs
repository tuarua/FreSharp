using System;
using System.Collections;

namespace FreSharp {

    /// <summary>
    /// FreObjectSharp wraps a C FREObject with helper methods.
    /// </summary>
    public class FreObjectSharp {

        private readonly IntPtr _freObject = IntPtr.Zero;

        /// <summary>
        /// Creates an empty C# FREObject
        /// </summary>
        public FreObjectSharp() {}

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
            _freObject = FreSharpHelper.Core.getFREObject(value);
        }
        /// <summary>
        /// Creates a C# FREObject from a bool
        /// </summary>
        /// <param name="value"></param>
        public FreObjectSharp(bool value) {
            _freObject = FreSharpHelper.Core.getFREObject(value);
        }
        /// <summary>
        /// Creates a C# FREObject from a double
        /// </summary>
        /// <param name="value"></param>
        public FreObjectSharp(double value) {
            _freObject = FreSharpHelper.Core.getFREObject(value);
        }
        /// <summary>
        /// Creates a C# FREObject from an int
        /// </summary>
        /// <param name="value"></param>
        public FreObjectSharp(int value) {
            _freObject = FreSharpHelper.Core.getFREObject(value);
        }
        /// <summary>
        /// Creates a C# FREObject from a uint
        /// </summary>
        /// <param name="value"></param>
        public FreObjectSharp(uint value) {
            _freObject = FreSharpHelper.Core.getFREObject(value);
        }

        /// <summary>
        /// Creates a C# FREObject with given class name
        /// </summary>
        /// <param name="className"></param>
        /// <param name="args"></param>
        public FreObjectSharp(string className, ArrayList args) {
            _freObject = FreSharpHelper.Core.getFREObject(className, FreSharpHelper.ArgsToArgv(args), 
                FreSharpHelper.GetArgsC(args));
        }

        /// <summary>
        /// Calls a method on a C# FREObject.
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public FreObjectSharp CallMethod(string methodName, ArrayList args) {
            return new FreObjectSharp(FreSharpHelper.Core.callMethod(_freObject, methodName, 
                FreSharpHelper.ArgsToArgv(args), FreSharpHelper.GetArgsC(args)));
        }

        /// <summary>
        /// Returns the C# FREObject as a string.
        /// </summary>
        /// <returns></returns>
        public string GetAsString() {
            return FreSharpHelper.Core.getString(_freObject);
        }

        /// <summary>
        /// Returns the C# FREObject as a uint.
        /// </summary>
        /// <returns></returns>
        public uint GetAsUInt() {
            return FreSharpHelper.Core.getUInt32(_freObject);
        }

        /// <summary>
        /// Returns the C# FREObject as an int.
        /// </summary>
        /// <returns></returns>
        public int GetAsInt() {
            return FreSharpHelper.Core.getInt32(_freObject);
        }

        /// <summary>
        /// Returns the C# FREObject as a bool.
        /// </summary>
        /// <returns></returns>
        public bool GetAsBool() {
            return FreSharpHelper.Core.getBool(_freObject);
        }

        /// <summary>
        /// Returns the C# FREObject as a double.
        /// </summary>
        /// <returns></returns>
        public double GetAsDouble() {
            return FreSharpHelper.Core.getDouble(_freObject);
        }

        /// <summary>
        /// Returns the property of the C# FREObject of the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FreObjectSharp GetProperty(string name) {
            return new FreObjectSharp(FreSharpHelper.Core.getProperty(_freObject, name));
        }

        /// <summary>
        /// Sets the property of the C# FREObject as a C# FREObject value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetProperty(string name, FreObjectSharp value) {
            FreSharpHelper.Core.setProperty(_freObject, name, value.Get());
        }

        /// <summary>
        /// Sets the property of the C# FREObject as a string.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetProperty(string name, string value) {
            FreSharpHelper.Core.setProperty(_freObject,name,value);
        }

        /// <summary>
        /// Sets the property of the C# FREObject as a double.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetProperty(string name, double value) {
            FreSharpHelper.Core.setProperty(_freObject, name, value);
        }

        /// <summary>
        /// Sets the property of the C# FREObject as a bool.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetProperty(string name, bool value) {
            FreSharpHelper.Core.setProperty(_freObject, name, value);
        }

        /// <summary>
        /// Sets the property of the C# FREObject as an int.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetProperty(string name, int value) {
            FreSharpHelper.Core.setProperty(_freObject, name, value);
        }

        /// <summary>
        /// Sets the property of the C# FREObject as a uint.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetProperty(string name, uint value) {
            FreSharpHelper.Core.setProperty(_freObject, name, value);
        }

        /// <summary>
        /// Returns the Actionscript type of the C# FREObject. !Important - your ane must include ANEUtils.as in com.tuarua
        /// </summary>
        /// <returns></returns>
        public new FreObjectTypeSharp GetType() {
            var type = (FreObjectTypeSharp) FreSharpHelper.Core.getType(_freObject);
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
        /// Returns the associated C FREObject of the C# FREObject.
        /// </summary>
        /// <returns></returns>
        public IntPtr Get() {
            return _freObject;
        }

    }
}