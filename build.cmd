cd src\server\Sedio.Server
dotnet restore
dotnet build -c Release
dotnet publish -r linux-x64 -c Release -o ..\..\..\publish\server\linux-x64
dotnet publish -r osx-x64 -c Release -o ..\..\..\publish\server\osx-x64
dotnet publish -r win-x64 -c Release -o ..\..\..\publish\server\win-x64

7z a -tzip ..\..\..\publish\sedio-server-linux-x64 ..\..\..\publish\server\linux-x64\*
7z a -tzip ..\..\..\publish\sedio-server-osx-x64 ..\..\..\publish\server\osx-x64\*
7z a -tzip ..\..\..\publish\sedio-server-win-x64 ..\..\..\publish\server\win-x64\*

cd ..\..\..\

cd src\agent\Sedio.Agent
dotnet restore
dotnet build -c Release
dotnet publish -r linux-x64 -c Release -o ..\..\..\publish\agent\linux-x64
dotnet publish -r osx-x64 -c Release -o ..\..\..\publish\agent\osx-x64
dotnet publish -r win-x64 -c Release -o ..\..\..\publish\agent\win-x64

7z a -tzip ..\..\..\publish\sedio-agent-linux-x64 ..\..\..\publish\agent\linux-x64\*
7z a -tzip ..\..\..\publish\sedio-agent-osx-x64 ..\..\..\publish\agent\osx-x64\*
7z a -tzip ..\..\..\publish\sedio-agent-win-x64 ..\..\..\publish\agent\win-x64\*

cd ..\..\..\

cd src\client\Sedio.Client
dotnet restore
dotnet build -c Release
dotnet pack -c Release -o ..\..\..\publish\client 

cd ..\..\..\
echo All artifacts built