# Use Windows container SDK image
FROM mcr.microsoft.com/dotnet/sdk:6.0-windowsservercore-ltsc2022 AS build
WORKDIR /src
COPY DarZon/*.csproj ./DarZon/
RUN dotnet restore DarZon/DarZon.csproj
COPY . .
RUN dotnet publish DarZon/DarZon.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0-windowsservercore-ltsc2022 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DarZon.dll"]
