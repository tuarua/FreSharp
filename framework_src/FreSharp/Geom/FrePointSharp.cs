using System.Collections;
using System.Windows;
using TuaRua.FreSharp;
using FREObject = System.IntPtr;

namespace FreSharp.Geom {
    /// <summary>
    /// </summary>
    public class FrePointSharp {
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public FrePointSharp() { }

        /// <summary>
        /// Returns the associated C FREObject of the C# FREObject.
        /// </summary>
        /// <returns></returns>
        public FREObject RawValue { get; set; } = FREObject.Zero;

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="freObject"></param>
        public FrePointSharp(FREObject freObject) {
            RawValue = freObject;
        }

        /// <inheritdoc />
        /// <summary></summary>
        /// <param name="value"></param>
        public FrePointSharp(Point value) {
            uint resultPtr = 0;
            var args = new ArrayList {
                value.X,
                value.Y
            };

            RawValue = FreSharpHelper.Core.getFREObject("flash.geom.Point", FreSharpHelper.ArgsToArgv(args),
                FreSharpHelper.GetArgsC(args), ref resultPtr);
            var status = (FreResultSharp) resultPtr;

            if (status == FreResultSharp.Ok) {
                return;
            }
            FreSharpHelper.ThrowFreException(status, "cannot create point ", RawValue);
        }

        
        /// <summary>
        /// 
        /// </summary>
        public Point Value => new Point(
            RawValue.GetProp("x").AsDouble(), 
            RawValue.GetProp("y").AsDouble());
    }
}