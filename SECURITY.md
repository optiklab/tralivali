# Security Policy

## Supported Versions

As Tralivali is a self-hosted platform, we recommend always using the latest version.

| Version | Supported          |
| ------- | ------------------ |
| Latest  | :white_check_mark: |
| Older   | :x:                |

## Reporting a Vulnerability

**Please do not report security vulnerabilities through public GitHub issues.**

If you discover a security vulnerability, please send an email to the repository owner with:

- Description of the vulnerability
- Steps to reproduce
- Potential impact
- Suggested fix (if any)

We will respond as quickly as possible and work with you to address the issue.

## Security Best Practices

When deploying Tralivali:

### 1. Secrets Management
- Never commit secrets to version control
- Use Azure Key Vault or similar for production secrets
- Rotate secrets regularly
- Use strong, unique passwords

### 2. Network Security
- Always use HTTPS in production
- Use Let's Encrypt or proper SSL certificates
- Configure firewall rules appropriately
- Restrict database access to backend only

### 3. Authentication & Authorization
- Use strong JWT secret keys (minimum 32 characters)
- Set appropriate token expiration times
- Implement rate limiting on authentication endpoints
- Monitor for suspicious login attempts

### 4. Database Security
- Use strong MongoDB credentials
- Enable authentication on MongoDB
- Regularly backup your data
- Keep MongoDB updated

### 5. Message Queue Security
- Use strong RabbitMQ credentials
- Enable SSL/TLS for RabbitMQ connections
- Restrict network access to RabbitMQ
- Monitor queue metrics

### 6. Encryption
- End-to-end encryption is implemented client-side
- Private keys are encrypted with user passwords
- Never store plaintext passwords (BCrypt is used)
- Keep encryption libraries updated

### 7. Dependencies
- Regularly update all dependencies
- Monitor for security advisories
- Use Dependabot or similar tools
- Review dependency changes

### 8. Monitoring & Logging
- Enable Application Insights or similar
- Monitor for unusual activity
- Set up alerts for errors
- Log security-relevant events

### 9. Azure Deployment
- Use managed identities where possible
- Enable Azure AD authentication
- Configure network security groups
- Use Azure Security Center recommendations

### 10. Client Security
- Keep browser and Node.js updated
- Review client-side dependencies
- Implement Content Security Policy
- Use Subresource Integrity (SRI)

## Security Features

Tralivali includes several security features:

- **End-to-End Encryption**: Messages encrypted on client
- **Invite-Only**: Prevents unauthorized signups
- **JWT Authentication**: Secure token-based auth
- **Password Hashing**: BCrypt with salt
- **HTTPS Enforcement**: In production
- **CORS Configuration**: Controlled API access

## Known Limitations

- This is a self-hosted platform - security is your responsibility
- Client-side encryption depends on browser crypto APIs
- Rate limiting must be implemented at infrastructure level
- DDoS protection should be configured at cloud provider level

## Updates

We recommend:
- Subscribing to repository updates
- Monitoring security advisories for dependencies
- Testing updates in a staging environment first
- Having a rollback plan for deployments

## Compliance

This platform does not claim any specific compliance certifications. If you need to meet specific compliance requirements (HIPAA, GDPR, etc.), consult with security professionals and implement additional controls as needed.

## Contact

For security concerns, contact the repository owner directly.
