# Docker Setup - On Demand Tutor Website

## Prerequisites

- Docker & Docker Compose installed
- OpenSSL installed (for certificate generation)

## Quick Start

```bash
cd docker
bash start.sh
```

Or manually:

```bash
cd docker

# 1. Generate SSL certificates (first time only)
bash generate-certs.sh

# 2. Start all services (drops DB and re-seeds data every time)
docker compose down -v
docker compose up --build
```

## Services

| Service    | URL                            | Description       |
| ---------- | ------------------------------ | ----------------- |
| Frontend   | https://localhost:3000         | React App (nginx) |
| Backend    | https://localhost:7010         | ASP.NET Core API  |
| Swagger    | https://localhost:7010/swagger | API Documentation |
| SQL Server | localhost:1433                 | Database          |

## Test Accounts

All accounts use password: `123456`

| Role    | Email            |
| ------- | ---------------- |
| Admin   | admin@odt.com    |
| Tutor   | tutor1@odt.com   |
| Tutor   | tutor2@odt.com   |
| Tutor   | tutor3@odt.com   |
| Learner | learner1@odt.com |
| Learner | learner2@odt.com |
| Learner | learner3@odt.com |

## Environment Variables

Edit `docker/.env` to change:

- `SA_PASSWORD` - SQL Server SA password
- `CERT_PASSWORD` - HTTPS certificate password

## Notes

- `docker compose down -v` removes the SQL Server data volume
- Every `docker compose up` with the `-v` flag will drop and re-create the database with fresh seed data
- The frontend uses nginx as a reverse proxy to route `/api/*` and `/chatHub` requests to the backend
