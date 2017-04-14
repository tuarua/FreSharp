using System;
using System.Collections;
using System.Linq;

namespace FreSharp {

    public class FreObjectSharp {

        private readonly IntPtr _freObject = IntPtr.Zero;

        public FreObjectSharp() {}

        public FreObjectSharp(IntPtr freObject) {
            _freObject = freObject;
        }
        public FreObjectSharp(string value) {
            _freObject = FreSharpHelper.Core.getFREObject(value);
        }
        public FreObjectSharp(bool value) {
            _freObject = FreSharpHelper.Core.getFREObject(value);
        }
        public FreObjectSharp(double value) {
            _freObject = FreSharpHelper.Core.getFREObject(value);
        }
        public FreObjectSharp(int value) {
            _freObject = FreSharpHelper.Core.getFREObject(value);
        }
        public FreObjectSharp(uint value) {
            _freObject = FreSharpHelper.Core.getFREObject(value);
        }

        public FreObjectSharp(string className, ArrayList args) {
            _freObject = FreSharpHelper.Core.getFREObject(className, FreSharpHelper.ArgsToArgv(args), 
                FreSharpHelper.GetArgsC(args));
        }

        public FreObjectSharp CallMethod(string methodName, ArrayList args) {
            return new FreObjectSharp(FreSharpHelper.Core.callMethod(_freObject, methodName, 
                FreSharpHelper.ArgsToArgv(args), FreSharpHelper.GetArgsC(args)));
        }

        public string GetAsString() {
            return FreSharpHelper.Core.getString(_freObject);
        }

        public uint GetAsUInt() {
            return FreSharpHelper.Core.getUInt32(_freObject);
        }

        public int GetAsInt() {
            return FreSharpHelper.Core.getInt32(_freObject);
        }

        public bool GetAsBool() {
            return FreSharpHelper.Core.getBool(_freObject);
        }

        public double GetAsDouble() {
            return FreSharpHelper.Core.getDouble(_freObject);
        }

        public FreObjectSharp GetProperty(string name) {
            return new FreObjectSharp(FreSharpHelper.Core.getProperty(_freObject, name));
        }

        public void SetProperty(string name, FreObjectSharp value) {
            FreSharpHelper.Core.setProperty(_freObject, name, value.Get());
        }

        public void SetProperty(string name, string value) {
            FreSharpHelper.Core.setProperty(_freObject,name,value);
        }

        public void SetProperty(string name, double value) {
            FreSharpHelper.Core.setProperty(_freObject, name, value);
        }

        public void SetProperty(string name, bool value) {
            FreSharpHelper.Core.setProperty(_freObject, name, value);
        }

        public void SetProperty(string name, int value) {
            FreSharpHelper.Core.setProperty(_freObject, name, value);
        }

        public void SetProperty(string name, uint value) {
            FreSharpHelper.Core.setProperty(_freObject, name, value);
        }

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

        public IntPtr Get() {
            return _freObject;
        }

    }
}