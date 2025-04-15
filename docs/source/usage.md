# Verwendung

## Vorraussetzungen
- Docker

## Running the Application

1. Repository klonen.
2. Stelle sicher, dass Docker auf deinem System installiert ist.
3. Baue und starte das Projekt mit Docker Compose:

   ```bash
   docker-compose up --build
   ```

   Dadurch werden sowohl der Frontend- als auch der Backend-Service gestartet. Du kannst die Anwendung unter [http://localhost:1010](http://localhost:1010) (oder dem von dir konfigurierten Port) aufrufen.

## Stopping the Application

Um die Container zu stoppen:

```bash
docker-compose down
```