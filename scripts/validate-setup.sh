#!/bin/bash
# validate-setup.sh - Validate Docker setup and requirements

echo "🔍 Stadium Drink Ordering System - Setup Validation"
echo "=================================================="

# Check Docker
echo "🐳 Checking Docker..."
if command -v docker &> /dev/null; then
    echo "✅ Docker is installed: $(docker --version)"
    
    if docker info &> /dev/null; then
        echo "✅ Docker daemon is running"
    else
        echo "❌ Docker daemon is not running"
        exit 1
    fi
else
    echo "❌ Docker is not installed"
    exit 1
fi

# Check Docker Compose
echo ""
echo "📦 Checking Docker Compose..."
if docker compose version &> /dev/null; then
    echo "✅ Docker Compose is available: $(docker compose version)"
elif command -v docker-compose &> /dev/null; then
    echo "✅ Docker Compose (legacy) is available: $(docker-compose --version)"
else
    echo "❌ Docker Compose is not available"
    exit 1
fi

# Check required files
echo ""
echo "📁 Checking required files..."
files=(
    "docker-compose.yml"
    "docker-compose.override.yml"
    "docker-compose.prod.yml"
    ".dockerignore"
    "StadiumDrinkOrdering.API/Dockerfile"
    "StadiumDrinkOrdering.Customer/Dockerfile"
    "StadiumDrinkOrdering.Admin/Dockerfile"
)

for file in "${files[@]}"; do
    if [[ -f "$file" ]]; then
        echo "✅ $file"
    else
        echo "❌ $file (missing)"
    fi
done

# Validate Docker Compose configuration
echo ""
echo "⚙️ Validating Docker Compose configuration..."
if docker compose config > /dev/null 2>&1; then
    echo "✅ docker-compose.yml is valid"
else
    echo "❌ docker-compose.yml has validation errors"
fi

# Check if solution builds
echo ""
echo "🔨 Checking if .NET solution builds..."
if dotnet build StadiumDrinkOrdering.sln > /dev/null 2>&1; then
    echo "✅ .NET solution builds successfully"
else
    echo "❌ .NET solution has build errors"
fi

# Check available ports
echo ""
echo "🔌 Checking port availability..."
ports=(1433 8080 8081 8082 5001 5002 5003)
for port in "${ports[@]}"; do
    if ss -tuln 2>/dev/null | grep ":$port " > /dev/null; then
        echo "⚠️  Port $port is in use"
    else
        echo "✅ Port $port is available"
    fi
done

echo ""
echo "🎯 Setup validation complete!"
echo ""
echo "📋 Next steps:"
echo "  1. Run './scripts/start-dev.sh' for development"
echo "  2. Run './scripts/start-prod.sh' for production"
echo "  3. Access https://localhost:9020 (customer) and https://localhost:9030 (admin)"
