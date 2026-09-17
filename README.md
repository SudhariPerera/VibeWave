# 🎵 VibeWave
<hr>

VibeWave is a web-based concert booking and event management system developed using ASP.NET Core MVC. The system allows users to discover concerts, book tickets, make payments, receive QR-code booking confirmations, and interact with an AI-powered chatbot.

Administrators can manage concerts, users, bookings, categories, and other application data through the administration area.

## 🛠️ Technology Stack

The following technologies and tools are used in this project.

| Technology / Tool             | Version                | Purpose                          |
| ----------------------------- | ---------------------- | -------------------------------- |
| **C#**                        | 12                     | Main programming language        |
| **.NET**                      | 8.0                    | Application framework/runtime    |
| **ASP.NET Core MVC**          | 8.0                    | Web application framework        |
| **Entity Framework Core**     | 8.0.x                  | ORM and database access          |
| **Microsoft SQL Server**      | 2022                   | Relational database              |
| **ASP.NET Core Identity**     | 8.0.x                  | Authentication and authorization |
| **Razor Pages / Razor Views** | 8.0                    | User interface                   |
| **Bootstrap**                 | Project version        | Front-end UI styling             |
| **JavaScript**                | ES6+                   | Client-side functionality        |
| **QRCoder**                   | Project NuGet version  | QR-code generation               |
| **Stripe**                    | Project NuGet version  | Online payment processing        |
| **Google Gemini API**         | Gemini 2.5 Flash       | AI chatbot                       |
| **Docker**                    | Current stable version | Local SQL Server environment     |
| **Git**                       | Current stable version | Version control                  |
| **GitHub**                    | —                      | Source code repository           |

> **Important:** The application targets **.NET 8 (`net8.0`)**. A newer .NET SDK may be installed on the development machine, but the project itself should remain on the target framework specified in the `.csproj` file.

---

# Notes
## Entity Framework Core 8

The application uses Entity Framework Core for:

* Database access
* LINQ queries
* Entity mapping
* Database migrations
* Database updates

The project uses the **8.0.x** EF Core package family.

Check the project file: VibeWave/VibeWave.csproj

EF Core is distributed through NuGet packages and uses a database provider such as SQL Server.

---

## Microsoft SQL Server 2022

VibeWave uses: Microsoft SQL Server 2022

### Windows

The original project used SQL Server LocalDB.

Example: (localdb)\MSSQLLocalDB

### macOS / Linux

SQL Server LocalDB is not supported on macOS/Linux.

For macOS development, SQL Server 2022 can be run using Docker.

---

# 🗄️ SQL Server Docker Setup

Create the SQL Server container:

```bash
docker run \
  -e 'ACCEPT_EULA=Y' \
  -e 'MSSQL_SA_PASSWORD=YOUR_STRONG_PASSWORD' \
  -p 1433:1433 \
  --name vibewave-sql \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

Check that SQL Server is running:

```bash
docker ps
```

Check SQL Server logs:

```bash
docker logs vibewave-sql
```

Wait until the logs indicate that SQL Server is ready for client connections.

---

# 🔗 Database Connection

For macOS/Linux development using Docker:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=VibeWave2026;User Id=sa;Password=YOUR_STRONG_PASSWORD;TrustServerCertificate=True",
  "ApplicationDbContextConnection": "Server=localhost,1433;Database=VibeWave;User Id=sa;Password=YOUR_STRONG_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

Replace:

```text
YOUR_STRONG_PASSWORD
```

with the password configured when creating the SQL Server Docker container.

---

# 📦 NuGet Packages

The project uses NuGet packages for additional functionality.

Important package groups include:

### Entity Framework Core

```text
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Sqlite
Microsoft.EntityFrameworkCore.Tools
```

### ASP.NET Core Identity

```text
Microsoft.AspNetCore.Identity
Microsoft.AspNetCore.Identity.EntityFrameworkCore
```

### QR Code

```text
QRCoder
```

### Payment

```text
Stripe
```

### Other project dependencies

The exact package versions are defined in:

```text
VibeWave/VibeWave.csproj
```

Therefore, the `.csproj` file should be treated as the **source of truth for the exact NuGet package versions**.

---

# 🤖 Google Gemini AI

The VibeWave chatbot uses:

```text
Google Gemini
Model: gemini-2.5-flash
```

The API key is configured using:

```json
"Gemini": {
  "ApiKey": "YOUR_GEMINI_API_KEY"
}
```

---

# 💳 Stripe

VibeWave uses Stripe for payment processing.

Required configuration:

```json
"Stripe": {
  "SecretKey": "YOUR_STRIPE_SECRET_KEY",
  "PublishableKey": "YOUR_STRIPE_PUBLISHABLE_KEY"
}
```

---

 Name          | Function | GitHub  |
------------- | -------- | -------- |
 Yusu Wang   | Admin  |  <a href='https://github.com/WYS0318' >WYS0318</a> |
 SudhariPerera   | Forum  |  <a href='https://github.com/SudhariPerera' >SudhariPerera</a> |







