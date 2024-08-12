# Use the official .NET SDK as the build environment
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy the project files for the main application
COPY WineCollectionManagerApi/*.csproj ./WineCollectionManagerApi/

# Restore the main application's dependencies
RUN dotnet restore ./WineCollectionManagerApi/WineCollectionManagerApi.csproj

# Copy the main application's source code
COPY WineCollectionManagerApi/. ./WineCollectionManagerApi/

# Set the working directory to the main project
WORKDIR /app/WineCollectionManagerApi

# Publish the application in release mode to the 'out' directory
RUN dotnet publish ./WineCollectionManagerApi.csproj -c Release -o out

# Use the official ASP.NET Core runtime as the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/WineCollectionManagerApi/out ./

# Expose port 80 for the application
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "WineCollectionManagerApi.dll"]
