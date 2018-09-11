# FreSharp

[ ![Download](https://img.shields.io/nuget/v/TuaRua.FreSharp.svg) ](https://www.nuget.org/packages/TuaRua.FreSharp/)

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
| null | FREObject.Zero |  | return FREObject.Zero |

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
// int, uint, short, long, bool, string, double, Rect, Point, DateTime, Color, FREObject
var frePerson = new FREObject().Init("com.tuarua.Person", "Bob", "Doe", 28, myFREObject);
```

#### Calling Methods
```C#
// call a FREObject method passing args
// 
// The following param types are allowed: 
// int, uint, short, long, bool, string, double, Rect, Point, DateTime, Color, FREObject
var addition = freCalculator.Call("add", 100, 33);
```

#### Getting / Setting Properties
```C#
var oldAge = person.GetProp("age").AsInt();
var newAge = oldAge + 10;

// The following param types are allowed: 
// int, uint, short, long, bool, string, double, Rect, Point, DateTime, Color, FREObject
person.SetProp("age", newAge);

// create a FreSharpObject DynamicObject 
dynamic person = new FreObjectSharp("com.tuarua.Person", "Ben McBobster", 80);
int oldAge = person.age; // implicit conversion
var name = (string) person.name; // explicit conversion

// The following prop types are allowed: 
// int, uint, short, long, bool, string, double, Rect, Point, DateTime, Color, FREObject
person.age = oldAge + 10;
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

// push 2 elements to FREArray
freIntArray.Push(22, 33);

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
// Turn on logging to trace out any captured errors in FreSharp
FreSharpLogger.GetInstance().Context = Context;

person.Call("add", 100); // not passing enough args - traces captured error.

try {
    myCSharpFunc(); // call a C# method which can throw
}
catch (Exception e) {
    return new FreException(e).RawValue; // return as3 error and throw in swc
}
```

Advanced: Extending FreObjectSharp. Creating a C# version of flash.geom.point

```C#
using FREObject = System.IntPtr;
using Point = System.Windows.Point;

public static class FrePoint {
    public static FREObject ToFREObject(this Point value) {
        return new FREObject().Init("flash.geom.Point", value.X, value.Y);
    }

    public static Point AsPoint(this FREObject inFre) {
        dynamic fre = new FreObjectSharp(inFre);
        return new Point(fre.x, fre.y);
    }
}
```
----------

### Required AS3 classes
**com.tuarua.fre.ANEUtils.as** and **com.tuarua.fre.ANEError.as** are required by FreSharp and should be included in the AS3 library of your ANE

### Tech

Uses .NET 4.6

### Prerequisites

You will need
 
 - Visual Studio 2017
 - AIR 19+ SDK
