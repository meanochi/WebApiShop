## Plan: Microservices Architecture Plan

TL;DR: Define a microservices solution based on DDD boundaries, shared kernel artifacts, and modern .NET 9 infrastructure. Produce a new reference file `architecture_plan.prompt` describing the service structure, shared library, Program.cs configurations, communication flows, and container orchestration.

### Phase 1: Project Structure

Solution: `ShowsCenter.sln`

Projects:

- `ApiGateway` — YARP-based gateway for routing, authentication, and aggregation.
- `SharedKernel` — common contracts, DTOs, exceptions, events, and telemetry helpers.
- `Identity.Service` — user management, authentication, authorization, tokens, and local identity store.
- `Catalog.Service` — shows, categories, providers, seating sections, and public catalog queries.
- `Ordering.Service` — order lifecycle, checkout, seat reservation, and transactional order processing.
- `Notification.Service` — email, SMS, and event-driven notifications.
- `Ratings.Service` — user ratings, reviews, and rating aggregates.
- `Infrastructure.Common` (optional) — shared service registration extenders, MassTransit configuration, and cross-cutting middleware wiring.
- `Observability` (optional) — centralized dashboards or exporters if split from app services.
- `Tests` — integration and unit tests for each service project.

Folder hierarchy (root-level):

- `/src`
  - `/ApiGateway`
  - `/SharedKernel`
  - `/Identity.Service`
  - `/Catalog.Service`
  - `/Ordering.Service`
  - `/Notification.Service`
  - `/Ratings.Service`
  - `/Infrastructure.Common`
- `/tests`
  - `/Identity.Service.Tests`
  - `/Catalog.Service.Tests`
  - `/Ordering.Service.Tests`
  - `/Notification.Service.Tests`
  - `/Ratings.Service.Tests`

Each service project should contain:

- `Controllers/`
- `Grpc/` or `Protos/`
- `Domain/` and `Entities/`
- `Application/` (DTOs, commands, queries, handlers)
- `Infrastructure/` (EF Core, messaging, persistence)
- `Configurations/`
- `HealthChecks/`
- `Program.cs`

Each service is autonomous with its own database and schema. One SQL Server instance may host multiple service-specific databases during development.

### Phase 2: Shared Kernel

`SharedKernel` contains only shared artifacts:

- DTOs for cross-service contracts and API boundary models.
- Exception types and base error handling contracts.
- Domain events and integration event definitions.
- gRPC message contracts as shared Protobuf definitions or generated C# classes.
- Service bus message contracts and event enums.
- Common value objects such as `Money`, `EntityId`, and `PagedResult<T>`.
- OpenTelemetry instrumentation extensions and logging enrichment helpers.

Example shared elements:

- `UserCreatedEvent`
- `OrderPlacedEvent`
- `ShowUpdatedEvent`
- `SeatReservedEvent`
- `RatingSubmittedEvent`
- `ApiErrorException`
- `ValidationException`
- `IntegrationEventBase`

Keep `SharedKernel` lean. Do not add service-specific business rules or data access code.

### Phase 3: Implementation Details

Each service uses the same modern .NET 9 bootstrap pattern:

`Program.cs` core setup:

- `var builder = WebApplication.CreateBuilder(args);`
- `builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));`
- `builder.Services.AddControllers();`
- `builder.Services.AddHealthChecks();`
- `builder.Services.AddEndpointsApiExplorer();`
- `builder.Services.AddSwaggerGen();`
- `builder.Services.AddOpenTelemetryTracing(...)` with `AddAspNetCoreInstrumentation`, `AddHttpClientInstrumentation`, `AddGrpcClientInstrumentation`, `AddSqlClientInstrumentation`, and `AddJaegerExporter`/`AddOtlpExporter`.
- `builder.Services.AddDbContext<...>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
- `builder.Services.AddGrpc();`
- `builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(...);`
- `builder.Services.AddAuthorization();`
- `builder.Services.AddMassTransit(x => { ... });` or `AddSingleton<IBusControl>(...)` if MassTransit is not used.
- `builder.Services.AddStackExchangeRedisCache(...)` for distributed caching when needed.

Service-specific concerns:

- Identity.Service registers `IdentityDbContext`, ASP.NET Core Identity, JWT token generation, password and refresh token workflows.
- Catalog.Service exposes gRPC endpoints for `ShowQuery`, `ProviderQuery`, and `SectionQuery`, plus REST controllers for public catalog endpoints.
- Ordering.Service uses EF Core transactions and seat lock/checkout semantics. It listens to `SeatLocked`, `ShowUpdated`, and `UserCreated` events when needed.
- Notification.Service subscribes to events like `OrderPlacedEvent`, `PasswordResetRequestedEvent`, and `RatingSubmittedEvent` for email workflows.
- Ratings.Service owns rating persistence and publishes rating aggregate updates to the event bus.

gRPC configuration:

- Expose gRPC services internally on ports like `5001` + service offset.
- Add `builder.Services.AddGrpcReflection();` in development for diagnostics.
- Use Protobuf contracts from `SharedKernel/Protos` or shared generated classes.

MassTransit / RabbitMQ configuration:

- Use `builder.Services.AddMassTransit(x => { x.SetKebabCaseEndpointNameFormatter(); x.UsingRabbitMq((context, cfg) => { cfg.Host(...); cfg.ConfigureEndpoints(context); }); });`
- Register consumers for integration events.
- Publish domain events from service application flows.
- Use durable exchanges and queues with retry policies.

Health checks:

- `builder.Services.AddHealthChecks().AddSqlServer(..., name: "sql", tags: new[] { "db" }).AddRabbitMQ(..., name: "rabbitmq", tags: new[] { "broker" }).AddRedis(..., name: "redis", tags: new[] { "cache" });`
- Expose `/health/ready` and `/health/live`.
- API Gateway also exposes aggregated health status for all downstream services.

Key configuration files:

- `appsettings.json`
  - ConnectionStrings
  - Jwt
  - RabbitMQ
  - Redis
  - Serilog
  - OpenTelemetry
  - MassTransit
- `docker-compose.override.yml` for local overrides.

### Phase 4: Communication Map

Service interactions:

1. External client calls API Gateway.
   - Gateway routes external requests to service REST endpoints.
   - JWT tokens are validated at the gateway or forwarded to downstream services.

2. Synchronous internal communication via gRPC.
   - Ordering.Service calls Catalog.Service gRPC to validate seat and show availability.
   - Notification.Service may call Identity.Service gRPC to resolve user profile details for email templates.
   - Ratings.Service calls Catalog.Service gRPC when computing rating summaries tied to show metadata.

3. Asynchronous event-driven integration via RabbitMQ / MassTransit.
   - `Identity.Service` publishes `UserCreatedEvent`.
   - `Catalog.Service` publishes `ShowUpdatedEvent` and `SeatReservedEvent`.
   - `Ordering.Service` publishes `OrderPlacedEvent`, `OrderCancelledEvent`, and `PaymentFailedEvent`.
   - `Ratings.Service` publishes `RatingSubmittedEvent`.
   - `Notification.Service` consumes `OrderPlacedEvent`, `PasswordResetRequestedEvent`, and `RatingSubmittedEvent`.
   - `Catalog.Service` and `Ordering.Service` subscribe to relevant integration events for denormalized read models and eventual consistency.

Service dependency direction:

- API Gateway → all service APIs.
- Ordering.Service → Catalog.Service (gRPC), Identity.Service (gRPC for user lookup), Notification.Service (event publishing).
- Notification.Service → consumes events; no direct REST/gRPC dependency for core processing.
- Ratings.Service → Catalog.Service (optional gRPC), Identity.Service (optional gRPC) and publishes events.

Event flow example:

- User places an order via `ApiGateway` → `Ordering.Service`.
- `Ordering.Service` validates seats with `Catalog.Service` via gRPC.
- On success, `Ordering.Service` persists the order and publishes `OrderPlacedEvent`.
- `Notification.Service` consumes `OrderPlacedEvent` and sends confirmation email.
- `Catalog.Service` consumes `OrderPlacedEvent` if it maintains seat availability caches or denormalized counts.

### DevOps & Containerization

Docker Compose should include:

- `sqlserver` — SQL Server container for development.
- `rabbitmq` — RabbitMQ broker.
- `redis` — Redis cache.
- `api-gateway` — YARP gateway.
- `identity.service` — Identity.Service container.
- `catalog.service` — Catalog.Service container.
- `ordering.service` — Ordering.Service container.
- `notification.service` — Notification.Service container.
- `ratings.service` — Ratings.Service container.

Use separate database names for each service:

- `IdentityDb`
- `CatalogDb`
- `OrderingDb`
- `NotificationDb`
- `RatingsDb`

Docker Compose networking:

- All services join a shared application network.
- RabbitMQ and Redis are accessible by service containers by their container DNS names.
- Add `depends_on` for service startup ordering, but prefer retry loops in the services for resilience.

High-level container strategy:

- Use `replicas` in Kubernetes or Docker Swarm for production-scaled services.
- Keep services stateless; store state in service-owned databases and Redis where needed.
- Use health probes for readiness and liveness in the container orchestrator.

### Design Principles

- Autonomy: each microservice owns its own data and deployment lifecycle.
- Scalability: avoid shared monolithic database access and use independent read/write models.
- Availability: rely on resilient message transport, health checks, and retry policies.
- Clean code: apply SOLID, keep controllers thin, and use domain-oriented application layers.
- Observability: instrument every service with Serilog and OpenTelemetry.
- Versioning: use API versioning in controllers and stable gRPC contract versions.

### Notes for next implementation steps

- Start by creating the `SharedKernel` and the `ApiGateway` projects.
- Build `Identity.Service` first to establish authentication and token flows.
- Define Protobuf contracts in `SharedKernel/Protos` and share them across services.
- Wire MassTransit and RabbitMQ in a central `Infrastructure.Common` extension for consistency.
- Keep integration event contracts immutable once published.
- Validate the design with a simple end-to-end scenario: user signup, show search, order creation, notification delivery.
