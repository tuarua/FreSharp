

# FreSharp

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=5UR2T52J633RC)

### Features
 - Build Adobe Air Native Extensions using C#

The package is hosted on NuGet at https://www.nuget.org/packages/TuaRua.FreSharp/

----------

### Getting Started

A basic Hello World [starter project](/starter_project) is included 


### How to use
###### Converting from FREObject args into C# types, returning FREObjects
The following table shows the primitive as3 types which can easily be converted to/from C# types


| AS3 type | C# type | AS3 param->C# | return C#->AS3 |
|:--------:|:--------:|:--------------|:-----------|
| String | string | `var str = argv[0].AsString()` | `return str.ToFREObject()`|
| int | int | `var i = argv[0].AsInt()` | `return i.ToFREObject()`|
| Boolean | bool | `var b = argv[0].AsBool()` | `return b.ToFREObject()`|
| Number | double | `var dbl = argv[0].AsDouble()` | `return dbl.ToFREObject()`|
| uint ARGB | Color | `var clr = argv[0].AsColor()` | `return clr.ToFREObject()`|
| Date | DateTime | `var dt = argv[0].AsDateTime()` | `return dt.ToFREObject()`|
| Rectangle | Rect | `var rect = argv[0].AsRect()` | `return rect.ToFREObject()` |
| Point | Point | `var pnt = argv[0].AsPoint()` | `return pnt.ToFREObject()` |
| BitmapData | Bitmap | `var bmp = argv[0].AsBitmap()` | `return bmp.ToFREObject()` |
| Array | string[] | `var arr = argv[0].AsStringArray()` | `return arr.ToFREObject()`|
| Array | int[] | `var arr = argv[0].AsIntArray()` | `return arr.ToFREObject()`|
| Array | double[] | `var arr = argv[0].AsDoubleArray()` | `return arr.ToFREObject()`|
| Array | bool[] | `var arr = argv[0].AsBoolArray()` | `return arr.ToFREObject()`|
| Object | Dictionary | `var dct = argv[0].AsDictionary()` | N/A |

#### Basic Types
```C#
string myString = argv[0].AsString();
int myInt = argv[1].AsInt();
bool myBool = argv[2].AsBool();

const string sharpString = "I am a string from C#";
return sharpString.ToFREObject();
```

#### Creating new FREObjects
```C#
var frePerson = new FREObject().Init("com.tuarua.Person");

// create a FREObject passing args
// 
// The following param types are allowed: 
// int, uint, short, long, bool, string, double, FREObject
var frePerson = new FREObject().Init("com.tuarua.Person", "Bob", "Doe", 28, myFREObject);
```

#### Calling Methods
```C#
// call a FREObject method passing args
// 
// The following param types are allowed: 
// int, uint, short, long, bool, string, double, FREObject
var addition = freCalculator.Call("add", 100, 33);
```

#### Getting / Setting Properties
```C#
var oldAge = person.GetProp("age").AsInt();
var newAge = oldAge + 10;

// The following param types are allowed: 
// int, uint, short, long, bool, string, double, FREObject
person.SetProp("age", newAge);
```

#### Arrays
```C#
var inFre0 = new FREArray(argv[0]);
// convert to a C# [string]
var airStringVector = inFre0.AsStringArray();

// create a Vector.<com.tuarua.Person> with fixed length of 5
var newFreArray = new FREArray("com.tuarua.Person", 5, true);
var len = newFreArray.Length;

// loop over FREArray
foreach (var fre in freIntArray) {
    Trace(fre.AsInt());
}

// set element 1 to 123
freIntArray[1] = 123.ToFREObject();

// return C# [int] to AIR
var marks = new[] {99, 98, 92, 97, 95};
return marks.ToFREObject();
```

#### Sending Events back to AIR

```C#
Trace("Hi", "There");

// with interpolation
Trace($"My name is: {name}");

DispatchEvent(name: "MY_EVENT", value: "this is a test"); 
```

#### Bitmapdata
```C#
// read AS3 bitmapData into a Bitmap
var bitmap = new FreBitmapDataSharp(argv[0]).AsBitmap();

return bitmap.ToFREObject();
```

#### ByteArrays
```C#
var ba = new FreByteArraySharp(inFre);
ba.Acquire();
var byteData = ba.Bytes;
var base64Encoded = Convert.ToBase64String(byteData);
ba.Release();
```

#### Error Handling
```C#
try {
    testString.Call("noStringFunc"); //call method on a string
}
catch (Exception e) {
    return new FreException(e).RawValue; //return as3 error and throw in swc
}
```

Advanced: Extending FreObjectSharp. Creating a C# version of flash.geom.point

```C#
using System.Collections;
using System.Windows;
using TuaRua.FreSharp;
using FREObject = System.IntPtr;

namespace FreSharp.Geom {
    public class FrePointSharp {
        public FrePointSharp() { }

        public FREObject RawValue { get; set; } = FREObject.Zero;

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

        
        public Point Value => new Point(
            RawValue.GetProp("x").AsDouble(), 
            RawValue.GetProp("y").AsDouble());
    }
}
public static Point AsPoint(this FREObject inFre) => new FrePointSharp(inFre).Value;
public static FREObject ToFREObject(this Point point) => new FrePointSharp(point).RawValue;
```

### Tech

Uses .NET 4.6

### Prerequisites

You will need
 
 - Visual Studio 2017
 - AIR 19+ SDK
