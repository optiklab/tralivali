# TelegramLite

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

```bash
git clone https://github.com/optiklab/tralivali.git
cd tralivali
```

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