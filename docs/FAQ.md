# Frequently Asked Questions (FAQ)

## General Questions

### What is Tralivali?
Tralivali is a self-hosted, invite-only messaging platform designed for family and friends. It features end-to-end encryption, ensuring your conversations remain private.

### Why self-hosted?
Self-hosting gives you complete control over your data, privacy, and infrastructure. You're not dependent on any third-party service.

### What makes it invite-only?
Users can only join with an invite code from an existing member, creating a trusted, closed community.

### Is it really end-to-end encrypted?
Yes! Messages are encrypted on the client side using the Web Crypto API (RSA-OAEP for key exchange, AES-GCM for message content) before being sent to the server.

## Technical Questions

### What technologies are used?
- **Backend**: .NET Core 8, MongoDB, RabbitMQ
- **Frontend**: React with TypeScript, Vite
- **iOS**: Rust with Tauri 2.0
- **Deployment**: Azure, Docker

### Can I use PostgreSQL instead of MongoDB?
The current implementation uses MongoDB. You could adapt it to use PostgreSQL, but it would require modifying the data access layer.

### Can I use Redis instead of RabbitMQ?
RabbitMQ is used for message queuing. While Redis Pub/Sub could work, RabbitMQ provides better durability and message persistence.

### Does it work on Android?
Currently, only web and iOS are supported. Android support could be added using Tauri, React Native, or Flutter.

### Can I run it without Docker?
Yes! You can install MongoDB and RabbitMQ directly on your system. Just update the connection strings in `appsettings.json`.

## Setup & Deployment

### How much does it cost to run on Azure?
Estimated $70-100/month for basic usage:
- App Service B1: ~$13
- MongoDB Atlas M10: ~$57
- Azure Service Bus: ~$0.05
- Static Web Apps: Free tier

### Can I deploy on AWS or Google Cloud?
Yes! The application is cloud-agnostic. You'll need to adapt the deployment scripts for your chosen platform.

### Can I run it on a Raspberry Pi?
Yes, for small-scale personal use. You may need to adjust the MongoDB and RabbitMQ configurations for ARM architecture.

### How do I get Let's Encrypt SSL?
On Azure App Service, use the built-in managed certificate feature. For self-hosted, use Certbot with nginx/Apache.

### How do I backup my data?
Regularly backup MongoDB:
```bash
mongodump --uri="mongodb://localhost:27017/tralivali" --out=/path/to/backup
```

## Security Questions

### Are messages really private?
Yes, messages are encrypted client-side. The server never sees plaintext messages. However, server security is your responsibility.

### What if I lose my password?
There's no password recovery. Your private key is encrypted with your password. If lost, you can't decrypt old messages.

### How secure is the encryption?
The implementation uses industry-standard algorithms (RSA-OAEP, AES-GCM) via the Web Crypto API. However, this is not audited.

### Should I use this for highly sensitive communications?
This is designed for family/friends messaging. For highly sensitive communications, consider audited, dedicated secure messaging apps.

### Can the server admin read messages?
No, if implemented correctly. Messages are encrypted client-side with keys only the users have.

## Usage Questions

### How do I invite someone?
1. Log in
2. Click "Invites"
3. Enter their email
4. Share the generated invite code with them

### Can I revoke an invite?
Currently not implemented in the UI, but you can delete invite codes directly from MongoDB.

### Can I delete messages?
The soft-delete flag exists in the Message model but UI implementation is not included yet.

### How many users can I have?
Limited only by your infrastructure resources. The design should handle hundreds of users comfortably.

### Can I have group chats?
Not yet implemented. The Conversation model supports multiple participants, but the UI and logic need extension.

### Can I send images or files?
Not implemented. You would need to add file upload, storage, and encryption for files.

## Troubleshooting

### Backend won't start
1. Check MongoDB connection: Can you connect with MongoDB Compass?
2. Check RabbitMQ: Is it running? `docker ps`
3. Check logs: Look at console output for specific errors
4. Verify .NET version: `dotnet --version` should be 8.0+

### Frontend shows "Network Error"
1. Is backend running? Check http://localhost:5000
2. Check API URL in `frontend/.env.local`
3. Check browser console for CORS errors
4. Verify backend CORS settings in `Program.cs`

### Can't register new user
1. Verify invite code is valid and not used
2. Check MongoDB has the invite code
3. Look at backend logs for validation errors
4. Ensure all required fields are filled

### Messages not delivering
1. Check RabbitMQ is running: `docker ps`
2. Verify RabbitMQ connection in backend logs
3. Check RabbitMQ management UI: http://localhost:15672
4. Look for queue creation and message publishing

### iOS app won't build
1. Ensure Tauri CLI is installed: `cargo install tauri-cli`
2. Check Rust version: `rustc --version`
3. macOS required for iOS builds
4. Review Tauri 2.0 iOS documentation

### High memory usage
1. Check MongoDB indexes are created
2. Limit message history in queries
3. Implement pagination for large conversations
4. Monitor with Application Insights

### Slow performance
1. Add database indexes for frequently queried fields
2. Implement caching (Redis) for user data
3. Use CDN for frontend static files
4. Optimize message queries with projections

## Development Questions

### How do I contribute?
See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

### Can I customize the UI?
Absolutely! The React components are in `frontend/src/components/`. Modify as needed.

### How do I add a new API endpoint?
1. Add method to appropriate controller
2. Add route in controller attribute
3. Update frontend API service
4. Add TypeScript types if needed

### How do I run tests?
```bash
# Backend
cd backend
dotnet test

# Frontend (if tests exist)
cd frontend
npm test
```

### How do I enable debug logging?
Update `appsettings.Development.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    }
  }
}
```

## License & Legal

### What's the license?
MIT License - see [LICENSE](LICENSE) file.

### Can I use this commercially?
Yes, under the MIT License terms.

### Do I need to credit Tralivali?
Not required, but appreciated!

### Can I sell hosting as a service?
Yes, under the MIT License terms.

## Still Have Questions?

- Check the [README](README.md) for more details
- Review the [Quick Start Guide](docs/QUICK_START.md)
- Create an issue on GitHub
- Review the source code - it's well-commented!

---

Don't see your question? Create an issue and we'll add it!
