# Taskly - **Task Manager**

---

## Spis treści
- [Technologie](#technologie)
- [Uruchamianie](#uruchamianie)
---

## Technologie

**Frontend**
- Angular
- Tailwind CSS + DaisyUI

**Backend**
- .NET **9** / ASP.NET Core
- Onion Architecture: **API** / **Application** / **Domain** / **Infrastructure**
- EF Core + PostgreSQL
- **JWT** (autentykacja), **BCrypt** (hasła), **AutoMapper**

**Deployment**
- Dockerfile
- Docker-compose

---

## Uruchamianie

```bash
cp .env.sample .env
docker compose -f deploy/docker-compose/docker-compose.yml up --build
# Frontend:  http://localhost:4200
# Backend: http://localhost:5001/api/health
```

