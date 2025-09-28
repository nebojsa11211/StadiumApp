# GEMINI.md

## Project Overview

This is a comprehensive stadium drink ordering and ticketing system. It's a microservices-based application built with .NET 8.0, Blazor, and Docker. The system consists of four main applications:

*   **API Backend:** A RESTful API built with ASP.NET Core 8.0 that handles business logic, authentication, and data access.
*   **Customer App:** A Blazor Server application for customers to purchase tickets and order drinks.
*   **Admin App:** A Blazor Server application for administrators to manage events, inventory, and view analytics.
*   **Staff App:** A Blazor Server application for staff to manage orders.

The system uses a PostgreSQL database hosted on Supabase and is fully containerized with Docker.

## Building and Running

### Docker (Recommended)

The recommended way to run the project is using Docker Compose.

**Prerequisites:**

*   Docker & Docker Compose
*   Git

**Commands:**

```bash
# Clone the repository
git clone <repository-url>
cd StadiumDrinkOrdering

# Make scripts executable (Linux/macOS)
chmod +x scripts/*.sh

# Start development environment
./scripts/start-dev.sh
```

**Access URLs:**

*   **Customer App:** https://localhost:9020
*   **Admin App:** https://localhost:9030
*   **Staff App:** https://localhost:9040
*   **API:** https://localhost:9010
*   **API Documentation:** https://localhost:9010/swagger

### Local Development

**Prerequisites:**

*   .NET 8.0 SDK
*   Visual Studio 2022 (Recommended for Windows) or a code editor of your choice.

**Option 1: Visual Studio (Recommended for Windows)**

1.  Open `StadiumDrinkOrdering.sln` in Visual Studio 2022.
2.  Set `StadiumDrinkOrdering.AppHost` as the startup project.
3.  Press F5 to run with Aspire orchestration.

**Option 2: Command Line**

```bash
# Start API
cd StadiumDrinkOrdering.API
dotnet run --urls "https://localhost:7010"

# Start Customer App (new terminal)
cd StadiumDrinkOrdering.Customer
dotnet run --urls "https://localhost:7020"

# Start Admin App (new terminal)
cd StadiumDrinkOrdering.Admin
dotnet run --urls "https://localhost:7030"

# Start Staff App (new terminal)
cd StadiumDrinkOrdering.Staff
dotnet run --urls "https://localhost:7040"
```

## Testing

The project has both .NET tests and Playwright tests for end-to-end testing of the frontend applications.

### .NET Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test StadiumDrinkOrdering.Tests
```

### Playwright Tests

The Playwright tests are located in the `tests` directory and are configured in `playwright.config.ts`.

```bash
# Install Playwright dependencies
npm install

# Run all Playwright tests
npm test

# Run Playwright tests in headed mode
npm run test:headed
```

## Development Conventions

*   **Configuration:** The project heavily relies on environment variables for configuration, especially for sensitive data. A `.env.docker.example` file is provided as a template.
*   **Database:** The project uses Entity Framework Core for database migrations. Migrations are applied automatically on startup.
*   **Authentication:** Authentication is handled using JWT.
*   **Authorization:** The project uses a policy-based authorization system.
*   **Coding Style:** The C# code follows the standard Microsoft C# coding conventions.
*   **Frontend:** The frontend applications are built with Blazor Server.
*   **Testing:** The project has a solid testing setup with both unit/integration tests for the backend and end-to-end tests for the frontend.
