# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

# Set working directory
WORKDIR /app

# Copy project files and restore dependencies
COPY StadiumDrinkOrdering.Staff/*.csproj ./StadiumDrinkOrdering.Staff/
COPY StadiumDrinkOrdering.Shared/*.csproj ./StadiumDrinkOrdering.Shared/

# Restore dependencies
RUN dotnet restore StadiumDrinkOrdering.Staff/StadiumDrinkOrdering.Staff.csproj

# Copy remaining source files
COPY StadiumDrinkOrdering.Staff/ ./StadiumDrinkOrdering.Staff/
COPY StadiumDrinkOrdering.Shared/ ./StadiumDrinkOrdering.Shared/

# Build and publish
RUN dotnet publish StadiumDrinkOrdering.Staff/StadiumDrinkOrdering.Staff.csproj -c Release -o out

# Use the ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Install necessary tools for troubleshooting and timezone data
RUN apt-get update && apt-get install -y curl tzdata && rm -rf /var/lib/apt/lists/*

# Set timezone
ENV TZ=Europe/Zagreb
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

# Set working directory
WORKDIR /app

# Copy built application
COPY --from=build-env /app/out .

# Create a non-root user for security
RUN groupadd -r appuser && useradd -r -g appuser appuser
RUN chown -R appuser:appuser /app
USER appuser

# Configure ASP.NET Core
ENV ASPNETCORE_URLS=http://+:8083
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Expose port
EXPOSE 8083

# Health check
HEALTHCHECK --interval=30s --timeout=3s --retries=3 \
  CMD curl -f http://localhost:8083/health || exit 1

# Start the application
ENTRYPOINT ["dotnet", "StadiumDrinkOrdering.Staff.dll"]