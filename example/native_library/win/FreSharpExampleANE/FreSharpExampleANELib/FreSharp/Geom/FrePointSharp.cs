using System;
using System.Collections;
using System.Windows;
using TuaRua.FreSharp;

namespace FreSharp.Geom {
    public class FrePointSharp : FreObjectSharp {
        public FrePointSharp() { }

        public FrePointSharp(IntPtr freObject) {
            RawValue = freObject;
        }

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
            FreSharpHelper.ThrowFreException(status, "cannot create point ", this);
        }

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
            FreSharpHelper.ThrowFreException(status, "cannot copyFrom ", this);
        }

        public new Point Value => new Point(
            Convert.ToDouble(GetProperty("x").Value),
            Convert.ToDouble(GetProperty("y").Value));
    }
}