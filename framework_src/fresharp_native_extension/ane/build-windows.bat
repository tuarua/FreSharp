REM Get the path to the script and trim to get the directory.
@echo off
SET SZIP="C:\Program Files\7-Zip\7z.exe"
SET AIR_PATH="D:\dev\sdks\AIR\AIRSDK_32\bin\"
echo Setting path to current directory to:
SET pathtome=%~dp0
echo %pathtome%

SET projectName=FreSharpANE

REM Setup the directory.
echo Making directories.

IF NOT EXIST %pathtome%platforms mkdir %pathtome%platforms
IF NOT EXIST %pathtome%platforms\win  %pathtome%platforms\win
IF NOT EXIST %pathtome%platforms\win\x86  %pathtome%platforms\win\x86
IF NOT EXIST %pathtome%platforms\win\x86\release mkdir %pathtome%platforms\win\x86\release
IF NOT EXIST %pathtome%platforms\win\x64  %pathtome%platforms\win\x64
IF NOT EXIST %pathtome%platforms\win\x64\release mkdir %pathtome%platforms\win\x64\release
IF NOT EXIST %pathtome%platforms\default mkdir %pathtome%platforms\default

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
copy %pathtome%library.swf %pathtome%platforms\default

REM Copy native libraries into place.
echo Copying native libraries into place.

copy %pathtome%..\..\example\native_library\win\FreSharpExampleANE\x86\Release\%projectName%.dll %pathtome%platforms\win\x86\release
copy %pathtome%..\..\example\native_library\win\FreSharpExampleANE\x64\Release\%projectName%.dll %pathtome%platforms\win\x64\release

copy %pathtome%..\..\example\native_library\win\FreSharpExampleANE\x86\Release\FreSharpCore.dll %pathtome%platforms\win\x86\release
copy %pathtome%..\..\example\native_library\win\FreSharpExampleANE\x64\Release\FreSharpCore.dll %pathtome%platforms\win\x64\release

copy %pathtome%..\..\example\native_library\win\FreSharpExampleANE\x86\Release\FreSharp.dll %pathtome%platforms\win\x86\release
copy %pathtome%..\..\example\native_library\win\FreSharpExampleANE\x64\Release\FreSharp.dll %pathtome%platforms\win\x64\release

REM Run the build command.
echo Building Release.
call %AIR_PATH%adt.bat -package -target ane %pathtome%FreSharp.ane %pathtome%extension.xml -swc %pathtome%%projectName%.swc ^
-platform Windows-x86 -C %pathtome%platforms\win\x86\release %projectName%.dll FreSharpCore.dll FreSharp.dll library.swf ^
-platform Windows-x86-64 -C %pathtome%platforms\win\x64\release %projectName%.dll FreSharpCore.dll FreSharp.dll library.swf ^
-platform default -C %pathtome%platforms\default library.swf

call DEL /F /Q /A %pathtome%platforms\win\x64\release\FreSharp.dll
call DEL /F /Q /A %pathtome%platforms\win\x64\release\FreSharpCore.dll
call DEL /F /Q /A %pathtome%platforms\win\x86\release\FreSharp.dll
call DEL /F /Q /A %pathtome%platforms\win\x86\release\FreSharpCore.dll
call DEL /F /Q /A %pathtome%platforms\win\x86\release\%projectName%.dll
call DEL /F /Q /A %pathtome%platforms\win\x64\release\%projectName%.dll
call DEL /F /Q /A %pathtome%platforms\win\x64\release\library.swf
call DEL /F /Q /A %pathtome%platforms\win\x86\release\library.swf
call DEL /F /Q /A %pathtome%platforms\default\library.swf
call DEL /F /Q /A %pathtome%%projectName%.swc
call DEL /F /Q /A %pathtome%library.swf
call DEL /F /Q /A %pathtome%catalog.xml

echo FIN
