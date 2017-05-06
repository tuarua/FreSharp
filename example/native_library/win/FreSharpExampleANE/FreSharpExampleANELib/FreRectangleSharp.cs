using System;
using System.Collections;
using System.Drawing;
using FreSharp;

namespace FreExampleSharpLib {
    public class FreRectangleSharp : FreObjectSharp {
        private readonly IntPtr _freObject = IntPtr.Zero;

        public FreRectangleSharp() { }

        public FreRectangleSharp(IntPtr freObject) {
            _freObject = freObject;
        }

        public FreRectangleSharp(Rectangle value) {
            uint resultPtr = 0;
            var args = new ArrayList {
                new FreObjectSharp(value.X),
                new FreObjectSharp(value.Y),
                new FreObjectSharp(value.Width),
                new FreObjectSharp(value.Height)
            };

            _freObject = FreSharpHelper.Core.getFREObject("flash.geom.Rectangle", FreSharpHelper.ArgsToArgv(args),
                FreSharpHelper.GetArgsC(args), ref resultPtr);
            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) {
                return;
            }
            Console.WriteLine(@"got un erreur");
            //it's internal set to public
            //ThrowFreException(status, "cannot create class " + className, this);
        }

        public Rectangle GetAsRectangle() {
            var rect = new Rectangle(GetProperty("x").GetAsInt(), 
                GetProperty("y").GetAsInt(), 
                GetProperty("width").GetAsInt(), 
                GetProperty("height").GetAsInt());
            return rect;
        }

        public new IntPtr Get() {
            return _freObject;
        }
    }
}