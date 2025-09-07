-- Stadium Drink Ordering System - PostgreSQL Schema for Supabase
-- Generated from Entity Framework Migration for .NET 9
-- Run this script in your Supabase SQL Editor

-- Create migration history table
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" varchar(150) NOT NULL PRIMARY KEY,
    "ProductVersion" varchar(32) NOT NULL
);

-- Create main tables
CREATE TABLE "Users" (
    "Id" serial PRIMARY KEY,
    "Username" text NOT NULL,
    "Email" text NOT NULL,
    "PasswordHash" text NOT NULL,
    "Role" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "LastLoginAt" timestamp with time zone
);

CREATE TABLE "Drinks" (
    "Id" serial PRIMARY KEY,
    "Name" text NOT NULL,
    "Description" text,
    "Price" decimal(18,2) NOT NULL,
    "StockQuantity" integer NOT NULL,
    "ImageUrl" text,
    "Category" integer NOT NULL,
    "IsAvailable" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone
);

CREATE TABLE "Events" (
    "Id" serial PRIMARY KEY,
    "EventName" text NOT NULL,
    "EventType" text NOT NULL,
    "EventDate" timestamp with time zone NOT NULL,
    "VenueId" integer,
    "TotalSeats" integer NOT NULL,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    "Description" text,
    "ImageUrl" text,
    "BaseTicketPrice" decimal(18,2)
);

CREATE TABLE "StadiumSections" (
    "Id" serial PRIMARY KEY,
    "SectionCode" text NOT NULL,
    "SectionName" text NOT NULL,
    "TotalRows" integer NOT NULL,
    "SeatsPerRow" integer NOT NULL,
    "PriceMultiplier" decimal(18,2) NOT NULL,
    "IsActive" boolean NOT NULL,
    "Color" text NOT NULL
);

CREATE TABLE "StadiumSeats" (
    "Id" serial PRIMARY KEY,
    "Section" text NOT NULL,
    "RowNumber" integer NOT NULL,
    "SeatNumber" integer NOT NULL,
    "XCoordinate" integer NOT NULL,
    "YCoordinate" integer NOT NULL,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL
);

CREATE TABLE "Seats" (
    "Id" serial PRIMARY KEY,
    "SectionId" integer NOT NULL REFERENCES "StadiumSections"("Id") ON DELETE RESTRICT,
    "RowNumber" integer NOT NULL,
    "SeatNumber" integer NOT NULL,
    "SeatCode" text NOT NULL,
    "IsAccessible" boolean NOT NULL,
    "XCoordinate" integer NOT NULL,
    "YCoordinate" integer NOT NULL
);

CREATE TABLE "Orders" (
    "Id" serial PRIMARY KEY,
    "CustomerId" integer NOT NULL REFERENCES "Users"("Id") ON DELETE CASCADE,
    "EventId" integer REFERENCES "Events"("Id") ON DELETE SET NULL,
    "Status" integer NOT NULL,
    "TotalAmount" decimal(18,2) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    "DeliveryLocation" text,
    "CustomerNotes" text,
    "StaffNotes" text,
    "CompletedAt" timestamp with time zone,
    "OrderType" integer NOT NULL DEFAULT 0,
    "SessionId" text,
    "CustomerEmail" text,
    "CustomerPhone" text,
    "BillingAddress" text,
    "PaymentStatus" integer NOT NULL DEFAULT 0,
    "PaymentMethodId" text,
    "PaymentIntentId" text,
    "RefundId" text,
    "TicketSessionId" uuid
);

CREATE TABLE "OrderItems" (
    "Id" serial PRIMARY KEY,
    "OrderId" integer NOT NULL REFERENCES "Orders"("Id") ON DELETE CASCADE,
    "DrinkId" integer REFERENCES "Drinks"("Id") ON DELETE SET NULL,
    "Quantity" integer NOT NULL,
    "UnitPrice" decimal(18,2) NOT NULL,
    "TotalPrice" decimal(18,2) NOT NULL,
    "SpecialInstructions" text
);

CREATE TABLE "Tickets" (
    "Id" serial PRIMARY KEY,
    "UserId" integer REFERENCES "Users"("Id") ON DELETE SET NULL,
    "EventId" integer NOT NULL REFERENCES "Events"("Id") ON DELETE CASCADE,
    "SectionId" integer REFERENCES "StadiumSections"("Id") ON DELETE SET NULL,
    "SeatId" integer REFERENCES "Seats"("Id") ON DELETE SET NULL,
    "TicketCode" text NOT NULL,
    "Status" integer NOT NULL,
    "PurchasePrice" decimal(18,2) NOT NULL,
    "PurchasedAt" timestamp with time zone NOT NULL,
    "ValidatedAt" timestamp with time zone,
    "ValidatedBy" integer REFERENCES "Users"("Id") ON DELETE SET NULL,
    "QRCode" text,
    "CustomerEmail" text,
    "CustomerName" text,
    "CustomerPhone" text,
    "OrderId" integer REFERENCES "Orders"("Id") ON DELETE SET NULL
);

CREATE TABLE "Payments" (
    "Id" serial PRIMARY KEY,
    "OrderId" integer NOT NULL REFERENCES "Orders"("Id") ON DELETE CASCADE,
    "Amount" decimal(18,2) NOT NULL,
    "PaymentMethod" text NOT NULL,
    "PaymentStatus" integer NOT NULL,
    "TransactionId" text,
    "PaymentIntentId" text,
    "CreatedAt" timestamp with time zone NOT NULL,
    "ProcessedAt" timestamp with time zone,
    "FailureReason" text
);

CREATE TABLE "LogEntries" (
    "Id" serial PRIMARY KEY,
    "Source" text NOT NULL,
    "Action" text NOT NULL,
    "Category" text NOT NULL,
    "UserId" integer REFERENCES "Users"("Id") ON DELETE SET NULL,
    "UserEmail" text,
    "IpAddress" text,
    "UserAgent" text,
    "RequestPath" text,
    "Details" text,
    "Level" text NOT NULL,
    "Timestamp" timestamp with time zone NOT NULL,
    "Exception" text,
    "StackTrace" text
);

CREATE TABLE "ShoppingCarts" (
    "Id" serial PRIMARY KEY,
    "SessionId" text NOT NULL,
    "UserId" integer REFERENCES "Users"("Id") ON DELETE SET NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    "ExpiresAt" timestamp with time zone NOT NULL
);

CREATE TABLE "CartItems" (
    "Id" serial PRIMARY KEY,
    "CartId" integer NOT NULL REFERENCES "ShoppingCarts"("Id") ON DELETE CASCADE,
    "EventId" integer NOT NULL REFERENCES "Events"("Id") ON DELETE CASCADE,
    "SectionId" integer NOT NULL REFERENCES "StadiumSections"("Id") ON DELETE CASCADE,
    "SeatId" integer REFERENCES "Seats"("Id") ON DELETE SET NULL,
    "Row" integer NOT NULL,
    "SeatNumber" integer NOT NULL,
    "Price" decimal(18,2) NOT NULL,
    "AddedAt" timestamp with time zone NOT NULL
);

CREATE TABLE "SeatReservations" (
    "Id" serial PRIMARY KEY,
    "EventId" integer NOT NULL REFERENCES "Events"("Id") ON DELETE CASCADE,
    "SectionId" integer NOT NULL REFERENCES "StadiumSections"("Id") ON DELETE CASCADE,
    "SeatId" integer REFERENCES "Seats"("Id") ON DELETE SET NULL,
    "Row" integer NOT NULL,
    "SeatNumber" integer NOT NULL,
    "SessionId" text NOT NULL,
    "UserId" integer REFERENCES "Users"("Id") ON DELETE SET NULL,
    "ReservedAt" timestamp with time zone NOT NULL,
    "ExpiresAt" timestamp with time zone NOT NULL,
    "IsConfirmed" boolean NOT NULL DEFAULT false
);

CREATE TABLE "TicketSessions" (
    "Id" uuid PRIMARY KEY,
    "SessionId" text NOT NULL,
    "EventId" integer NOT NULL REFERENCES "Events"("Id") ON DELETE CASCADE,
    "CustomerEmail" text,
    "CustomerName" text,
    "CustomerPhone" text,
    "CreatedAt" timestamp with time zone NOT NULL,
    "ExpiresAt" timestamp with time zone NOT NULL,
    "IsCompleted" boolean NOT NULL DEFAULT false
);

CREATE TABLE "OrderSessions" (
    "Id" serial PRIMARY KEY,
    "SessionId" text NOT NULL,
    "EventId" integer REFERENCES "Events"("Id") ON DELETE SET NULL,
    "CustomerEmail" text,
    "CustomerName" text,
    "CustomerPhone" text,
    "TotalAmount" decimal(18,2),
    "Status" text NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "ExpiresAt" timestamp with time zone NOT NULL,
    "CompletedAt" timestamp with time zone
);

CREATE TABLE "EventAnalytics" (
    "Id" serial PRIMARY KEY,
    "EventId" integer NOT NULL REFERENCES "Events"("Id") ON DELETE CASCADE,
    "TotalTicketsSold" integer NOT NULL,
    "TotalRevenue" decimal(18,2) NOT NULL,
    "TotalOrders" integer NOT NULL,
    "TotalDrinksSold" integer NOT NULL,
    "AverageOrderValue" decimal(18,2) NOT NULL,
    "PeakOrderTime" timestamp with time zone,
    "MostPopularDrink" text,
    "CalculatedAt" timestamp with time zone NOT NULL
);

CREATE TABLE "EventStaffAssignments" (
    "Id" serial PRIMARY KEY,
    "EventId" integer NOT NULL REFERENCES "Events"("Id") ON DELETE CASCADE,
    "StaffId" integer NOT NULL REFERENCES "Users"("Id") ON DELETE CASCADE,
    "AssignedSections" text,
    "Role" text NOT NULL,
    "ShiftStart" timestamp with time zone,
    "ShiftEnd" timestamp with time zone,
    "IsActive" boolean NOT NULL DEFAULT true
);

CREATE TABLE "Tribunes" (
    "Id" serial PRIMARY KEY,
    "TribuneCode" text NOT NULL,
    "Name" text NOT NULL,
    "Description" text
);

-- Create indexes for better performance
CREATE INDEX "IX_Orders_CustomerId" ON "Orders"("CustomerId");
CREATE INDEX "IX_Orders_EventId" ON "Orders"("EventId");
CREATE INDEX "IX_Orders_TicketSessionId" ON "Orders"("TicketSessionId");
CREATE INDEX "IX_OrderItems_OrderId" ON "OrderItems"("OrderId");
CREATE INDEX "IX_OrderItems_DrinkId" ON "OrderItems"("DrinkId");
CREATE INDEX "IX_Tickets_UserId" ON "Tickets"("UserId");
CREATE INDEX "IX_Tickets_EventId" ON "Tickets"("EventId");
CREATE INDEX "IX_Tickets_SectionId" ON "Tickets"("SectionId");
CREATE INDEX "IX_Tickets_SeatId" ON "Tickets"("SeatId");
CREATE INDEX "IX_Tickets_ValidatedBy" ON "Tickets"("ValidatedBy");
CREATE INDEX "IX_Tickets_OrderId" ON "Tickets"("OrderId");
CREATE INDEX "IX_Payments_OrderId" ON "Payments"("OrderId");
CREATE INDEX "IX_LogEntries_UserId" ON "LogEntries"("UserId");
CREATE INDEX "IX_ShoppingCarts_UserId" ON "ShoppingCarts"("UserId");
CREATE INDEX "IX_CartItems_CartId" ON "CartItems"("CartId");
CREATE INDEX "IX_CartItems_EventId" ON "CartItems"("EventId");
CREATE INDEX "IX_CartItems_SectionId" ON "CartItems"("SectionId");
CREATE INDEX "IX_CartItems_SeatId" ON "CartItems"("SeatId");
CREATE INDEX "IX_SeatReservations_EventId" ON "SeatReservations"("EventId");
CREATE INDEX "IX_SeatReservations_SectionId" ON "SeatReservations"("SectionId");
CREATE INDEX "IX_SeatReservations_SeatId" ON "SeatReservations"("SeatId");
CREATE INDEX "IX_SeatReservations_UserId" ON "SeatReservations"("UserId");
CREATE INDEX "IX_TicketSessions_EventId" ON "TicketSessions"("EventId");
CREATE INDEX "IX_OrderSessions_EventId" ON "OrderSessions"("EventId");
CREATE INDEX "IX_EventAnalytics_EventId" ON "EventAnalytics"("EventId");
CREATE INDEX "IX_EventStaffAssignments_EventId" ON "EventStaffAssignments"("EventId");
CREATE INDEX "IX_EventStaffAssignments_StaffId" ON "EventStaffAssignments"("StaffId");
CREATE INDEX "IX_Seats_SectionId" ON "Seats"("SectionId");

-- Create unique constraints
CREATE UNIQUE INDEX "IX_Users_Email" ON "Users"("Email");
CREATE UNIQUE INDEX "IX_Users_Username" ON "Users"("Username");
CREATE UNIQUE INDEX "IX_Tickets_TicketCode" ON "Tickets"("TicketCode");
CREATE UNIQUE INDEX "IX_StadiumSections_SectionCode" ON "StadiumSections"("SectionCode");
CREATE UNIQUE INDEX "IX_Seats_SeatCode" ON "Seats"("SeatCode");
CREATE UNIQUE INDEX "IX_ShoppingCarts_SessionId" ON "ShoppingCarts"("SessionId");
CREATE UNIQUE INDEX "IX_Tribunes_TribuneCode" ON "Tribunes"("TribuneCode");

-- Insert migration record
INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion") 
VALUES ('20250905220800_InitialPostgreSQLMigration', '9.0.8');