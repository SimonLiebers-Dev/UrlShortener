# UrlShortener

[![CI/CD](https://github.com/SimonLiebers-Dev/UrlShortener/actions/workflows/cicd.yml/badge.svg?branch=main)](https://github.com/SimonLiebers-Dev/UrlShortener/actions/workflows/cicd.yml)
[![Security](https://github.com/SimonLiebers-Dev/UrlShortener/actions/workflows/security.yml/badge.svg?branch=main)](https://github.com/SimonLiebers-Dev/UrlShortener/actions/workflows/security.yml)
[![Docker](https://github.com/SimonLiebers-Dev/UrlShortener/actions/workflows/docker.yml/badge.svg?branch=main)](https://github.com/SimonLiebers-Dev/UrlShortener/actions/workflows/docker.yml)
[![Documentation](https://github.com/SimonLiebers-Dev/UrlShortener/actions/workflows/docs.yml/badge.svg?branch=main)](https://github.com/SimonLiebers-Dev/UrlShortener/actions/workflows/docs.yml)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SimonLiebers-Dev_UrlShortener&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=SimonLiebers-Dev_UrlShortener)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=SimonLiebers-Dev_UrlShortener&metric=coverage)](https://sonarcloud.io/summary/new_code?id=SimonLiebers-Dev_UrlShortener)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=SimonLiebers-Dev_UrlShortener&metric=bugs)](https://sonarcloud.io/summary/new_code?id=SimonLiebers-Dev_UrlShortener)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=SimonLiebers-Dev_UrlShortener&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=SimonLiebers-Dev_UrlShortener)

**UrlShortener** ist ein selbst entwickeltes Tool zum kürzen von langen URLs mit integrierter Nutzerverwaltung und detailliertem Klick-Tracking. Die AnwendunPg ermöglicht registrierten Nutzer:innen, lange URLs zu verkürzen und deren Nutzung auszuwerten. Die Architektur basiert vollständig auf dem .NET-Ökosystem und ist für den produktiven Einsatz containerisiert.

📄 **Die vollständige Projektdokumentation ist verfügbar unter:**  
👉 [ReadTheDocs](https://urlshortener.readthedocs.io/)
👉 [GitHub Page](https://simonliebers-dev.github.io/UrlShortener/)

## Funktionen

- **Benutzerauthentifizierung**: Registrierung und Login mit JWT-basierter Authentifizierung.
- **URL-Kürzung**: Lange URLs können in kurze Links umgewandelt und verwaltet werden.
- **Tracking & Analyse**: Jeder Zugriff auf eine Kurz-URL wird getrackt, inklusive Geräteinformationen aus dem User-Agent.

## Technologie-Stack

- **Backend**: .NET 9, ASP.NET Core, Entity Framework Core
- **Frontend**: Blazor Web App (gehostetes Modell)
- **Datenbank**: Microsoft SQL Server
- **Authentifizierung**: JWT (JSON Web Token)
- **Codequalität**: SonarQube
- **Containerisierung**: Docker

## Erste Schritte

### Voraussetzungen

- Installiertes Docker

### Anwendung starten

1. Repository klonen.
2. Sicherstellen, dass Docker installiert ist.
3. Anwendung mit Docker Compose bauen und starten:

   ```bash
   docker-compose up --build
   ```

   Die Anwendung ist anschließend unter [http://localhost:1010](http://localhost:1010) erreichbar (abhängig von der Port-Konfiguration).

### Anwendung stoppen

```bash
docker-compose down
```

## License
MIT License