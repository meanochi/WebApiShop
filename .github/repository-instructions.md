Repository developer instructions — ShowsCenter
==============================================

Purpose
-------
This file is a concise, actionable guide for contributors and automated agents working with this repository. It complements `.github/copilot-instructions.md` with repository-specific rules, conventions and helpful commands.

Quick summary
-------------
- API: ASP.NET Core Web API (.NET 9)
- Layers: `WebApiShop` (API), `Services`, `Repositories`, `Entities` (EF Core), `DTOs`, `Tests` (xUnit + Sqlite in-memory)
- Use AutoMapper for mappings, JWT for auth, optional Redis for caching, NLog for logging.

Local dev setup (minimal)
-------------------------
1. Install .NET 9 SDK.
2. (Optional) Install EF CLI: `dotnet tool install --global dotnet-ef`.
3. Restore + build: `dotnet restore` && `dotnet build` (root)
4. Configure runtime keys in `WebApiShop/appsettings.Development.json`:
   - `ConnectionStrings:ShowsCenter` (SQL Server) — for local DB
   - `Jwt:Issuer`, `Jwt:Audience`, `Jwt:Key`
   - `Redis:ConnectionString` (optional)
   - `Email` section to test email-sending flows (or leave empty in dev)
5. Run API for manual testing: `cd WebApiShop` && `dotnet run`.

Testing
-------
- Run all tests: `dotnet test` (project uses in-memory Sqlite; no external DB required).
- Tests seed data explicitly; when changing entities, update tests to match required fields.

EF Core / Migrations
--------------------
- Add migration (from repo root):
  `dotnet ef migrations add <Name> --project Entities --startup-project WebApiShop`
- Apply migration:
  `dotnet ef database update --project Entities --startup-project WebApiShop`
- Always pass `--startup-project WebApiShop` so Program.cs and appsettings are used.

Configuration keys of interest
------------------------------
- `ConnectionStrings:ShowsCenter` or `DefaultConnection`
- `Jwt:Issuer`, `Jwt:Audience`, `Jwt:Key`
- `Redis:ConnectionString`, `Redis:TTLMinutes`
- `Email` (Smtp host, port, username, password, FromAddress)

Common pitfalls & required checks
--------------------------------
- Do not register `ShowsCenterContext` twice in DI. Verify `Program.cs` after changes.
- Preserve UTF-8 encoding (files contain Hebrew text).
- When changing public controller routes or request/response DTOs, add/update integration tests.
- When adding services, register them as `Scoped` in `Program.cs` and add interface types (`I*Service`).

Coding & PR guidelines
----------------------
- Keep controllers thin; place business logic in `Services`.
- Use `async/await` everywhere IO-bound.
- Add `record` DTOs for request/response models; map via AutoMapper profiles.
- Unit test services; integration test repositories.
- Branches: `feature/<short>`, `fix/<short>`; open PRs against `master`.
- PR content: description, testing steps, migration notes (if any).

Small checklist before pushing
-----------------------------
- `dotnet build` succeeds (0 errors)
- `dotnet test` passes
- If EF model changed: add migration and update `Entities/Migrations`
- Update `README.md` or this file for any new configuration keys or runtime behavior

Where to add new code (short)
-----------------------------
- New entity: `Entities/` + register DbSet in `ShowsCenterContext`.
- DTOs: `DTOs/`.
- Repository: `Repositories/` (add interface in same project)
- Service: `Services/` (add interface and implementation)
- Controller: `WebApiShop/Controllers/` (use `[Route("api/[controller]")]`)

Contact / maintainers
---------------------
- Main repo: https://github.com/meanochi/WebApiShop
- Open issues / PRs in GitHub for discussion.

Keep this file concise and update it whenever you add infrastructure, DI registrations, or change runtime configuration.
