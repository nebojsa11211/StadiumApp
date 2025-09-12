# Base stage for debugging
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8081

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
COPY StadiumDrinkOrdering.Customer/StadiumDrinkOrdering.Customer.csproj StadiumDrinkOrdering.Customer/
COPY StadiumDrinkOrdering.Shared/StadiumDrinkOrdering.Shared.csproj StadiumDrinkOrdering.Shared/

# Restore dependencies
RUN dotnet restore StadiumDrinkOrdering.Customer/StadiumDrinkOrdering.Customer.csproj

# Copy source code
COPY StadiumDrinkOrdering.Customer/ StadiumDrinkOrdering.Customer/
COPY StadiumDrinkOrdering.Shared/ StadiumDrinkOrdering.Shared/

# Build the application
WORKDIR /src/StadiumDrinkOrdering.Customer
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

# Set ownership
RUN chown -R appuser:appuser /app
USER appuser

# Configure environment for Docker container
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=60s --retries=3 \
    CMD curl -f http://localhost:8081 || exit 1

EXPOSE 8081
ENTRYPOINT ["dotnet", "StadiumDrinkOrdering.Customer.dll"]
