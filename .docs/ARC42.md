# arc42 Documentation: URL Shortener

## 1. Introduction and Goals
### 1.1 Requirements Overview
This project is a custom URL shortener that allows users to shorten, manage, and track URLs. It provides analytics, security checks, and QR code generation for enhanced usability.

### 1.2 Quality Goals
- **Performance**: Fast URL redirection with minimal latency.
- **Scalability**: Ability to handle a large number of short links.
- **Security**: Protect users from malicious links and unauthorized access.
- **Usability**: Simple and intuitive interface for link management.

### 1.3 Stakeholders
- **End Users**: Individuals and businesses that want to shorten and track links.
- **Developers**: Maintain and enhance the system.
- **Administrators**: Manage user accounts and security settings.

---

## 2. Architecture Constraints
- **Technology Stack**: Blazor WebAssembly (C#), ASP.NET Core (C#), Microsoft SQL Server.
- **Authentication**: JWT-based authentication.
- **Hosting**: Self-hosted or cloud deployment.
- **Third-party APIs**: [ApicAgent API](https://www.apicagent.com/)

---

## 3. System Scope and Context
### 3.1 Business Context
The URL shortener enables users to shorten long URLs, track clicks, and manage their links securely.

### 3.2 Technical Context
- **Frontend**: Blazor WebAssembly
- **Backend**: ASP.NET Core Web API
- **Database**: SQL Server
- **External APIs**: Safe browsing, geolocation, QR generation

---

## 4. Solution Strategy
- **Microservices-oriented backend** for modularity.
- **JWT authentication** for secure user management.
- **Asynchronous operations** for performance optimization.

---

## 5. Building Block View
### 5.1 Overview

TODO: DIAGRAM

### 5.2 Components
- **Frontend**: UI using Blazor WebAssembly.
- **Backend**: RESTful API in ASP.NET Core.
- **Database**: Stores URL mappings, user data, analytics.
- **APIs**: Used for parsing the user agent of the client.

---

## 6. Runtime View
### 6.1 Shortening a URL
1. User enters a long URL.
2. The backend validates the request.
3. A short URL is generated and stored in the database.
4. The user receives the shortened link.

### 6.2 Redirecting a Short URL
1. User clicks a short URL.
2. The backend retrieves the original URL.
3. User is redirected, and analytics are recorded.

---

## 7. Deployment View
- **Self-hosted** or **cloud-based** deployment options.
- **Containerization** via Docker (optional).
- **SQL Server** hosted on-premise or cloud.

---

## 8. Cross-cutting Concepts
- **Security**: JWT authentication, HTTPS, API security.
- **Error Handling**: Graceful error handling and user-friendly messages.

---

## 9. Architecture Decisions
- **ADR 1**: Implemented a custom URL shortener instead of using third-party services.
- **ADR 2**: JWT-based authentication for scalability and security.
- **ADR 3**: SQL Server chosen for structured data storage.
- **ADR 4**: Blazor WebAssembly selected for frontend development.

View [DECISIONS.md](DECISIONS.md) for more details.

---

## 10. Quality Requirements
- **Performance**: Ensure fast redirections.
- **Security**: Prevent access to users data.
- **Reliability**: Ensure high uptime.

---

## 11. Risks and Technical Debt
- **Risk**: Potential API rate limits from third-party services.
- **Mitigation**: Purchase api key in production.
- **Technical Debt**: Initial simple authentication system, potential for OAuth2 upgrade.

---

## 12. Glossary
- **Short URL**: A compact, redirecting link.
- **JWT**: JSON Web Token, used for authentication.
- **Blazor**: A UI framework for building web applications using C#.
- **ASP.NET Core**: The backend framework used for API development.