using System.Collections;
using System.Windows;
using TuaRua.FreSharp;
using FREObject = System.IntPtr;

namespace FreSharp.Geom {
    /// <summary>
    /// 
    /// </summary>
    public class FrePointSharp : FreObjectSharp {
        /// <summary>
        /// 
        /// </summary>
        public FrePointSharp() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="freObject"></param>
        public FrePointSharp(FREObject freObject) {
            RawValue = freObject;
        }

        /// <summary>
        /// 
        /// </summary>
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
        /// <param name="sourcePoint"></param>
        public void CopyFrom(FrePointSharp sourcePoint) {
            uint resultPtr = 0;
            var args = new ArrayList {
                sourcePoint.RawValue,
            };
            FreSharpHelper.Core.callMethod(RawValue, "copyFrom", FreSharpHelper.ArgsToArgv(args),
                FreSharpHelper.GetArgsC(args), ref resultPtr);


            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) {
                return;
            }
            FreSharpHelper.ThrowFreException(status, "cannot copyFrom ", FREObject.Zero);
        }

        /// <summary>
        /// 
        /// </summary>
        public new Point Value => new Point(
            RawValue.GetProp("x").AsDouble(), 
            RawValue.GetProp("y").AsDouble());
    }
}