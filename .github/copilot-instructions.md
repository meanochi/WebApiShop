Copilot instructions — ShowsCenter (WebApiShop)
=============================================

What this app does
------------------
ShowsCenter is an ASP.NET Core Web API that manages shows, providers, seating sections, orders and users. It exposes REST endpoints for CRUD operations, seat locking/checkout flows, password reset, and sends order confirmation emails. A small static UI in `WebApiShop/wwwroot` is useful for manual end-to-end testing.

Tech stack
----------
- Platform: .NET 9 (C# 13)
- Web: ASP.NET Core Web API
- ORM: EF Core (DbContext in `Entities`)
- Mapping: AutoMapper
- Auth: JWT Bearer tokens (cookies supported)
- Caching: StackExchange.Redis (optional)
- Email: Smtp client (configurable via `Email` section)
- Logging: NLog
- Tests: xUnit integration tests use in-memory Sqlite

Project structure (important folders)
------------------------------------
ShowsCenter/
├─ WebApiShop/      # API entry (Program.cs), controllers, static UI, appsettings
├─ Entities/        # EF models, DbContext, migrations
├─ Repositories/    # data access layer (EF implementations)
├─ Services/        # business logic, helpers, DI registrations, AutoMapper profiles
├─ DTOs/            # request/response DTOs
└─ Tests/           # xUnit integration/unit tests (Sqlite in-memory)

Key files to review first
-------------------------
- `WebApiShop/Program.cs` — DI, auth, Redis, middleware and Swagger wiring.
- `Entities/ShowsCenterContext.cs` — EF model configuration and table mappings.
- `Services/` — business rules and where to add new service logic.
- `Repositories/` — where EF queries and transactions live; tests cover these.
- `WebApiShop/appsettings*.json` — required runtime keys (ConnectionStrings, Jwt, Redis, Email).

How to build, run and test (quick)
---------------------------------
1. Build: `dotnet build` (root)
2. Run API locally: `cd WebApiShop` then `dotnet run`
3. Apply migrations (when changing EF model):
   `dotnet ef database update --project Entities --startup-project WebApiShop`
   (Always pass `--startup-project` so Program.cs and appsettings are picked up.)
4. Run tests: `dotnet test` (tests use in-memory Sqlite and call EnsureCreated)

Common pitfalls & notes
----------------------
- Program.cs: avoid registering `ShowsCenterContext` multiple times — keep a single `AddDbContext` pointing at the intended connection string.
- appsettings keys used: `ConnectionStrings:ShowsCenter` (or DefaultConnection), `Jwt:Issuer`, `Jwt:Audience`, `Jwt:Key`, `Redis:ConnectionString`, `Redis:TTLMinutes`, `Email` section.
- Tests use Sqlite in-memory; when changing entity mappings/migrations update tests that rely on EnsureCreated or snapshots.
- Some source contains Hebrew text; keep files UTF-8 to avoid encoding issues.

Coding guidelines (follow the example repository style)
----------------------------------------------------
1. Layering: Controllers -> Services -> Repositories -> DbContext. Keep controllers thin.
2. Async first: prefer `async Task<T>` signatures throughout services and repositories.
3. Mapping: use AutoMapper profiles for entity <-> DTO mappings. Add new mappings under `Services/AutoMapper.cs`.
4. DTOs: use `record` types for DTOs; never return Entity objects from controllers.
5. Authorization: use IAuth and Manager checks where needed; add attributes/middleware for cross-cutting concerns.
6. Caching: use `ICacheService` for user-level caches; invalidate on update.

Feature development checklist (recommended order)
----------------------------------------------
1. Add/adjust Entity in `Entities/` and register `DbSet` in `ShowsCenterContext`.
2. Add DTO records in `DTOs/` and AutoMapper mapping.
3. Add repository interface + implementation in `Repositories/`.
4. Add service interface + implementation in `Services/` and register in `Program.cs` as `Scoped`.
5. Add controller in `WebApiShop/Controllers/` with route `api/<resource>` and proper model validation and auth attributes.
6. Add unit tests for service logic and an integration test for repository behavior (Sqlite in-memory).

Tests and CI notes
------------------
- Integration tests (in `Tests/`) use Sqlite in-memory and call `EnsureCreated`. They seed data explicitly — inspect tests for required fields when creating entities.
- Run `dotnet test` after changes; when EF model changes require migrations, update/verify tests or use an in-memory approach in tests.

Middleware & pipeline
---------------------
Order matters: exception handling -> rating middleware -> static files -> authentication -> authorization -> controllers. See `Program.cs` for exact wiring.

Commands & tools
----------------
- EF tools: `dotnet tool install --global dotnet-ef`
- Run: `dotnet run --project WebApiShop`
- Tests: `dotnet test`

Repository behaviour hints
-------------------------
- Password strength is checked using zxcvbn via `IPasswordService` (strength >= 2 required in places).
- JWT tokens are produced by `IAuth.GenerateToken` and the app also reads tokens from a `jwt` cookie in `JwtBearerEvents.OnMessageReceived`.
- Email sending uses `EmailSenderOptions` (`Email` section in appsettings); tests may not send email — EmailSender is used in production flows.

Keep this file synced with `README.md` and `Program.cs` when you add services, DI registrations or configuration keys.
