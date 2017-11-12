using System;
using System.Collections;
using TuaRua.FreSharp.Display;
using TuaRua.FreSharp.Geom;
using FREObject = System.IntPtr;
namespace TuaRua.FreSharp {
    /// <summary>
    ///  FREArray wraps a C FREObject (Array or Vector) with helper methods.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class FREArray : FreObjectSharp {
        /// <summary>
        /// Creates an Empty C# FreArray.
        /// </summary>
        public FREArray() { }

        /// <summary>
        /// Creates a C# FreArray from a C FREObject.
        /// </summary>
        /// <param name="freObject"></param>
        public FREArray(FREObject freObject) {
            RawValue = freObject;
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
                FreSharpHelper.ThrowFreException(status, "cannot get length of array", FREObject.Zero);
                return 0;
            }
        }

        /// <summary>
        /// Returns the C# FreObject from the C# FreArray at i.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public FREObject At(uint i) {
            uint resultPtr = 0;
            return new FreObjectSharp(FreSharpHelper.Core.getObjectAt(RawValue, i, ref resultPtr)).RawValue;
        }

        /// <summary>
        /// Sets the C# FreObject in the C# FreArray at i.
        /// </summary>
        
        public void Set(uint index, object value) {
            uint resultPtr = 0;
            var v = new FreObjectSharp(FreSharpHelper.FreObjectSharpFromObject(value).RawValue);
            FreSharpHelper.Core.setObjectAt(RawValue, index, v.RawValue, ref resultPtr);
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
                var itm = At(i);
                var type = itm.Type();
                switch (type) {
                    case FreObjectTypeSharp.String:
                        al.Add(FreSharpHelper.GetAsString(itm));
                        break;
                    case FreObjectTypeSharp.Bytearray:
                        var ba = new FreByteArraySharp(itm);
                        ba.Acquire();
                        var baTarget = new byte[ba.Length];
                        ba.Bytes.CopyTo(baTarget, 0);
                        al.Add(baTarget);
                        ba.Release();
                        break;
                    case FreObjectTypeSharp.Array:
                    case FreObjectTypeSharp.Vector:
                        var arrFre = new FREArray(itm);
                        al.Add(arrFre.GetAsArrayList());
                        break;
                    case FreObjectTypeSharp.Bitmapdata:
                        var bmdFre = new FreBitmapDataSharp(itm);
                        al.Add(bmdFre.GetAsBitmap());
                        break;
                    case FreObjectTypeSharp.Boolean:
                        al.Add(FreSharpHelper.GetAsBool(itm));
                        break;
                    case FreObjectTypeSharp.Null:
                        break;
                    case FreObjectTypeSharp.Int:
                        al.Add(FreSharpHelper.GetAsInt(itm));
                        break;
                    case FreObjectTypeSharp.Object:
                    case FreObjectTypeSharp.Class:
                        al.Add(FreSharpHelper.GetAsDictionary(itm));
                        break;
                    case FreObjectTypeSharp.Number:
                        al.Add(FreSharpHelper.GetAsDouble(itm));
                        break;
                    case FreObjectTypeSharp.Rectangle:
                        var rectFre = new FreRectangleSharp(itm);
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