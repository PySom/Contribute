# Contribute — IMT 2016 Crowdfund

Simple full-stack project with:
- ASP.NET Core Web API backend (EF Core + MariaDB)
- React + Vite frontend (contrib)

This README explains how to run both parts locally and common troubleshooting steps.

---

## Prerequisites

- .NET SDK 7+ (or the version this project targets)
- MariaDB (or MySQL-compatible) server
- Node.js 18+ and npm (or pnpm/yarn)
- (Optional) dotnet-ef tool for migrations:
  dotnet tool install --global dotnet-ef

---

## Backend — ASP.NET Core

Paths: project root (this repository), main API project (Program.cs)

1. Configure connection string

Open `appsettings.Development.json` (or `appsettings.json`) and set your DB connection e.g.:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=127.0.0.1;Port=3306;Database=contribute_db;User=root;Password=yourpassword;"
  }
}

If using MariaDB, ensure Program.cs config uses matching ServerVersion (example in project: `10.11.13-mariadb`).

2. Configure secret settings (if used)

If code reads a `Secret` config key, add it to `appsettings.*.json` or set an environment variable:
- Windows (PowerShell):
  $Env:Secret = "supersecretvalue"

3. Apply EF migrations (if using EF Core)

From the API project directory:

- Create migration (if none exist)
  dotnet ef migrations add InitialCreate

- Apply migrations to DB
  dotnet ef database update

4. Run (development)

- Run via dotnet:
  dotnet run

By default Kestrel exposes http (5000) and https (5001). Check console output for exact URLs.

5. CORS

The backend contains a named CORS policy. To allow the local frontend (Vite) to call the API, configure the policy to include the frontend origin and credentials if needed:

Example in Program.cs when registering CORS:
options.AddPolicy(name: "AllowAllOrigins", policy =>
{
  policy.WithOrigins("http://localhost:5173") // Vite default
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials(); // only if you send cookies/credentials
});

Ensure `app.UseCors("AllowAllOrigins")` is called before mapping endpoints.

6. Swagger

In Development the app exposes Swagger UI. Visit the URL printed by dotnet run (usually https://localhost:5001/swagger).

---

## Frontend — React + Vite

Folder: `frontend/contrib` (or `c:\AssemblyHQ\Contribute\frontend\contrib`)

1. Install

Open terminal at the frontend folder and run:

npm install

2. Environment

Vite requires env vars to be prefixed with `VITE_`. Create `.env.local` in the frontend folder:

VITE_API_URL=https://localhost:5001/api
# adjust protocol/port to your backend

3. Run dev server

npm run dev

Default Vite port is 5173. The app will open at http://localhost:5173.

4. Build / Preview

- Build:
  npm run build

- Preview the production build:
  npm run preview

5. Notes

- If you keep the backend on https and the frontend on http, browsers will block mixed active content (http -> https is okay but not https -> http). Prefer matching protocols, or access the frontend via http while backend is https; CORS and certificate issues may arise.
- Use VITE_API_URL in your client API helper when calling the backend.

---

## Key Routes

- Home: `/`
- Contributions list / recipient: `/contributions/:slug`

The Contributions page expects to receive `slug` route param and fetch recipient data accordingly.

---

## Accessibility & Styling

- Theme variables and CSS live under `src` (index.css / App.css / components/*).
- Form validations and aria attributes are present on inputs and error messages.

---

## Troubleshooting

- CORS errors
  - Confirm backend `UseCors` is enabled and includes your frontend origin.
  - If sending credentials (cookies/auth), do NOT use `AllowAnyOrigin()` with `AllowCredentials()`; specify a concrete origin.
  - Check browser console network tab for the failing OPTIONS preflight and server response.

- Mixed content / HTTPS
  - If backend runs HTTPS and frontend HTTP, cross-origin certificates or mixed content rules may block requests. Use consistent protocols or accept the dev certificate.

- Double network calls in dev
  - React 18 StrictMode mounts/unmounts components in development to find side-effects — your effect may run twice. Make effects idempotent or memoize functions used in dependency arrays.

- EF / Database
  - Migration errors: check `ServerVersion.Parse(...)` for correct MariaDB version string.
  - Connection errors: verify DB is running and credentials are correct.

- Ports
  - If ports conflict, set explicit ports:
    - dotnet: use `--urls` or `launchSettings.json`
    - Vite: set `PORT` or use `vite.config.ts` dev server port

---

## Useful commands (Windows)

Backend (API folder)
- dotnet run
- dotnet ef migrations add MyMigration
- dotnet ef database update

Frontend (frontend/contrib)
- npm install
- npm run dev
- npm run build
- npm run preview

---

## Contributing

1. Create a feature branch.
2. Add tests where appropriate.
3. Open a PR with a description of changes.

---

If you want, I can:
- Add a sample `.env.local` file to the repo
- Move the form into the Contributions page and wire the route param through
- Add a small Postman collection / curl examples for the API
