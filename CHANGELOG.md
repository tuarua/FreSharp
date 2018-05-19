### 1.5.1
- Added FreBitmapDataSharp.AsByteArray()
- Fixed possible null pointer exception in FreObjectSharp

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

