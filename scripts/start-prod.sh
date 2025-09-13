#!/bin/bash
# start-prod.sh - Start the production environment

echo "🏟️ Starting Stadium Drink Ordering System (Production)"
echo "======================================================"

# Check if Docker is running
if ! docker info >/dev/null 2>&1; then
    echo "❌ Docker is not running. Please start Docker first."
    exit 1
fi

# Check for required environment variables
if [[ -z "${SQL_SA_PASSWORD}" ]]; then
    echo "⚠️  SQL_SA_PASSWORD environment variable not set. Using default."
fi

if [[ -z "${JWT_SECRET_KEY}" ]]; then
    echo "⚠️  JWT_SECRET_KEY environment variable not set. Using default."
fi

# Stop any existing containers
echo "🛑 Stopping existing containers..."
docker-compose -f docker-compose.yml -f docker-compose.prod.yml down

# Build and start services
echo "🔨 Building and starting services..."
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up --build -d

# Wait for services to be healthy
echo "⏳ Waiting for services to be ready..."

while true; do
    db_health=$(docker inspect --format='{{.State.Health.Status}}' stadium-sqlserver 2>/dev/null || echo "starting")
    api_health=$(docker inspect --format='{{.State.Health.Status}}' stadium-api 2>/dev/null || echo "starting")
    customer_health=$(docker inspect --format='{{.State.Health.Status}}' stadium-customer 2>/dev/null || echo "starting")
    admin_health=$(docker inspect --format='{{.State.Health.Status}}' stadium-admin 2>/dev/null || echo "starting")
    
    echo "📊 Service Status:"
    echo "   Database: $db_health"
    echo "   API: $api_health"
    echo "   Customer App: $customer_health"
    echo "   Admin App: $admin_health"
    echo ""
    
    if [[ "$db_health" == "healthy" && "$api_health" == "healthy" && "$customer_health" == "healthy" && "$admin_health" == "healthy" ]]; then
        break
    fi
    
    sleep 10
done

echo "✅ All services are ready!"
echo ""
echo "🌐 Access URLs:"
echo "   Customer App: https://localhost:9020"
echo "   Admin App:    https://localhost:9030"
echo "   API:          https://localhost:9010"
echo "   API Swagger:  https://localhost:9010/swagger"
echo ""
echo "🔑 Default Admin Login:"
echo "   Email:    admin@stadium.com"
echo "   Password: admin123"
echo ""
echo "📋 Monitoring Commands:"
echo "   View logs:      docker-compose -f docker-compose.yml -f docker-compose.prod.yml logs -f [service]"
echo "   Stop services:  docker-compose -f docker-compose.yml -f docker-compose.prod.yml down"
echo "   Restart:        docker-compose -f docker-compose.yml -f docker-compose.prod.yml restart [service]"
echo ""
echo "🚀 Production environment ready!"
