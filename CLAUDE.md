This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

Project Overview
Stadium Drink Ordering System - A microservices-based .NET 8.0 application for stadium beverage ordering with three main components: API backend, Customer Blazor Server app, and Admin Blazor Server app. Uses SQLite for development, SQL Server for production, with JWT authentication and SignalR for real-time updates.

Development Commands
Docker Development (Recommended)
Start development environment: ./scripts/start-dev.sh (Linux/macOS) or run docker-compose up --build -d

Stop services: docker-compose down

View logs: docker-compose logs -f [service-name]

Restart specific service: docker-compose restart [service-name]

Local .NET Development
Build solution: dotnet build StadiumDrinkOrdering.sln

Run API: cd StadiumDrinkOrdering.API && dotnet run --urls "https://localhost:7000;http://localhost:7001"

Run Customer App: cd StadiumDrinkOrdering.Customer && dotnet run --urls "https://localhost:7002;http://localhost:7003"

Run Admin App: cd StadiumDrinkOrdering.Admin && dotnet run --urls "https://localhost:7004;http://localhost:7005"

Database Management
Add migration: dotnet ef migrations add <MigrationName> -p StadiumDrinkOrdering.API

Update database: dotnet ef database update -p StadiumDrinkOrdering.API

Generate migration script: dotnet ef migrations script -p StadiumDrinkOrdering.API

Testing
Run all tests: dotnet test

Run Playwright tests: npx playwright test

Run specific test project: dotnet test StadiumDrinkOrdering.Tests

Playwright Multi-Context Testing Guidance
When running end-to-end tests with Playwright that require simulating multiple, independent users (e.g., an Admin and a Customer), use separate BrowserContext objects. This ensures that session data, cookies, and local storage are not shared between users.

Example Scenario: Admin Creates Product, Customer Buys It
Launch the Browser: Start a new Chromium browser instance.

Create Contexts: Create two independent BrowserContext objects. Use one for the admin and one for the customer to prevent state overlap.

Admin Login:

Navigate to the API's authentication endpoint or the Admin app's login page.

Log in using the default admin credentials.

Verify a successful login.

Create a New Product:

Navigate to the product management page in the Admin app.

Add a new product with all required details.

Confirm the product was created successfully.

Customer Login & Purchase:

Using the separate BrowserContext for the customer, navigate to the Customer app's login page.

Log in as a regular customer.

Search for or navigate to the product created by the admin.

Add the product to the cart and complete the purchase.

Verify the order is successfully placed.

Teardown: Close both contexts and the browser instance.

Architecture
Solution Structure
StadiumDrinkOrdering.API: ASP.NET Core Web API with JWT auth, Entity Framework, SignalR

StadiumDrinkOrdering.Customer: Blazor Server app for customers

StadiumDrinkOrdering.Admin: Blazor Server app for staff/admin

StadiumDrinkOrdering.Shared: Shared models and DTOs

Key Technologies
.NET 8.0 with ASP.NET Core and Blazor Server

Entity Framework Core with SQLite (dev) / SQL Server (prod)

JWT Bearer Authentication with BCrypt password hashing

SignalR for real-time order updates via BartenderHub

Docker containerization with docker-compose orchestration

Database Context
Primary: ApplicationDbContext in StadiumDrinkOrdering.API/Data/

Development: SQLite database (StadiumDrinkOrdering.db)

Production: SQL Server with connection string from environment

Authentication Flow
JWT tokens issued by API /api/auth/login endpoint

Role-based authorization (Admin, Staff, Customer roles)

Default admin: admin@stadium.com / admin123

Service Ports
API: Development 7000/7001, Docker 9000

Customer: Development 7002/7003, Docker 9001

Admin: Development 7004/7005, Docker 9002

SQL Server: Docker 14330:1433

Configuration
Development
API uses SQLite with connection string: Data Source=StadiumDrinkOrdering.db

Docker/Production
Environment variables in docker-compose.yml:

ConnectionStrings__DefaultConnection: SQL Server connection

JwtSettings__SecretKey: JWT signing key (min 32 chars)

ASPNETCORE_ENVIRONMENT: Development/Production

Frontend Configuration
Customer and Admin apps connect to API via ApiSettings__BaseUrl setting

Key Implementation Notes
Entity Models
Located in StadiumDrinkOrdering.Shared/Models/: User, Order, OrderItem, Drink, Ticket, Payment, StadiumSeat

API Controllers
AuthController: Login/register endpoints

OrdersController: CRUD operations for orders

DrinksController: Drink catalog management

TicketsController: Ticket validation

Real-time Updates
SignalR BartenderHub at /bartenderHub endpoint provides real-time order status updates

Order Workflow
Pending → Accepted → In Preparation → Ready → Out for Delivery → Delivered

Sample Data
Database automatically seeded with drinks (Beer, Soft Drinks, Water, Coffee) and test tickets (TICKET001-TICKET005)