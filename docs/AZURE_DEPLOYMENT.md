# Azure Deployment Configuration for Tralivali

## Prerequisites
- Azure subscription
- Azure CLI installed
- Docker installed (for containerization)

## Azure Resources Required
1. App Service (Linux) for backend API
2. Azure Database for MongoDB (or MongoDB Atlas)
3. Azure Service Bus (alternative to RabbitMQ) or self-hosted RabbitMQ on VM
4. Azure Communication Services for email
5. Azure Key Vault for secrets
6. Azure Application Insights for monitoring

## Deployment Steps

### 1. Create Resource Group
```bash
az group create --name tralivali-rg --location eastus
```

### 2. Create App Service Plan
```bash
az appservice plan create \
  --name tralivali-plan \
  --resource-group tralivali-rg \
  --is-linux \
  --sku B1
```

### 3. Create Web App
```bash
az webapp create \
  --name tralivali-api \
  --resource-group tralivali-rg \
  --plan tralivali-plan \
  --runtime "DOTNETCORE:8.0"
```

### 4. Configure SSL with Let's Encrypt
- Enable HTTPS Only in Azure Portal
- Add custom domain
- Use App Service Managed Certificate (free) or configure Let's Encrypt via:
  - Extension: https://github.com/shibayan/appservice-acmebot

### 5. Configure Application Settings
```bash
az webapp config appsettings set \
  --name tralivali-api \
  --resource-group tralivali-rg \
  --settings \
    MongoDb__ConnectionString="<your-mongodb-connection-string>" \
    MongoDb__DatabaseName="tralivali" \
    RabbitMq__HostName="<rabbitmq-host>" \
    Jwt__SecretKey="<your-secret-key>" \
    AzureCommunication__ConnectionString="<acs-connection-string>"
```

### 6. Deploy Backend
```bash
cd backend
dotnet publish -c Release -o ./publish
cd publish
zip -r ../deploy.zip .
az webapp deployment source config-zip \
  --name tralivali-api \
  --resource-group tralivali-rg \
  --src ../deploy.zip
```

### 7. Setup Azure Communication Services
```bash
# Create Azure Communication Services resource
az communication create \
  --name tralivali-acs \
  --resource-group tralivali-rg \
  --location global \
  --data-location UnitedStates

# Get connection string
az communication list-key \
  --name tralivali-acs \
  --resource-group tralivali-rg
```

### 8. Deploy Frontend to Azure Static Web Apps
```bash
# Install Static Web Apps CLI
npm install -g @azure/static-web-apps-cli

# Deploy
cd frontend
npm run build
az staticwebapp create \
  --name tralivali-web \
  --resource-group tralivali-rg \
  --source ./dist \
  --location "East US 2" \
  --branch main \
  --app-location "/" \
  --output-location "dist"
```

## iOS App Distribution via TestFlight

### Prerequisites
- Apple Developer Account ($99/year)
- Xcode installed on macOS
- App Store Connect access

### Steps

1. **Build iOS App with Tauri**
```bash
cd ios
# Install Tauri CLI
cargo install tauri-cli --version "^2.0.0"
# Build for iOS
cargo tauri ios build --release
```

2. **Configure App Store Connect**
- Create App ID in Apple Developer Portal
- Create provisioning profiles
- Configure App Store Connect entry

3. **Upload to TestFlight**
- Archive the app in Xcode
- Upload to App Store Connect
- Submit for TestFlight review
- Add internal/external testers

4. **Distribute Invite Links**
- Generate public link in TestFlight
- Share with beta testers
- Monitor feedback and crashes

## Environment Variables

Create `.env` files for each environment:

**backend/.env.production**
```
MongoDb__ConnectionString=<production-mongodb>
RabbitMq__HostName=<production-rabbitmq>
Jwt__SecretKey=<strong-secret-key>
AzureCommunication__ConnectionString=<acs-connection>
```

**frontend/.env.production**
```
VITE_API_URL=https://tralivali-api.azurewebsites.net/api
```

## Monitoring and Maintenance

1. Enable Application Insights
2. Set up alerts for errors and performance
3. Configure automated backups for MongoDB
4. Implement CI/CD with GitHub Actions or Azure DevOps
5. Regular security updates

## Cost Estimation (Monthly)

- App Service B1: ~$13
- MongoDB Atlas M10: ~$57
- Azure Service Bus Basic: ~$0.05
- Azure Communication Services: Pay per use
- Static Web Apps: Free tier available
- Total: ~$70-100/month (excluding email costs)
