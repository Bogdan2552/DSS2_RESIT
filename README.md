
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
