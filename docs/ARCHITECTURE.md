# Tralivali Architecture

## System Overview

```
┌─────────────────────────────────────────────────────────────────┐
│                         CLIENT LAYER                             │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  ┌──────────────────┐           ┌──────────────────┐           │
│  │   Web Client     │           │   iOS App        │           │
│  │   React/TypeScript│          │   Tauri 2.0/Rust │           │
│  │   - Login UI     │           │   - Native UI    │           │
│  │   - Messaging UI │           │   - Encryption   │           │
│  │   - Encryption   │           │   - Push Notify  │           │
│  └──────────────────┘           └──────────────────┘           │
│           │                              │                       │
│           └──────────────────────────────┘                       │
│                          │                                       │
│                    HTTPS/WSS                                     │
└────────────────────────────┼───────────────────────────────────┘
                             │
┌────────────────────────────▼───────────────────────────────────┐
│                      API GATEWAY                                │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  ┌────────────────────────────────────────────────────────┐    │
│  │            .NET Core 8 Web API                         │    │
│  │                                                         │    │
│  │  ┌──────────────┐  ┌──────────────┐  ┌─────────────┐ │    │
│  │  │ Auth         │  │ Invites      │  │ Messages    │ │    │
│  │  │ Controller   │  │ Controller   │  │ Controller  │ │    │
│  │  └──────────────┘  └──────────────┘  └─────────────┘ │    │
│  │                                                         │    │
│  │  ┌─────────────────────────────────────────────────┐  │    │
│  │  │         JWT Authentication Middleware           │  │    │
│  │  └─────────────────────────────────────────────────┘  │    │
│  │                                                         │    │
│  │  ┌─────────────────────────────────────────────────┐  │    │
│  │  │              CORS Configuration                 │  │    │
│  │  └─────────────────────────────────────────────────┘  │    │
│  └────────────────────────────────────────────────────────┘    │
│                          │                                      │
└──────────────────────────┼─────────────────────────────────────┘
                           │
            ┌──────────────┴──────────────┐
            │                              │
┌───────────▼──────────┐      ┌───────────▼──────────┐
│   DATA LAYER         │      │   MESSAGE QUEUE      │
│                      │      │                      │
│  ┌────────────────┐ │      │  ┌────────────────┐ │
│  │   MongoDB      │ │      │  │   RabbitMQ     │ │
│  │                │ │      │  │                │ │
│  │  - Users       │ │      │  │  - Message     │ │
│  │  - Messages    │ │      │  │    Delivery    │ │
│  │  - Invites     │ │      │  │  - Real-time   │ │
│  │  - Convos      │ │      │  │    Events      │ │
│  └────────────────┘ │      │  └────────────────┘ │
│                      │      │                      │
└──────────────────────┘      └──────────────────────┘
```

## Data Flow

### 1. User Registration
```
Client → API → Validate Invite → Hash Password → Generate Keys → 
Store in MongoDB → Return JWT Token
```

### 2. User Login
```
Client → API → Verify Password → Generate JWT → Update Last Login → 
Return Token
```

### 3. Send Message
```
Client → Encrypt Message → API → Store in MongoDB → 
Publish to RabbitMQ → Deliver to Recipient
```

### 4. Receive Message
```
RabbitMQ → API → Client WebSocket/Polling → Decrypt Message → Display
```

## Security Layers

```
┌─────────────────────────────────────────────────┐
│           End-to-End Encryption                 │
│  - RSA-OAEP for key exchange                   │
│  - AES-GCM for message content                 │
│  - Client-side encryption/decryption           │
└─────────────────────────────────────────────────┘
                     │
┌─────────────────────────────────────────────────┐
│        Transport Layer Security                 │
│  - HTTPS/TLS 1.3                               │
│  - Let's Encrypt SSL Certificates              │
└─────────────────────────────────────────────────┘
                     │
┌─────────────────────────────────────────────────┐
│       Application Layer Security                │
│  - JWT Authentication                          │
│  - BCrypt Password Hashing                     │
│  - CORS Configuration                          │
│  - Invite-Only Access Control                  │
└─────────────────────────────────────────────────┘
                     │
┌─────────────────────────────────────────────────┐
│        Data Layer Security                      │
│  - MongoDB Authentication                       │
│  - RabbitMQ Authentication                      │
│  - Azure Key Vault for Secrets                 │
└─────────────────────────────────────────────────┘
```

## Deployment Architecture (Azure)

```
┌─────────────────────────────────────────────────────────────┐
│                      Azure Cloud                            │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  ┌────────────────────────────────────────────────────┐    │
│  │              Azure CDN / Front Door                │    │
│  │           (Global Load Balancing, DDoS)            │    │
│  └─────────────────────┬──────────────────────────────┘    │
│                        │                                     │
│  ┌─────────────────────▼──────────────────────────────┐    │
│  │         Azure Static Web Apps                      │    │
│  │         (Frontend - React)                         │    │
│  └────────────────────────────────────────────────────┘    │
│                        │                                     │
│  ┌─────────────────────▼──────────────────────────────┐    │
│  │         Azure App Service                          │    │
│  │         (.NET Core 8 API)                         │    │
│  │         - Auto-scaling                            │    │
│  │         - SSL Certificate                         │    │
│  └─────────────────────┬──────────────────────────────┘    │
│                        │                                     │
│         ┌──────────────┴──────────────┐                    │
│         │                              │                     │
│  ┌──────▼────────┐          ┌─────────▼──────────┐        │
│  │ MongoDB Atlas │          │  Azure Service Bus │        │
│  │ (or Cosmos DB)│          │  (or RabbitMQ VM)  │        │
│  └───────────────┘          └────────────────────┘        │
│                                                              │
│  ┌────────────────────────────────────────────────────┐    │
│  │         Azure Communication Services               │    │
│  │         (Email Invites)                           │    │
│  └────────────────────────────────────────────────────┘    │
│                                                              │
│  ┌────────────────────────────────────────────────────┐    │
│  │         Azure Key Vault                            │    │
│  │         (Secrets Management)                       │    │
│  └────────────────────────────────────────────────────┘    │
│                                                              │
│  ┌────────────────────────────────────────────────────┐    │
│  │         Application Insights                       │    │
│  │         (Monitoring & Logging)                     │    │
│  └────────────────────────────────────────────────────┘    │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

## Encryption Flow

### Message Encryption
```
1. Generate random AES-256 key (message key)
2. Encrypt message with AES-GCM + message key
3. Encrypt message key with recipient's RSA public key
4. Send both encrypted content and encrypted key to server
5. Server stores without decrypting
```

### Message Decryption
```
1. Retrieve encrypted message and encrypted key from server
2. Decrypt message key with user's RSA private key
3. Decrypt message content with decrypted message key
4. Display plaintext message
```

### Key Management
```
User Registration:
1. Generate RSA key pair (2048-bit)
2. Public key → stored on server
3. Private key → encrypted with user password (AES-256)
4. Encrypted private key → stored locally

User Login:
1. Retrieve encrypted private key
2. Decrypt with user password
3. Keep in memory for session
4. Use for decrypting incoming messages
```

## Database Schema

### Users Collection
```json
{
  "_id": "ObjectId",
  "username": "string",
  "email": "string",
  "passwordHash": "string (BCrypt)",
  "publicKey": "string (base64)",
  "privateKeyEncrypted": "string (base64)",
  "createdAt": "DateTime",
  "lastLoginAt": "DateTime",
  "isActive": "boolean",
  "invitedBy": ["userId"],
  "invitedUsers": ["userId"]
}
```

### Messages Collection
```json
{
  "_id": "ObjectId",
  "senderId": "string",
  "recipientId": "string",
  "conversationId": "string",
  "encryptedContent": "string (base64)",
  "encryptedKey": "string (base64)",
  "sentAt": "DateTime",
  "deliveredAt": "DateTime?",
  "readAt": "DateTime?",
  "isDeleted": "boolean"
}
```

### Conversations Collection
```json
{
  "_id": "ObjectId",
  "participantIds": ["string"],
  "lastMessageId": "string",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

### Invites Collection
```json
{
  "_id": "ObjectId",
  "inviterUserId": "string",
  "inviteCode": "string",
  "email": "string",
  "createdAt": "DateTime",
  "expiresAt": "DateTime",
  "isUsed": "boolean",
  "usedAt": "DateTime?",
  "usedByUserId": "string?"
}
```

## Technology Stack Summary

| Component | Technology | Purpose |
|-----------|-----------|---------|
| Backend API | .NET Core 8 | REST API, Business Logic |
| Database | MongoDB | Document Storage |
| Message Queue | RabbitMQ | Real-time Message Delivery |
| Web Client | React + TypeScript | User Interface |
| iOS App | Tauri 2.0 + Rust | Native iOS Application |
| Authentication | JWT + BCrypt | Secure Auth |
| Encryption | Web Crypto API | E2E Encryption |
| Cloud | Azure | Hosting & Services |
| CI/CD | GitHub Actions | Automation |
| Containerization | Docker | Local Development |

## Scalability Considerations

1. **Horizontal Scaling**: App Service can scale out with multiple instances
2. **Database**: MongoDB supports sharding for large datasets
3. **Message Queue**: RabbitMQ clustering for high throughput
4. **Caching**: Can add Redis for session/user data caching
5. **CDN**: Static assets served via Azure CDN
6. **Load Balancing**: Azure Front Door for global distribution

## Monitoring & Observability

- **Application Insights**: Performance monitoring, error tracking
- **Log Analytics**: Centralized logging
- **Metrics**: CPU, memory, request rates
- **Alerts**: Configured for critical errors and performance issues
- **Availability Tests**: Synthetic monitoring for uptime
