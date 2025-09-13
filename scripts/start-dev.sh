#!/bin/bash
# start-dev.sh - Start the development environment

echo "🏟️ Starting Stadium Drink Ordering System (Development)"
echo "=================================================="

# Check if Docker is running
if ! docker info >/dev/null 2>&1; then
    echo "❌ Docker is not running. Please start Docker first."
    exit 1
fi

# Stop any existing containers
echo "🛑 Stopping existing containers..."
docker-compose down

# Remove old images (optional - uncomment if needed)
# echo "🗑️ Removing old images..."
# docker-compose down --rmi all

# Build and start services
echo "🔨 Building and starting services..."
docker-compose up --build -d

# Wait for services to be healthy
echo "⏳ Waiting for services to be ready..."
echo "   - Database initialization may take 60-90 seconds..."
echo "   - API will start after database is ready..."
echo "   - Frontend apps will start after API is ready..."

# Monitor service health
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
echo "📋 Useful Commands:"
echo "   View logs:      docker-compose logs -f [service]"
echo "   Stop services:  docker-compose down"
echo "   Restart:        docker-compose restart [service]"
echo ""
echo "🎉 Happy coding!"
