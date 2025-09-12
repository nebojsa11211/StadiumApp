# Base stage for debugging
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Install curl for health checks and timezone data
RUN apt-get update && apt-get install -y curl tzdata && rm -rf /var/lib/apt/lists/*

# Set timezone
ENV TZ=Europe/Zagreb
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY StadiumDrinkOrdering.sln ./
COPY StadiumDrinkOrdering.API/StadiumDrinkOrdering.API.csproj StadiumDrinkOrdering.API/
COPY StadiumDrinkOrdering.Shared/StadiumDrinkOrdering.Shared.csproj StadiumDrinkOrdering.Shared/

# Restore dependencies
RUN dotnet restore StadiumDrinkOrdering.API/StadiumDrinkOrdering.API.csproj

# Copy source code
COPY StadiumDrinkOrdering.API/ StadiumDrinkOrdering.API/
COPY StadiumDrinkOrdering.Shared/ StadiumDrinkOrdering.Shared/

# Build the application
WORKDIR /src/StadiumDrinkOrdering.API
RUN dotnet build -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Install curl for health checks and timezone data
RUN apt-get update && apt-get install -y curl tzdata && rm -rf /var/lib/apt/lists/*

# Set timezone
ENV TZ=Europe/Zagreb
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

# Create non-root user
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copy published application
COPY --from=publish /app/publish .

# Create directory for SQLite database with proper permissions
RUN mkdir -p /app/data && chown -R appuser:appuser /app
USER appuser

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=60s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

EXPOSE 8080
ENTRYPOINT ["dotnet", "StadiumDrinkOrdering.API.dll"]
