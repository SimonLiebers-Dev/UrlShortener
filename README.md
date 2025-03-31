# UrlShortener
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SimonLiebers-Dev_UrlShortener&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=SimonLiebers-Dev_UrlShortener)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=SimonLiebers-Dev_UrlShortener&metric=coverage)](https://sonarcloud.io/summary/new_code?id=SimonLiebers-Dev_UrlShortener)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=SimonLiebers-Dev_UrlShortener&metric=bugs)](https://sonarcloud.io/summary/new_code?id=SimonLiebers-Dev_UrlShortener)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=SimonLiebers-Dev_UrlShortener&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=SimonLiebers-Dev_UrlShortener)
[![Pipeline](https://github.com/SimonLiebers-Dev/UrlShortener/actions/workflows/workflow.yml/badge.svg?branch=main)](https://github.com/SimonLiebers-Dev/UrlShortener/actions/workflows/workflow.yml)

This is a custom URL shortener built with Blazor WebAssembly and ASP.NET Core. It allows users to shorten URLs and track usage.

## Features
- User Authentication: User registration and login system with JWT authentication.
- URL Shortening: Generate short URLs from long links.
- Redirect Tracking: Track the number of clicks for each short link. Capture visitor details from user agent.

## Tech Stack
- Backend: .NET 9, ASP.NET Core, Entity Framework Core
- Frontend: Blazor Web App (Hosted)
- Database: Microsoft SQL Server 
- Authentication: JWT
- Code Quality: SonarQube
- Containerization: Docker

## Getting Started

### Prerequisites
- Docker

### Running the Application

1. Clone this repository.
2. Ensure Docker is installed on your machine.
3. Build and run the project using Docker Compose:

   ```bash
   docker-compose up --build
   ```

   This will start both the frontend and backend services. You can access the app at [http://localhost:1010](http://localhost:1010) (or your configured port).

### Stopping the Application

To stop the services:

```bash
docker-compose down
```

## License
MIT License