# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY src/FarmManagement.Domain/*.csproj ./FarmManagement.Domain/
COPY src/FarmManagement.Application/*.csproj ./FarmManagement.Application/
COPY src/FarmManagement.Infrastructure/*.csproj ./FarmManagement.Infrastructure/
COPY src/FarmManagement.API/*.csproj ./FarmManagement.API/

# Restore dependencies
RUN dotnet restore ./FarmManagement.API/FarmManagement.API.csproj

# Copy everything else
COPY src/ .

# Build and publish
RUN dotnet publish ./FarmManagement.API/FarmManagement.API.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

# Expose port (Railway uses PORT env variable)
EXPOSE 8080

# Set environment to Production
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "FarmManagement.API.dll"]
