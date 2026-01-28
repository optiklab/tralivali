# Tralivali Setup Script for Windows

Write-Host "🚀 Starting Tralivali setup..." -ForegroundColor Green

# Check if Docker is installed
if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
    Write-Host "❌ Docker is not installed. Please install Docker Desktop first." -ForegroundColor Red
    exit 1
}

Write-Host "✅ Docker found" -ForegroundColor Green

# Start MongoDB and RabbitMQ
Write-Host "📦 Starting MongoDB and RabbitMQ..." -ForegroundColor Yellow
docker-compose up -d mongodb rabbitmq

# Wait for services to be ready
Write-Host "⏳ Waiting for services to start (20 seconds)..." -ForegroundColor Yellow
Start-Sleep -Seconds 20

# Check if services are running
$mongodb = docker ps | Select-String "tralivali-mongodb"
$rabbitmq = docker ps | Select-String "tralivali-rabbitmq"

if ($mongodb) {
    Write-Host "✅ MongoDB is running" -ForegroundColor Green
} else {
    Write-Host "❌ MongoDB failed to start" -ForegroundColor Red
    exit 1
}

if ($rabbitmq) {
    Write-Host "✅ RabbitMQ is running" -ForegroundColor Green
} else {
    Write-Host "❌ RabbitMQ failed to start" -ForegroundColor Red
    exit 1
}

# Create first invite code in MongoDB
Write-Host "🎫 Creating initial invite code..." -ForegroundColor Yellow
$mongoScript = @"
use tralivali
db.invites.insertOne({
  inviterUserId: 'system',
  inviteCode: 'WELCOME1',
  email: 'admin@tralivali.local',
  createdAt: new Date(),
  expiresAt: new Date(Date.now() + 365 * 24 * 60 * 60 * 1000),
  isUsed: false
})
print('Invite code created: WELCOME1')
"@

$mongoScript | docker exec -i tralivali-mongodb mongosh

Write-Host ""
Write-Host "✅ Setup complete!" -ForegroundColor Green
Write-Host ""
Write-Host "📝 Next steps:" -ForegroundColor Cyan
Write-Host "1. Backend: cd backend && dotnet run --project Tralivali.API"
Write-Host "2. Frontend: cd frontend && npm run dev"
Write-Host "3. Use invite code 'WELCOME1' to register your first user"
Write-Host ""
Write-Host "🔗 Services:" -ForegroundColor Cyan
Write-Host "   - API: http://localhost:5000"
Write-Host "   - Web: http://localhost:5173"
Write-Host "   - MongoDB: localhost:27017"
Write-Host "   - RabbitMQ Management: http://localhost:15672 (guest/guest)"
Write-Host ""
