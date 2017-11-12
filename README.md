# FreSharp

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=5UR2T52J633RC)

### Features
 - Build Adobe Air Native Extensions using C#

A basic starter project is included here https://github.com/tuarua/FreSharp/tree/master/starter_project

A complete feature rich example is in https://github.com/tuarua/FreSharp/tree/master/framework_src/example

The package is hosted on NuGet at https://www.nuget.org/packages/TuaRua.FreSharp/


----------

The ANE is comprised of 3 parts.

1. A C++ dll which is packaged as your ANE. It exposes methods to AIR and acts as a thin C++ API layer to the C# code.
2. A C# dll which contains the main logic of the ANE.
3. 2 dlls (FreSharp.dll, FreSharpCore.dll) which contain the translation of FlashRuntimeExtensions to C#.


* For debug builds, the 3 dlls(FreSharp.dll, FreSharpCore.dll, HelloWorldLib.dll) need to be copied to the same folder as adl.exe in your AIRSDK. 
* For release builds, the 3 dlls(FreSharp.dll, FreSharpCore.dll, HelloWorldLib.dll) need to be packaged in the same folder as your exe.  
It is highly recommended you package your app for release using an installer.  
Please see the win_installer/ folder for an example Inno Setup project which handles .NET 4.6 and MSV2015 dependencies.

* This ANE was built with MS Visual Studio 2015. As such your machine (and user's machines) will need to have Microsoft Visual C++ 2015 Redistributable (x86) runtime installed.
https://www.microsoft.com/en-us/download/details.aspx?id=48145

* This ANE also uses .NET 4.6 Framework. As such your machine (and user's machines) will need to have to have this installed.
https://www.microsoft.com/en-us/download/details.aspx?id=48130

----------

HelloWorldANE/HelloWorldANE.cpp is the entry point of the ANE. It acts as a thin layered API to your C# controller.  
Add the number of methods here 

````C++
static FRENamedFunction extensionFunctions[] = {
     MAP_FUNCTION(load)
    ,MAP_FUNCTION(goBack)
};
`````
    

HelloWorldANELib/MainController.cs  
Add C# method(s) to the functionsToSet Dictionary in getFunctions()

````C#
FunctionsDict = new Dictionary<string, Func<FREObject, uint, FREObject[], FREObject>> {
     {"load", Load}
    ,{"goBack", GoBack}
};
`````


Add C# method(s)

````C#
public FREObject Load(FREContext ctx, uint argc, FREObject[] argv) {
    //your code here
    return FREObject.Zero;
}

public FREObject GoBack(FREContext ctx, uint argc, FREObject[] argv) {
    //your code here
    return FREObject.Zero;
}
`````

----------

### How to use
###### Converting from FREObject args into C# types, returning FREObjects
The following table shows the primitive as3 types which can easily be converted to/from C# types


| AS3 type | C# type | AS3 param->C# | return C#->AS3 |
|:--------:|:--------:|:--------------|:-----------|
| String | string | `var str = argv[0].AsString()` | `return str.ToFREObject()`|
| int | int | `var i = argv[0].AsInt()` | `return i.ToFREObject()`|
| Boolean | bool | `var b = argv[0].AsBool()` | `return b.ToFREObject()`|
| Number | double | `var dbl = argv[0].AsDouble()` | `return dbl.ToFREObject()`|
| Rectangle | Rect | `var rect = argv[0].AsRect()` | `return rect.ToFREObject()` |
| Point | Point | `var pnt = argv[0].AsPoint()` | `return pnt.ToFREObject()` |
| Object | Dictionary | `var dct = argv[0].AsDictionary()` | N/A |


Example - Convert a FREObject into a String, and String into FREObject

````C#
try {
   var airString = argv[0].AsString();
   Trace("String passed from AIR:" + airString);
}
catch (Exception e) {
    Console.WriteLine(@"caught in C#: type: {0} message: {1}", e.GetType(), e.Message);
}
const string sharpString = "I am a string from C#";
return sharpString.ToFREObject();
`````

Example - Call a method on an FREObject
````C#
var person = argv[0];
var addition = person.Call("add", 100, 33);
Trace("result is: ", addition.AsInt());
`````

Example - Get a property of a FREObject
````C#
var oldAge = person.GetProp("age").AsInt();
Trace("result is: ", oldAge);
`````

Example - Convert a FREObject Object into a Dictionary
````C#
var dictionary = person.AsDictionary();
var name = dictionary["name"];
`````

Example - Create a new FREObject
````C#
var newPerson = new FREObject().Init("com.tuarua.Person");
Trace("We created a new person. type =", newPerson.Type());
`````

Example - Error handling
````C#
var testString = argv[0];
try {
    testString.Call("noStringFunc"); //call method on a string
}
catch (Exception e) {
    return new FreException(e).RawValue; //return as3 error and throw in swc
}
`````

Advanced: AIR Native Stage. Adding native elements over the AIR window  
The example includes AIRNativeANE for creating a child Window which can be used to contain WPF controls and graphics.
In Windows 8.1+ the Window can be transparent. 
Buttons and images can be added easily from actionscript.

````actionscript
ANStage.init(stage, new Rectangle(100, 0, 400, 600), true, true);
ANStage.add();

var nativeButton = new ANButton(new TestButton(), new TestButtonHover());
nativeButton.addEventListener(MouseEvent.CLICK, onNativeClick);
var nativeSprite:ANSprite = new ANSprite();
nativeSprite.x = 150;

var nativeImage:ANImage = new ANImage(new TestImage());
ANStage.addChild(nativeSprite);
ANStage.addChild(nativeImage);
ANStage.addChild(nativeButton);
`````
Advanced: Extending FreObjectSharp. Creating a C# version of flash.geom.point

````C#
using System.Collections;
using System.Windows;
using TuaRua.FreSharp;
using FREObject = System.IntPtr;

namespace FreSharp.Geom {
    public class FrePointSharp : FreObjectSharp {
        public FrePointSharp() { }
        public FrePointSharp(FREObject freObject) {
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
            FreSharpHelper.ThrowFreException(status, "cannot create point ", RawValue);
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
            FreSharpHelper.ThrowFreException(status, "cannot copyFrom ", FREObject.Zero);
        }
        public new Point Value => new Point(
            RawValue.GetProp("x").AsDouble(), 
            RawValue.GetProp("y").AsDouble());
    }
}
`````

### Tech

Uses .NET 4.6

### Prerequisites

You will need
 
 - Visual Studio 2017
 - AIR 19+ SDK