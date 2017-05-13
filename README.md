# FreSharp

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=5UR2T52J633RC)

### Features
 - Build Adobe Air Native Extensions using C#

Please see the feature rich example in https://github.com/tuarua/FreSharp/tree/master/example

The package is hosted on NuGet at https://www.nuget.org/packages/TuaRua.FreSharp/

----------

### How to use

Example - Convert a FREObject into a String, and String into FREObject

````C#
var inFre = argv[0];
try {
   var airString = new FreObjectSharp(inFre).Value as string;
   Trace("String passed from AIR:" + airString);
}
catch (Exception e) {
  Console.WriteLine(@"caught in C#: type: {0} message: {1}", e.GetType(), e.Message);
}
const string sharpString = "I am a string from C#";
return new FreObjectSharp(sharpString).RawValue;
`````

Example - Call a method on an FREObject

````C#
var addition = person.CallMethod("add", new ArrayList {
    100,
    33
});
var sum = (int) addition.Value;
Trace("result is: " + sum);
`````

Advanced Example - Extending FreObjectSharp. Creating a C# version of flash.geom.point

````C#
using System;
using System.Collections;
using System.Drawing;
using TuaRua.FreSharp;

namespace FreSharp.Geom {
    public class FrePointSharp : FreObjectSharp {
        public FrePointSharp() {
        }

        public FrePointSharp(IntPtr freObject) {
            RawValue = freObject;
        }

        public FrePointSharp(Point value) {
            uint resultPtr = 0;
            var args = new ArrayList
            {
                value.X,
                value.Y
            };

            RawValue = FreSharpHelper.Core.getFREObject("flash.geom.Point", FreSharpHelper.ArgsToArgv(args),
                FreSharpHelper.GetArgsC(args), ref resultPtr);
            var status = (FreResultSharp)resultPtr;

            if (status == FreResultSharp.Ok) {
                return;
            }
            FreSharpHelper.ThrowFreException(status, "cannot create point ", this);
        }


        public void CopyFrom(FrePointSharp sourcePoint) {
            uint resultPtr = 0;
            var args = new ArrayList
            {
                sourcePoint.RawValue,
            };
            FreSharpHelper.Core.callMethod(RawValue, "copyFrom", FreSharpHelper.ArgsToArgv(args),
                FreSharpHelper.GetArgsC(args), ref resultPtr);

            var status = (FreResultSharp)resultPtr;
            if (status == FreResultSharp.Ok) {
                return;
            }
            FreSharpHelper.ThrowFreException(status, "cannot copyFrom ", this);
        }

        public new Point Value => new Point((int)GetProperty("x").Value,
            (int)GetProperty("y").Value);
    }
}

`````

### Tech

Uses .NET 4.5.2

### Prerequisites

You will need
 
 - Visual Studio 2015
 - AIR 19+ SDK

### Todos
