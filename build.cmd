cd src\server\Sedio.Server
dotnet restore
dotnet build -c Release
dotnet publish -r linux-x64 -c Release -o ..\..\..\publish\server\linux-x64
dotnet publish -r osx-x64 -c Release -o ..\..\..\publish\server\osx-x64
dotnet publish -r win-x64 -c Release -o ..\..\..\publish\server\win-x64

cd ..\..\..\
cd src\agent\Sedio.Agent
dotnet restore
dotnet build -c Release
dotnet publish -r linux-x64 -c Release -o ..\..\..\publish\server\linux-x64
dotnet publish -r osx-x64 -c Release -o ..\..\..\publish\server\osx-x64
dotnet publish -r win-x64 -c Release -o ..\..\..\publish\server\win-x64

cd ..\..\..\
cd src\client\Sedio.Client
dotnet restore
dotnet build -c Release
dotnet package