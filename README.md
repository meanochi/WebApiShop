# 🎭 ShowsCenter — Web API

A clean, production-ready REST API for managing shows, seating, orders and providers. Built on **.NET 9** with a layered architecture designed for testability and extensibility.

> 🖥️ **Frontend companion:** [ShowsCenter (Angular)](https://github.com/meanochi/ShowsCenter)

![.NET](https://img.shields.io/badge/.NET_9-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=csharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat-square&logo=docker&logoColor=white)

---

## ✨ Features

- Full CRUD for shows, categories, providers, and seating sections
- Filtering, sorting and pagination on show listings
- JWT-based authentication and role management
- AutoMapper between entities and DTOs
- EF Core with SQL Server
- Docker support via `docker-compose`
- Unit tests in dedicated `Tests` project

---

## 🚀 Quick Start

### Prerequisites
- .NET 9 SDK
- SQL Server (local or Docker)

### Run locally

```bash
# 1. Clone
git clone https://github.com/meanochi/WebApiShop.git
cd WebApiShop

# 2. Configure connection string in WebApiShop/appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=.;Initial Catalog=ShowsCenter;Integrated Security=True;TrustServerCertificate=True"
  }
}

# 3. Apply migrations
cd WebApiShop
dotnet ef database update --project ../Entities --startup-project .

# 4. Run
dotnet run
```

### Run with Docker

```bash
docker-compose up
```

---

## 🏗️ Architecture

```
WebApiShop/
├── Entities/        # POCO models & EF Core DbContext
├── Repositories/    # Data access layer (EF Core queries)
├── Services/        # Business logic & validation
├── WebApiShop/      # ASP.NET Core controllers, DI, Swagger
├── DTOs/            # Request/response data transfer objects
└── Tests/           # Unit tests
```

Each layer has a single responsibility — controllers stay thin, business rules live in Services, data access is isolated in Repositories.

---

## 📡 Example Requests

```bash
# Get paginated, sorted shows
GET /api/shows?position=1&skip=10&sortField=price&sortOrder=1

# Get show by ID
GET /api/shows/5

# Create a show
POST /api/shows
{
  "title": "The Little Concert",
  "date": "2026-06-15",
  "beginTime": "19:30:00",
  "endTime": "21:30:00",
  "sector": "Main Hall",
  "providerId": 1,
  "categoryId": 2
}
```

Full interactive docs available at `/swagger` after running.

---

## 🧪 Tests

```bash
cd Tests
dotnet test
```
