using System;
using System.Collections;
using System.Windows;

namespace TuaRua.FreSharp.Geom {
    /// <summary>
    /// FreRectangleSharp wraps a flash.geom.Rectangle with helper methods.
    /// </summary>
    public class FreRectangleSharp {
        /// <summary>
        /// Creates an empty FreRectangleSharp
        /// </summary>
        public FreRectangleSharp() { }

        /// <summary>
        /// Returns the associated C FREObject of the C# FREObject.
        /// </summary>
        /// <returns></returns>
        public IntPtr RawValue { get; set; } = IntPtr.Zero;

        /// <summary>
        /// Creates a C# FREObject from a C FREObject
        /// </summary>
        /// <param name="freObject"></param>
        public FreRectangleSharp(IntPtr freObject) {
            RawValue = freObject;
        }

        /// <summary>
        /// Creates a C# FREObject from a Rectangle
        /// </summary>
        /// <param name="value"></param>
        public FreRectangleSharp(Rect value) {
            uint resultPtr = 0;
            var args = new ArrayList {
               value.X,
               value.Y,
               value.Width,
               value.Height
            };

            RawValue = FreSharpHelper.Core.getFREObject("flash.geom.Rectangle", FreSharpHelper.ArgsToArgv(args),
                FreSharpHelper.GetArgsC(args), ref resultPtr);
            var status = (FreResultSharp)resultPtr;

            if (status == FreResultSharp.Ok) {
                return;
            }
            FreSharpHelper.ThrowFreException(status, "cannot create rectangle ", RawValue);
        }

        /// <summary>
        /// Returns the C# FREObject as a Rect.
        /// </summary>
        public Rect Value => new Rect(
            RawValue.GetProp("x").AsDouble(), 
            RawValue.GetProp("y").AsDouble(), 
            RawValue.GetProp("width").AsDouble(), 
            RawValue.GetProp("height").AsDouble());
    }
}