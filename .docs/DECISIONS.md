# Architectural Decision Records (ADR)

## ADR 1: Choice of URL Shortener
**Status**: Decided  
**Date**: 2025-03-18

### Decision:
I have decided to implement the URL shortener myself, rather than relying on an external service. I store all shortened URLs in my own database and allow users to create and manage URLs.
### Justification:
- Full control over URL management and data.
- Ability to integrate additional features like URL tracking and analytics.
- Users can create personalized links.
- Customizability for future features.

### Alternatives:
- Use of an external URL shortening service like Bitly or TinyURL.
    - **Advantages**: Faster implementation, no need for own infrastructure.
    - **Disadvantages**: Limited customizability, dependency on third parties, possibly additional costs.

---

## ADR 2: Choice of Authentication Method
**Status**: Decided  
**Date**: 2025-03-18

### Decision:
I am using JSON Web Tokens (JWT) for user authentication and authorization. The token is stored in the user's browser local storage and sent with every request to the backend.

### Justification:
- JWTs are lightweight and efficient.
- They enable stateless authentication (no session data stored on the server).
- Widely adopted and well-documented, easy integration with Blazor and ASP.NET Core.
- Token-based authentication allows for secure and scalable API design.

### Alternatives:
- Session-based authentication.
    - **Advantages**: Easy to implement for small applications.
    - **Disadvantages**: Requires server resources to manage sessions, not scalable.

---

## ADR 3: Choice of Database
**Status**: Decided  
**Date**: 2025-03-19

### Decision:
I am using SQL Server (Microsoft SQL Server) for the backend, as it integrates well with .NET Core and is required for handling transactions and complex queries for URL data and user information.

### Justification:
- SQL Server is robust, supports ACID transactions, and offers excellent performance.
- Seamless integration with Entity Framework Core.
- Already in use within the company, no additional training required.

### Alternatives:
- Use of NoSQL databases like MongoDB.
    - **Advantages**: Better for unstructured data and horizontal scalability.
    - **Disadvantages**: Less support for complex queries, inconsistent transactions.

---

## ADR 4: Choice of Frontend Technology
**Status**: Decided  
**Date**: 2025-03-19

### Decision:
I am using Blazor WebAssembly for the frontend, as it provides a powerful way to write interactive web applications in C# and benefit from code reuse between frontend and backend.

### Justification:
- C# code can be used on both the frontend and backend, simplifying development.
- Blazor WebAssembly offers native support for .NET and allows WebAssembly execution in the browser.
- Good integration with ASP.NET Core and the ability to use existing UI component libraries.

### Alternatives:
- Use of React or Angular for the frontend.
    - **Advantages**: Widely adopted, large developer community.
    - **Disadvantages**: Requires JavaScript development, less tight integration with the backend.

---