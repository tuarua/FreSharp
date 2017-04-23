using System;
using System.Collections;

namespace FreSharp {
    /// <summary>
    ///  FreArraySharp wraps a C FREObject (Array or Vector) with helper methods.
    /// </summary>
    public class FreArraySharp : FreObjectSharp {
        private readonly IntPtr _freArray = IntPtr.Zero;
        /// <summary>
        /// Creates an Empty C# FreArray.
        /// </summary>
        public FreArraySharp() { }
        /// <summary>
        /// Creates a C# FreArray from a C FREObject.
        /// </summary>
        /// <param name="freArray"></param>
        public FreArraySharp(IntPtr freArray) {
            _freArray = freArray;
        }

        /// <summary>
        /// Returns the length of the C# FreArray.
        /// </summary>
        /// <returns></returns>
        public uint GetLength() {
            uint resultPtr = 0;
            var ret = FreSharpHelper.Core.getArrayLength(_freArray, ref resultPtr);
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) {
                return ret;
            }
            ThrowFreException(status, "cannot get FREObject as Double");
            return 0;
        }

        /// <summary>
        /// Returns the C# FreObject from the C# FreArray at i.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public FreObjectSharp GetObjectAt(uint i) {
            uint resultPtr = 0;
            return new FreObjectSharp(FreSharpHelper.Core.getObjectAt(_freArray, i, ref resultPtr));
        }

        /// <summary>
        /// Sets the C# FreObject in the C# FreArray at i.
        /// </summary>
        public void SetObjectAt(FreObjectSharp value, uint i) {
            uint resultPtr = 0;
            FreSharpHelper.Core.setObjectAt(_freArray, i, value.Get(), ref resultPtr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ArrayList GetAsArrayList() {
            var al = new ArrayList();
            var len = GetLength();
            if (len <= 0) return al;
            var type = GetType();
            for (uint i = 0; i < GetLength(); i++) {
                switch (type) {
                    case FreObjectTypeSharp.Object:
                        break;
                    case FreObjectTypeSharp.String:
                        al.Add(GetAsString());
                        break;
                    case FreObjectTypeSharp.Bytearray:
                        break;
                    case FreObjectTypeSharp.Array:
                    case FreObjectTypeSharp.Vector:
                        break;
                    case FreObjectTypeSharp.Bitmapdata:
                        break;
                    case FreObjectTypeSharp.Boolean:
                        al.Add(GetAsBool());
                        break;
                    case FreObjectTypeSharp.Null:
                        break;
                    case FreObjectTypeSharp.Int:
                        al.Add(GetAsInt());
                        break;
                    case FreObjectTypeSharp.Custom:
                        break;
                    case FreObjectTypeSharp.Number:
                        al.Add(GetAsDouble());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return al;
        }

        /// <summary>
        /// Returns the associated C FREObject of the C# FREArray.
        /// </summary>
        /// <returns></returns>
        public new IntPtr Get() {
            return _freArray;
        }

    }
}
