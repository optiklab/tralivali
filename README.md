# Tralivali

A self-hosted, invite-only messaging platform for family and friends with end-to-end encryption.

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

### Local Development

1. **Clone the repository**
```bash
git clone https://github.com/optiklab/tralivali.git
cd tralivali
```

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