# 🚛 project-exec.md

## 📦 Requirements

Make sure the following prerequisites are installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)
- (Optional) [MongoDB Compass](https://www.mongodb.com/try/download/compass) to visualize persisted events

---

## ⚙️ Configuration

### Environment Variables and Secrets

The application uses `UserSecrets` and local HTTPS certificates. Make sure the `volumes` paths are correct in the `docker-compose.yml`:

```yaml
volumes:
  - ${APPDATA}/Microsoft/UserSecrets:/home/app/.microsoft/usersecrets:ro
  - ${APPDATA}/ASP.NET/Https:/home/app/.aspnet/https:ro
```

If you're on Linux or macOS, adjust the paths accordingly for your OS.

---

## ▶️ Running with Docker

In the root of the project (where the `docker-compose.yml` file is located):

```bash
docker-compose up --build
```

The application will be available at:

- API: [http://localhost:8080](http://localhost:8080)
- Swagger: [http://localhost:8080/swagger](http://localhost:8080/swagger)
- MongoDB: `mongodb://developer:ev@luAt10n@localhost:27017`
- PostgreSQL: `Host=localhost;Port=5432;Database=developer_evaluation;Username=developer;Password=ev@luAt10n`

---

## 🧲 How to Test

You can test the application in the following ways:

### 1. Swagger

Visit `http://localhost:8080/swagger` to execute the endpoints directly.

### 2. Postman

Manually import the endpoints or use a `.postman_collection.json` file if available.

### 3. MongoDB Compass

Connect using:

- **Host**: `localhost`
- **Port**: `27017`
- **User**: `developer`
- **Password**: `ev@luAt10n`

View persisted events in the `DomainEvents` collection of the configured database.

---

## 🧹 Stopping and Cleaning Up

To stop the containers and remove volumes:

```bash
docker-compose down -v
```

This command stops the containers and removes persistent database data.

---

## 🖊️ Notes

- PostgreSQL is used for relational persistence.
- MongoDB is used as an event store for domain events.
- Events can be validated directly in MongoDB based on the `DomainEvents` collection.
- The application follows the DDD pattern with event publishing for `SaleCreated`, `SaleModified`, etc.

---
