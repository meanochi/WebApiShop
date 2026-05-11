# Repository developer instructions — ShowsCenter

## **Purpose**

This file is the repository-level source of truth for developers and automation working on ShowsCenter.
It documents project structure, conventions, and safe change patterns.

## **Application overview**

ShowsCenter is an event ticketing backend that includes:

| Domain               | Responsibility                                                           |
| -------------------- | ------------------------------------------------------------------------ |
| **Shows**            | Manage event listings, schedules, categories, and provider relationships |
| **Providers**        | Manage event organizers and venue operators                              |
| **Seating Sections** | Define priced seating tiers and venue sections                           |
| **Orders**           | Reserve seats, checkout, and manage order status                         |
| **Users**            | Register, authenticate, and manage profiles                              |
| **Password Reset**   | Issue reset codes, validate tokens, and update passwords securely        |

The app also sends order confirmation emails and exposes a small static UI for manual verification.

## **Project structure**

Root solution: `WebApiShop.sln`

| Project         | Role                                                                    |
| --------------- | ----------------------------------------------------------------------- |
| `WebApiShop/`   | API startup, controllers, middleware, configuration, Swagger, static UI |
| `Entities/`     | EF Core entities, `ShowsCenterContext`, migrations                      |
| `Repositories/` | Repository interfaces and EF persistence implementations                |
| `Services/`     | Business services, validation, DI registration, AutoMapper profiles     |
| `DTOs/`         | Request/response contract records for API boundaries                    |
| `Tests/`        | xUnit unit and integration tests                                        |

### **Folder expectations**

- **`DTOs/`**: request and response records only. Do not put persistence logic here.
- **`Entities/`**: domain models, navigation properties, DB mapping.
- **`Repositories/`**: data access, queries, and persistence operations.
- **`Services/`**: business logic, validation, and orchestration.
- **`WebApiShop/Controllers/`**: HTTP request handling, model binding, and response shaping.

## **Layering rule**

The canonical dependency flow is:

**Controllers → Services → Repositories → Entities**

### **Do not do**

- Do not use `ShowsCenterContext` in controllers.
- Do not place business rules in controllers.
- Do not bypass service or repository boundaries.
- Do not put data access logic in `Services/` or `Controllers/`.

## **Build, run, and test**

### **Commands**

| Command                       | Purpose                |
| ----------------------------- | ---------------------- |
| `dotnet restore`              | Restore NuGet packages |
| `dotnet build`                | Build the solution     |
| `cd WebApiShop && dotnet run` | Run the API locally    |
| `dotnet test`                 | Run all tests          |

### **EF Core migrations**

| Action          | Command                                                                           |
| --------------- | --------------------------------------------------------------------------------- |
| Add migration   | `dotnet ef migrations add <Name> --project Entities --startup-project WebApiShop` |
| Apply migration | `dotnet ef database update --project Entities --startup-project WebApiShop`       |

## **Configuration and runtime validation**

### **Required keys**

| Key                             | Purpose                          |
| ------------------------------- | -------------------------------- |
| `ConnectionStrings:ShowsCenter` | SQL Server connection            |
| `Jwt:Issuer`                    | JWT issuer validation            |
| `Jwt:Audience`                  | JWT audience validation          |
| `Jwt:Key`                       | JWT signing key                  |
| `Redis:ConnectionString`        | Redis cache endpoint             |
| `Redis:TTLMinutes`              | Redis cache expiration           |
| `Email:SmtpHost`                | SMTP server host                 |
| `Email:SmtpPort`                | SMTP server port                 |
| `Email:Username`                | SMTP login username              |
| `Email:Password`                | SMTP login password              |
| `Email:FromAddress`             | Sender email address             |
| `PasswordReset:*`               | Password reset workflow settings |

### **Validation rules**

- Validate required config values at startup.
- Fail fast with explicit error messages.
- Do not hardcode secrets.
- Accept that `appsettings.Development.json` may differ from production configs.

## **Coding conventions**

- Use C# 13 idioms when they improve clarity.
- Prefer primary constructors for DI.
- Use collection expressions for simple collections.
- Use `record` types for DTOs in `DTOs/`.
- Use `required` properties in DTOs when values are mandatory.
- Keep methods async with `Task` / `Task<T>`.
- Accept `CancellationToken` on async I/O and long-running methods.
- Use file-scoped namespaces.

### **Example**

```csharp
public sealed class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository) => _orderRepository = orderRepository;
}
```

## **Error handling and logging**

- Use centralized exception handling middleware.
- Use `ProblemDetails` for consistent API errors.
- Map domain and validation failures to appropriate HTTP responses.
- Use `ILogger<T>` for structured logging.
- Do not log passwords, secrets, or PII.
- Keep logging consistent and descriptive.

## **Testing requirements**

- New features must include automated tests.
- Add xUnit unit tests for service or business logic.
- Add integration tests when persistence or API routes are involved.
- Use in-memory Sqlite for integration tests.
- Seed test data explicitly.
- Keep tests deterministic and isolated.

## **Repository-specific rules**

- Register services and repositories as `Scoped` by default.
- Keep `Program.cs` focused on DI, middleware, and startup configuration.
- Avoid duplicate `ShowsCenterContext` registrations.
- Prefer interface injection over concrete types in controllers.
- Keep AutoMapper profiles in `Services/AutoMapper.cs`.

## **Agent checklist**

- Review `WebApiShop/Program.cs`, `Entities/ShowsCenterContext.cs`, and `Tests/`.
- Review `WebApiShop/appsettings.json` and `WebApiShop/appsettings.Development.json`.
- Confirm required config keys used by the current startup code.
- Run `dotnet test` after each code change.
- Preserve UTF-8 encoding for Hebrew text.

## **Common failure modes**

- Missing required configuration keys.
- Controllers accessing `DbContext`.
- Missing tests for new service behavior.
- Adding runtime dependencies without DI/config updates.
- Changing DTO/routes without updating tests.

## **Notes**

- There is no `CONTRIBUTING.md` in this repo.
- Use `README.md`, `Program.cs`, and `Tests/` as the main authoritative sources.
- Update documentation when new startup or configuration behavior is added.
