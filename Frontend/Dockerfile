# ---- Stage 1: build frontend ----
FROM node:20-alpine AS build
WORKDIR /app

COPY client/package*.json ./
RUN npm ci

COPY client/. .
RUN npm run build -- --configuration=production

# ---- Stage 2:
FROM nginxinc/nginx-unprivileged:stable-alpine

COPY --from=build /app/dist/client/browser/ /usr/share/nginx/html/

COPY client/nginx.local.conf /etc/nginx/conf.d/default.conf

EXPOSE 8080
CMD ["nginx", "-g", "daemon off;"]
