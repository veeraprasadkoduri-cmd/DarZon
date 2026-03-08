# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy only project files first for faster restore
COPY DarZon/*.csproj ./DarZon/
RUN dotnet restore DarZon/DarZon.csproj

# Copy the rest of the code
COPY . .
RUN dotnet publish DarZon/DarZon.csproj -c Release -o /app/publish /p:UseWasmBuild=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5000
ENTRYPOINT ["dotnet", "DarZon.dll"]
