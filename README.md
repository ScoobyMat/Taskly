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
- K8s

---

## Uruchamianie

Kubernetes + Ingress-NGINX
```bash
kubectl apply -k deploy/k8s/
```
