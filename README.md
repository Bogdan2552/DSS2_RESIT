
The backend runs on:
http://localhost:4058
The frontend uses this API by default:
VITE_API_URL=http://localhost:4058


## Demo account

On a fresh database, the backend seeds one demo account:

```text
Email: alice@example.com
Password: Password123!
```

You can also register new users from the frontend.

## Run with Docker

From the project root:

```bash
docker compose up --build
```

Open:

```text
Frontend: http://localhost:5173
Backend Swagger: http://localhost:4058/swagger
```

The Docker setup stores the SQLite database in the `backend-data` Docker volume.

To stop containers:

```bash
docker compose down
```

To remove the database volume as well:

```bash
docker compose down -v
```

## Run locally without Docker

### 1. Start the backend

Requirements:

- .NET 8 SDK

Commands:

```bash
cd backend/MovieWatchlist.Api
dotnet restore
dotnet run --urls http://localhost:4058
```

The database file `movie-watchlist.db` is created automatically on first run.

### 2. Start the frontend

Requirements:

- Node.js 20+

Commands:

```bash
cd frontend
npm install
VITE_API_URL=http://localhost:4058 npm run dev
```

Open:

http://localhost:5173
```

On Windows PowerShell, use:

powershell:
$env:VITE_API_URL="http://localhost:4058"; npm run dev
```

## Run Cypress tests

Start both backend and frontend first, then run:

```bash
cd frontend
npm run test:e2e
```

Interactive Cypress mode:

```bash
npm run cypress:open
```

The Cypress suite covers:

- public community movie catalog access without login
- guest restriction for protected watchlist route
- registration
- login
- authenticated watchlist create/edit/delete
- authenticated public community movie contribution
- non-owner authorization rejection for community movie edit/delete
- validation error display

## Main API endpoints

### Authentication

```text
POST /api/auth/register
POST /api/auth/login
GET  /api/auth/me
```

### Protected watchlist

```text
GET    /api/watchlist
GET    /api/watchlist/{id}
POST   /api/watchlist
PUT    /api/watchlist/{id}
DELETE /api/watchlist/{id}
```

### Public community movies

```text
GET    /api/community-movies
GET    /api/community-movies/{id}
POST   /api/community-movies
PUT    /api/community-movies/{id}
DELETE /api/community-movies/{id}
```

## Authentication flow

1. A user registers or logs in with email and password.
2. The backend verifies the credentials and returns a JWT plus a safe user DTO.
3. The frontend stores the token in `localStorage` as `movieWatchlistToken`.
4. Authenticated API requests send `Authorization: Bearer <token>`.
5. Protected endpoints read the current user id from the JWT claims.
6. Watchlist queries always filter by the current user id.
7. Community movie update/delete checks that the current user is the creator.

## Validation rules

The backend validates:

- required fields
- email format
- password length
- password confirmation match
- max string lengths
- valid watchlist status values
- rating range from 1 to 10
- release year from 1888 to current year + 2

ASP.NET Core returns readable validation errors that the frontend displays in form error alerts.

## Notes for submission

- Add screenshots or a short demo video after running the app. A screenshot checklist is in `docs/screenshots/README.md`.
- A development log is included in `docs/development-log.md`.
- The original assignment specification is copied into `docs/` for reference.
