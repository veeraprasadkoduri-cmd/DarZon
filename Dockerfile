# -------- Build Stage --------
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy project files
COPY DarZon.sln .
COPY DarZon/*.csproj ./DarZon/

# Restore dependencies
RUN dotnet restore

# Copy all source code
COPY . .

# Publish application
RUN dotnet publish DarZon/DarZon.csproj -c Release -o /app/publish


# -------- Runtime Stage --------
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

# Copy only published output
COPY --from=build /app/publish .

# Run application
ENTRYPOINT ["dotnet", "DarZon.dll"]
