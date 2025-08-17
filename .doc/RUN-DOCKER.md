# Running with Docker

This guide explains how to run the Developer Evaluation project using Docker.

## 🚀 Quick Start (Recommended)

If you want a clean build and reset all volumes:

```bash
docker-compose down -v
docker-compose up --build
```

This will:
- Stop and remove existing containers, networks, and volumes.
- Rebuild and start the services defined in `docker-compose.yml`.

Once complete, the application will be running and ready to use.

---

## 🐳 Project Requirements

- [Docker](https://docs.docker.com/get-docker/) (v20+ recommended)
- [Docker Compose](https://docs.docker.com/compose/install/)

---

## 🧪 Running Tests

To run tests inside the container:

```bash
docker exec -it <container_name> dotnet test
```

> Tip: Use `docker ps` to find the running container name.

---

## 🌐 Accessing the API

Once the containers are up, access the API at:

```
http://localhost:8080/swagger/index.html
```
---

## 🧹 Clean Up

To stop the application and remove all containers and volumes:

```bash
docker-compose down -v
```

---

## 🛠️ Troubleshooting

- If ports are already in use, make sure previous containers are stopped:
  ```bash
  docker ps
  docker stop <container_id>
  ```

- If rebuilds are not reflecting changes, use:
  ```bash
  docker-compose build --no-cache
  ```

---

For any other issues, please refer to the project documentation or contact the maintainers.


## ⚙️ How to Run the Application

### 🔧 Requirements

- Docker & Docker Compose

### ▶ Run with Docker

```bash
docker-compose up --build