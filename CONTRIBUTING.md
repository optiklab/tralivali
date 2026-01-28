# Contributing to Tralivali

Thank you for your interest in contributing to Tralivali! This document provides guidelines for contributing to the project.

## Code of Conduct

Please be respectful and considerate of others when contributing to this project.

## How to Contribute

### Reporting Bugs

If you find a bug, please create an issue with:
- A clear description of the problem
- Steps to reproduce
- Expected vs actual behavior
- Your environment (OS, .NET version, Node version, etc.)

### Suggesting Features

Feature suggestions are welcome! Please create an issue describing:
- The problem you're trying to solve
- Your proposed solution
- Any alternatives you've considered

### Pull Requests

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes
4. Run tests and ensure code builds
5. Commit your changes (`git commit -m 'Add amazing feature'`)
6. Push to the branch (`git push origin feature/amazing-feature`)
7. Open a Pull Request

### Development Setup

1. Install prerequisites:
   - .NET 8.0 SDK
   - Node.js 20+
   - Docker & Docker Compose
   - Rust (for iOS app)

2. Clone the repository:
   ```bash
   git clone https://github.com/optiklab/tralivali.git
   cd tralivali
   ```

3. Run the setup script:
   ```bash
   ./setup.sh  # Linux/Mac
   # or
   ./setup.ps1  # Windows
   ```

4. Start development servers as needed

### Code Style

#### Backend (.NET)
- Follow standard C# coding conventions
- Use meaningful variable and method names
- Add XML documentation for public APIs
- Keep methods focused and concise

#### Frontend (TypeScript/React)
- Use TypeScript strict mode
- Follow React best practices
- Use functional components with hooks
- Keep components focused and reusable

#### iOS (Rust)
- Follow Rust naming conventions
- Use `cargo fmt` before committing
- Write documentation for public functions
- Handle errors appropriately

### Testing

- Write tests for new features
- Ensure existing tests pass
- Aim for good test coverage
- Test security-critical code thoroughly

### Commit Messages

- Use clear, descriptive commit messages
- Start with a verb (Add, Fix, Update, etc.)
- Keep the first line under 72 characters
- Add more details in the body if needed

Example:
```
Add user profile editing feature

- Add ProfileController with update endpoint
- Create profile edit UI component
- Add validation for profile fields
```

### Security

- Never commit secrets or credentials
- Review security implications of changes
- Report security vulnerabilities privately
- Follow secure coding practices

## Project Structure

```
tralivali/
├── backend/              # .NET Core API
├── frontend/             # React web client
├── ios/                  # Tauri iOS app
├── docs/                 # Documentation
└── .github/workflows/    # CI/CD pipelines
```

## Getting Help

If you need help:
- Check existing issues and documentation
- Create a discussion for questions
- Reach out to maintainers

## License

By contributing, you agree that your contributions will be licensed under the MIT License.
