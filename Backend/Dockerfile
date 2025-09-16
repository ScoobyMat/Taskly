# ---- Stage 1: Build ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY API/API.csproj API/
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY Taskly.sln .

RUN dotnet restore API/API.csproj

COPY . .
WORKDIR /src/API

RUN dotnet publish API.csproj -c Release -o /out /p:UseAppHost=false --no-restore

# ---- Stage 2: Runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /out .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

RUN groupadd -g 1001 myapp && useradd -u 1001 -g myapp myapp
USER myapp:myapp

ENTRYPOINT ["dotnet","API.dll"]
