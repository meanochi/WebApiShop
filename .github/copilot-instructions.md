# Copilot instructions — ShowsCenter (WebApiShop)

## **What this app does**

ShowsCenter is a layered ASP.NET Core 9 Web API for event management and ticketing.
It manages the following core entities:

| Domain               | Responsibility                                                      |
| -------------------- | ------------------------------------------------------------------- |
| **Shows**            | Create, update, publish, and query event listings                   |
| **Providers**        | Manage event operators and show owners                              |
| **Seating Sections** | Define venue sections, seat tiers, and pricing                      |
| **Orders**           | Reserve seats, create orders, checkout, and manage order lifecycles |
| **Users**            | Register, authenticate, and manage user profiles                    |
| **Password Reset**   | Issue reset codes, validate tokens, and update passwords            |

The app also sends order confirmation emails and includes a small static UI under `WebApiShop/wwwroot` for manual validation.

## **Tech stack**

| Area        | Technology                           |
| ----------- | ------------------------------------ |
| Framework   | .NET 9, C# 13                        |
| Web         | ASP.NET Core Web API                 |
| Data access | Entity Framework Core 9 (Code First) |
| Mapping     | AutoMapper                           |
| Auth        | JWT Bearer tokens + cookie fallback  |
| Cache       | StackExchange.Redis (optional)       |
| Logging     | NLog                                 |
| Testing     | xUnit with in-memory Sqlite          |
| API docs    | Swagger / OpenAPI                    |

## **Domain and architecture overview**

This project uses a layered monolith architecture.
The canonical flow is:

**Controllers → Services → Repositories → Entities**

### **Layer responsibilities**

| Layer        | Responsibility                                         |
| ------------ | ------------------------------------------------------ |
| Controllers  | Handle HTTP requests, validate input, return responses |
| Services     | Business logic, validation, orchestration              |
| Repositories | EF Core persistence and queries                        |
| Entities     | Domain models, navigation properties, DB mapping       |

### **Architecture rules**

- **Controllers must never access `DbContext` directly.**
- **Controllers should only call service interfaces.**
- **Services may call repositories and perform business logic.**
- **Repositories may only interact with EF Core.**
- **Do not place AutoMapper or DTO mapping inside repositories.**

## **Solution structure**

Root solution: `WebApiShop.sln`

| Project         | Purpose                                                               |
| --------------- | --------------------------------------------------------------------- |
| `WebApiShop/`   | API startup, controllers, middleware, appsettings, Swagger, static UI |
| `Entities/`     | EF entities, `ShowsCenterContext`, migrations                         |
| `Repositories/` | Repository interfaces and EF implementations                          |
| `Services/`     | Business logic, service interfaces, AutoMapper profiles               |
| `DTOs/`         | Request/response contract records                                     |
| `Tests/`        | xUnit unit and integration tests                                      |

### **Folder responsibilities**

- `DTOs/`: request and response DTO records only.
- `Entities/`: domain models, navigation properties, and DbContext mapping.
- `Repositories/`: persistence operations and query logic.
- `Services/`: business rules, validation, and orchestration.
- `WebApiShop/Controllers/`: HTTP-specific handling and routing.

## **Modern .NET 9 standards**

Use idiomatic C# 13 features across the codebase.

### **Standards**

- Prefer primary constructors for DI in services, repositories, and controllers.
- Use collection expressions for DTO initialization.
- Use `required` properties in DTOs when values are mandatory.
- Use file-scoped namespaces.
- Use `async Task` / `Task<T>` for asynchronous I/O.
- Accept `CancellationToken` in service and repository methods.

### **Example**

```csharp
public sealed class ShowService : IShowService
{
    private readonly IShowRepository _showRepository;

    public ShowService(IShowRepository showRepository) => _showRepository = showRepository;
}
```

## **Error handling and logging**

This repository uses centralized middleware and `ProblemDetails`.

### **Error handling**

- Use `app.UseExceptionHandler()` in `Program.cs`.
- Implement custom domain and validation exceptions.
- Convert exceptions to `ProblemDetails` responses.
- Avoid throwing raw `Exception` from service code whenever possible.

### **Logging**

- Use NLog for structured logging.
- Inject `ILogger<T>` in services and controllers.
- Enrich logs with request IDs and correlation data.
- Avoid logging secrets or sensitive values.

## **Configuration and security**

### **Mandatory configuration keys**

| Key                             | Purpose                          |
| ------------------------------- | -------------------------------- |
| `ConnectionStrings:ShowsCenter` | Database connection              |
| `Jwt:Issuer`                    | JWT issuer validation            |
| `Jwt:Audience`                  | JWT audience validation          |
| `Jwt:Key`                       | JWT signing key                  |
| `Redis:ConnectionString`        | Redis endpoint                   |
| `Email:SmtpHost`                | SMTP server host                 |
| `Email:SmtpPort`                | SMTP server port                 |
| `Email:Username`                | SMTP username                    |
| `Email:Password`                | SMTP password                    |
| `Email:FromAddress`             | Sender email address             |
| `PasswordReset`                 | Password reset workflow settings |

### **Validation guidance**

- Validate required config keys at startup.
- Fail fast with clear messages if settings are missing.
- Prefer `IConfiguration.GetValue<string>(...)`.
- Do not hardcode secrets.

## **Testing guidance**

Tests are the source of truth.

### **Requirements**

- New features must include tests.
- Use xUnit for unit and integration tests.
- Use in-memory Sqlite for persistence tests.
- Seed test data explicitly.
- Call `EnsureCreated()` when required.
- Keep tests deterministic.

### **Recommended coverage**

- Unit tests for service business logic.
- Repository tests for EF mapping and queries.
- Integration tests for controller routes and persistence flows.

## **Preserve Hebrew support**

- Save files as UTF-8.
- Preserve Hebrew text and punctuation.
- Do not remove or corrupt Hebrew comments.

## **Agent quick start**

1. Read `WebApiShop/Program.cs`.
2. Review `Entities/ShowsCenterContext.cs`.
3. Inspect `Services/` and `Repositories/`.
4. Review `Tests/` for expected behavior.
5. Check `WebApiShop/appsettings.json` and `WebApiShop/appsettings.Development.json`.

## **Build and run commands**

- `dotnet restore`
- `dotnet build`
- `cd WebApiShop && dotnet run`
- `dotnet test`
- `dotnet ef migrations add <Name> --project Entities --startup-project WebApiShop`
- `dotnet ef database update --project Entities --startup-project WebApiShop`

## **Important rules and pitfalls**

- Do not inject `ShowsCenterContext` into controllers.
- Do not query the database from controllers.
- Keep controllers thin.
- Keep `Program.cs` focused on startup wiring.
- Avoid duplicate `DbContext` registrations.
- Use `record` DTOs for API contracts.
- Prefer `Scoped` for services and repositories.
- Update docs and tests when configuration or runtime behavior changes.

## **If uncertain**

- Prefer small, incremental changes.
- Run `dotnet test` frequently.
- Validate behavior against existing controllers and tests.
- Keep the layered architecture intact.
