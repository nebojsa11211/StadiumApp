# Base stage for debugging
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8082

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY StadiumDrinkOrdering.sln ./
COPY StadiumDrinkOrdering.Admin/StadiumDrinkOrdering.Admin.csproj StadiumDrinkOrdering.Admin/
COPY StadiumDrinkOrdering.Shared/StadiumDrinkOrdering.Shared.csproj StadiumDrinkOrdering.Shared/

# Restore dependencies
RUN dotnet restore StadiumDrinkOrdering.Admin/StadiumDrinkOrdering.Admin.csproj

# Copy source code
COPY StadiumDrinkOrdering.Admin/ StadiumDrinkOrdering.Admin/
COPY StadiumDrinkOrdering.Shared/ StadiumDrinkOrdering.Shared/

# Build the application
WORKDIR /src/StadiumDrinkOrdering.Admin
RUN dotnet build -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Create non-root user
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copy published application
COPY --from=publish /app/publish .

# Set ownership
RUN chown -R appuser:appuser /app
USER appuser

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=60s --retries=3 \
    CMD curl -f http://localhost:8082 || exit 1

EXPOSE 8082
ENTRYPOINT ["dotnet", "StadiumDrinkOrdering.Admin.dll"]
