REM Get the path to the script and trim to get the directory.
@echo off
SET SZIP="C:\Program Files\7-Zip\7z.exe"
SET AIR_PATH="D:\dev\sdks\AIR\AIRSDK_28\bin\"
SET AIR_PATH_64="D:\dev\sdks\AIR\AIRSDK_28_64\bin\"
echo Setting path to current directory to:
SET pathtome=%~dp0
echo %pathtome%

SET projectName=FreSharpExampleANE

REM Setup the directory.
echo Making directories.

IF NOT EXIST %pathtome%platforms mkdir %pathtome%platforms
IF NOT EXIST %pathtome%platforms\win  %pathtome%platforms\win
IF NOT EXIST %pathtome%platforms\win\x86  %pathtome%platforms\win\x86
IF NOT EXIST %pathtome%platforms\win\x86\release mkdir %pathtome%platforms\win\x86\release
IF NOT EXIST %pathtome%platforms\win\x64  %pathtome%platforms\win\x64
IF NOT EXIST %pathtome%platforms\win\x64\release mkdir %pathtome%platforms\win\x64\release

REM Copy SWC into place.
echo Copying SWC into place.
echo %pathtome%..\bin\%projectName%.swc
copy %pathtome%..\bin\%projectName%.swc %pathtome%

REM contents of SWC.
echo Extracting files form SWC.
echo %pathtome%%projectName%.swc
copy %pathtome%%projectName%.swc %pathtome%%projectName%Extract.swc
ren %pathtome%%projectName%Extract.swc %projectName%Extract.zip

call %SZIP% e %pathtome%%projectName%Extract.zip -o%pathtome%

del %pathtome%%projectName%Extract.zip

REM Copy library.swf to folders.
echo Copying library.swf into place.
copy %pathtome%library.swf %pathtome%platforms\win\x86\release
copy %pathtome%library.swf %pathtome%platforms\win\x64\release


REM Copy native libraries into place.
echo Copying native libraries into place.

copy %pathtome%..\..\native_library\win\%projectName%\x86\Release\%projectName%.dll %pathtome%platforms\win\x86\release
copy %pathtome%..\..\native_library\win\%projectName%\x64\Release\%projectName%.dll %pathtome%platforms\win\x64\release

copy %pathtome%..\..\native_library\win\%projectName%\x86\Release\%projectName%Lib.dll %AIR_PATH%%projectName%Lib.dll
copy %pathtome%..\..\native_library\win\%projectName%\x86\Release\%projectName%Lib.pdb %AIR_PATH%%projectName%Lib.pdb

copy %pathtome%..\..\native_library\win\%projectName%\x64\Release\%projectName%Lib.dll %AIR_PATH_64%%projectName%Lib.dll
copy %pathtome%..\..\native_library\win\%projectName%\x64\Release\%projectName%Lib.pdb %AIR_PATH_64%%projectName%Lib.pdb

copy %pathtome%..\..\native_library\win\%projectName%\x86\Release\%projectName%Lib.dll %pathtome%..\..\c_sharp_libs_x86\%projectName%Lib.dll
copy %pathtome%..\..\native_library\win\%projectName%\x64\Release\%projectName%Lib.dll %pathtome%..\..\c_sharp_libs_x64\%projectName%Lib.dll

copy %pathtome%..\..\native_library\win\%projectName%\x86\Release\FreSharpCore.dll %AIR_PATH%FreSharpCore.dll
copy %pathtome%..\..\native_library\win\%projectName%\x86\Release\FreSharpCore.pdb %AIR_PATH%FreSharpCore.pdb

copy %pathtome%..\..\native_library\win\%projectName%\x64\Release\FreSharpCore.dll %AIR_PATH_64%FreSharpCore.dll
copy %pathtome%..\..\native_library\win\%projectName%\x64\Release\FreSharpCore.pdb %AIR_PATH_64%FreSharpCore.pdb

copy %pathtome%..\..\native_library\win\%projectName%\x86\Release\FreSharp.dll %AIR_PATH%FreSharp.dll
copy %pathtome%..\..\native_library\win\%projectName%\x86\Release\FreSharp.pdb %AIR_PATH%FreSharp.pdb

copy %pathtome%..\..\native_library\win\%projectName%\x64\Release\FreSharp.dll %AIR_PATH_64%FreSharp.dll
copy %pathtome%..\..\native_library\win\%projectName%\x64\Release\FreSharp.pdb %AIR_PATH_64%FreSharp.pdb

copy %pathtome%..\..\native_library\win\%projectName%\x86\Release\FreSharpCore.dll %pathtome%..\..\c_sharp_libs_x86\FreSharpCore.dll
copy %pathtome%..\..\native_library\win\%projectName%\x86\Release\FreSharp.dll %pathtome%..\..\c_sharp_libs_x86\FreSharp.dll

copy %pathtome%..\..\native_library\win\%projectName%\x64\Release\FreSharpCore.dll %pathtome%..\..\c_sharp_libs_x64\FreSharpCore.dll
copy %pathtome%..\..\native_library\win\%projectName%\x64\Release\FreSharp.dll %pathtome%..\..\c_sharp_libs_x64\FreSharp.dll

REM Run the build command.
echo Building Release.
call %AIR_PATH%adt.bat -package -target ane %pathtome%%projectName%.ane %pathtome%extension_win.xml -swc %pathtome%%projectName%.swc ^
-platform Windows-x86 -C %pathtome%platforms\win\x86\release %projectName%.dll library.swf ^
-platform Windows-x86-64 -C %pathtome%platforms\win\x64\release %projectName%.dll library.swf


call DEL /F /Q /A %pathtome%%projectName%.swc
call DEL /F /Q /A %pathtome%library.swf
call DEL /F /Q /A %pathtome%catalog.xml

echo FIN
