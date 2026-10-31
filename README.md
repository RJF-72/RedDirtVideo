# Red Dirt Video

Red Dirt Video™ is an ASP.NET Core + PostgreSQL starter project for a Red Dirt and Texas-country AI music video SaaS platform.

## Goals
- Artist profiles and project management
- Music/video generation workflows
- Red Dirt Venue™ concept support
- PostgreSQL persistence with EF Core
- JWT-ready API foundation

## Solution structure
- `src/RedDirtVideo.API` — ASP.NET Core API
- `src/RedDirtVideo.Domain` — domain entities
- `src/RedDirtVideo.Infrastructure` — EF Core DbContext and data access

## Run locally

```bash
dotnet restore
dotnet build
dotnet run --project src/RedDirtVideo.API
```

## Database

Update the connection string in `src/RedDirtVideo.API/appsettings.Development.json` and run:

```bash
dotnet ef database update --project src/RedDirtVideo.Infrastructure --startup-project src/RedDirtVideo.API
```

## Notes
This repository is intentionally a production-minded starter foundation for the MVP described in the Red Dirt Video specification.
