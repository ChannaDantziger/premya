# Setup

## Prerequisites

- .NET SDK 10 or compatible SDK.
- Node.js and Angular CLI will be required when the client application is added.

## Backend

From the repository root:

```powershell
dotnet restore src/Premya.Api/Premya.Api.csproj
dotnet run --project src/Premya.Api/Premya.Api.csproj
```

The SQLite database will be configured as a local file by the backend.

There are currently no environment variables or external services required.
