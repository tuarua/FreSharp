# FreSharp

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=5UR2T52J633RC)

### Features
 - Build Adobe Air Native Extensions using C#

Please see the feature rich example in https://github.com/tuarua/FreSharp/tree/master/example

The package is hosted on NuGet at https://www.nuget.org/packages/TuaRua.FreSharp/

----------

### How to use
######  The methods exposed by FreSharp are very similar to the Java API for Air Native Extensions. 

Example - Convert a FREObject into a String, and String into FREObject

````C#
var inFre = argv[0];
try {
  var airString = new FreObjectSharp(inFre).GetAsString();
  Trace("String passed from AIR:" + airString);
}
catch (Exception e) {
  Console.WriteLine(@"caught in C#: type: {0} message: {1}", e.GetType(), e.Message);
}
const string sharpString = "I am a string from C#";
return new FreObjectSharp(sharpString).Get();
`````

Example - Call a method on an FREObject

````C#
var addition = person.CallMethod("add", new ArrayList
 {
  new FreObjectSharp(100),
  new FreObjectSharp(33)
 });
var sum = addition.GetAsInt();
Trace("result is: " + sum);
`````

### Tech

Uses .NET 4.5.2

### Prerequisites

You will need
 
 - Visual Studio 2015
 - AIR 19+ SDK

### Todos
