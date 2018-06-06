IF "%APPVEYOR_BUILD_VERSION%" == "" GOTO NOPATH
:YESPATH
@ECHO APPVEYOR_BUILD_VERSION environment variable was detected.
GOTO END
:NOPATH
@ECHO The APPVEYOR_BUILD_VERSION environment variable was NOT detected.
SET APPVEYOR_BUILD_VERSION=1.0.0
GOTO END
:END

cd src\server\Sedio.Server
dotnet restore
dotnet build -c Release
dotnet publish -r linux-x64 -c Release -o ..\..\..\publish\server\linux-x64
dotnet publish -r osx-x64 -c Release -o ..\..\..\publish\server\osx-x64
dotnet publish -r win-x64 -c Release -o ..\..\..\publish\server\win-x64

7z a -tzip -saa ..\..\..\publish\sedio-server-linux-x64-%APPVEYOR_BUILD_VERSION% ..\..\..\publish\server\linux-x64\*
7z a -tzip -saa ..\..\..\publish\sedio-server-osx-x64-%APPVEYOR_BUILD_VERSION% ..\..\..\publish\server\osx-x64\*
7z a -tzip -saa ..\..\..\publish\sedio-server-win-x64-%APPVEYOR_BUILD_VERSION% ..\..\..\publish\server\win-x64\*

cd ..\..\..\

cd src\agent\Sedio.Agent
dotnet restore
dotnet build -c Release
dotnet publish -r linux-x64 -c Release -o ..\..\..\publish\agent\linux-x64
dotnet publish -r osx-x64 -c Release -o ..\..\..\publish\agent\osx-x64
dotnet publish -r win-x64 -c Release -o ..\..\..\publish\agent\win-x64

7z a -tzip -saa ..\..\..\publish\sedio-agent-linux-x64-%APPVEYOR_BUILD_VERSION% ..\..\..\publish\agent\linux-x64\*
7z a -tzip -saa ..\..\..\publish\sedio-agent-osx-x64-%APPVEYOR_BUILD_VERSION% ..\..\..\publish\agent\osx-x64\*
7z a -tzip -saa ..\..\..\publish\sedio-agent-win-x64-%APPVEYOR_BUILD_VERSION% ..\..\..\publish\agent\win-x64\*

cd ..\..\..\

cd src\client\Sedio.Client
dotnet restore
dotnet build -c Release
dotnet pack -c Release -o ..\..\..\publish\client 

cd ..\..\..\
echo All artifacts built