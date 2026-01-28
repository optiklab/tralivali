# Quick Start Guide - Tralivali

Get Tralivali up and running in minutes!

## Prerequisites

Before you begin, ensure you have:
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 20+](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

## 5-Minute Setup

### Step 1: Clone the Repository
```bash
git clone https://github.com/optiklab/tralivali.git
cd tralivali
```

### Step 2: Start Infrastructure Services
```bash
# Linux/Mac
./setup.sh

# Windows (PowerShell)
./setup.ps1
```

This will:
- Start MongoDB and RabbitMQ in Docker
- Create your first invite code: `WELCOME1`

### Step 3: Start the Backend
Open a new terminal:
```bash
cd backend
dotnet run --project Tralivali.API
```

The API will start at `http://localhost:5000`

### Step 4: Start the Frontend
Open another terminal:
```bash
cd frontend
npm install  # First time only
npm run dev
```

The web app will start at `http://localhost:5173`

### Step 5: Create Your First Account

1. Open http://localhost:5173 in your browser
2. Click "Register"
3. Fill in your details:
   - Username: your-name
   - Email: your-email@example.com
   - Password: your-secure-password
   - Invite Code: `WELCOME1`
4. Click "Register"

That's it! You're now ready to use Tralivali.

## Next Steps

### Invite Others
1. Log in to your account
2. Click the "Invites" button
3. Enter an email address
4. Share the generated invite code

### Send Messages
1. Start a conversation with another user
2. Type your message
3. Messages are automatically encrypted before sending

### Monitor Services

Access these URLs to monitor your services:

- **RabbitMQ Management**: http://localhost:15672
  - Username: `guest`
  - Password: `guest`

- **MongoDB**: `localhost:27017`
  - Connect with MongoDB Compass or any MongoDB client

## Configuration

### Backend Configuration
Edit `backend/Tralivali.API/appsettings.json`:
```json
{
  "Jwt": {
    "SecretKey": "change-this-to-a-secure-key-at-least-32-chars"
  }
}
```

### Frontend Configuration
Create `frontend/.env.local`:
```
VITE_API_URL=http://localhost:5000/api
```

## Troubleshooting

### Backend won't start
- Check if MongoDB and RabbitMQ are running: `docker ps`
- Verify .NET 8.0 is installed: `dotnet --version`

### Frontend won't start
- Delete `node_modules` and run `npm install` again
- Check Node.js version: `node --version` (should be 20+)

### Can't register
- Verify the invite code is correct
- Check backend logs for errors
- Ensure MongoDB is accessible

### Messages not sending
- Check RabbitMQ is running
- Verify backend can connect to RabbitMQ
- Check browser console for errors

## Manual MongoDB Setup (Alternative)

If you prefer not to use Docker:

1. Install MongoDB locally
2. Start MongoDB: `mongod`
3. Create invite code:
```javascript
use tralivali
db.invites.insertOne({
  inviterUserId: "system",
  inviteCode: "WELCOME1",
  email: "admin@tralivali.local",
  createdAt: new Date(),
  expiresAt: new Date(Date.now() + 365 * 24 * 60 * 60 * 1000),
  isUsed: false
})
```

## Production Deployment

For production deployment to Azure, see [AZURE_DEPLOYMENT.md](AZURE_DEPLOYMENT.md)

## Need Help?

- Check the [main README](README.md) for detailed documentation
- Review [CONTRIBUTING.md](CONTRIBUTING.md) for development guidelines
- Create an issue on GitHub for bugs or questions

---

Happy messaging! 🚀
