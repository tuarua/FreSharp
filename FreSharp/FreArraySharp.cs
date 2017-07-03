using System;
using System.Collections;
using TuaRua.FreSharp.Display;
using TuaRua.FreSharp.Geom;

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
        public uint Length {
            get {
                uint resultPtr = 0;
                var ret = FreSharpHelper.Core.getArrayLength(RawValue, ref resultPtr);
                var status = (FreResultSharp) resultPtr;
                if (status == FreResultSharp.Ok) {
                    return ret;
                }
                FreSharpHelper.ThrowFreException(status, "cannot get length of array", null);
                return 0;
            }
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
            var len = Length;
            if (len <= 0) return al;
            for (uint i = 0; i < len; i++) {
                var itm = GetObjectAt(i);
                var type = itm.GetType();
                switch (type) {
                    case FreObjectTypeSharp.String:
                        al.Add(FreSharpHelper.GetAsString(itm.RawValue));
                        break;
                    case FreObjectTypeSharp.Bytearray:
                        var ba = new FreByteArraySharp(itm.RawValue);
                        ba.Acquire();
                        var baTarget = new byte[ba.Length];
                        ba.Bytes.CopyTo(baTarget, 0);
                        al.Add(baTarget);
                        ba.Release();
                        break;
                    case FreObjectTypeSharp.Array:
                    case FreObjectTypeSharp.Vector:
                        var arrFre = new FreArraySharp(itm.RawValue);
                        al.Add(arrFre.GetAsArrayList());
                        break;
                    case FreObjectTypeSharp.Bitmapdata:
                        var bmdFre = new FreBitmapDataSharp(itm.RawValue);
                        al.Add(bmdFre.GetAsBitmap());
                        break;
                    case FreObjectTypeSharp.Boolean:
                        al.Add(FreSharpHelper.GetAsBool(itm.RawValue));
                        break;
                    case FreObjectTypeSharp.Null:
                        break;
                    case FreObjectTypeSharp.Int:
                        al.Add(FreSharpHelper.GetAsInt(itm.RawValue));
                        break;
                    case FreObjectTypeSharp.Object:
                    case FreObjectTypeSharp.Class:
                        al.Add(FreSharpHelper.GetAsDictionary(itm.RawValue));
                        break;
                    case FreObjectTypeSharp.Number:
                        al.Add(FreSharpHelper.GetAsDouble(itm.RawValue));
                        break;
                    case FreObjectTypeSharp.Rectangle:
                        var rectFre = new FreRectangleSharp(itm.RawValue);
                        al.Add(rectFre.Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return al;
        }
    }
}