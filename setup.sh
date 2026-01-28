#!/bin/bash
# Tralivali Setup Script

set -e

echo "🚀 Starting Tralivali setup..."

# Check if Docker is installed
if ! command -v docker &> /dev/null; then
    echo "❌ Docker is not installed. Please install Docker first."
    exit 1
fi

if ! command -v docker-compose &> /dev/null; then
    echo "❌ Docker Compose is not installed. Please install Docker Compose first."
    exit 1
fi

echo "✅ Docker and Docker Compose found"

# Start MongoDB and RabbitMQ
echo "📦 Starting MongoDB and RabbitMQ..."
docker-compose up -d mongodb rabbitmq

# Wait for services to be ready
echo "⏳ Waiting for services to start (20 seconds)..."
sleep 20

# Check if services are running
if docker ps | grep -q tralivali-mongodb; then
    echo "✅ MongoDB is running"
else
    echo "❌ MongoDB failed to start"
    exit 1
fi

if docker ps | grep -q tralivali-rabbitmq; then
    echo "✅ RabbitMQ is running"
else
    echo "❌ RabbitMQ failed to start"
    exit 1
fi

# Create first invite code in MongoDB
echo "🎫 Creating initial invite code..."
docker exec -i tralivali-mongodb mongosh <<EOF
use tralivali
db.invites.insertOne({
  inviterUserId: "system",
  inviteCode: "WELCOME1",
  email: "admin@tralivali.local",
  createdAt: new Date(),
  expiresAt: new Date(Date.now() + 365 * 24 * 60 * 60 * 1000),
  isUsed: false
})
print("Invite code created: WELCOME1")
EOF

echo ""
echo "✅ Setup complete!"
echo ""
echo "📝 Next steps:"
echo "1. Backend: cd backend && dotnet run --project Tralivali.API"
echo "2. Frontend: cd frontend && npm run dev"
echo "3. Use invite code 'WELCOME1' to register your first user"
echo ""
echo "🔗 Services:"
echo "   - API: http://localhost:5000"
echo "   - Web: http://localhost:5173"
echo "   - MongoDB: localhost:27017"
echo "   - RabbitMQ Management: http://localhost:15672 (guest/guest)"
echo ""
