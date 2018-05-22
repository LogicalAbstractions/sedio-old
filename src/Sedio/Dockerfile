FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 50156
EXPOSE 44370

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY src/Sedio.Server/Sedio.Server.csproj src/Sedio.Server/
RUN dotnet restore src/Sedio.Server/Sedio.Server.csproj
COPY . .
WORKDIR /src/src/Sedio.Server
RUN dotnet build Sedio.Server.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Sedio.Server.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Sedio.Server.dll"]
