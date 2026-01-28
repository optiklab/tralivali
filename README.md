# TelegramLite OR Tralivali

A self-hosted, invite-only messaging platform for family and friends with end-to-end encryption.

A Telegram-like messaging application built with .NET 8, featuring end-to-end encryption, real-time messaging, and cloud deployment capabilities.

## Project Structure

```
tralivali/
├── src/
│   ├── TelegramLite.Api/          # ASP.NET Core Web API
│   ├── TelegramLite.Domain/       # Domain entities
│   ├── TelegramLite.Infrastructure/  # Data access and external services
│   ├── TelegramLite.Auth/         # Authentication services
│   ├── TelegramLite.Messaging/    # Messaging services
│   └── TelegramLite.Workers/      # Background workers
├── tests/
│   └── TelegramLite.Tests/        # Unit and integration tests
├── docker-compose.yml             # Local development environment
└── .env.example                   # Environment variables template
```

## Prerequisites

- .NET 8 SDK
- Docker and Docker Compose
- MongoDB
- RabbitMQ
- Redis

## Getting Started

### 1. Clone the Repository

## Features

- 🔒 **End-to-End Encryption**: Messages are encrypted on the client side using RSA-OAEP and AES-GCM
- 👥 **Invite-Only**: New users can only join with an invite code from existing members
- 💬 **Real-time Messaging**: Built on RabbitMQ for instant message delivery
- 🌐 **Cross-Platform**: Web client (React) and iOS app (Tauri 2.0)
- ☁️ **Cloud-Ready**: Designed for Azure deployment with Let's Encrypt SSL
- 📧 **Email Integration**: Azure Communication Services for invite emails

## Architecture

### Backend
- **Framework**: .NET Core 8
- **Database**: MongoDB (for user data, messages, conversations)
- **Message Queue**: RabbitMQ (for real-time message delivery)
- **Authentication**: JWT with BCrypt password hashing

### Frontend
- **Web**: React with TypeScript
- **iOS**: Rust-based app using Tauri 2.0
- **Encryption**: Web Crypto API for client-side encryption

### Deployment
- **Cloud**: Azure App Service
- **SSL**: Let's Encrypt (via Azure App Service Managed Certificate)
- **Email**: Azure Communication Services
- **iOS Distribution**: TestFlight

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- Node.js 20+
- Docker and Docker Compose
- MongoDB (or use Docker)
- RabbitMQ (or use Docker)

### 2. Start Local Services

```bash
docker-compose up -d
```

This will start:
- MongoDB on port 27017
- RabbitMQ on ports 5672 (AMQP) and 15672 (Management UI)
- Redis on port 6379

### 3. Configure Environment

Copy the example environment file and update with your settings:

```bash
cp .env.example .env
```

### 4. Build the Solution

```bash
dotnet build
```

### 5. Run Tests

```bash
dotnet test
```

### 6. Run the API

```bash
cd src/TelegramLite.Api
dotnet run
```

The API will be available at `http://localhost:5000` and `https://localhost:5001`.

## Features Implemented

### Phase 1: Backend Foundation (In Progress)

- ✅ .NET 8 solution with Clean Architecture
- ✅ Serilog structured logging
- ✅ Docker Compose for local development (MongoDB, RabbitMQ, Redis)
- ✅ Domain entities with full XML documentation
- ✅ MongoDB repository pattern with generic CRUD operations
- ✅ RabbitMQ infrastructure with resilience policies
- ⏳ JWT authentication service (Pending)
- ⏳ Magic-link authentication (Pending)
- ⏳ Invite system with QR codes (Pending)

### Upcoming Phases

- Phase 2: Real-Time Messaging Backend
- Phase 3: Message Retention, Archival & Backup
- Phase 4: Web Client (React + TypeScript)
- Phase 5: iOS App (Tauri 2.0)
- Phase 6: End-to-End Encryption
- Phase 7: File Sharing & Media
- Phase 8: Azure Deployment
- Phase 9: Testing
- Phase 10: Documentation

## Architecture

### Clean Architecture Layers

1. **Domain Layer** (`TelegramLite.Domain`)
   - No dependencies on other layers
   - Contains entities and domain logic
   - Entities: User, Conversation, Message, Invite, FileAttachment, Backup

2. **Infrastructure Layer** (`TelegramLite.Infrastructure`)
   - Depends on Domain
   - Implements data access patterns
   - MongoDB repositories
   - External service integrations

3. **Application Layer** (`TelegramLite.Auth`, `TelegramLite.Messaging`, `TelegramLite.Workers`)
   - Depends on Domain
   - Contains business logic and use cases

4. **Presentation Layer** (`TelegramLite.Api`)
   - Depends on all layers
   - ASP.NET Core Web API
   - SignalR for real-time communication

### Database Schema

#### Users Collection
- Stores user information, credentials, and device registrations
- Unique index on `email` field

#### Conversations Collection
- Supports both direct (1-on-1) and group conversations
- Compound index on `Participants.UserId` and `LastMessageAt`

#### Messages Collection
- Stores all messages with encryption support
- Compound index on `ConversationId` and `CreatedAt`

#### Invites Collection
- Invite tokens with HMAC-SHA256 signing
- Unique index on `Token`
- TTL index on `ExpiresAt` for automatic cleanup

#### Files Collection
- File metadata and blob storage references
- Supports thumbnails and EXIF data

#### Backups Collection
- Tracks system backups
- Includes blob paths and backup status

## Development

### Running Locally

1. Ensure all services are running:
   ```bash
   docker-compose ps
   ```

2. Check service logs:
   ```bash
   docker-compose logs -f mongodb
   docker-compose logs -f rabbitmq
   docker-compose logs -f redis
   ```

3. Access RabbitMQ Management UI: http://localhost:15672
   - Username: `admin`
   - Password: `password`

### Project Commands

```bash
# Build solution
dotnet build

# Run tests
dotnet test

# Run API with hot reload
cd src/TelegramLite.Api
dotnet watch run

# Clean build artifacts
dotnet clean
```

## Logging

The application uses Serilog for structured logging:

- Console output with colored levels
- File logging with daily rolling (logs/telegramlite-YYYYMMDD.txt)
- Configurable log levels in appsettings.json

## Contributing

1. Create a feature branch
2. Make your changes
3. Ensure all tests pass
4. Submit a pull request

## License

[License information to be added]

## Roadmap

See the full 82-task implementation plan in the problem statement document.
2. **Start infrastructure services**
```bash
docker-compose up -d mongodb rabbitmq
```

3. **Run the backend**
```bash
cd backend
dotnet restore
dotnet run --project Tralivali.API
```
The API will be available at `http://localhost:5000`

4. **Run the frontend**
```bash
cd frontend
npm install
npm run dev
```
The web app will be available at `http://localhost:5173`

5. **Access RabbitMQ Management** (optional)
```
http://localhost:15672
Username: guest
Password: guest
```

### Configuration

Update `backend/Tralivali.API/appsettings.json` with your settings:

```json
{
  "MongoDb": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "tralivali"
  },
  "RabbitMq": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest"
  },
  "Jwt": {
    "SecretKey": "your-secret-key-min-32-chars",
    "Issuer": "tralivali",
    "Audience": "tralivali-users",
    "ExpirationMinutes": 1440
  }
}
```

### Creating the First User

Since the platform is invite-only, you need to manually create the first invite code in MongoDB:

```javascript
// Connect to MongoDB
use tralivali

// Create first invite
db.invites.insertOne({
  inviterUserId: "system",
  inviteCode: "WELCOME1",
  email: "your-email@example.com",
  createdAt: new Date(),
  expiresAt: new Date(Date.now() + 7 * 24 * 60 * 60 * 1000), // 7 days
  isUsed: false
})
```

## Project Structure

```
tralivali/
├── backend/
│   ├── Tralivali.API/          # Web API and controllers
│   ├── Tralivali.Core/         # Domain models and interfaces
│   ├── Tralivali.Infrastructure/ # MongoDB, RabbitMQ services
│   └── Tralivali.sln
├── frontend/                    # React TypeScript web app
│   ├── src/
│   │   ├── components/         # React components
│   │   ├── services/           # API and crypto services
│   │   └── types/              # TypeScript types
│   └── package.json
├── ios/                         # Tauri 2.0 iOS app
│   ├── src/
│   └── Cargo.toml
├── docs/
│   └── AZURE_DEPLOYMENT.md     # Azure deployment guide
└── docker-compose.yml
```

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user with invite code
- `POST /api/auth/login` - Login with email/password

### Invites
- `POST /api/invites` - Create new invite
- `GET /api/invites` - Get user's invites

### Messages
- `POST /api/messages` - Send encrypted message
- `GET /api/messages/conversations/:id` - Get messages in conversation
- `GET /api/messages/conversations` - Get all conversations

## Security

- All messages are encrypted client-side before transmission
- Passwords are hashed using BCrypt
- JWT tokens for authentication
- Private keys are encrypted with user's password
- HTTPS enforced in production

## Deployment

See [AZURE_DEPLOYMENT.md](docs/AZURE_DEPLOYMENT.md) for detailed deployment instructions.

### Quick Deploy to Azure

1. Build and push backend:
```bash
cd backend
dotnet publish -c Release
# Deploy to Azure App Service
```

2. Build and deploy frontend:
```bash
cd frontend
npm run build
# Deploy to Azure Static Web Apps
```

3. Configure environment variables in Azure Portal

## iOS App

The iOS app is built using Tauri 2.0 with Rust:

```bash
cd ios
cargo install tauri-cli --version "^2.0.0"
cargo tauri ios build --release
```

Distribute via TestFlight (requires Apple Developer Account).

## Contributing

This is a private, self-hosted platform for family/friends. Fork and customize for your needs.

## License

MIT License - See LICENSE file for details

## Support

For issues and questions, create an issue on GitHub.

---

Built with ❤️ for secure family communication
