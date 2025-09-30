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
- Helm chart (deploy/helm/taskly)

---

## Uruchamianie

Kubernetes + Helm + Ingress-NGINX
```bash
helm dependency update deploy/helm/taskly

helm upgrade --install taskly deploy/helm/taskly -n taskly --create-namespace

```
