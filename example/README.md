This is an example of how to use FreSharp.   
It also shows how to add native controls over the AIR Window with transparency supported on Win8.1 +   

##### Windows Installation - Important!

* Copy the contents of the "c_sharp_libs" folder into the bin folder of your AIRSDK.   

* This ANE was built with .NET 4.6  As such your machine (and user's machines) will need to have this installed.    

* This ANE was built with MS Visual Studio 2015. As such your machine (and user's machines) will need to have Microsoft Visual C++ 2015 Redistributable (x86) runtime installed.   
https://www.microsoft.com/en-us/download/details.aspx?id=48145   

* For release builds, these files need to be packaged in the same folder as your exe.  
It is highly recommended you package your app for release using an installer.  
Please see the win_installer folder for an example Inno Setup project which handles .NET and MSV2015 dependencies.