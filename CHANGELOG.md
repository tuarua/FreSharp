### 2.0.0
- Fix FREObject.AsDateTime() UTC bug
- repurpose FreObjectSharp for use as a dynamic object
- Added FREObject.toString()
- Added FREObject.hasOwnProperty(string name)
- Added FREArray.Append()
- FREObject.AsColor(bool hasAlpha = true) hasAlpha default value is now TRUE
- mark FreSharpHelper as internal
- Add FreSharpLogger to trace any FREExceptions
- Remove SendEvent method
- Refactor
- **C++ project now needs reference to ..\packages\TuaRua.FreSharp.2.0.0\FreSharp\$(PlatformTarget)\FreSharp.dll**

### 1.7.0
- deprecate SendEvent, use DispatchEvent instead
- deprecate FREArray(className), use FREArray(className, length, fixedSize) instead (className only no <Vector. prefix)
- **C++ project now needs reference to ..\packages\TuaRua.FreSharp.1.7.0\FreSharp\$(PlatformTarget)\FreSharp.dll**

### 1.6.0
- Added FreBitmapDataSharp.AsByteArray()
- Fixed possible null pointer exception in FreObjectSharp
- Improve UTF-8 support
- Added subscript setter/getter for FREArray i.e. myFreArray[0] = myFREObject
- Added iterator for FREArray i.e.  foreach (var fre in myFreArray) { }
- Upgraded to AIR SDK 30
- **C++ project now needs reference to ..\packages\TuaRua.FreSharp.1.6.0\FreSharp\$(PlatformTarget)\FreSharp.dll**

### 1.5.0
- Added FREObject.AsDateTime()
- Added DateTime.ToFREObject()
- Refactor FrePointSharp
- Refactor FreRectangleSharp
- Added Color.ToFREObject()
- Added FREObject.AsColor()
- Added int, double, boolean, string conversions to FREArray

### 1.4.0
Allow GetAsString to return null strings
Updated ANEUtils

### 1.3.0
- Fix for Bitmap conversion on 64bit
- Added AsColor() method
- Added GetScaleFactor() method to WinAPI
- Added OnFinalise() required method
- **C++ project now needs reference to ..\packages\TuaRua.FreSharp.1.5.0\FreSharp\$(PlatformTarget)\FreSharp.dll**
- Refactor

