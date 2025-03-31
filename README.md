# UrlShortener
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SimonLiebers-Dev_UrlShortener&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=SimonLiebers-Dev_UrlShortener)
[![Pipeline](https://github.com/SimonLiebers-Dev/UrlShortener/actions/workflows/workflow.yml/badge.svg?branch=main)](https://github.com/SimonLiebers-Dev/UrlShortener/actions/workflows/workflow.yml)

A URL shortener service with user authentication, URL mapping, and redirect tracking.

## Features
- User Authentication: Register and log in to the service using JWT tokens.
- URL Shortening: Create custom short URLs and map them to long URLs.
- Redirect Tracking: Track user interactions with short URLs including IP and user agent.
- SonarQube Integration: Code quality analysis with SonarQube.

## Technologies
- Backend: .NET 9, ASP.NET Core, Entity Framework Core
- Frontend: Blazor Web App (Hosted)
- Database: SQL Server 
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