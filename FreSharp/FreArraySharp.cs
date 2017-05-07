using System;
using System.Collections;
using TuaRua.FreSharp.Display;

namespace TuaRua.FreSharp {
    /// <summary>
    ///  FreArraySharp wraps a C FREObject (Array or Vector) with helper methods.
    /// </summary>
    public class FreArraySharp : FreObjectSharp {
        /// <summary>
        /// Creates an Empty C# FreArray.
        /// </summary>
        public FreArraySharp() { }
        /// <summary>
        /// Creates a C# FreArray from a C FREObject.
        /// </summary>
        /// <param name="freArray"></param>
        public FreArraySharp(IntPtr freArray) {
            RawValue = freArray;
        }

        /// <summary>
        /// Returns the length of the C# FreArray.
        /// </summary>
        /// <returns></returns>
        public uint GetLength() {
            uint resultPtr = 0;
            var ret = FreSharpHelper.Core.getArrayLength(RawValue, ref resultPtr);
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) {
                return ret;
            }
            FreSharpHelper.ThrowFreException(status, "cannot get length of array", null);
            return 0;
        }

        /// <summary>
        /// Returns the C# FreObject from the C# FreArray at i.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public FreObjectSharp GetObjectAt(uint i) {
            uint resultPtr = 0;
            return new FreObjectSharp(FreSharpHelper.Core.getObjectAt(RawValue, i, ref resultPtr));
        }

        /// <summary>
        /// Sets the C# FreObject in the C# FreArray at i.
        /// </summary>
        public void SetObjectAt(FreObjectSharp value, uint i) {
            uint resultPtr = 0;
            FreSharpHelper.Core.setObjectAt(RawValue, i, value.RawValue, ref resultPtr);
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
                    case FreObjectTypeSharp.String:
                        al.Add(FreSharpHelper.GetAsString(RawValue));
                        break;
                    case FreObjectTypeSharp.Bytearray: //TODO
                        break;
                    case FreObjectTypeSharp.Array:
                    case FreObjectTypeSharp.Vector: //TODO
                        break;
                    case FreObjectTypeSharp.Bitmapdata:
                        var bmdFre = new FreBitmapDataSharp(RawValue);
                        al.Add(bmdFre.GetAsBitmap());
                        break;
                    case FreObjectTypeSharp.Boolean:
                        al.Add(FreSharpHelper.GetAsBool(RawValue));
                        break;
                    case FreObjectTypeSharp.Null:
                        break;
                    case FreObjectTypeSharp.Int:
                        al.Add(FreSharpHelper.GetAsInt(RawValue));
                        break;
                    case FreObjectTypeSharp.Object:
                    case FreObjectTypeSharp.Class:
                        al.Add(FreSharpHelper.GetAsDictionary(RawValue));
                        break;
                    case FreObjectTypeSharp.Number:
                        al.Add(FreSharpHelper.GetAsDouble(RawValue));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return al;
        }

    }
}
