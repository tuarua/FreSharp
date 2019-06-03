# FreSharp Starter Project

1. Modify AIR_PATH in native_extension\ane\build-windows.bat
2. Open HelloWorld.sln and set values for AIRSDK.    
   Properties > C/C++ > Additional Include Directories    
   Properties > Linker > General > Additional Library Directories    
3. Restore NuGet packages on HelloWorldANELib
4. Build for Release x86

The ANE is comprised of 2 parts.

1. A C++ dll which is packaged as your ANE. It exposes methods to AIR and acts as a thin C++ API layer to the C# code.
2. A C# dll which contains the main logic of the ANE.

### Required Dependencies
Starting from version 2.2.0 FreSharpANE.ane is required in your AIR project.     
This ANE contains the required FreSharp dlls and also handles the searching for and loading of a project's C# dll when packaged inside the ANE.    
No more copying these files into your AIRSDK!

----------

* It is highly recommended you package your app for release using an installer.  
Please see the win_installer/ folder for an example Inno Setup project which handles .NET 4.6 and MSV2015 dependencies.

* This ANE was built with MS Visual Studio 2015. As such your machine (and user's machines) will need to have Microsoft Visual C++ 2015 Redistributable (x86) runtime installed.
https://www.microsoft.com/en-us/download/details.aspx?id=48145

* This ANE also uses .NET 4.6 Framework. As such your machine (and user's machines) will need to have to have this installed.
https://www.microsoft.com/en-us/download/details.aspx?id=48130

----------

HelloWorldANE/HelloWorldANE.cpp is the entry point of the ANE. It acts as a thin layered API to your C# controller.  
Add the number of methods here 

```C++
static FRENamedFunction extensionFunctions[] = {
     MAP_FUNCTION(load)
    ,MAP_FUNCTION(goBack)
};
```
    

HelloWorldANELib/MainController.cs  
Add C# method(s) to the functionsToSet Dictionary in getFunctions()

```C#
FunctionsDict = new Dictionary<string, Func<FREObject, uint, FREObject[], FREObject>> {
     {"load", Load}
    ,{"goBack", GoBack}
};
```


Add C# method(s)

```C#
public FREObject Load(FREContext ctx, uint argc, FREObject[] argv) {
    //your code here
    return FREObject.Zero;
}

public FREObject GoBack(FREContext ctx, uint argc, FREObject[] argv) {
    //your code here
    return FREObject.Zero;
}
```

### Prerequisites

You will need:
 - 7Zip
 - Visual Studio 2017
 - AIRSDK 27
