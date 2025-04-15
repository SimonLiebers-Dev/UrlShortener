# Usage

## Prerequisites
- Docker

## Running the Application

1. Clone this repository.
2. Ensure Docker is installed on your machine.
3. Build and run the project using Docker Compose:

   ```bash
   docker-compose up --build
   ```

   This will start both the frontend and backend services. You can access the app at [http://localhost:1010](http://localhost:1010) (or your configured port).

## Stopping the Application

To stop the services:

```bash
docker-compose down
```