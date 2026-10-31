# Red Dirt Video

Red Dirt Video™ is a production-minded C# ASP.NET Core starter for a Red Dirt, Texas Country, and Americana AI music video SaaS.

## What is included
- ASP.NET Core 9 API
- PostgreSQL configuration with EF Core
- JWT authentication foundation
- Artist, Song, Project, Venue, Job, Payment, and Billing models
- Red Dirt Venue™ seed data
- API endpoints for auth, artists, songs, projects, venues, jobs, billing, and full-project generation

## Solution layout
- `src/RedDirtVideo.API` — API layer
- `src/RedDirtVideo.Domain` — domain entities
- `src/RedDirtVideo.Infrastructure` — EF Core data access

## Run locally

```bash
dotnet restore
dotnet build
dotnet run --project src/RedDirtVideo.API
```

## Database

Set your PostgreSQL connection string in:
- `src/RedDirtVideo.API/appsettings.Development.json`

Then run:

```bash
dotnet ef database update --project src/RedDirtVideo.Infrastructure --startup-project src/RedDirtVideo.API
```

## API entry points

- `POST /api/v1/auth/register`
- `POST /api/v1/auth/login`
- `GET /api/v1/auth/me`
- `GET /api/v1/artists`
- `POST /api/v1/artists`
- `GET /api/v1/songs`
- `POST /api/v1/songs`
- `POST /api/v1/projects`
- `GET /api/v1/venues`
- `POST /api/v1/generate/full-project`
- `GET /api/v1/jobs`
- `GET /api/v1/billing/usage`
- `GET /api/v1/health`

## Product vision
This project is built for:
- artist profiles
- music video generation
- Red Dirt Venue™ concert creation
- lyric and social clip generation
- SaaS billing and usage tracking

This is a real starter foundation for the Red Dirt Video MVP and can be extended into digital twins, voice cloning, venue generation, and broader AI entertainment workflows.
