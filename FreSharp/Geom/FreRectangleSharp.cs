using System;
using System.Collections;
using System.Drawing;

namespace TuaRua.FreSharp.Geom {
    /// <summary>
    /// FreRectangleSharp wraps a flash.geom.Rectangle with helper methods.
    /// </summary>
    public class FreRectangleSharp : FreObjectSharp {
        /// <summary>
        /// Creates an empty FreRectangleSharp
        /// </summary>
        public FreRectangleSharp() { }
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
        public FreRectangleSharp(Rectangle value) {
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
            FreSharpHelper.ThrowFreException(status, "cannot create rectangle ", this);
        }

        /// <summary>
        /// Returns the C# FREObject as a Rectangle.
        /// </summary>
        public new Rectangle Value => new Rectangle((int)GetProperty("x").Value,
            (int)GetProperty("y").Value,
            (int)GetProperty("width").Value,
            (int)GetProperty("height").Value);
    }
}