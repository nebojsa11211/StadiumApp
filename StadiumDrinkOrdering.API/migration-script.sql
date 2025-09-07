CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;

CREATE TABLE "Drinks" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Drinks" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Description" TEXT NULL,
    "Price" TEXT NOT NULL,
    "StockQuantity" INTEGER NOT NULL,
    "ImageUrl" TEXT NULL,
    "Category" INTEGER NOT NULL,
    "IsAvailable" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL
);

CREATE TABLE "Events" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Events" PRIMARY KEY AUTOINCREMENT,
    "EventName" TEXT NOT NULL,
    "EventType" TEXT NOT NULL,
    "EventDate" TEXT NOT NULL,
    "VenueId" INTEGER NULL,
    "TotalSeats" INTEGER NOT NULL,
    "IsActive" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NULL,
    "Description" TEXT NULL,
    "ImageUrl" TEXT NULL,
    "BaseTicketPrice" TEXT NULL
);

CREATE TABLE "StadiumSeats" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_StadiumSeats" PRIMARY KEY AUTOINCREMENT,
    "Section" TEXT NOT NULL,
    "RowNumber" INTEGER NOT NULL,
    "SeatNumber" INTEGER NOT NULL,
    "XCoordinate" INTEGER NOT NULL,
    "YCoordinate" INTEGER NOT NULL,
    "IsActive" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL
);

CREATE TABLE "StadiumSections" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_StadiumSections" PRIMARY KEY AUTOINCREMENT,
    "SectionCode" TEXT NOT NULL,
    "SectionName" TEXT NOT NULL,
    "TotalRows" INTEGER NOT NULL,
    "SeatsPerRow" INTEGER NOT NULL,
    "PriceMultiplier" TEXT NOT NULL,
    "IsActive" INTEGER NOT NULL,
    "Color" TEXT NOT NULL
);

CREATE TABLE "Users" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Users" PRIMARY KEY AUTOINCREMENT,
    "Username" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "PasswordHash" TEXT NOT NULL,
    "Role" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "LastLoginAt" TEXT NULL
);

CREATE TABLE "EventAnalytics" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_EventAnalytics" PRIMARY KEY AUTOINCREMENT,
    "EventId" INTEGER NOT NULL,
    "TotalTicketsSold" INTEGER NOT NULL,
    "TotalRevenue" TEXT NOT NULL,
    "TotalOrders" INTEGER NOT NULL,
    "TotalDrinksSold" INTEGER NOT NULL,
    "AverageOrderValue" TEXT NOT NULL,
    "PeakOrderTime" TEXT NULL,
    "MostPopularDrink" TEXT NULL,
    "CalculatedAt" TEXT NOT NULL,
    CONSTRAINT "FK_EventAnalytics_Events_EventId" FOREIGN KEY ("EventId") REFERENCES "Events" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Seats" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Seats" PRIMARY KEY AUTOINCREMENT,
    "SectionId" INTEGER NOT NULL,
    "RowNumber" INTEGER NOT NULL,
    "SeatNumber" INTEGER NOT NULL,
    "SeatCode" TEXT NOT NULL,
    "IsAccessible" INTEGER NOT NULL,
    "XCoordinate" INTEGER NOT NULL,
    "YCoordinate" INTEGER NOT NULL,
    CONSTRAINT "FK_Seats_StadiumSections_SectionId" FOREIGN KEY ("SectionId") REFERENCES "StadiumSections" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "EventStaffAssignments" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_EventStaffAssignments" PRIMARY KEY AUTOINCREMENT,
    "EventId" INTEGER NOT NULL,
    "StaffId" INTEGER NOT NULL,
    "AssignedSections" TEXT NULL,
    "Role" TEXT NOT NULL,
    "ShiftStart" TEXT NULL,
    "ShiftEnd" TEXT NULL,
    "IsActive" INTEGER NOT NULL,
    CONSTRAINT "FK_EventStaffAssignments_Events_EventId" FOREIGN KEY ("EventId") REFERENCES "Events" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_EventStaffAssignments_Users_StaffId" FOREIGN KEY ("StaffId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "Notifications" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Notifications" PRIMARY KEY AUTOINCREMENT,
    "UserId" INTEGER NULL,
    "Type" TEXT NOT NULL,
    "Title" TEXT NOT NULL,
    "Message" TEXT NOT NULL,
    "Data" TEXT NULL,
    "IsRead" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "ReadAt" TEXT NULL,
    "Priority" TEXT NOT NULL,
    "TargetRole" TEXT NULL,
    "EventId" INTEGER NULL,
    CONSTRAINT "FK_Notifications_Events_EventId" FOREIGN KEY ("EventId") REFERENCES "Events" ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_Notifications_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Tickets" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Tickets" PRIMARY KEY AUTOINCREMENT,
    "TicketNumber" TEXT NOT NULL,
    "EventId" INTEGER NOT NULL,
    "SeatId" INTEGER NOT NULL,
    "QRCode" TEXT NOT NULL,
    "QRCodeToken" TEXT NOT NULL,
    "CustomerName" TEXT NULL,
    "CustomerEmail" TEXT NULL,
    "CustomerPhone" TEXT NULL,
    "PurchaseDate" TEXT NOT NULL,
    "Price" TEXT NOT NULL,
    "IsUsed" INTEGER NOT NULL,
    "UsedAt" TEXT NULL,
    "Status" TEXT NOT NULL,
    "SeatNumber" TEXT NULL,
    "Section" TEXT NULL,
    "Row" TEXT NULL,
    "EventName" TEXT NULL,
    "EventDate" TEXT NULL,
    "IsActive" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    CONSTRAINT "FK_Tickets_Events_EventId" FOREIGN KEY ("EventId") REFERENCES "Events" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Tickets_Seats_SeatId" FOREIGN KEY ("SeatId") REFERENCES "Seats" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "OrderSessions" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_OrderSessions" PRIMARY KEY AUTOINCREMENT,
    "TicketId" INTEGER NOT NULL,
    "SessionToken" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "ExpiresAt" TEXT NOT NULL,
    "IsActive" INTEGER NOT NULL,
    "IpAddress" TEXT NULL,
    "UserAgent" TEXT NULL,
    "CartData" TEXT NULL,
    "CartTotal" TEXT NULL,
    "ItemCount" INTEGER NOT NULL,
    "LastActivity" TEXT NULL,
    CONSTRAINT "FK_OrderSessions_Tickets_TicketId" FOREIGN KEY ("TicketId") REFERENCES "Tickets" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "OrderItems" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_OrderItems" PRIMARY KEY AUTOINCREMENT,
    "OrderId" INTEGER NOT NULL,
    "DrinkId" INTEGER NOT NULL,
    "Quantity" INTEGER NOT NULL,
    "UnitPrice" TEXT NOT NULL,
    "TotalPrice" TEXT NOT NULL,
    "SpecialInstructions" TEXT NULL,
    CONSTRAINT "FK_OrderItems_Drinks_DrinkId" FOREIGN KEY ("DrinkId") REFERENCES "Drinks" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_OrderItems_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Orders" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Orders" PRIMARY KEY AUTOINCREMENT,
    "EventId" INTEGER NULL,
    "SeatId" INTEGER NULL,
    "PaymentId" INTEGER NULL,
    "SessionId" INTEGER NULL,
    "DeliveryNotes" TEXT NULL,
    "EstimatedDeliveryTime" TEXT NULL,
    "ActualDeliveryTime" TEXT NULL,
    "AssignedStaffId" INTEGER NULL,
    "TicketNumber" TEXT NOT NULL,
    "SeatNumber" TEXT NOT NULL,
    "CustomerId" INTEGER NOT NULL,
    "TotalAmount" TEXT NOT NULL,
    "Status" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "AcceptedAt" TEXT NULL,
    "PreparedAt" TEXT NULL,
    "DeliveredAt" TEXT NULL,
    "CancelledAt" TEXT NULL,
    "AcceptedByUserId" INTEGER NULL,
    "PreparedByUserId" INTEGER NULL,
    "DeliveredByUserId" INTEGER NULL,
    "Notes" TEXT NULL,
    "CustomerNotes" TEXT NULL,
    "StadiumSeatId" INTEGER NULL,
    "TicketId" INTEGER NULL,
    CONSTRAINT "FK_Orders_Events_EventId" FOREIGN KEY ("EventId") REFERENCES "Events" ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_Orders_OrderSessions_SessionId" FOREIGN KEY ("SessionId") REFERENCES "OrderSessions" ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_Orders_Seats_SeatId" FOREIGN KEY ("SeatId") REFERENCES "Seats" ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_Orders_StadiumSeats_StadiumSeatId" FOREIGN KEY ("StadiumSeatId") REFERENCES "StadiumSeats" ("Id"),
    CONSTRAINT "FK_Orders_Tickets_TicketId" FOREIGN KEY ("TicketId") REFERENCES "Tickets" ("Id"),
    CONSTRAINT "FK_Orders_Users_AcceptedByUserId" FOREIGN KEY ("AcceptedByUserId") REFERENCES "Users" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Orders_Users_AssignedStaffId" FOREIGN KEY ("AssignedStaffId") REFERENCES "Users" ("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_Orders_Users_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "Users" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Orders_Users_DeliveredByUserId" FOREIGN KEY ("DeliveredByUserId") REFERENCES "Users" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Orders_Users_PreparedByUserId" FOREIGN KEY ("PreparedByUserId") REFERENCES "Users" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Orders_Payments_PaymentId" FOREIGN KEY ("PaymentId") REFERENCES "Payments" ("Id") ON DELETE SET NULL
);

CREATE TABLE "Payments" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Payments" PRIMARY KEY AUTOINCREMENT,
    "OrderId" INTEGER NOT NULL,
    "PaymentMethod" TEXT NOT NULL,
    "TransactionId" TEXT NULL,
    "Amount" TEXT NOT NULL,
    "Currency" TEXT NOT NULL,
    "Status" TEXT NOT NULL,
    "PaymentDate" TEXT NOT NULL,
    "PaymentGatewayResponse" TEXT NULL,
    "RefundAmount" TEXT NULL,
    "RefundDate" TEXT NULL,
    "RefundReason" TEXT NULL,
    "CreatedAt" TEXT NOT NULL,
    "ProcessedAt" TEXT NULL,
    "FailedAt" TEXT NULL,
    "FailureReason" TEXT NULL,
    CONSTRAINT "FK_Payments_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders" ("Id") ON DELETE CASCADE
);

INSERT INTO "Drinks" ("Id", "Category", "CreatedAt", "Description", "ImageUrl", "IsAvailable", "Name", "Price", "StockQuantity", "UpdatedAt")
VALUES (1, 2, '2025-08-30 00:18:38.9626164', 'Classic Coca Cola', NULL, 1, 'Coca Cola', '3.5', 100, NULL);
SELECT changes();

INSERT INTO "Drinks" ("Id", "Category", "CreatedAt", "Description", "ImageUrl", "IsAvailable", "Name", "Price", "StockQuantity", "UpdatedAt")
VALUES (2, 2, '2025-08-30 00:18:38.9626177', 'Pepsi Cola', NULL, 1, 'Pepsi', '3.5', 100, NULL);
SELECT changes();

INSERT INTO "Drinks" ("Id", "Category", "CreatedAt", "Description", "ImageUrl", "IsAvailable", "Name", "Price", "StockQuantity", "UpdatedAt")
VALUES (3, 3, '2025-08-30 00:18:38.9626179', 'Bottled Water', NULL, 1, 'Water', '2.0', 200, NULL);
SELECT changes();

INSERT INTO "Drinks" ("Id", "Category", "CreatedAt", "Description", "ImageUrl", "IsAvailable", "Name", "Price", "StockQuantity", "UpdatedAt")
VALUES (4, 1, '2025-08-30 00:18:38.962618', 'Local Draft Beer', NULL, 1, 'Beer', '6.0', 150, NULL);
SELECT changes();

INSERT INTO "Drinks" ("Id", "Category", "CreatedAt", "Description", "ImageUrl", "IsAvailable", "Name", "Price", "StockQuantity", "UpdatedAt")
VALUES (5, 4, '2025-08-30 00:18:38.9626181', 'Hot Coffee', NULL, 1, 'Coffee', '4.0', 80, NULL);
SELECT changes();

INSERT INTO "Drinks" ("Id", "Category", "CreatedAt", "Description", "ImageUrl", "IsAvailable", "Name", "Price", "StockQuantity", "UpdatedAt")
VALUES (6, 6, '2025-08-30 00:18:38.9626183', 'Fresh Orange Juice', NULL, 1, 'Orange Juice', '4.5', 60, NULL);
SELECT changes();

INSERT INTO "Drinks" ("Id", "Category", "CreatedAt", "Description", "ImageUrl", "IsAvailable", "Name", "Price", "StockQuantity", "UpdatedAt")
VALUES (7, 7, '2025-08-30 00:18:38.9626184', 'Energy Drink', NULL, 1, 'Red Bull', '5.0', 90, NULL);
SELECT changes();

INSERT INTO "Drinks" ("Id", "Category", "CreatedAt", "Description", "ImageUrl", "IsAvailable", "Name", "Price", "StockQuantity", "UpdatedAt")
VALUES (8, 5, '2025-08-30 00:18:38.9626185', 'Hot Green Tea', NULL, 1, 'Green Tea', '3.0', 70, NULL);
SELECT changes();


INSERT INTO "Events" ("Id", "BaseTicketPrice", "CreatedAt", "Description", "EventDate", "EventName", "EventType", "ImageUrl", "IsActive", "TotalSeats", "UpdatedAt", "VenueId")
VALUES (1, '50.0', '2025-08-30 00:18:38.96265', 'Championship final match', '2025-08-30 19:00:00', 'Championship Match', 'Football', NULL, 1, 100, NULL, NULL);
SELECT changes();


INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (6, '2025-08-30 00:18:38.9626849', 1, 1, 1, 'A', 50, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (7, '2025-08-30 00:18:38.9626855', 1, 1, 2, 'A', 65, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (8, '2025-08-30 00:18:38.9626862', 1, 1, 3, 'A', 80, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (9, '2025-08-30 00:18:38.9626863', 1, 1, 4, 'A', 95, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (10, '2025-08-30 00:18:38.9626864', 1, 1, 5, 'A', 110, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (11, '2025-08-30 00:18:38.9626865', 1, 1, 6, 'A', 125, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (12, '2025-08-30 00:18:38.9626866', 1, 1, 7, 'A', 140, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (13, '2025-08-30 00:18:38.9626867', 1, 1, 8, 'A', 155, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (14, '2025-08-30 00:18:38.9626868', 1, 1, 9, 'A', 170, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (15, '2025-08-30 00:18:38.9626869', 1, 1, 10, 'A', 185, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (16, '2025-08-30 00:18:38.962687', 1, 2, 1, 'A', 50, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (17, '2025-08-30 00:18:38.9626871', 1, 2, 2, 'A', 65, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (18, '2025-08-30 00:18:38.9626872', 1, 2, 3, 'A', 80, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (19, '2025-08-30 00:18:38.9626873', 1, 2, 4, 'A', 95, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (20, '2025-08-30 00:18:38.9626874', 1, 2, 5, 'A', 110, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (21, '2025-08-30 00:18:38.9626874', 1, 2, 6, 'A', 125, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (22, '2025-08-30 00:18:38.9626875', 1, 2, 7, 'A', 140, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (23, '2025-08-30 00:18:38.9626877', 1, 2, 8, 'A', 155, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (24, '2025-08-30 00:18:38.9626878', 1, 2, 9, 'A', 170, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (25, '2025-08-30 00:18:38.9626879', 1, 2, 10, 'A', 185, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (26, '2025-08-30 00:18:38.9626879', 1, 3, 1, 'A', 50, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (27, '2025-08-30 00:18:38.962688', 1, 3, 2, 'A', 65, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (28, '2025-08-30 00:18:38.9626881', 1, 3, 3, 'A', 80, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (29, '2025-08-30 00:18:38.9626882', 1, 3, 4, 'A', 95, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (30, '2025-08-30 00:18:38.9626882', 1, 3, 5, 'A', 110, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (31, '2025-08-30 00:18:38.9626883', 1, 3, 6, 'A', 125, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (32, '2025-08-30 00:18:38.9626884', 1, 3, 7, 'A', 140, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (33, '2025-08-30 00:18:38.9626885', 1, 3, 8, 'A', 155, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (34, '2025-08-30 00:18:38.9626885', 1, 3, 9, 'A', 170, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (35, '2025-08-30 00:18:38.9626934', 1, 3, 10, 'A', 185, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (36, '2025-08-30 00:18:38.9626936', 1, 4, 1, 'A', 50, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (37, '2025-08-30 00:18:38.9626937', 1, 4, 2, 'A', 65, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (38, '2025-08-30 00:18:38.9626937', 1, 4, 3, 'A', 80, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (39, '2025-08-30 00:18:38.9626939', 1, 4, 4, 'A', 95, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (40, '2025-08-30 00:18:38.962694', 1, 4, 5, 'A', 110, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (41, '2025-08-30 00:18:38.9626941', 1, 4, 6, 'A', 125, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (42, '2025-08-30 00:18:38.9626941', 1, 4, 7, 'A', 140, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (43, '2025-08-30 00:18:38.9626942', 1, 4, 8, 'A', 155, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (44, '2025-08-30 00:18:38.9626943', 1, 4, 9, 'A', 170, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (45, '2025-08-30 00:18:38.9626944', 1, 4, 10, 'A', 185, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (46, '2025-08-30 00:18:38.9626945', 1, 5, 1, 'A', 50, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (47, '2025-08-30 00:18:38.9626945', 1, 5, 2, 'A', 65, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (48, '2025-08-30 00:18:38.9626946', 1, 5, 3, 'A', 80, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (49, '2025-08-30 00:18:38.9626947', 1, 5, 4, 'A', 95, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (50, '2025-08-30 00:18:38.9626948', 1, 5, 5, 'A', 110, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (51, '2025-08-30 00:18:38.9626948', 1, 5, 6, 'A', 125, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (52, '2025-08-30 00:18:38.9626949', 1, 5, 7, 'A', 140, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (53, '2025-08-30 00:18:38.962695', 1, 5, 8, 'A', 155, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (54, '2025-08-30 00:18:38.9626951', 1, 5, 9, 'A', 170, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (55, '2025-08-30 00:18:38.9626951', 1, 5, 10, 'A', 185, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (56, '2025-08-30 00:18:38.9626952', 1, 6, 1, 'A', 50, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (57, '2025-08-30 00:18:38.9626953', 1, 6, 2, 'A', 65, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (58, '2025-08-30 00:18:38.9626954', 1, 6, 3, 'A', 80, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (59, '2025-08-30 00:18:38.9626955', 1, 6, 4, 'A', 95, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (60, '2025-08-30 00:18:38.9626955', 1, 6, 5, 'A', 110, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (61, '2025-08-30 00:18:38.9626956', 1, 6, 6, 'A', 125, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (62, '2025-08-30 00:18:38.9626957', 1, 6, 7, 'A', 140, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (63, '2025-08-30 00:18:38.9626958', 1, 6, 8, 'A', 155, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (64, '2025-08-30 00:18:38.9626958', 1, 6, 9, 'A', 170, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (65, '2025-08-30 00:18:38.9626959', 1, 6, 10, 'A', 185, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (66, '2025-08-30 00:18:38.962696', 1, 7, 1, 'A', 50, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (67, '2025-08-30 00:18:38.9626961', 1, 7, 2, 'A', 65, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (68, '2025-08-30 00:18:38.9626961', 1, 7, 3, 'A', 80, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (69, '2025-08-30 00:18:38.9626962', 1, 7, 4, 'A', 95, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (70, '2025-08-30 00:18:38.9626963', 1, 7, 5, 'A', 110, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (71, '2025-08-30 00:18:38.9626964', 1, 7, 6, 'A', 125, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (72, '2025-08-30 00:18:38.9626965', 1, 7, 7, 'A', 140, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (73, '2025-08-30 00:18:38.9626966', 1, 7, 8, 'A', 155, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (74, '2025-08-30 00:18:38.9626966', 1, 7, 9, 'A', 170, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (75, '2025-08-30 00:18:38.9626967', 1, 7, 10, 'A', 185, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (76, '2025-08-30 00:18:38.9626968', 1, 8, 1, 'A', 50, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (77, '2025-08-30 00:18:38.9626969', 1, 8, 2, 'A', 65, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (78, '2025-08-30 00:18:38.9626969', 1, 8, 3, 'A', 80, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (79, '2025-08-30 00:18:38.962697', 1, 8, 4, 'A', 95, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (80, '2025-08-30 00:18:38.9626971', 1, 8, 5, 'A', 110, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (81, '2025-08-30 00:18:38.9626972', 1, 8, 6, 'A', 125, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (82, '2025-08-30 00:18:38.9626973', 1, 8, 7, 'A', 140, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (83, '2025-08-30 00:18:38.9626973', 1, 8, 8, 'A', 155, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (84, '2025-08-30 00:18:38.9626974', 1, 8, 9, 'A', 170, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (85, '2025-08-30 00:18:38.9626975', 1, 8, 10, 'A', 185, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (86, '2025-08-30 00:18:38.9626975', 1, 9, 1, 'A', 50, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (87, '2025-08-30 00:18:38.9626976', 1, 9, 2, 'A', 65, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (88, '2025-08-30 00:18:38.9626977', 1, 9, 3, 'A', 80, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (89, '2025-08-30 00:18:38.9626978', 1, 9, 4, 'A', 95, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (90, '2025-08-30 00:18:38.9626979', 1, 9, 5, 'A', 110, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (91, '2025-08-30 00:18:38.9626979', 1, 9, 6, 'A', 125, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (92, '2025-08-30 00:18:38.962698', 1, 9, 7, 'A', 140, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (93, '2025-08-30 00:18:38.9626981', 1, 9, 8, 'A', 155, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (94, '2025-08-30 00:18:38.9626981', 1, 9, 9, 'A', 170, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (95, '2025-08-30 00:18:38.9626982', 1, 9, 10, 'A', 185, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (96, '2025-08-30 00:18:38.9626983', 1, 10, 1, 'A', 50, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (97, '2025-08-30 00:18:38.9626984', 1, 10, 2, 'A', 65, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (98, '2025-08-30 00:18:38.9626984', 1, 10, 3, 'A', 80, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (99, '2025-08-30 00:18:38.9626985', 1, 10, 4, 'A', 95, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (100, '2025-08-30 00:18:38.9626986', 1, 10, 5, 'A', 110, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (101, '2025-08-30 00:18:38.9626987', 1, 10, 6, 'A', 125, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (102, '2025-08-30 00:18:38.9626987', 1, 10, 7, 'A', 140, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (103, '2025-08-30 00:18:38.9626988', 1, 10, 8, 'A', 155, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (104, '2025-08-30 00:18:38.9627024', 1, 10, 9, 'A', 170, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (105, '2025-08-30 00:18:38.9627026', 1, 10, 10, 'A', 185, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (106, '2025-08-30 00:18:38.9627027', 1, 1, 1, 'B', 300, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (107, '2025-08-30 00:18:38.9627029', 1, 1, 2, 'B', 315, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (108, '2025-08-30 00:18:38.9627029', 1, 1, 3, 'B', 330, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (109, '2025-08-30 00:18:38.962703', 1, 1, 4, 'B', 345, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (110, '2025-08-30 00:18:38.9627031', 1, 1, 5, 'B', 360, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (111, '2025-08-30 00:18:38.9627032', 1, 1, 6, 'B', 375, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (112, '2025-08-30 00:18:38.9627032', 1, 1, 7, 'B', 390, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (113, '2025-08-30 00:18:38.9627033', 1, 1, 8, 'B', 405, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (114, '2025-08-30 00:18:38.9627034', 1, 1, 9, 'B', 420, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (115, '2025-08-30 00:18:38.9627035', 1, 1, 10, 'B', 435, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (116, '2025-08-30 00:18:38.9627036', 1, 2, 1, 'B', 300, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (117, '2025-08-30 00:18:38.9627037', 1, 2, 2, 'B', 315, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (118, '2025-08-30 00:18:38.9627037', 1, 2, 3, 'B', 330, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (119, '2025-08-30 00:18:38.9627038', 1, 2, 4, 'B', 345, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (120, '2025-08-30 00:18:38.9627039', 1, 2, 5, 'B', 360, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (121, '2025-08-30 00:18:38.962704', 1, 2, 6, 'B', 375, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (122, '2025-08-30 00:18:38.962704', 1, 2, 7, 'B', 390, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (123, '2025-08-30 00:18:38.9627041', 1, 2, 8, 'B', 405, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (124, '2025-08-30 00:18:38.9627042', 1, 2, 9, 'B', 420, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (125, '2025-08-30 00:18:38.9627043', 1, 2, 10, 'B', 435, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (126, '2025-08-30 00:18:38.9627043', 1, 3, 1, 'B', 300, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (127, '2025-08-30 00:18:38.9627044', 1, 3, 2, 'B', 315, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (128, '2025-08-30 00:18:38.9627045', 1, 3, 3, 'B', 330, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (129, '2025-08-30 00:18:38.9627046', 1, 3, 4, 'B', 345, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (130, '2025-08-30 00:18:38.9627046', 1, 3, 5, 'B', 360, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (131, '2025-08-30 00:18:38.9627047', 1, 3, 6, 'B', 375, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (132, '2025-08-30 00:18:38.9627048', 1, 3, 7, 'B', 390, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (133, '2025-08-30 00:18:38.9627049', 1, 3, 8, 'B', 405, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (134, '2025-08-30 00:18:38.9627049', 1, 3, 9, 'B', 420, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (135, '2025-08-30 00:18:38.9627051', 1, 3, 10, 'B', 435, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (136, '2025-08-30 00:18:38.9627052', 1, 4, 1, 'B', 300, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (137, '2025-08-30 00:18:38.9627053', 1, 4, 2, 'B', 315, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (138, '2025-08-30 00:18:38.9627053', 1, 4, 3, 'B', 330, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (139, '2025-08-30 00:18:38.9627054', 1, 4, 4, 'B', 345, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (140, '2025-08-30 00:18:38.9627055', 1, 4, 5, 'B', 360, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (141, '2025-08-30 00:18:38.9627056', 1, 4, 6, 'B', 375, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (142, '2025-08-30 00:18:38.9627057', 1, 4, 7, 'B', 390, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (143, '2025-08-30 00:18:38.9627057', 1, 4, 8, 'B', 405, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (144, '2025-08-30 00:18:38.9627058', 1, 4, 9, 'B', 420, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (145, '2025-08-30 00:18:38.9627059', 1, 4, 10, 'B', 435, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (146, '2025-08-30 00:18:38.962706', 1, 5, 1, 'B', 300, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (147, '2025-08-30 00:18:38.962706', 1, 5, 2, 'B', 315, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (148, '2025-08-30 00:18:38.9627061', 1, 5, 3, 'B', 330, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (149, '2025-08-30 00:18:38.9627062', 1, 5, 4, 'B', 345, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (150, '2025-08-30 00:18:38.9627063', 1, 5, 5, 'B', 360, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (151, '2025-08-30 00:18:38.9627063', 1, 5, 6, 'B', 375, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (152, '2025-08-30 00:18:38.9627064', 1, 5, 7, 'B', 390, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (153, '2025-08-30 00:18:38.9627065', 1, 5, 8, 'B', 405, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (154, '2025-08-30 00:18:38.9627066', 1, 5, 9, 'B', 420, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (155, '2025-08-30 00:18:38.9627066', 1, 5, 10, 'B', 435, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (156, '2025-08-30 00:18:38.9627067', 1, 6, 1, 'B', 300, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (157, '2025-08-30 00:18:38.9627068', 1, 6, 2, 'B', 315, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (158, '2025-08-30 00:18:38.9627068', 1, 6, 3, 'B', 330, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (159, '2025-08-30 00:18:38.9627069', 1, 6, 4, 'B', 345, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (160, '2025-08-30 00:18:38.962707', 1, 6, 5, 'B', 360, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (161, '2025-08-30 00:18:38.9627071', 1, 6, 6, 'B', 375, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (162, '2025-08-30 00:18:38.9627071', 1, 6, 7, 'B', 390, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (163, '2025-08-30 00:18:38.9627072', 1, 6, 8, 'B', 405, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (164, '2025-08-30 00:18:38.9627073', 1, 6, 9, 'B', 420, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (165, '2025-08-30 00:18:38.9627073', 1, 6, 10, 'B', 435, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (166, '2025-08-30 00:18:38.9627074', 1, 7, 1, 'B', 300, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (167, '2025-08-30 00:18:38.9627075', 1, 7, 2, 'B', 315, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (168, '2025-08-30 00:18:38.9627111', 1, 7, 3, 'B', 330, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (169, '2025-08-30 00:18:38.9627113', 1, 7, 4, 'B', 345, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (170, '2025-08-30 00:18:38.9627113', 1, 7, 5, 'B', 360, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (171, '2025-08-30 00:18:38.9627114', 1, 7, 6, 'B', 375, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (172, '2025-08-30 00:18:38.9627115', 1, 7, 7, 'B', 390, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (173, '2025-08-30 00:18:38.9627115', 1, 7, 8, 'B', 405, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (174, '2025-08-30 00:18:38.9627116', 1, 7, 9, 'B', 420, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (175, '2025-08-30 00:18:38.9627117', 1, 7, 10, 'B', 435, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (176, '2025-08-30 00:18:38.9627118', 1, 8, 1, 'B', 300, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (177, '2025-08-30 00:18:38.9627118', 1, 8, 2, 'B', 315, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (178, '2025-08-30 00:18:38.9627119', 1, 8, 3, 'B', 330, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (179, '2025-08-30 00:18:38.962712', 1, 8, 4, 'B', 345, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (180, '2025-08-30 00:18:38.962712', 1, 8, 5, 'B', 360, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (181, '2025-08-30 00:18:38.9627121', 1, 8, 6, 'B', 375, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (182, '2025-08-30 00:18:38.9627122', 1, 8, 7, 'B', 390, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (183, '2025-08-30 00:18:38.9627123', 1, 8, 8, 'B', 405, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (184, '2025-08-30 00:18:38.9627123', 1, 8, 9, 'B', 420, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (185, '2025-08-30 00:18:38.9627124', 1, 8, 10, 'B', 435, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (186, '2025-08-30 00:18:38.9627125', 1, 9, 1, 'B', 300, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (187, '2025-08-30 00:18:38.9627126', 1, 9, 2, 'B', 315, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (188, '2025-08-30 00:18:38.9627126', 1, 9, 3, 'B', 330, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (189, '2025-08-30 00:18:38.9627127', 1, 9, 4, 'B', 345, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (190, '2025-08-30 00:18:38.9627128', 1, 9, 5, 'B', 360, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (191, '2025-08-30 00:18:38.9627128', 1, 9, 6, 'B', 375, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (192, '2025-08-30 00:18:38.9627129', 1, 9, 7, 'B', 390, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (193, '2025-08-30 00:18:38.962713', 1, 9, 8, 'B', 405, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (194, '2025-08-30 00:18:38.962713', 1, 9, 9, 'B', 420, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (195, '2025-08-30 00:18:38.9627131', 1, 9, 10, 'B', 435, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (196, '2025-08-30 00:18:38.9627132', 1, 10, 1, 'B', 300, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (197, '2025-08-30 00:18:38.9627133', 1, 10, 2, 'B', 315, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (198, '2025-08-30 00:18:38.9627133', 1, 10, 3, 'B', 330, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (199, '2025-08-30 00:18:38.9627134', 1, 10, 4, 'B', 345, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (200, '2025-08-30 00:18:38.9627135', 1, 10, 5, 'B', 360, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (201, '2025-08-30 00:18:38.9627135', 1, 10, 6, 'B', 375, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (202, '2025-08-30 00:18:38.9627136', 1, 10, 7, 'B', 390, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (203, '2025-08-30 00:18:38.9627137', 1, 10, 8, 'B', 405, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (204, '2025-08-30 00:18:38.9627137', 1, 10, 9, 'B', 420, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (205, '2025-08-30 00:18:38.9627138', 1, 10, 10, 'B', 435, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (206, '2025-08-30 00:18:38.9627139', 1, 1, 1, 'C', 550, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (207, '2025-08-30 00:18:38.9627141', 1, 1, 2, 'C', 565, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (208, '2025-08-30 00:18:38.9627141', 1, 1, 3, 'C', 580, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (209, '2025-08-30 00:18:38.9627142', 1, 1, 4, 'C', 595, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (210, '2025-08-30 00:18:38.9627143', 1, 1, 5, 'C', 610, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (211, '2025-08-30 00:18:38.9627143', 1, 1, 6, 'C', 625, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (212, '2025-08-30 00:18:38.9627144', 1, 1, 7, 'C', 640, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (213, '2025-08-30 00:18:38.9627145', 1, 1, 8, 'C', 655, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (214, '2025-08-30 00:18:38.9627146', 1, 1, 9, 'C', 670, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (215, '2025-08-30 00:18:38.9627146', 1, 1, 10, 'C', 685, 50);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (216, '2025-08-30 00:18:38.9627147', 1, 2, 1, 'C', 550, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (217, '2025-08-30 00:18:38.9627148', 1, 2, 2, 'C', 565, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (218, '2025-08-30 00:18:38.9627149', 1, 2, 3, 'C', 580, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (219, '2025-08-30 00:18:38.962715', 1, 2, 4, 'C', 595, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (220, '2025-08-30 00:18:38.962715', 1, 2, 5, 'C', 610, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (221, '2025-08-30 00:18:38.9627151', 1, 2, 6, 'C', 625, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (222, '2025-08-30 00:18:38.9627152', 1, 2, 7, 'C', 640, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (223, '2025-08-30 00:18:38.9627153', 1, 2, 8, 'C', 655, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (224, '2025-08-30 00:18:38.9627153', 1, 2, 9, 'C', 670, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (225, '2025-08-30 00:18:38.9627154', 1, 2, 10, 'C', 685, 65);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (226, '2025-08-30 00:18:38.9627155', 1, 3, 1, 'C', 550, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (227, '2025-08-30 00:18:38.9627156', 1, 3, 2, 'C', 565, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (228, '2025-08-30 00:18:38.9627156', 1, 3, 3, 'C', 580, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (229, '2025-08-30 00:18:38.9627157', 1, 3, 4, 'C', 595, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (230, '2025-08-30 00:18:38.9627158', 1, 3, 5, 'C', 610, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (231, '2025-08-30 00:18:38.9627159', 1, 3, 6, 'C', 625, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (232, '2025-08-30 00:18:38.9627159', 1, 3, 7, 'C', 640, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (233, '2025-08-30 00:18:38.962716', 1, 3, 8, 'C', 655, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (234, '2025-08-30 00:18:38.9627161', 1, 3, 9, 'C', 670, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (235, '2025-08-30 00:18:38.9627162', 1, 3, 10, 'C', 685, 80);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (236, '2025-08-30 00:18:38.9627163', 1, 4, 1, 'C', 550, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (237, '2025-08-30 00:18:38.9627163', 1, 4, 2, 'C', 565, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (238, '2025-08-30 00:18:38.9627164', 1, 4, 3, 'C', 580, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (239, '2025-08-30 00:18:38.9627165', 1, 4, 4, 'C', 595, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (240, '2025-08-30 00:18:38.9627166', 1, 4, 5, 'C', 610, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (241, '2025-08-30 00:18:38.9627166', 1, 4, 6, 'C', 625, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (242, '2025-08-30 00:18:38.9627167', 1, 4, 7, 'C', 640, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (243, '2025-08-30 00:18:38.9627168', 1, 4, 8, 'C', 655, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (244, '2025-08-30 00:18:38.9627169', 1, 4, 9, 'C', 670, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (245, '2025-08-30 00:18:38.962717', 1, 4, 10, 'C', 685, 95);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (246, '2025-08-30 00:18:38.962717', 1, 5, 1, 'C', 550, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (247, '2025-08-30 00:18:38.9627171', 1, 5, 2, 'C', 565, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (248, '2025-08-30 00:18:38.9627172', 1, 5, 3, 'C', 580, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (249, '2025-08-30 00:18:38.9627173', 1, 5, 4, 'C', 595, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (250, '2025-08-30 00:18:38.9627173', 1, 5, 5, 'C', 610, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (251, '2025-08-30 00:18:38.9627174', 1, 5, 6, 'C', 625, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (252, '2025-08-30 00:18:38.9627175', 1, 5, 7, 'C', 640, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (253, '2025-08-30 00:18:38.9627175', 1, 5, 8, 'C', 655, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (254, '2025-08-30 00:18:38.9627208', 1, 5, 9, 'C', 670, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (255, '2025-08-30 00:18:38.9627209', 1, 5, 10, 'C', 685, 110);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (256, '2025-08-30 00:18:38.962721', 1, 6, 1, 'C', 550, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (257, '2025-08-30 00:18:38.962721', 1, 6, 2, 'C', 565, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (258, '2025-08-30 00:18:38.9627211', 1, 6, 3, 'C', 580, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (259, '2025-08-30 00:18:38.9627212', 1, 6, 4, 'C', 595, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (260, '2025-08-30 00:18:38.9627213', 1, 6, 5, 'C', 610, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (261, '2025-08-30 00:18:38.9627214', 1, 6, 6, 'C', 625, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (262, '2025-08-30 00:18:38.9627214', 1, 6, 7, 'C', 640, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (263, '2025-08-30 00:18:38.9627216', 1, 6, 8, 'C', 655, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (264, '2025-08-30 00:18:38.9627217', 1, 6, 9, 'C', 670, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (265, '2025-08-30 00:18:38.9627218', 1, 6, 10, 'C', 685, 125);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (266, '2025-08-30 00:18:38.9627219', 1, 7, 1, 'C', 550, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (267, '2025-08-30 00:18:38.962722', 1, 7, 2, 'C', 565, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (268, '2025-08-30 00:18:38.9627221', 1, 7, 3, 'C', 580, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (269, '2025-08-30 00:18:38.9627221', 1, 7, 4, 'C', 595, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (270, '2025-08-30 00:18:38.9627222', 1, 7, 5, 'C', 610, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (271, '2025-08-30 00:18:38.9627223', 1, 7, 6, 'C', 625, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (272, '2025-08-30 00:18:38.9627224', 1, 7, 7, 'C', 640, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (273, '2025-08-30 00:18:38.9627224', 1, 7, 8, 'C', 655, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (274, '2025-08-30 00:18:38.9627225', 1, 7, 9, 'C', 670, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (275, '2025-08-30 00:18:38.9627226', 1, 7, 10, 'C', 685, 140);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (276, '2025-08-30 00:18:38.9627227', 1, 8, 1, 'C', 550, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (277, '2025-08-30 00:18:38.9627228', 1, 8, 2, 'C', 565, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (278, '2025-08-30 00:18:38.9627228', 1, 8, 3, 'C', 580, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (279, '2025-08-30 00:18:38.9627229', 1, 8, 4, 'C', 595, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (280, '2025-08-30 00:18:38.962723', 1, 8, 5, 'C', 610, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (281, '2025-08-30 00:18:38.9627231', 1, 8, 6, 'C', 625, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (282, '2025-08-30 00:18:38.9627231', 1, 8, 7, 'C', 640, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (283, '2025-08-30 00:18:38.9627232', 1, 8, 8, 'C', 655, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (284, '2025-08-30 00:18:38.9627233', 1, 8, 9, 'C', 670, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (285, '2025-08-30 00:18:38.9627234', 1, 8, 10, 'C', 685, 155);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (286, '2025-08-30 00:18:38.9627234', 1, 9, 1, 'C', 550, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (287, '2025-08-30 00:18:38.9627235', 1, 9, 2, 'C', 565, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (288, '2025-08-30 00:18:38.9627236', 1, 9, 3, 'C', 580, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (289, '2025-08-30 00:18:38.9627237', 1, 9, 4, 'C', 595, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (290, '2025-08-30 00:18:38.9627238', 1, 9, 5, 'C', 610, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (291, '2025-08-30 00:18:38.9627238', 1, 9, 6, 'C', 625, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (292, '2025-08-30 00:18:38.9627239', 1, 9, 7, 'C', 640, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (293, '2025-08-30 00:18:38.962724', 1, 9, 8, 'C', 655, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (294, '2025-08-30 00:18:38.9627241', 1, 9, 9, 'C', 670, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (295, '2025-08-30 00:18:38.9627241', 1, 9, 10, 'C', 685, 170);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (296, '2025-08-30 00:18:38.9627265', 1, 10, 1, 'C', 550, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (297, '2025-08-30 00:18:38.9627267', 1, 10, 2, 'C', 565, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (298, '2025-08-30 00:18:38.9627267', 1, 10, 3, 'C', 580, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (299, '2025-08-30 00:18:38.9627268', 1, 10, 4, 'C', 595, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (300, '2025-08-30 00:18:38.9627269', 1, 10, 5, 'C', 610, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (301, '2025-08-30 00:18:38.962727', 1, 10, 6, 'C', 625, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (302, '2025-08-30 00:18:38.962727', 1, 10, 7, 'C', 640, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (303, '2025-08-30 00:18:38.9627271', 1, 10, 8, 'C', 655, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (304, '2025-08-30 00:18:38.9627272', 1, 10, 9, 'C', 670, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (305, '2025-08-30 00:18:38.9627273', 1, 10, 10, 'C', 685, 185);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (306, '2025-08-30 00:18:38.9627274', 1, 1, 1, 'D', 350, 370);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (307, '2025-08-30 00:18:38.9627275', 1, 1, 2, 'D', 365, 370);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (308, '2025-08-30 00:18:38.9627276', 1, 1, 3, 'D', 380, 370);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (309, '2025-08-30 00:18:38.9627277', 1, 1, 4, 'D', 395, 370);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (310, '2025-08-30 00:18:38.9627278', 1, 1, 5, 'D', 410, 370);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (311, '2025-08-30 00:18:38.9627278', 1, 1, 6, 'D', 425, 370);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (312, '2025-08-30 00:18:38.9627279', 1, 1, 7, 'D', 440, 370);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (313, '2025-08-30 00:18:38.962728', 1, 1, 8, 'D', 455, 370);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (314, '2025-08-30 00:18:38.9627281', 1, 1, 9, 'D', 470, 370);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (315, '2025-08-30 00:18:38.9627282', 1, 1, 10, 'D', 485, 370);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (316, '2025-08-30 00:18:38.9627282', 1, 1, 11, 'D', 500, 370);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (317, '2025-08-30 00:18:38.9627283', 1, 1, 12, 'D', 515, 370);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (318, '2025-08-30 00:18:38.9627284', 1, 1, 13, 'D', 530, 370);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (319, '2025-08-30 00:18:38.9627285', 1, 1, 14, 'D', 545, 370);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (320, '2025-08-30 00:18:38.9627286', 1, 2, 1, 'D', 350, 385);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (321, '2025-08-30 00:18:38.9627287', 1, 2, 2, 'D', 365, 385);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (322, '2025-08-30 00:18:38.9627287', 1, 2, 3, 'D', 380, 385);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (323, '2025-08-30 00:18:38.9627288', 1, 2, 4, 'D', 395, 385);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (324, '2025-08-30 00:18:38.9627289', 1, 2, 5, 'D', 410, 385);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (325, '2025-08-30 00:18:38.9627289', 1, 2, 6, 'D', 425, 385);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (326, '2025-08-30 00:18:38.962729', 1, 2, 7, 'D', 440, 385);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (327, '2025-08-30 00:18:38.9627291', 1, 2, 8, 'D', 455, 385);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (328, '2025-08-30 00:18:38.9627292', 1, 2, 9, 'D', 470, 385);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (329, '2025-08-30 00:18:38.9627292', 1, 2, 10, 'D', 485, 385);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (330, '2025-08-30 00:18:38.9627293', 1, 2, 11, 'D', 500, 385);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (331, '2025-08-30 00:18:38.9627294', 1, 2, 12, 'D', 515, 385);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (332, '2025-08-30 00:18:38.9627295', 1, 2, 13, 'D', 530, 385);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (333, '2025-08-30 00:18:38.9627295', 1, 2, 14, 'D', 545, 385);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (334, '2025-08-30 00:18:38.9627296', 1, 3, 1, 'D', 350, 400);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (335, '2025-08-30 00:18:38.9627297', 1, 3, 2, 'D', 365, 400);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (336, '2025-08-30 00:18:38.9627298', 1, 3, 3, 'D', 380, 400);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (337, '2025-08-30 00:18:38.9627299', 1, 3, 4, 'D', 395, 400);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (338, '2025-08-30 00:18:38.9627299', 1, 3, 5, 'D', 410, 400);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (339, '2025-08-30 00:18:38.96273', 1, 3, 6, 'D', 425, 400);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (340, '2025-08-30 00:18:38.9627301', 1, 3, 7, 'D', 440, 400);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (341, '2025-08-30 00:18:38.9627301', 1, 3, 8, 'D', 455, 400);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (342, '2025-08-30 00:18:38.9627302', 1, 3, 9, 'D', 470, 400);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (343, '2025-08-30 00:18:38.9627303', 1, 3, 10, 'D', 485, 400);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (344, '2025-08-30 00:18:38.9627304', 1, 3, 11, 'D', 500, 400);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (345, '2025-08-30 00:18:38.9627304', 1, 3, 12, 'D', 515, 400);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (346, '2025-08-30 00:18:38.9627305', 1, 3, 13, 'D', 530, 400);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (347, '2025-08-30 00:18:38.9627306', 1, 3, 14, 'D', 545, 400);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (348, '2025-08-30 00:18:38.9627307', 1, 4, 1, 'D', 350, 415);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (349, '2025-08-30 00:18:38.9627307', 1, 4, 2, 'D', 365, 415);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (350, '2025-08-30 00:18:38.9627308', 1, 4, 3, 'D', 380, 415);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (351, '2025-08-30 00:18:38.9627309', 1, 4, 4, 'D', 395, 415);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (352, '2025-08-30 00:18:38.962731', 1, 4, 5, 'D', 410, 415);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (353, '2025-08-30 00:18:38.962731', 1, 4, 6, 'D', 425, 415);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (354, '2025-08-30 00:18:38.9627311', 1, 4, 7, 'D', 440, 415);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (355, '2025-08-30 00:18:38.9627312', 1, 4, 8, 'D', 455, 415);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (356, '2025-08-30 00:18:38.9627313', 1, 4, 9, 'D', 470, 415);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (357, '2025-08-30 00:18:38.9627313', 1, 4, 10, 'D', 485, 415);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (358, '2025-08-30 00:18:38.9627314', 1, 4, 11, 'D', 500, 415);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (359, '2025-08-30 00:18:38.9627315', 1, 4, 12, 'D', 515, 415);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (360, '2025-08-30 00:18:38.9627315', 1, 4, 13, 'D', 530, 415);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (361, '2025-08-30 00:18:38.9627316', 1, 4, 14, 'D', 545, 415);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (362, '2025-08-30 00:18:38.9627317', 1, 5, 1, 'D', 350, 430);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (363, '2025-08-30 00:18:38.9627318', 1, 5, 2, 'D', 365, 430);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (364, '2025-08-30 00:18:38.9627318', 1, 5, 3, 'D', 380, 430);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (365, '2025-08-30 00:18:38.9627319', 1, 5, 4, 'D', 395, 430);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (366, '2025-08-30 00:18:38.962732', 1, 5, 5, 'D', 410, 430);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (367, '2025-08-30 00:18:38.9627321', 1, 5, 6, 'D', 425, 430);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (368, '2025-08-30 00:18:38.9627321', 1, 5, 7, 'D', 440, 430);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (369, '2025-08-30 00:18:38.9627322', 1, 5, 8, 'D', 455, 430);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (370, '2025-08-30 00:18:38.9627323', 1, 5, 9, 'D', 470, 430);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (371, '2025-08-30 00:18:38.9627324', 1, 5, 10, 'D', 485, 430);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (372, '2025-08-30 00:18:38.9627324', 1, 5, 11, 'D', 500, 430);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (373, '2025-08-30 00:18:38.9627325', 1, 5, 12, 'D', 515, 430);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (374, '2025-08-30 00:18:38.9627326', 1, 5, 13, 'D', 530, 430);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (375, '2025-08-30 00:18:38.9627327', 1, 5, 14, 'D', 545, 430);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (376, '2025-08-30 00:18:38.9627327', 1, 6, 1, 'D', 350, 445);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (377, '2025-08-30 00:18:38.9627328', 1, 6, 2, 'D', 365, 445);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (378, '2025-08-30 00:18:38.9627329', 1, 6, 3, 'D', 380, 445);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (379, '2025-08-30 00:18:38.9627414', 1, 6, 4, 'D', 395, 445);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (380, '2025-08-30 00:18:38.9627415', 1, 6, 5, 'D', 410, 445);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (381, '2025-08-30 00:18:38.9627416', 1, 6, 6, 'D', 425, 445);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (382, '2025-08-30 00:18:38.9627417', 1, 6, 7, 'D', 440, 445);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (383, '2025-08-30 00:18:38.9627417', 1, 6, 8, 'D', 455, 445);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (384, '2025-08-30 00:18:38.9627418', 1, 6, 9, 'D', 470, 445);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (385, '2025-08-30 00:18:38.9627419', 1, 6, 10, 'D', 485, 445);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (386, '2025-08-30 00:18:38.962742', 1, 6, 11, 'D', 500, 445);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (387, '2025-08-30 00:18:38.962742', 1, 6, 12, 'D', 515, 445);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (388, '2025-08-30 00:18:38.9627421', 1, 6, 13, 'D', 530, 445);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (389, '2025-08-30 00:18:38.9627422', 1, 6, 14, 'D', 545, 445);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (390, '2025-08-30 00:18:38.9627423', 1, 7, 1, 'D', 350, 460);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (391, '2025-08-30 00:18:38.9627423', 1, 7, 2, 'D', 365, 460);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (392, '2025-08-30 00:18:38.9627424', 1, 7, 3, 'D', 380, 460);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (393, '2025-08-30 00:18:38.9627425', 1, 7, 4, 'D', 395, 460);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (394, '2025-08-30 00:18:38.9627426', 1, 7, 5, 'D', 410, 460);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (395, '2025-08-30 00:18:38.9627426', 1, 7, 6, 'D', 425, 460);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (396, '2025-08-30 00:18:38.9627427', 1, 7, 7, 'D', 440, 460);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (397, '2025-08-30 00:18:38.9627428', 1, 7, 8, 'D', 455, 460);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (398, '2025-08-30 00:18:38.9627429', 1, 7, 9, 'D', 470, 460);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (399, '2025-08-30 00:18:38.9627429', 1, 7, 10, 'D', 485, 460);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (400, '2025-08-30 00:18:38.962743', 1, 7, 11, 'D', 500, 460);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (401, '2025-08-30 00:18:38.9627431', 1, 7, 12, 'D', 515, 460);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (402, '2025-08-30 00:18:38.9627431', 1, 7, 13, 'D', 530, 460);
SELECT changes();

INSERT INTO "StadiumSeats" ("Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate")
VALUES (403, '2025-08-30 00:18:38.9627432', 1, 7, 14, 'D', 545, 460);
SELECT changes();


INSERT INTO "StadiumSections" ("Id", "Color", "IsActive", "PriceMultiplier", "SeatsPerRow", "SectionCode", "SectionName", "TotalRows")
VALUES (1, '#007bff', 1, '1.0', 10, 'A', 'Section A', 10);
SELECT changes();

INSERT INTO "StadiumSections" ("Id", "Color", "IsActive", "PriceMultiplier", "SeatsPerRow", "SectionCode", "SectionName", "TotalRows")
VALUES (2, '#28a745', 1, '1.2', 10, 'B', 'Section B', 10);
SELECT changes();

INSERT INTO "StadiumSections" ("Id", "Color", "IsActive", "PriceMultiplier", "SeatsPerRow", "SectionCode", "SectionName", "TotalRows")
VALUES (3, '#ffc107', 1, '1.5', 10, 'C', 'Section C', 10);
SELECT changes();

INSERT INTO "StadiumSections" ("Id", "Color", "IsActive", "PriceMultiplier", "SeatsPerRow", "SectionCode", "SectionName", "TotalRows")
VALUES (4, '#dc3545', 1, '2.0', 14, 'D', 'Section D VIP', 7);
SELECT changes();


INSERT INTO "Users" ("Id", "CreatedAt", "Email", "LastLoginAt", "PasswordHash", "Role", "Username")
VALUES (1, '2025-08-30 00:18:38.9625224', 'admin@stadium.com', NULL, '$2a$11$cnFDUR1doXuho784NiwgGO3WZoZ.J2VDMcRXXLoOlv6S0GvOQyifu', 2, 'admin');
SELECT changes();


INSERT INTO "Seats" ("Id", "IsAccessible", "RowNumber", "SeatCode", "SeatNumber", "SectionId", "XCoordinate", "YCoordinate")
VALUES (1, 0, 1, 'A-R1-S1', 1, 1, 50, 50);
SELECT changes();

INSERT INTO "Seats" ("Id", "IsAccessible", "RowNumber", "SeatCode", "SeatNumber", "SectionId", "XCoordinate", "YCoordinate")
VALUES (2, 0, 1, 'A-R1-S2', 2, 1, 65, 50);
SELECT changes();

INSERT INTO "Seats" ("Id", "IsAccessible", "RowNumber", "SeatCode", "SeatNumber", "SectionId", "XCoordinate", "YCoordinate")
VALUES (3, 0, 5, 'B-R5-S5', 5, 2, 375, 110);
SELECT changes();

INSERT INTO "Seats" ("Id", "IsAccessible", "RowNumber", "SeatCode", "SeatNumber", "SectionId", "XCoordinate", "YCoordinate")
VALUES (4, 0, 10, 'C-R10-S10', 10, 3, 685, 185);
SELECT changes();

INSERT INTO "Seats" ("Id", "IsAccessible", "RowNumber", "SeatCode", "SeatNumber", "SectionId", "XCoordinate", "YCoordinate")
VALUES (5, 0, 3, 'D-R3-S7', 7, 4, 440, 400);
SELECT changes();


INSERT INTO "Tickets" ("Id", "CreatedAt", "CustomerEmail", "CustomerName", "CustomerPhone", "EventDate", "EventId", "EventName", "IsActive", "IsUsed", "Price", "PurchaseDate", "QRCode", "QRCodeToken", "Row", "SeatId", "SeatNumber", "Section", "Status", "TicketNumber", "UsedAt")
VALUES (1, '2025-08-30 00:18:38.9626683', NULL, NULL, NULL, '2025-08-30 19:00:00', 1, 'Championship Match', 1, 0, '50.0', '2025-08-30 00:18:38.9626682', '', '4ee0dced-f9a1-46fd-b4a1-e073ebcf0cdf', '1', 1, 'A1', 'A', 'Active', 'TK001', NULL);
SELECT changes();

INSERT INTO "Tickets" ("Id", "CreatedAt", "CustomerEmail", "CustomerName", "CustomerPhone", "EventDate", "EventId", "EventName", "IsActive", "IsUsed", "Price", "PurchaseDate", "QRCode", "QRCodeToken", "Row", "SeatId", "SeatNumber", "Section", "Status", "TicketNumber", "UsedAt")
VALUES (2, '2025-08-30 00:18:38.9626771', NULL, NULL, NULL, '2025-08-30 19:00:00', 1, 'Championship Match', 1, 0, '50.0', '2025-08-30 00:18:38.962677', '', 'e464fe6d-b2c4-4e77-96c1-33707b52120b', '1', 2, 'A2', 'A', 'Active', 'TK002', NULL);
SELECT changes();

INSERT INTO "Tickets" ("Id", "CreatedAt", "CustomerEmail", "CustomerName", "CustomerPhone", "EventDate", "EventId", "EventName", "IsActive", "IsUsed", "Price", "PurchaseDate", "QRCode", "QRCodeToken", "Row", "SeatId", "SeatNumber", "Section", "Status", "TicketNumber", "UsedAt")
VALUES (3, '2025-08-30 00:18:38.9626779', NULL, NULL, NULL, '2025-08-30 19:00:00', 1, 'Championship Match', 1, 0, '60.0', '2025-08-30 00:18:38.9626779', '', 'c2838b7d-fc76-4c94-bd39-a2eeaf432f6b', '5', 3, 'B5', 'B', 'Active', 'TK003', NULL);
SELECT changes();

INSERT INTO "Tickets" ("Id", "CreatedAt", "CustomerEmail", "CustomerName", "CustomerPhone", "EventDate", "EventId", "EventName", "IsActive", "IsUsed", "Price", "PurchaseDate", "QRCode", "QRCodeToken", "Row", "SeatId", "SeatNumber", "Section", "Status", "TicketNumber", "UsedAt")
VALUES (4, '2025-08-30 00:18:38.9626786', NULL, NULL, NULL, '2025-08-30 19:00:00', 1, 'Championship Match', 1, 0, '75.0', '2025-08-30 00:18:38.9626785', '', 'e0c5a531-7e4b-4670-ad35-096bb82fb717', '10', 4, 'C10', 'C', 'Active', 'TK004', NULL);
SELECT changes();

INSERT INTO "Tickets" ("Id", "CreatedAt", "CustomerEmail", "CustomerName", "CustomerPhone", "EventDate", "EventId", "EventName", "IsActive", "IsUsed", "Price", "PurchaseDate", "QRCode", "QRCodeToken", "Row", "SeatId", "SeatNumber", "Section", "Status", "TicketNumber", "UsedAt")
VALUES (5, '2025-08-30 00:18:38.9626801', NULL, NULL, NULL, '2025-08-30 19:00:00', 1, 'Championship Match', 1, 0, '100.0', '2025-08-30 00:18:38.96268', '', 'e8e85cc3-7487-4b6c-8d06-80d17a570dd3', '15', 5, 'D15', 'D', 'Active', 'TK005', NULL);
SELECT changes();


CREATE UNIQUE INDEX "IX_EventAnalytics_EventId" ON "EventAnalytics" ("EventId");

CREATE INDEX "IX_Events_EventName" ON "Events" ("EventName");

CREATE UNIQUE INDEX "IX_EventStaffAssignments_EventId_StaffId" ON "EventStaffAssignments" ("EventId", "StaffId");

CREATE INDEX "IX_EventStaffAssignments_StaffId" ON "EventStaffAssignments" ("StaffId");

CREATE INDEX "IX_Notifications_CreatedAt" ON "Notifications" ("CreatedAt");

CREATE INDEX "IX_Notifications_EventId" ON "Notifications" ("EventId");

CREATE INDEX "IX_Notifications_UserId_IsRead" ON "Notifications" ("UserId", "IsRead");

CREATE INDEX "IX_OrderItems_DrinkId" ON "OrderItems" ("DrinkId");

CREATE INDEX "IX_OrderItems_OrderId" ON "OrderItems" ("OrderId");

CREATE INDEX "IX_Orders_AcceptedByUserId" ON "Orders" ("AcceptedByUserId");

CREATE INDEX "IX_Orders_AssignedStaffId" ON "Orders" ("AssignedStaffId");

CREATE INDEX "IX_Orders_CustomerId" ON "Orders" ("CustomerId");

CREATE INDEX "IX_Orders_DeliveredByUserId" ON "Orders" ("DeliveredByUserId");

CREATE INDEX "IX_Orders_EventId" ON "Orders" ("EventId");

CREATE UNIQUE INDEX "IX_Orders_PaymentId" ON "Orders" ("PaymentId");

CREATE INDEX "IX_Orders_PreparedByUserId" ON "Orders" ("PreparedByUserId");

CREATE INDEX "IX_Orders_SeatId" ON "Orders" ("SeatId");

CREATE INDEX "IX_Orders_SessionId" ON "Orders" ("SessionId");

CREATE INDEX "IX_Orders_StadiumSeatId" ON "Orders" ("StadiumSeatId");

CREATE INDEX "IX_Orders_TicketId" ON "Orders" ("TicketId");

CREATE INDEX "IX_Orders_TicketNumber" ON "Orders" ("TicketNumber");

CREATE UNIQUE INDEX "IX_OrderSessions_SessionToken" ON "OrderSessions" ("SessionToken");

CREATE INDEX "IX_OrderSessions_TicketId" ON "OrderSessions" ("TicketId");

CREATE UNIQUE INDEX "IX_Payments_OrderId" ON "Payments" ("OrderId");

CREATE UNIQUE INDEX "IX_Payments_TransactionId" ON "Payments" ("TransactionId");

CREATE UNIQUE INDEX "IX_Seats_SectionId_RowNumber_SeatNumber" ON "Seats" ("SectionId", "RowNumber", "SeatNumber");

CREATE UNIQUE INDEX "IX_StadiumSeats_Section_RowNumber_SeatNumber" ON "StadiumSeats" ("Section", "RowNumber", "SeatNumber");

CREATE UNIQUE INDEX "IX_StadiumSections_SectionName" ON "StadiumSections" ("SectionName");

CREATE INDEX "IX_Tickets_EventId" ON "Tickets" ("EventId");

CREATE UNIQUE INDEX "IX_Tickets_QRCodeToken" ON "Tickets" ("QRCodeToken");

CREATE INDEX "IX_Tickets_SeatId" ON "Tickets" ("SeatId");

CREATE UNIQUE INDEX "IX_Tickets_TicketNumber" ON "Tickets" ("TicketNumber");

CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");

CREATE UNIQUE INDEX "IX_Users_Username" ON "Users" ("Username");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250830001839_AddEventManagementSystemV2', '8.0.8');

COMMIT;

BEGIN TRANSACTION;

CREATE TABLE "Tribunes" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Tribunes" PRIMARY KEY AUTOINCREMENT,
    "Code" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "Description" TEXT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NOT NULL
);

CREATE TABLE "Rings" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Rings" PRIMARY KEY AUTOINCREMENT,
    "TribuneId" INTEGER NOT NULL,
    "Number" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NOT NULL,
    CONSTRAINT "FK_Rings_Tribunes_TribuneId" FOREIGN KEY ("TribuneId") REFERENCES "Tribunes" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Sectors" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Sectors" PRIMARY KEY AUTOINCREMENT,
    "RingId" INTEGER NOT NULL,
    "Code" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "TotalRows" INTEGER NOT NULL,
    "SeatsPerRow" INTEGER NOT NULL,
    "StartRow" INTEGER NOT NULL,
    "StartSeat" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NOT NULL,
    CONSTRAINT "FK_Sectors_Rings_RingId" FOREIGN KEY ("RingId") REFERENCES "Rings" ("Id") ON DELETE CASCADE
);

CREATE TABLE "StadiumSeatsNew" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_StadiumSeatsNew" PRIMARY KEY AUTOINCREMENT,
    "SectorId" INTEGER NOT NULL,
    "RowNumber" INTEGER NOT NULL,
    "SeatNumber" INTEGER NOT NULL,
    "UniqueCode" TEXT NOT NULL,
    "IsAvailable" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NOT NULL,
    CONSTRAINT "FK_StadiumSeatsNew_Sectors_SectorId" FOREIGN KEY ("SectorId") REFERENCES "Sectors" ("Id") ON DELETE CASCADE
);

UPDATE "Drinks" SET "CreatedAt" = '2025-08-31 20:53:22.3067089'
WHERE "Id" = 1;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-08-31 20:53:22.3067099'
WHERE "Id" = 2;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-08-31 20:53:22.30671'
WHERE "Id" = 3;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-08-31 20:53:22.3067102'
WHERE "Id" = 4;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-08-31 20:53:22.3067104'
WHERE "Id" = 5;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-08-31 20:53:22.3067105'
WHERE "Id" = 6;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-08-31 20:53:22.3067107'
WHERE "Id" = 7;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-08-31 20:53:22.3067108'
WHERE "Id" = 8;
SELECT changes();


UPDATE "Events" SET "CreatedAt" = '2025-08-31 20:53:22.3067232', "EventDate" = '2025-08-31 19:00:00'
WHERE "Id" = 1;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067666'
WHERE "Id" = 6;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067669'
WHERE "Id" = 7;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067673'
WHERE "Id" = 8;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067674'
WHERE "Id" = 9;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067675'
WHERE "Id" = 10;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067676'
WHERE "Id" = 11;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067677'
WHERE "Id" = 12;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067678'
WHERE "Id" = 13;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067679'
WHERE "Id" = 14;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067681'
WHERE "Id" = 15;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067682'
WHERE "Id" = 16;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067683'
WHERE "Id" = 17;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067684'
WHERE "Id" = 18;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067685'
WHERE "Id" = 19;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067686'
WHERE "Id" = 20;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067687'
WHERE "Id" = 21;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067687'
WHERE "Id" = 22;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067689'
WHERE "Id" = 23;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306769'
WHERE "Id" = 24;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067691'
WHERE "Id" = 25;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067692'
WHERE "Id" = 26;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067693'
WHERE "Id" = 27;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067694'
WHERE "Id" = 28;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067695'
WHERE "Id" = 29;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067696'
WHERE "Id" = 30;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067696'
WHERE "Id" = 31;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067697'
WHERE "Id" = 32;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067698'
WHERE "Id" = 33;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067699'
WHERE "Id" = 34;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.30677'
WHERE "Id" = 35;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067701'
WHERE "Id" = 36;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067702'
WHERE "Id" = 37;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067703'
WHERE "Id" = 38;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067704'
WHERE "Id" = 39;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067705'
WHERE "Id" = 40;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067706'
WHERE "Id" = 41;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067707'
WHERE "Id" = 42;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067707'
WHERE "Id" = 43;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067708'
WHERE "Id" = 44;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067709'
WHERE "Id" = 45;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306771'
WHERE "Id" = 46;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067711'
WHERE "Id" = 47;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067712'
WHERE "Id" = 48;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067713'
WHERE "Id" = 49;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067751'
WHERE "Id" = 50;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067752'
WHERE "Id" = 51;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067753'
WHERE "Id" = 52;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067754'
WHERE "Id" = 53;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067754'
WHERE "Id" = 54;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067755'
WHERE "Id" = 55;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067757'
WHERE "Id" = 56;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067758'
WHERE "Id" = 57;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067759'
WHERE "Id" = 58;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067759'
WHERE "Id" = 59;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306776'
WHERE "Id" = 60;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067761'
WHERE "Id" = 61;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067762'
WHERE "Id" = 62;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067763'
WHERE "Id" = 63;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067764'
WHERE "Id" = 64;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067764'
WHERE "Id" = 65;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067765'
WHERE "Id" = 66;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067766'
WHERE "Id" = 67;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067767'
WHERE "Id" = 68;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067768'
WHERE "Id" = 69;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067769'
WHERE "Id" = 70;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067771'
WHERE "Id" = 71;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067771'
WHERE "Id" = 72;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067772'
WHERE "Id" = 73;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067773'
WHERE "Id" = 74;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067774'
WHERE "Id" = 75;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067775'
WHERE "Id" = 76;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067776'
WHERE "Id" = 77;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067776'
WHERE "Id" = 78;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067777'
WHERE "Id" = 79;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067778'
WHERE "Id" = 80;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067779'
WHERE "Id" = 81;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306778'
WHERE "Id" = 82;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067781'
WHERE "Id" = 83;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067781'
WHERE "Id" = 84;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067782'
WHERE "Id" = 85;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067783'
WHERE "Id" = 86;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067784'
WHERE "Id" = 87;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067785'
WHERE "Id" = 88;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067786'
WHERE "Id" = 89;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067786'
WHERE "Id" = 90;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067787'
WHERE "Id" = 91;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067788'
WHERE "Id" = 92;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067789'
WHERE "Id" = 93;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306779'
WHERE "Id" = 94;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067791'
WHERE "Id" = 95;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067792'
WHERE "Id" = 96;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067792'
WHERE "Id" = 97;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067793'
WHERE "Id" = 98;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067794'
WHERE "Id" = 99;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067795'
WHERE "Id" = 100;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067796'
WHERE "Id" = 101;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067797'
WHERE "Id" = 102;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067797'
WHERE "Id" = 103;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067798'
WHERE "Id" = 104;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067799'
WHERE "Id" = 105;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067801'
WHERE "Id" = 106;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067802'
WHERE "Id" = 107;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067803'
WHERE "Id" = 108;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067804'
WHERE "Id" = 109;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067805'
WHERE "Id" = 110;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067806'
WHERE "Id" = 111;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067806'
WHERE "Id" = 112;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067807'
WHERE "Id" = 113;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067808'
WHERE "Id" = 114;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067809'
WHERE "Id" = 115;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306781'
WHERE "Id" = 116;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067811'
WHERE "Id" = 117;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067812'
WHERE "Id" = 118;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067813'
WHERE "Id" = 119;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067813'
WHERE "Id" = 120;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067814'
WHERE "Id" = 121;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067815'
WHERE "Id" = 122;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067816'
WHERE "Id" = 123;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067838'
WHERE "Id" = 124;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067839'
WHERE "Id" = 125;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306784'
WHERE "Id" = 126;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067841'
WHERE "Id" = 127;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067842'
WHERE "Id" = 128;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067843'
WHERE "Id" = 129;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067843'
WHERE "Id" = 130;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067844'
WHERE "Id" = 131;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067845'
WHERE "Id" = 132;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067846'
WHERE "Id" = 133;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067847'
WHERE "Id" = 134;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067849'
WHERE "Id" = 135;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306785'
WHERE "Id" = 136;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306785'
WHERE "Id" = 137;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067851'
WHERE "Id" = 138;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067852'
WHERE "Id" = 139;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067853'
WHERE "Id" = 140;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067854'
WHERE "Id" = 141;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067854'
WHERE "Id" = 142;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067855'
WHERE "Id" = 143;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067856'
WHERE "Id" = 144;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067857'
WHERE "Id" = 145;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067858'
WHERE "Id" = 146;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067859'
WHERE "Id" = 147;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306786'
WHERE "Id" = 148;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306786'
WHERE "Id" = 149;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067861'
WHERE "Id" = 150;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067862'
WHERE "Id" = 151;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067863'
WHERE "Id" = 152;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067864'
WHERE "Id" = 153;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067865'
WHERE "Id" = 154;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067865'
WHERE "Id" = 155;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067866'
WHERE "Id" = 156;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067867'
WHERE "Id" = 157;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067868'
WHERE "Id" = 158;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067869'
WHERE "Id" = 159;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306787'
WHERE "Id" = 160;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067871'
WHERE "Id" = 161;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067871'
WHERE "Id" = 162;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067872'
WHERE "Id" = 163;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067873'
WHERE "Id" = 164;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067874'
WHERE "Id" = 165;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067875'
WHERE "Id" = 166;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067876'
WHERE "Id" = 167;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067877'
WHERE "Id" = 168;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067877'
WHERE "Id" = 169;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067878'
WHERE "Id" = 170;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067879'
WHERE "Id" = 171;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306788'
WHERE "Id" = 172;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067881'
WHERE "Id" = 173;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067881'
WHERE "Id" = 174;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067882'
WHERE "Id" = 175;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067883'
WHERE "Id" = 176;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067884'
WHERE "Id" = 177;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067885'
WHERE "Id" = 178;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067886'
WHERE "Id" = 179;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067886'
WHERE "Id" = 180;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067887'
WHERE "Id" = 181;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067888'
WHERE "Id" = 182;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067889'
WHERE "Id" = 183;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306789'
WHERE "Id" = 184;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067967'
WHERE "Id" = 185;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067968'
WHERE "Id" = 186;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067969'
WHERE "Id" = 187;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306797'
WHERE "Id" = 188;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067971'
WHERE "Id" = 189;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067972'
WHERE "Id" = 190;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067972'
WHERE "Id" = 191;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067973'
WHERE "Id" = 192;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067974'
WHERE "Id" = 193;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067975'
WHERE "Id" = 194;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067975'
WHERE "Id" = 195;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067976'
WHERE "Id" = 196;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067977'
WHERE "Id" = 197;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067978'
WHERE "Id" = 198;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067979'
WHERE "Id" = 199;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306798'
WHERE "Id" = 200;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306798'
WHERE "Id" = 201;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067981'
WHERE "Id" = 202;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067982'
WHERE "Id" = 203;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067983'
WHERE "Id" = 204;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067984'
WHERE "Id" = 205;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067985'
WHERE "Id" = 206;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067987'
WHERE "Id" = 207;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067988'
WHERE "Id" = 208;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067989'
WHERE "Id" = 209;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067989'
WHERE "Id" = 210;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306799'
WHERE "Id" = 211;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067991'
WHERE "Id" = 212;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067992'
WHERE "Id" = 213;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067993'
WHERE "Id" = 214;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067993'
WHERE "Id" = 215;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067994'
WHERE "Id" = 216;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067995'
WHERE "Id" = 217;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067996'
WHERE "Id" = 218;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067997'
WHERE "Id" = 219;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067998'
WHERE "Id" = 220;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067999'
WHERE "Id" = 221;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3067999'
WHERE "Id" = 222;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068'
WHERE "Id" = 223;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068001'
WHERE "Id" = 224;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068002'
WHERE "Id" = 225;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068003'
WHERE "Id" = 226;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068003'
WHERE "Id" = 227;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068004'
WHERE "Id" = 228;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068005'
WHERE "Id" = 229;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068006'
WHERE "Id" = 230;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068007'
WHERE "Id" = 231;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068007'
WHERE "Id" = 232;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068008'
WHERE "Id" = 233;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068009'
WHERE "Id" = 234;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306801'
WHERE "Id" = 235;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068011'
WHERE "Id" = 236;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068012'
WHERE "Id" = 237;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068012'
WHERE "Id" = 238;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068013'
WHERE "Id" = 239;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068014'
WHERE "Id" = 240;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068015'
WHERE "Id" = 241;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068016'
WHERE "Id" = 242;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068016'
WHERE "Id" = 243;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068017'
WHERE "Id" = 244;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068018'
WHERE "Id" = 245;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068019'
WHERE "Id" = 246;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306802'
WHERE "Id" = 247;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306802'
WHERE "Id" = 248;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068021'
WHERE "Id" = 249;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068022'
WHERE "Id" = 250;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068023'
WHERE "Id" = 251;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068024'
WHERE "Id" = 252;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068025'
WHERE "Id" = 253;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068025'
WHERE "Id" = 254;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068026'
WHERE "Id" = 255;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068027'
WHERE "Id" = 256;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068028'
WHERE "Id" = 257;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068028'
WHERE "Id" = 258;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068029'
WHERE "Id" = 259;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306803'
WHERE "Id" = 260;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068031'
WHERE "Id" = 261;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068032'
WHERE "Id" = 262;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068084'
WHERE "Id" = 263;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068085'
WHERE "Id" = 264;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068086'
WHERE "Id" = 265;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068087'
WHERE "Id" = 266;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068087'
WHERE "Id" = 267;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068088'
WHERE "Id" = 268;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068089'
WHERE "Id" = 269;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306809'
WHERE "Id" = 270;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068091'
WHERE "Id" = 271;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068092'
WHERE "Id" = 272;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068092'
WHERE "Id" = 273;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068093'
WHERE "Id" = 274;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068094'
WHERE "Id" = 275;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068095'
WHERE "Id" = 276;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068096'
WHERE "Id" = 277;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068096'
WHERE "Id" = 278;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068097'
WHERE "Id" = 279;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068098'
WHERE "Id" = 280;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068099'
WHERE "Id" = 281;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.30681'
WHERE "Id" = 282;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.30681'
WHERE "Id" = 283;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068101'
WHERE "Id" = 284;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068102'
WHERE "Id" = 285;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068103'
WHERE "Id" = 286;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068104'
WHERE "Id" = 287;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068104'
WHERE "Id" = 288;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068105'
WHERE "Id" = 289;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068106'
WHERE "Id" = 290;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068107'
WHERE "Id" = 291;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068108'
WHERE "Id" = 292;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068109'
WHERE "Id" = 293;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068109'
WHERE "Id" = 294;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306811'
WHERE "Id" = 295;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068111'
WHERE "Id" = 296;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068112'
WHERE "Id" = 297;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068113'
WHERE "Id" = 298;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068113'
WHERE "Id" = 299;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068114'
WHERE "Id" = 300;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068115'
WHERE "Id" = 301;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068116'
WHERE "Id" = 302;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068117'
WHERE "Id" = 303;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068117'
WHERE "Id" = 304;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068118'
WHERE "Id" = 305;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306812'
WHERE "Id" = 306;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068121'
WHERE "Id" = 307;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068122'
WHERE "Id" = 308;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068123'
WHERE "Id" = 309;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068124'
WHERE "Id" = 310;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068125'
WHERE "Id" = 311;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068126'
WHERE "Id" = 312;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068161'
WHERE "Id" = 313;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068162'
WHERE "Id" = 314;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068163'
WHERE "Id" = 315;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068164'
WHERE "Id" = 316;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068165'
WHERE "Id" = 317;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068166'
WHERE "Id" = 318;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068166'
WHERE "Id" = 319;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068168'
WHERE "Id" = 320;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068168'
WHERE "Id" = 321;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068169'
WHERE "Id" = 322;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306817'
WHERE "Id" = 323;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068171'
WHERE "Id" = 324;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068172'
WHERE "Id" = 325;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068173'
WHERE "Id" = 326;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068174'
WHERE "Id" = 327;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068174'
WHERE "Id" = 328;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068175'
WHERE "Id" = 329;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068176'
WHERE "Id" = 330;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068177'
WHERE "Id" = 331;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068178'
WHERE "Id" = 332;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068178'
WHERE "Id" = 333;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068179'
WHERE "Id" = 334;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306818'
WHERE "Id" = 335;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068181'
WHERE "Id" = 336;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068182'
WHERE "Id" = 337;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068183'
WHERE "Id" = 338;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068184'
WHERE "Id" = 339;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068184'
WHERE "Id" = 340;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068185'
WHERE "Id" = 341;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068186'
WHERE "Id" = 342;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068187'
WHERE "Id" = 343;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068188'
WHERE "Id" = 344;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068188'
WHERE "Id" = 345;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068189'
WHERE "Id" = 346;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306819'
WHERE "Id" = 347;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068191'
WHERE "Id" = 348;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068192'
WHERE "Id" = 349;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068192'
WHERE "Id" = 350;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068193'
WHERE "Id" = 351;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068194'
WHERE "Id" = 352;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068195'
WHERE "Id" = 353;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068196'
WHERE "Id" = 354;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068196'
WHERE "Id" = 355;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068197'
WHERE "Id" = 356;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068198'
WHERE "Id" = 357;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068199'
WHERE "Id" = 358;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068199'
WHERE "Id" = 359;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.30682'
WHERE "Id" = 360;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068201'
WHERE "Id" = 361;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068203'
WHERE "Id" = 362;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068203'
WHERE "Id" = 363;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068204'
WHERE "Id" = 364;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068205'
WHERE "Id" = 365;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068206'
WHERE "Id" = 366;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068207'
WHERE "Id" = 367;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068207'
WHERE "Id" = 368;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068208'
WHERE "Id" = 369;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068209'
WHERE "Id" = 370;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306821'
WHERE "Id" = 371;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068211'
WHERE "Id" = 372;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068211'
WHERE "Id" = 373;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068212'
WHERE "Id" = 374;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068213'
WHERE "Id" = 375;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068214'
WHERE "Id" = 376;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068215'
WHERE "Id" = 377;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068215'
WHERE "Id" = 378;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068216'
WHERE "Id" = 379;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068217'
WHERE "Id" = 380;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068218'
WHERE "Id" = 381;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068219'
WHERE "Id" = 382;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068219'
WHERE "Id" = 383;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306822'
WHERE "Id" = 384;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068221'
WHERE "Id" = 385;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068222'
WHERE "Id" = 386;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068222'
WHERE "Id" = 387;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068223'
WHERE "Id" = 388;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068224'
WHERE "Id" = 389;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068225'
WHERE "Id" = 390;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068226'
WHERE "Id" = 391;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068227'
WHERE "Id" = 392;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068228'
WHERE "Id" = 393;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068228'
WHERE "Id" = 394;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068229'
WHERE "Id" = 395;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306823'
WHERE "Id" = 396;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068231'
WHERE "Id" = 397;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068232'
WHERE "Id" = 398;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.306827'
WHERE "Id" = 399;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068271'
WHERE "Id" = 400;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068272'
WHERE "Id" = 401;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068272'
WHERE "Id" = 402;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-08-31 20:53:22.3068273'
WHERE "Id" = 403;
SELECT changes();


UPDATE "Tickets" SET "CreatedAt" = '2025-08-31 20:53:22.3067514', "EventDate" = '2025-08-31 19:00:00', "PurchaseDate" = '2025-08-31 20:53:22.3067513', "QRCodeToken" = 'd1818e9d-0bb6-4bb2-86f5-781ac6041331'
WHERE "Id" = 1;
SELECT changes();


UPDATE "Tickets" SET "CreatedAt" = '2025-08-31 20:53:22.3067593', "EventDate" = '2025-08-31 19:00:00', "PurchaseDate" = '2025-08-31 20:53:22.3067593', "QRCodeToken" = '012b1ad1-9ec1-4cf7-a31b-28a2fbded507'
WHERE "Id" = 2;
SELECT changes();


UPDATE "Tickets" SET "CreatedAt" = '2025-08-31 20:53:22.3067602', "EventDate" = '2025-08-31 19:00:00', "PurchaseDate" = '2025-08-31 20:53:22.3067601', "QRCodeToken" = '97f119a8-a72b-4f2d-a42b-8787e70b8c93'
WHERE "Id" = 3;
SELECT changes();


UPDATE "Tickets" SET "CreatedAt" = '2025-08-31 20:53:22.3067609', "EventDate" = '2025-08-31 19:00:00', "PurchaseDate" = '2025-08-31 20:53:22.3067609', "QRCodeToken" = '897e96fb-7db2-48e8-853d-e4af69be7b11'
WHERE "Id" = 4;
SELECT changes();


UPDATE "Tickets" SET "CreatedAt" = '2025-08-31 20:53:22.3067636', "EventDate" = '2025-08-31 19:00:00', "PurchaseDate" = '2025-08-31 20:53:22.3067636', "QRCodeToken" = 'f732060d-1d99-4928-b83d-72339b7b5706'
WHERE "Id" = 5;
SELECT changes();


UPDATE "Users" SET "CreatedAt" = '2025-08-31 20:53:22.3066314', "PasswordHash" = '$2a$11$LiSYqQQSarXcQPQr7Pa.oe3ErP0TGNiv0qX7jPpZ/0ZyErYb8.Z4S'
WHERE "Id" = 1;
SELECT changes();


CREATE UNIQUE INDEX "IX_Rings_TribuneId_Number" ON "Rings" ("TribuneId", "Number");

CREATE UNIQUE INDEX "IX_Sectors_RingId_Code" ON "Sectors" ("RingId", "Code");

CREATE UNIQUE INDEX "IX_StadiumSeatsNew_SectorId_RowNumber_SeatNumber" ON "StadiumSeatsNew" ("SectorId", "RowNumber", "SeatNumber");

CREATE UNIQUE INDEX "IX_StadiumSeatsNew_UniqueCode" ON "StadiumSeatsNew" ("UniqueCode");

CREATE UNIQUE INDEX "IX_Tribunes_Code" ON "Tribunes" ("Code");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250831205324_StadiumStructureEntities', '8.0.8');

COMMIT;

BEGIN TRANSACTION;

CREATE TABLE "LogEntries" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_LogEntries" PRIMARY KEY AUTOINCREMENT,
    "Timestamp" TEXT NOT NULL,
    "Level" TEXT NOT NULL,
    "Category" TEXT NOT NULL,
    "Action" TEXT NOT NULL,
    "UserId" TEXT NULL,
    "UserEmail" TEXT NULL,
    "UserRole" TEXT NULL,
    "IPAddress" TEXT NULL,
    "UserAgent" TEXT NULL,
    "RequestPath" TEXT NULL,
    "HttpMethod" TEXT NULL,
    "Message" TEXT NULL,
    "Details" TEXT NULL,
    "ExceptionType" TEXT NULL,
    "StackTrace" TEXT NULL,
    "Source" TEXT NOT NULL
);

UPDATE "Drinks" SET "CreatedAt" = '2025-09-01 07:35:21.2708006'
WHERE "Id" = 1;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-09-01 07:35:21.2708035'
WHERE "Id" = 2;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-09-01 07:35:21.2708037'
WHERE "Id" = 3;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-09-01 07:35:21.2708038'
WHERE "Id" = 4;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-09-01 07:35:21.270804'
WHERE "Id" = 5;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-09-01 07:35:21.2708041'
WHERE "Id" = 6;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-09-01 07:35:21.2708043'
WHERE "Id" = 7;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-09-01 07:35:21.2708044'
WHERE "Id" = 8;
SELECT changes();


UPDATE "Events" SET "CreatedAt" = '2025-09-01 07:35:21.2708301', "EventDate" = '2025-09-01 19:00:00'
WHERE "Id" = 1;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708671'
WHERE "Id" = 6;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708684'
WHERE "Id" = 7;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708688'
WHERE "Id" = 8;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708689'
WHERE "Id" = 9;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270869'
WHERE "Id" = 10;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708692'
WHERE "Id" = 11;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708695'
WHERE "Id" = 12;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708696'
WHERE "Id" = 13;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708697'
WHERE "Id" = 14;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708698'
WHERE "Id" = 15;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.27087'
WHERE "Id" = 16;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.27087'
WHERE "Id" = 17;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708701'
WHERE "Id" = 18;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708702'
WHERE "Id" = 19;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708703'
WHERE "Id" = 20;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708704'
WHERE "Id" = 21;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708705'
WHERE "Id" = 22;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708706'
WHERE "Id" = 23;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708707'
WHERE "Id" = 24;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708708'
WHERE "Id" = 25;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708709'
WHERE "Id" = 26;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270871'
WHERE "Id" = 27;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708711'
WHERE "Id" = 28;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708712'
WHERE "Id" = 29;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708713'
WHERE "Id" = 30;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708713'
WHERE "Id" = 31;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708714'
WHERE "Id" = 32;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708715'
WHERE "Id" = 33;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708716'
WHERE "Id" = 34;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708717'
WHERE "Id" = 35;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708718'
WHERE "Id" = 36;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708718'
WHERE "Id" = 37;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708719'
WHERE "Id" = 38;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708763'
WHERE "Id" = 39;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708764'
WHERE "Id" = 40;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708764'
WHERE "Id" = 41;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708765'
WHERE "Id" = 42;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708766'
WHERE "Id" = 43;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708767'
WHERE "Id" = 44;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708768'
WHERE "Id" = 45;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708769'
WHERE "Id" = 46;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270877'
WHERE "Id" = 47;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270877'
WHERE "Id" = 48;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708771'
WHERE "Id" = 49;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708772'
WHERE "Id" = 50;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708773'
WHERE "Id" = 51;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708774'
WHERE "Id" = 52;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708775'
WHERE "Id" = 53;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708776'
WHERE "Id" = 54;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708776'
WHERE "Id" = 55;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708778'
WHERE "Id" = 56;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708778'
WHERE "Id" = 57;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708779'
WHERE "Id" = 58;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270878'
WHERE "Id" = 59;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708781'
WHERE "Id" = 60;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708782'
WHERE "Id" = 61;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708783'
WHERE "Id" = 62;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708783'
WHERE "Id" = 63;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708784'
WHERE "Id" = 64;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708785'
WHERE "Id" = 65;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708786'
WHERE "Id" = 66;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708787'
WHERE "Id" = 67;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708788'
WHERE "Id" = 68;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708788'
WHERE "Id" = 69;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708789'
WHERE "Id" = 70;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708791'
WHERE "Id" = 71;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708792'
WHERE "Id" = 72;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708793'
WHERE "Id" = 73;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708794'
WHERE "Id" = 74;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708795'
WHERE "Id" = 75;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708796'
WHERE "Id" = 76;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708796'
WHERE "Id" = 77;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708797'
WHERE "Id" = 78;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708798'
WHERE "Id" = 79;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708799'
WHERE "Id" = 80;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.27088'
WHERE "Id" = 81;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708801'
WHERE "Id" = 82;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708801'
WHERE "Id" = 83;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708802'
WHERE "Id" = 84;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708803'
WHERE "Id" = 85;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708804'
WHERE "Id" = 86;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708805'
WHERE "Id" = 87;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708806'
WHERE "Id" = 88;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708807'
WHERE "Id" = 89;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708807'
WHERE "Id" = 90;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708808'
WHERE "Id" = 91;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708809'
WHERE "Id" = 92;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270881'
WHERE "Id" = 93;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708811'
WHERE "Id" = 94;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708812'
WHERE "Id" = 95;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708813'
WHERE "Id" = 96;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708814'
WHERE "Id" = 97;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708814'
WHERE "Id" = 98;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708815'
WHERE "Id" = 99;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708816'
WHERE "Id" = 100;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708817'
WHERE "Id" = 101;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708818'
WHERE "Id" = 102;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708819'
WHERE "Id" = 103;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708819'
WHERE "Id" = 104;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270882'
WHERE "Id" = 105;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708822'
WHERE "Id" = 106;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708824'
WHERE "Id" = 107;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708824'
WHERE "Id" = 108;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708857'
WHERE "Id" = 109;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708859'
WHERE "Id" = 110;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270886'
WHERE "Id" = 111;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708861'
WHERE "Id" = 112;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708861'
WHERE "Id" = 113;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708862'
WHERE "Id" = 114;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708863'
WHERE "Id" = 115;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708864'
WHERE "Id" = 116;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708865'
WHERE "Id" = 117;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708866'
WHERE "Id" = 118;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708867'
WHERE "Id" = 119;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708868'
WHERE "Id" = 120;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708868'
WHERE "Id" = 121;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708869'
WHERE "Id" = 122;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270887'
WHERE "Id" = 123;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708871'
WHERE "Id" = 124;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708872'
WHERE "Id" = 125;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708873'
WHERE "Id" = 126;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708874'
WHERE "Id" = 127;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708874'
WHERE "Id" = 128;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708875'
WHERE "Id" = 129;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708876'
WHERE "Id" = 130;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708877'
WHERE "Id" = 131;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708878'
WHERE "Id" = 132;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708879'
WHERE "Id" = 133;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708879'
WHERE "Id" = 134;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708881'
WHERE "Id" = 135;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708882'
WHERE "Id" = 136;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708883'
WHERE "Id" = 137;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708884'
WHERE "Id" = 138;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708885'
WHERE "Id" = 139;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708886'
WHERE "Id" = 140;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708886'
WHERE "Id" = 141;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708887'
WHERE "Id" = 142;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708888'
WHERE "Id" = 143;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708889'
WHERE "Id" = 144;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270889'
WHERE "Id" = 145;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708891'
WHERE "Id" = 146;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708892'
WHERE "Id" = 147;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708892'
WHERE "Id" = 148;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708893'
WHERE "Id" = 149;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708894'
WHERE "Id" = 150;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708895'
WHERE "Id" = 151;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708895'
WHERE "Id" = 152;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708896'
WHERE "Id" = 153;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708897'
WHERE "Id" = 154;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708898'
WHERE "Id" = 155;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708899'
WHERE "Id" = 156;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708899'
WHERE "Id" = 157;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.27089'
WHERE "Id" = 158;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708901'
WHERE "Id" = 159;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708902'
WHERE "Id" = 160;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708903'
WHERE "Id" = 161;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708903'
WHERE "Id" = 162;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708904'
WHERE "Id" = 163;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708905'
WHERE "Id" = 164;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708906'
WHERE "Id" = 165;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708907'
WHERE "Id" = 166;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708908'
WHERE "Id" = 167;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708909'
WHERE "Id" = 168;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708909'
WHERE "Id" = 169;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270891'
WHERE "Id" = 170;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708911'
WHERE "Id" = 171;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708912'
WHERE "Id" = 172;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708944'
WHERE "Id" = 173;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708945'
WHERE "Id" = 174;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708946'
WHERE "Id" = 175;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708947'
WHERE "Id" = 176;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708948'
WHERE "Id" = 177;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708948'
WHERE "Id" = 178;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708949'
WHERE "Id" = 179;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270895'
WHERE "Id" = 180;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708951'
WHERE "Id" = 181;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708952'
WHERE "Id" = 182;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708952'
WHERE "Id" = 183;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708954'
WHERE "Id" = 184;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708954'
WHERE "Id" = 185;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708955'
WHERE "Id" = 186;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708956'
WHERE "Id" = 187;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708957'
WHERE "Id" = 188;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708958'
WHERE "Id" = 189;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708958'
WHERE "Id" = 190;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708959'
WHERE "Id" = 191;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270896'
WHERE "Id" = 192;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708961'
WHERE "Id" = 193;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708962'
WHERE "Id" = 194;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708963'
WHERE "Id" = 195;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708964'
WHERE "Id" = 196;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708965'
WHERE "Id" = 197;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708966'
WHERE "Id" = 198;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708966'
WHERE "Id" = 199;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708967'
WHERE "Id" = 200;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708968'
WHERE "Id" = 201;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708969'
WHERE "Id" = 202;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270897'
WHERE "Id" = 203;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270897'
WHERE "Id" = 204;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708971'
WHERE "Id" = 205;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708973'
WHERE "Id" = 206;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708974'
WHERE "Id" = 207;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708975'
WHERE "Id" = 208;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708976'
WHERE "Id" = 209;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708976'
WHERE "Id" = 210;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708977'
WHERE "Id" = 211;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708978'
WHERE "Id" = 212;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708979'
WHERE "Id" = 213;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270898'
WHERE "Id" = 214;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270898'
WHERE "Id" = 215;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708981'
WHERE "Id" = 216;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708982'
WHERE "Id" = 217;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708983'
WHERE "Id" = 218;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708984'
WHERE "Id" = 219;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708985'
WHERE "Id" = 220;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708986'
WHERE "Id" = 221;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708986'
WHERE "Id" = 222;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708987'
WHERE "Id" = 223;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708988'
WHERE "Id" = 224;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708989'
WHERE "Id" = 225;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270899'
WHERE "Id" = 226;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270899'
WHERE "Id" = 227;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708991'
WHERE "Id" = 228;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708992'
WHERE "Id" = 229;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708993'
WHERE "Id" = 230;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708994'
WHERE "Id" = 231;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708994'
WHERE "Id" = 232;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708995'
WHERE "Id" = 233;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708996'
WHERE "Id" = 234;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708997'
WHERE "Id" = 235;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708998'
WHERE "Id" = 236;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708998'
WHERE "Id" = 237;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2708999'
WHERE "Id" = 238;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709'
WHERE "Id" = 239;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709001'
WHERE "Id" = 240;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709002'
WHERE "Id" = 241;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709003'
WHERE "Id" = 242;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709003'
WHERE "Id" = 243;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709004'
WHERE "Id" = 244;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709005'
WHERE "Id" = 245;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709006'
WHERE "Id" = 246;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709007'
WHERE "Id" = 247;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709008'
WHERE "Id" = 248;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709008'
WHERE "Id" = 249;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709009'
WHERE "Id" = 250;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270901'
WHERE "Id" = 251;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709011'
WHERE "Id" = 252;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709011'
WHERE "Id" = 253;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709012'
WHERE "Id" = 254;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709013'
WHERE "Id" = 255;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709014'
WHERE "Id" = 256;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709015'
WHERE "Id" = 257;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709016'
WHERE "Id" = 258;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270905'
WHERE "Id" = 259;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709051'
WHERE "Id" = 260;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709052'
WHERE "Id" = 261;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709053'
WHERE "Id" = 262;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709055'
WHERE "Id" = 263;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709056'
WHERE "Id" = 264;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709057'
WHERE "Id" = 265;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709058'
WHERE "Id" = 266;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709059'
WHERE "Id" = 267;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709059'
WHERE "Id" = 268;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270906'
WHERE "Id" = 269;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709061'
WHERE "Id" = 270;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709062'
WHERE "Id" = 271;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709063'
WHERE "Id" = 272;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709063'
WHERE "Id" = 273;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709064'
WHERE "Id" = 274;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709065'
WHERE "Id" = 275;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709066'
WHERE "Id" = 276;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709067'
WHERE "Id" = 277;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709068'
WHERE "Id" = 278;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709068'
WHERE "Id" = 279;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709069'
WHERE "Id" = 280;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270907'
WHERE "Id" = 281;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709071'
WHERE "Id" = 282;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709072'
WHERE "Id" = 283;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709072'
WHERE "Id" = 284;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709073'
WHERE "Id" = 285;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709074'
WHERE "Id" = 286;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709075'
WHERE "Id" = 287;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709076'
WHERE "Id" = 288;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709077'
WHERE "Id" = 289;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709077'
WHERE "Id" = 290;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709078'
WHERE "Id" = 291;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709079'
WHERE "Id" = 292;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270908'
WHERE "Id" = 293;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270908'
WHERE "Id" = 294;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709081'
WHERE "Id" = 295;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709082'
WHERE "Id" = 296;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709083'
WHERE "Id" = 297;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709084'
WHERE "Id" = 298;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709085'
WHERE "Id" = 299;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709086'
WHERE "Id" = 300;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709115'
WHERE "Id" = 301;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709117'
WHERE "Id" = 302;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709117'
WHERE "Id" = 303;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709118'
WHERE "Id" = 304;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709119'
WHERE "Id" = 305;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709121'
WHERE "Id" = 306;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709122'
WHERE "Id" = 307;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709123'
WHERE "Id" = 308;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709124'
WHERE "Id" = 309;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709124'
WHERE "Id" = 310;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709125'
WHERE "Id" = 311;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709126'
WHERE "Id" = 312;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709127'
WHERE "Id" = 313;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709127'
WHERE "Id" = 314;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709128'
WHERE "Id" = 315;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709129'
WHERE "Id" = 316;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270913'
WHERE "Id" = 317;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709131'
WHERE "Id" = 318;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709132'
WHERE "Id" = 319;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709133'
WHERE "Id" = 320;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709133'
WHERE "Id" = 321;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709134'
WHERE "Id" = 322;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709135'
WHERE "Id" = 323;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709136'
WHERE "Id" = 324;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709137'
WHERE "Id" = 325;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709137'
WHERE "Id" = 326;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709138'
WHERE "Id" = 327;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709139'
WHERE "Id" = 328;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270914'
WHERE "Id" = 329;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709141'
WHERE "Id" = 330;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709141'
WHERE "Id" = 331;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709142'
WHERE "Id" = 332;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709143'
WHERE "Id" = 333;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709144'
WHERE "Id" = 334;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709145'
WHERE "Id" = 335;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709146'
WHERE "Id" = 336;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709147'
WHERE "Id" = 337;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709147'
WHERE "Id" = 338;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709148'
WHERE "Id" = 339;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709149'
WHERE "Id" = 340;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270915'
WHERE "Id" = 341;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709151'
WHERE "Id" = 342;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709152'
WHERE "Id" = 343;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709153'
WHERE "Id" = 344;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709153'
WHERE "Id" = 345;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709154'
WHERE "Id" = 346;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709155'
WHERE "Id" = 347;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709156'
WHERE "Id" = 348;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709157'
WHERE "Id" = 349;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709158'
WHERE "Id" = 350;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709159'
WHERE "Id" = 351;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709159'
WHERE "Id" = 352;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270916'
WHERE "Id" = 353;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709161'
WHERE "Id" = 354;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709162'
WHERE "Id" = 355;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709163'
WHERE "Id" = 356;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709163'
WHERE "Id" = 357;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709164'
WHERE "Id" = 358;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709165'
WHERE "Id" = 359;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709166'
WHERE "Id" = 360;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709167'
WHERE "Id" = 361;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709168'
WHERE "Id" = 362;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709168'
WHERE "Id" = 363;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709169'
WHERE "Id" = 364;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270917'
WHERE "Id" = 365;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709171'
WHERE "Id" = 366;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709172'
WHERE "Id" = 367;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709173'
WHERE "Id" = 368;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709173'
WHERE "Id" = 369;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709174'
WHERE "Id" = 370;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709175'
WHERE "Id" = 371;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709176'
WHERE "Id" = 372;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709177'
WHERE "Id" = 373;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709177'
WHERE "Id" = 374;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709178'
WHERE "Id" = 375;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709179'
WHERE "Id" = 376;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270918'
WHERE "Id" = 377;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709181'
WHERE "Id" = 378;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709182'
WHERE "Id" = 379;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709182'
WHERE "Id" = 380;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709183'
WHERE "Id" = 381;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709184'
WHERE "Id" = 382;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709185'
WHERE "Id" = 383;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709186'
WHERE "Id" = 384;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709186'
WHERE "Id" = 385;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709187'
WHERE "Id" = 386;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709214'
WHERE "Id" = 387;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709214'
WHERE "Id" = 388;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709215'
WHERE "Id" = 389;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709216'
WHERE "Id" = 390;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709217'
WHERE "Id" = 391;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709218'
WHERE "Id" = 392;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709218'
WHERE "Id" = 393;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709219'
WHERE "Id" = 394;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.270922'
WHERE "Id" = 395;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709221'
WHERE "Id" = 396;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709222'
WHERE "Id" = 397;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709222'
WHERE "Id" = 398;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709223'
WHERE "Id" = 399;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709224'
WHERE "Id" = 400;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709225'
WHERE "Id" = 401;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709225'
WHERE "Id" = 402;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-01 07:35:21.2709226'
WHERE "Id" = 403;
SELECT changes();


UPDATE "Tickets" SET "CreatedAt" = '2025-09-01 07:35:21.2708516', "EventDate" = '2025-09-01 19:00:00', "PurchaseDate" = '2025-09-01 07:35:21.2708515', "QRCodeToken" = '46e087a1-5cc4-454c-8d83-1fcc910f86b4'
WHERE "Id" = 1;
SELECT changes();


UPDATE "Tickets" SET "CreatedAt" = '2025-09-01 07:35:21.2708597', "EventDate" = '2025-09-01 19:00:00', "PurchaseDate" = '2025-09-01 07:35:21.2708596', "QRCodeToken" = '40f82cfb-5275-4965-860a-3e289e269741'
WHERE "Id" = 2;
SELECT changes();


UPDATE "Tickets" SET "CreatedAt" = '2025-09-01 07:35:21.2708606', "EventDate" = '2025-09-01 19:00:00', "PurchaseDate" = '2025-09-01 07:35:21.2708605', "QRCodeToken" = '7c4040b9-1cbc-42a7-a1fb-d9b5a1c7899c'
WHERE "Id" = 3;
SELECT changes();


UPDATE "Tickets" SET "CreatedAt" = '2025-09-01 07:35:21.2708618', "EventDate" = '2025-09-01 19:00:00', "PurchaseDate" = '2025-09-01 07:35:21.2708617', "QRCodeToken" = '9fd13088-8c53-4ea8-961f-b6c5c7adbf80'
WHERE "Id" = 4;
SELECT changes();


UPDATE "Tickets" SET "CreatedAt" = '2025-09-01 07:35:21.2708629', "EventDate" = '2025-09-01 19:00:00', "PurchaseDate" = '2025-09-01 07:35:21.2708628', "QRCodeToken" = '0fd8fb68-e6e4-4560-a188-eaf0d6595f9f'
WHERE "Id" = 5;
SELECT changes();


UPDATE "Users" SET "CreatedAt" = '2025-09-01 07:35:21.2707088', "PasswordHash" = '$2a$11$LKadlQGPjk6fR2qqzJ8MlO0cJ8pW2QJfxBTY7n7ON5wK/09Wm60MS'
WHERE "Id" = 1;
SELECT changes();


CREATE INDEX "IX_LogEntries_Level_Category" ON "LogEntries" ("Level", "Category");

CREATE INDEX "IX_LogEntries_Source" ON "LogEntries" ("Source");

CREATE INDEX "IX_LogEntries_Timestamp" ON "LogEntries" ("Timestamp");

CREATE INDEX "IX_LogEntries_UserId" ON "LogEntries" ("UserId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250901073523_AddLoggingSystem', '8.0.8');

COMMIT;

BEGIN TRANSACTION;

CREATE TABLE "SeatReservations" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_SeatReservations" PRIMARY KEY AUTOINCREMENT,
    "EventId" INTEGER NOT NULL,
    "SectorId" INTEGER NOT NULL,
    "RowNumber" INTEGER NOT NULL,
    "SeatNumber" INTEGER NOT NULL,
    "SeatCode" TEXT NOT NULL,
    "SessionId" TEXT NOT NULL,
    "UserId" INTEGER NULL,
    "ReservedAt" TEXT NOT NULL,
    "ReservedUntil" TEXT NOT NULL,
    "Status" INTEGER NOT NULL,
    CONSTRAINT "FK_SeatReservations_Events_EventId" FOREIGN KEY ("EventId") REFERENCES "Events" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_SeatReservations_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE SET NULL
);

CREATE TABLE "ShoppingCarts" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_ShoppingCarts" PRIMARY KEY AUTOINCREMENT,
    "SessionId" TEXT NOT NULL,
    "UserId" INTEGER NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NOT NULL,
    "ExpiresAt" TEXT NOT NULL,
    CONSTRAINT "FK_ShoppingCarts_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE SET NULL
);

CREATE TABLE "CartItems" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_CartItems" PRIMARY KEY AUTOINCREMENT,
    "ShoppingCartId" INTEGER NOT NULL,
    "EventId" INTEGER NOT NULL,
    "SectorId" INTEGER NOT NULL,
    "RowNumber" INTEGER NOT NULL,
    "SeatNumber" INTEGER NOT NULL,
    "SeatCode" TEXT NOT NULL,
    "Price" TEXT NOT NULL,
    "AddedAt" TEXT NOT NULL,
    "ReservedUntil" TEXT NOT NULL,
    CONSTRAINT "FK_CartItems_Events_EventId" FOREIGN KEY ("EventId") REFERENCES "Events" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_CartItems_ShoppingCarts_ShoppingCartId" FOREIGN KEY ("ShoppingCartId") REFERENCES "ShoppingCarts" ("Id") ON DELETE CASCADE
);

UPDATE "Drinks" SET "CreatedAt" = '2025-09-02 19:50:36.6254683'
WHERE "Id" = 1;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-09-02 19:50:36.6254736'
WHERE "Id" = 2;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-09-02 19:50:36.6254738'
WHERE "Id" = 3;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-09-02 19:50:36.625474'
WHERE "Id" = 4;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-09-02 19:50:36.6254742'
WHERE "Id" = 5;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-09-02 19:50:36.6254743'
WHERE "Id" = 6;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-09-02 19:50:36.6254745'
WHERE "Id" = 7;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-09-02 19:50:36.6254748'
WHERE "Id" = 8;
SELECT changes();


UPDATE "Events" SET "CreatedAt" = '2025-09-02 19:50:36.6254966', "EventDate" = '2025-09-02 19:00:00'
WHERE "Id" = 1;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255523'
WHERE "Id" = 6;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255547'
WHERE "Id" = 7;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255549'
WHERE "Id" = 8;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625555'
WHERE "Id" = 9;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255551'
WHERE "Id" = 10;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255553'
WHERE "Id" = 11;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255554'
WHERE "Id" = 12;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255555'
WHERE "Id" = 13;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255556'
WHERE "Id" = 14;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255558'
WHERE "Id" = 15;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255559'
WHERE "Id" = 16;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625556'
WHERE "Id" = 17;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255561'
WHERE "Id" = 18;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255562'
WHERE "Id" = 19;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255563'
WHERE "Id" = 20;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255564'
WHERE "Id" = 21;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255565'
WHERE "Id" = 22;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255567'
WHERE "Id" = 23;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255568'
WHERE "Id" = 24;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255569'
WHERE "Id" = 25;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625557'
WHERE "Id" = 26;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255571'
WHERE "Id" = 27;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255572'
WHERE "Id" = 28;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255573'
WHERE "Id" = 29;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255574'
WHERE "Id" = 30;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255575'
WHERE "Id" = 31;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255576'
WHERE "Id" = 32;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255577'
WHERE "Id" = 33;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255577'
WHERE "Id" = 34;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255578'
WHERE "Id" = 35;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255579'
WHERE "Id" = 36;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255581'
WHERE "Id" = 37;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255581'
WHERE "Id" = 38;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255583'
WHERE "Id" = 39;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255584'
WHERE "Id" = 40;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255585'
WHERE "Id" = 41;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255586'
WHERE "Id" = 42;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255587'
WHERE "Id" = 43;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255588'
WHERE "Id" = 44;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255589'
WHERE "Id" = 45;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625559'
WHERE "Id" = 46;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255591'
WHERE "Id" = 47;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255638'
WHERE "Id" = 48;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255639'
WHERE "Id" = 49;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625564'
WHERE "Id" = 50;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255641'
WHERE "Id" = 51;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255642'
WHERE "Id" = 52;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255643'
WHERE "Id" = 53;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255644'
WHERE "Id" = 54;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255645'
WHERE "Id" = 55;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255646'
WHERE "Id" = 56;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255647'
WHERE "Id" = 57;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255648'
WHERE "Id" = 58;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255649'
WHERE "Id" = 59;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625565'
WHERE "Id" = 60;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625565'
WHERE "Id" = 61;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255651'
WHERE "Id" = 62;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255652'
WHERE "Id" = 63;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255653'
WHERE "Id" = 64;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255654'
WHERE "Id" = 65;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255655'
WHERE "Id" = 66;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255656'
WHERE "Id" = 67;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255657'
WHERE "Id" = 68;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255658'
WHERE "Id" = 69;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255659'
WHERE "Id" = 70;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255661'
WHERE "Id" = 71;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255661'
WHERE "Id" = 72;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255662'
WHERE "Id" = 73;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255663'
WHERE "Id" = 74;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255664'
WHERE "Id" = 75;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255665'
WHERE "Id" = 76;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255666'
WHERE "Id" = 77;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255667'
WHERE "Id" = 78;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255668'
WHERE "Id" = 79;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255669'
WHERE "Id" = 80;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625567'
WHERE "Id" = 81;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255671'
WHERE "Id" = 82;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255672'
WHERE "Id" = 83;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255673'
WHERE "Id" = 84;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255673'
WHERE "Id" = 85;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255675'
WHERE "Id" = 86;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255675'
WHERE "Id" = 87;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255676'
WHERE "Id" = 88;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255677'
WHERE "Id" = 89;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255678'
WHERE "Id" = 90;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625568'
WHERE "Id" = 91;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625568'
WHERE "Id" = 92;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255681'
WHERE "Id" = 93;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255682'
WHERE "Id" = 94;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255683'
WHERE "Id" = 95;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255685'
WHERE "Id" = 96;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255686'
WHERE "Id" = 97;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255686'
WHERE "Id" = 98;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255687'
WHERE "Id" = 99;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255688'
WHERE "Id" = 100;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255689'
WHERE "Id" = 101;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625569'
WHERE "Id" = 102;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255691'
WHERE "Id" = 103;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255692'
WHERE "Id" = 104;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255693'
WHERE "Id" = 105;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255694'
WHERE "Id" = 106;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255696'
WHERE "Id" = 107;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255697'
WHERE "Id" = 108;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255698'
WHERE "Id" = 109;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255699'
WHERE "Id" = 110;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.62557'
WHERE "Id" = 111;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255701'
WHERE "Id" = 112;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255701'
WHERE "Id" = 113;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255702'
WHERE "Id" = 114;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255703'
WHERE "Id" = 115;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255704'
WHERE "Id" = 116;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255705'
WHERE "Id" = 117;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255706'
WHERE "Id" = 118;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255707'
WHERE "Id" = 119;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255708'
WHERE "Id" = 120;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255709'
WHERE "Id" = 121;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625571'
WHERE "Id" = 122;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255746'
WHERE "Id" = 123;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255747'
WHERE "Id" = 124;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255748'
WHERE "Id" = 125;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625575'
WHERE "Id" = 126;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625575'
WHERE "Id" = 127;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255751'
WHERE "Id" = 128;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255752'
WHERE "Id" = 129;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255753'
WHERE "Id" = 130;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255754'
WHERE "Id" = 131;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255755'
WHERE "Id" = 132;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255756'
WHERE "Id" = 133;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255757'
WHERE "Id" = 134;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255759'
WHERE "Id" = 135;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625576'
WHERE "Id" = 136;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255761'
WHERE "Id" = 137;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255762'
WHERE "Id" = 138;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255763'
WHERE "Id" = 139;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255764'
WHERE "Id" = 140;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255765'
WHERE "Id" = 141;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255766'
WHERE "Id" = 142;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255766'
WHERE "Id" = 143;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255767'
WHERE "Id" = 144;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255768'
WHERE "Id" = 145;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255769'
WHERE "Id" = 146;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625577'
WHERE "Id" = 147;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255771'
WHERE "Id" = 148;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255772'
WHERE "Id" = 149;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255773'
WHERE "Id" = 150;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255774'
WHERE "Id" = 151;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255775'
WHERE "Id" = 152;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255776'
WHERE "Id" = 153;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255777'
WHERE "Id" = 154;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255777'
WHERE "Id" = 155;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255778'
WHERE "Id" = 156;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255779'
WHERE "Id" = 157;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625578'
WHERE "Id" = 158;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255781'
WHERE "Id" = 159;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255782'
WHERE "Id" = 160;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255783'
WHERE "Id" = 161;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255784'
WHERE "Id" = 162;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255785'
WHERE "Id" = 163;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255786'
WHERE "Id" = 164;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255786'
WHERE "Id" = 165;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255787'
WHERE "Id" = 166;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255788'
WHERE "Id" = 167;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255789'
WHERE "Id" = 168;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625579'
WHERE "Id" = 169;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255791'
WHERE "Id" = 170;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255792'
WHERE "Id" = 171;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255793'
WHERE "Id" = 172;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255794'
WHERE "Id" = 173;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255794'
WHERE "Id" = 174;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255795'
WHERE "Id" = 175;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255796'
WHERE "Id" = 176;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255797'
WHERE "Id" = 177;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255798'
WHERE "Id" = 178;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255799'
WHERE "Id" = 179;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.62558'
WHERE "Id" = 180;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255801'
WHERE "Id" = 181;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255802'
WHERE "Id" = 182;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255803'
WHERE "Id" = 183;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255804'
WHERE "Id" = 184;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255805'
WHERE "Id" = 185;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255806'
WHERE "Id" = 186;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255843'
WHERE "Id" = 187;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255844'
WHERE "Id" = 188;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255845'
WHERE "Id" = 189;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255846'
WHERE "Id" = 190;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255847'
WHERE "Id" = 191;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255847'
WHERE "Id" = 192;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255848'
WHERE "Id" = 193;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255849'
WHERE "Id" = 194;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625585'
WHERE "Id" = 195;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255851'
WHERE "Id" = 196;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255852'
WHERE "Id" = 197;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255853'
WHERE "Id" = 198;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255854'
WHERE "Id" = 199;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255855'
WHERE "Id" = 200;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255856'
WHERE "Id" = 201;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255857'
WHERE "Id" = 202;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255857'
WHERE "Id" = 203;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255858'
WHERE "Id" = 204;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255859'
WHERE "Id" = 205;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255861'
WHERE "Id" = 206;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255862'
WHERE "Id" = 207;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255863'
WHERE "Id" = 208;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255864'
WHERE "Id" = 209;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255865'
WHERE "Id" = 210;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255866'
WHERE "Id" = 211;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255867'
WHERE "Id" = 212;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255868'
WHERE "Id" = 213;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255869'
WHERE "Id" = 214;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625587'
WHERE "Id" = 215;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255871'
WHERE "Id" = 216;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255872'
WHERE "Id" = 217;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255873'
WHERE "Id" = 218;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255874'
WHERE "Id" = 219;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255874'
WHERE "Id" = 220;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255875'
WHERE "Id" = 221;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255876'
WHERE "Id" = 222;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255877'
WHERE "Id" = 223;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255878'
WHERE "Id" = 224;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255879'
WHERE "Id" = 225;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625588'
WHERE "Id" = 226;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255881'
WHERE "Id" = 227;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255882'
WHERE "Id" = 228;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255883'
WHERE "Id" = 229;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255884'
WHERE "Id" = 230;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255886'
WHERE "Id" = 231;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255888'
WHERE "Id" = 232;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255889'
WHERE "Id" = 233;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625589'
WHERE "Id" = 234;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255891'
WHERE "Id" = 235;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255892'
WHERE "Id" = 236;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255893'
WHERE "Id" = 237;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255895'
WHERE "Id" = 238;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255896'
WHERE "Id" = 239;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255897'
WHERE "Id" = 240;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255898'
WHERE "Id" = 241;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.62559'
WHERE "Id" = 242;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255901'
WHERE "Id" = 243;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255902'
WHERE "Id" = 244;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255903'
WHERE "Id" = 245;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255904'
WHERE "Id" = 246;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255906'
WHERE "Id" = 247;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255907'
WHERE "Id" = 248;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255908'
WHERE "Id" = 249;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625591'
WHERE "Id" = 250;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255911'
WHERE "Id" = 251;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255912'
WHERE "Id" = 252;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255913'
WHERE "Id" = 253;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255914'
WHERE "Id" = 254;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255916'
WHERE "Id" = 255;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255917'
WHERE "Id" = 256;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255918'
WHERE "Id" = 257;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255919'
WHERE "Id" = 258;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255921'
WHERE "Id" = 259;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255922'
WHERE "Id" = 260;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255923'
WHERE "Id" = 261;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255924'
WHERE "Id" = 262;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255979'
WHERE "Id" = 263;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255981'
WHERE "Id" = 264;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255983'
WHERE "Id" = 265;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255984'
WHERE "Id" = 266;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255985'
WHERE "Id" = 267;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255986'
WHERE "Id" = 268;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255988'
WHERE "Id" = 269;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255989'
WHERE "Id" = 270;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625599'
WHERE "Id" = 271;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255991'
WHERE "Id" = 272;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255992'
WHERE "Id" = 273;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255994'
WHERE "Id" = 274;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255995'
WHERE "Id" = 275;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255996'
WHERE "Id" = 276;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255998'
WHERE "Id" = 277;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6255999'
WHERE "Id" = 278;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256'
WHERE "Id" = 279;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256001'
WHERE "Id" = 280;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256003'
WHERE "Id" = 281;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256004'
WHERE "Id" = 282;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256005'
WHERE "Id" = 283;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256006'
WHERE "Id" = 284;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256007'
WHERE "Id" = 285;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256009'
WHERE "Id" = 286;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625601'
WHERE "Id" = 287;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256011'
WHERE "Id" = 288;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256012'
WHERE "Id" = 289;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256013'
WHERE "Id" = 290;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256015'
WHERE "Id" = 291;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256016'
WHERE "Id" = 292;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256017'
WHERE "Id" = 293;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256019'
WHERE "Id" = 294;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625602'
WHERE "Id" = 295;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256021'
WHERE "Id" = 296;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256023'
WHERE "Id" = 297;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256024'
WHERE "Id" = 298;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256025'
WHERE "Id" = 299;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256026'
WHERE "Id" = 300;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256028'
WHERE "Id" = 301;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256029'
WHERE "Id" = 302;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625603'
WHERE "Id" = 303;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256031'
WHERE "Id" = 304;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256033'
WHERE "Id" = 305;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256035'
WHERE "Id" = 306;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256037'
WHERE "Id" = 307;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256038'
WHERE "Id" = 308;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256039'
WHERE "Id" = 309;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256041'
WHERE "Id" = 310;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256042'
WHERE "Id" = 311;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256043'
WHERE "Id" = 312;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256045'
WHERE "Id" = 313;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256046'
WHERE "Id" = 314;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256101'
WHERE "Id" = 315;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256103'
WHERE "Id" = 316;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256104'
WHERE "Id" = 317;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256105'
WHERE "Id" = 318;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256106'
WHERE "Id" = 319;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256108'
WHERE "Id" = 320;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256109'
WHERE "Id" = 321;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625611'
WHERE "Id" = 322;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256112'
WHERE "Id" = 323;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256113'
WHERE "Id" = 324;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256114'
WHERE "Id" = 325;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256116'
WHERE "Id" = 326;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256117'
WHERE "Id" = 327;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256118'
WHERE "Id" = 328;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256119'
WHERE "Id" = 329;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256121'
WHERE "Id" = 330;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256122'
WHERE "Id" = 331;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256124'
WHERE "Id" = 332;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256125'
WHERE "Id" = 333;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256126'
WHERE "Id" = 334;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256127'
WHERE "Id" = 335;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256129'
WHERE "Id" = 336;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625613'
WHERE "Id" = 337;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256131'
WHERE "Id" = 338;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256133'
WHERE "Id" = 339;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256134'
WHERE "Id" = 340;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256135'
WHERE "Id" = 341;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256136'
WHERE "Id" = 342;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256137'
WHERE "Id" = 343;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256139'
WHERE "Id" = 344;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625614'
WHERE "Id" = 345;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256141'
WHERE "Id" = 346;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256143'
WHERE "Id" = 347;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256144'
WHERE "Id" = 348;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256146'
WHERE "Id" = 349;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256147'
WHERE "Id" = 350;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256148'
WHERE "Id" = 351;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625615'
WHERE "Id" = 352;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256151'
WHERE "Id" = 353;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256152'
WHERE "Id" = 354;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256153'
WHERE "Id" = 355;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256155'
WHERE "Id" = 356;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256156'
WHERE "Id" = 357;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256157'
WHERE "Id" = 358;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256159'
WHERE "Id" = 359;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625616'
WHERE "Id" = 360;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256161'
WHERE "Id" = 361;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256163'
WHERE "Id" = 362;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256164'
WHERE "Id" = 363;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256165'
WHERE "Id" = 364;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256167'
WHERE "Id" = 365;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256168'
WHERE "Id" = 366;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256169'
WHERE "Id" = 367;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256171'
WHERE "Id" = 368;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256172'
WHERE "Id" = 369;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256173'
WHERE "Id" = 370;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256175'
WHERE "Id" = 371;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256176'
WHERE "Id" = 372;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256177'
WHERE "Id" = 373;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256179'
WHERE "Id" = 374;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625618'
WHERE "Id" = 375;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256181'
WHERE "Id" = 376;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256183'
WHERE "Id" = 377;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256184'
WHERE "Id" = 378;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256185'
WHERE "Id" = 379;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256186'
WHERE "Id" = 380;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256188'
WHERE "Id" = 381;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256189'
WHERE "Id" = 382;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.625619'
WHERE "Id" = 383;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256191'
WHERE "Id" = 384;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256193'
WHERE "Id" = 385;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256194'
WHERE "Id" = 386;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256195'
WHERE "Id" = 387;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256197'
WHERE "Id" = 388;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256198'
WHERE "Id" = 389;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256199'
WHERE "Id" = 390;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256201'
WHERE "Id" = 391;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256202'
WHERE "Id" = 392;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256203'
WHERE "Id" = 393;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256204'
WHERE "Id" = 394;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256206'
WHERE "Id" = 395;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256207'
WHERE "Id" = 396;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256208'
WHERE "Id" = 397;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256209'
WHERE "Id" = 398;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256211'
WHERE "Id" = 399;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256245'
WHERE "Id" = 400;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256246'
WHERE "Id" = 401;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256247'
WHERE "Id" = 402;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 19:50:36.6256248'
WHERE "Id" = 403;
SELECT changes();


UPDATE "Tickets" SET "CreatedAt" = '2025-09-02 19:50:36.6255316', "EventDate" = '2025-09-02 19:00:00', "PurchaseDate" = '2025-09-02 19:50:36.6255316', "QRCodeToken" = 'b096e757-7070-4b58-8ce6-8feb6572cc92'
WHERE "Id" = 1;
SELECT changes();


UPDATE "Tickets" SET "CreatedAt" = '2025-09-02 19:50:36.6255423', "EventDate" = '2025-09-02 19:00:00', "PurchaseDate" = '2025-09-02 19:50:36.6255423', "QRCodeToken" = '96097e0a-417c-4aca-8f5f-d7243c50c104'
WHERE "Id" = 2;
SELECT changes();


UPDATE "Tickets" SET "CreatedAt" = '2025-09-02 19:50:36.6255446', "EventDate" = '2025-09-02 19:00:00', "PurchaseDate" = '2025-09-02 19:50:36.6255446', "QRCodeToken" = '855b3bd0-0694-4bf6-9675-931125c8b581'
WHERE "Id" = 3;
SELECT changes();


UPDATE "Tickets" SET "CreatedAt" = '2025-09-02 19:50:36.6255453', "EventDate" = '2025-09-02 19:00:00', "PurchaseDate" = '2025-09-02 19:50:36.6255453', "QRCodeToken" = '008b4b18-9991-419f-b18c-75334ce11afa'
WHERE "Id" = 4;
SELECT changes();


UPDATE "Tickets" SET "CreatedAt" = '2025-09-02 19:50:36.6255477', "EventDate" = '2025-09-02 19:00:00', "PurchaseDate" = '2025-09-02 19:50:36.6255477', "QRCodeToken" = '889fb7ad-6d15-4f65-ad9c-e684d72e42d4'
WHERE "Id" = 5;
SELECT changes();


UPDATE "Users" SET "CreatedAt" = '2025-09-02 19:50:36.6253487', "PasswordHash" = '$2a$11$AQP0dAJsKPVpup.5XFDOt.4Q93ylnmfrlUuKJICyx.ykmO1toVA5i'
WHERE "Id" = 1;
SELECT changes();


CREATE UNIQUE INDEX "IX_CartItems_EventId_SectorId_RowNumber_SeatNumber" ON "CartItems" ("EventId", "SectorId", "RowNumber", "SeatNumber");

CREATE INDEX "IX_CartItems_ReservedUntil" ON "CartItems" ("ReservedUntil");

CREATE INDEX "IX_CartItems_ShoppingCartId" ON "CartItems" ("ShoppingCartId");

CREATE INDEX "IX_SeatReservations_EventId_SectorId_RowNumber_SeatNumber" ON "SeatReservations" ("EventId", "SectorId", "RowNumber", "SeatNumber");

CREATE INDEX "IX_SeatReservations_ReservedUntil" ON "SeatReservations" ("ReservedUntil");

CREATE INDEX "IX_SeatReservations_SessionId" ON "SeatReservations" ("SessionId");

CREATE INDEX "IX_SeatReservations_Status" ON "SeatReservations" ("Status");

CREATE INDEX "IX_SeatReservations_UserId" ON "SeatReservations" ("UserId");

CREATE INDEX "IX_ShoppingCarts_ExpiresAt" ON "ShoppingCarts" ("ExpiresAt");

CREATE INDEX "IX_ShoppingCarts_SessionId" ON "ShoppingCarts" ("SessionId");

CREATE INDEX "IX_ShoppingCarts_UserId" ON "ShoppingCarts" ("UserId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250902195038_PerformanceIndexes', '8.0.8');

COMMIT;

BEGIN TRANSACTION;

UPDATE "Drinks" SET "CreatedAt" = '2025-09-02 20:51:49.3531013'
WHERE "Id" = 1;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-09-02 20:51:49.3531027'
WHERE "Id" = 2;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-09-02 20:51:49.3531029'
WHERE "Id" = 3;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-09-02 20:51:49.3531031'
WHERE "Id" = 4;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-09-02 20:51:49.3531033'
WHERE "Id" = 5;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-09-02 20:51:49.3531035'
WHERE "Id" = 6;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-09-02 20:51:49.3531037'
WHERE "Id" = 7;
SELECT changes();


UPDATE "Drinks" SET "CreatedAt" = '2025-09-02 20:51:49.3531039'
WHERE "Id" = 8;
SELECT changes();


UPDATE "Events" SET "CreatedAt" = '2025-09-02 20:51:49.3531253'
WHERE "Id" = 1;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3531809'
WHERE "Id" = 6;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3531816'
WHERE "Id" = 7;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3531828'
WHERE "Id" = 8;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3531829'
WHERE "Id" = 9;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353183'
WHERE "Id" = 10;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3531832'
WHERE "Id" = 11;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3531833'
WHERE "Id" = 12;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3531834'
WHERE "Id" = 13;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3531835'
WHERE "Id" = 14;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3531984'
WHERE "Id" = 15;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3531986'
WHERE "Id" = 16;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3531987'
WHERE "Id" = 17;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3531988'
WHERE "Id" = 18;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3531989'
WHERE "Id" = 19;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3531989'
WHERE "Id" = 20;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353199'
WHERE "Id" = 21;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3531992'
WHERE "Id" = 22;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3531995'
WHERE "Id" = 23;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3531996'
WHERE "Id" = 24;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3531997'
WHERE "Id" = 25;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3531998'
WHERE "Id" = 26;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3531999'
WHERE "Id" = 27;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532'
WHERE "Id" = 28;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532001'
WHERE "Id" = 29;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532002'
WHERE "Id" = 30;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532003'
WHERE "Id" = 31;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532004'
WHERE "Id" = 32;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532005'
WHERE "Id" = 33;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532006'
WHERE "Id" = 34;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532007'
WHERE "Id" = 35;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532008'
WHERE "Id" = 36;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532009'
WHERE "Id" = 37;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353201'
WHERE "Id" = 38;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532011'
WHERE "Id" = 39;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532012'
WHERE "Id" = 40;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532013'
WHERE "Id" = 41;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532014'
WHERE "Id" = 42;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532015'
WHERE "Id" = 43;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532015'
WHERE "Id" = 44;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532016'
WHERE "Id" = 45;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532017'
WHERE "Id" = 46;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532018'
WHERE "Id" = 47;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532019'
WHERE "Id" = 48;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353202'
WHERE "Id" = 49;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532021'
WHERE "Id" = 50;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532022'
WHERE "Id" = 51;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532023'
WHERE "Id" = 52;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532023'
WHERE "Id" = 53;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532024'
WHERE "Id" = 54;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532025'
WHERE "Id" = 55;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532026'
WHERE "Id" = 56;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532027'
WHERE "Id" = 57;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532028'
WHERE "Id" = 58;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532029'
WHERE "Id" = 59;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353203'
WHERE "Id" = 60;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532031'
WHERE "Id" = 61;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532031'
WHERE "Id" = 62;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532032'
WHERE "Id" = 63;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532033'
WHERE "Id" = 64;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532034'
WHERE "Id" = 65;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532035'
WHERE "Id" = 66;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532036'
WHERE "Id" = 67;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532037'
WHERE "Id" = 68;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532038'
WHERE "Id" = 69;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532039'
WHERE "Id" = 70;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532041'
WHERE "Id" = 71;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532042'
WHERE "Id" = 72;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532042'
WHERE "Id" = 73;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532043'
WHERE "Id" = 74;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532044'
WHERE "Id" = 75;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532045'
WHERE "Id" = 76;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532046'
WHERE "Id" = 77;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532136'
WHERE "Id" = 78;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532138'
WHERE "Id" = 79;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532139'
WHERE "Id" = 80;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353214'
WHERE "Id" = 81;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532141'
WHERE "Id" = 82;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532143'
WHERE "Id" = 83;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532144'
WHERE "Id" = 84;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532145'
WHERE "Id" = 85;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532147'
WHERE "Id" = 86;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532148'
WHERE "Id" = 87;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532149'
WHERE "Id" = 88;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353215'
WHERE "Id" = 89;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532151'
WHERE "Id" = 90;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532152'
WHERE "Id" = 91;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532153'
WHERE "Id" = 92;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532154'
WHERE "Id" = 93;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532155'
WHERE "Id" = 94;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532156'
WHERE "Id" = 95;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532157'
WHERE "Id" = 96;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532159'
WHERE "Id" = 97;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353216'
WHERE "Id" = 98;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532161'
WHERE "Id" = 99;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532162'
WHERE "Id" = 100;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532163'
WHERE "Id" = 101;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532164'
WHERE "Id" = 102;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532165'
WHERE "Id" = 103;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532166'
WHERE "Id" = 104;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532167'
WHERE "Id" = 105;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532169'
WHERE "Id" = 106;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532172'
WHERE "Id" = 107;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532173'
WHERE "Id" = 108;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532175'
WHERE "Id" = 109;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532176'
WHERE "Id" = 110;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532177'
WHERE "Id" = 111;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532178'
WHERE "Id" = 112;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532179'
WHERE "Id" = 113;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353218'
WHERE "Id" = 114;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532181'
WHERE "Id" = 115;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532182'
WHERE "Id" = 116;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532183'
WHERE "Id" = 117;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532184'
WHERE "Id" = 118;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532186'
WHERE "Id" = 119;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532187'
WHERE "Id" = 120;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532188'
WHERE "Id" = 121;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532189'
WHERE "Id" = 122;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353219'
WHERE "Id" = 123;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532191'
WHERE "Id" = 124;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532192'
WHERE "Id" = 125;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532193'
WHERE "Id" = 126;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532194'
WHERE "Id" = 127;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532196'
WHERE "Id" = 128;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532197'
WHERE "Id" = 129;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532199'
WHERE "Id" = 130;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.35322'
WHERE "Id" = 131;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532201'
WHERE "Id" = 132;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532203'
WHERE "Id" = 133;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532204'
WHERE "Id" = 134;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532207'
WHERE "Id" = 135;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532208'
WHERE "Id" = 136;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532209'
WHERE "Id" = 137;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532211'
WHERE "Id" = 138;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532212'
WHERE "Id" = 139;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532213'
WHERE "Id" = 140;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532214'
WHERE "Id" = 141;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532287'
WHERE "Id" = 142;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532289'
WHERE "Id" = 143;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353229'
WHERE "Id" = 144;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532291'
WHERE "Id" = 145;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532293'
WHERE "Id" = 146;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532294'
WHERE "Id" = 147;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532295'
WHERE "Id" = 148;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532296'
WHERE "Id" = 149;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532297'
WHERE "Id" = 150;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532298'
WHERE "Id" = 151;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532299'
WHERE "Id" = 152;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532301'
WHERE "Id" = 153;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532302'
WHERE "Id" = 154;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532303'
WHERE "Id" = 155;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532304'
WHERE "Id" = 156;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532305'
WHERE "Id" = 157;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532306'
WHERE "Id" = 158;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532307'
WHERE "Id" = 159;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532308'
WHERE "Id" = 160;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532309'
WHERE "Id" = 161;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353231'
WHERE "Id" = 162;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532311'
WHERE "Id" = 163;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532312'
WHERE "Id" = 164;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532314'
WHERE "Id" = 165;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532315'
WHERE "Id" = 166;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532316'
WHERE "Id" = 167;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532317'
WHERE "Id" = 168;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532318'
WHERE "Id" = 169;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532319'
WHERE "Id" = 170;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353232'
WHERE "Id" = 171;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532321'
WHERE "Id" = 172;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532322'
WHERE "Id" = 173;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532324'
WHERE "Id" = 174;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532325'
WHERE "Id" = 175;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532326'
WHERE "Id" = 176;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532327'
WHERE "Id" = 177;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532328'
WHERE "Id" = 178;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532329'
WHERE "Id" = 179;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353233'
WHERE "Id" = 180;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532331'
WHERE "Id" = 181;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532333'
WHERE "Id" = 182;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532334'
WHERE "Id" = 183;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532335'
WHERE "Id" = 184;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532336'
WHERE "Id" = 185;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532337'
WHERE "Id" = 186;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532339'
WHERE "Id" = 187;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353234'
WHERE "Id" = 188;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532341'
WHERE "Id" = 189;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532342'
WHERE "Id" = 190;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532343'
WHERE "Id" = 191;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532344'
WHERE "Id" = 192;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532345'
WHERE "Id" = 193;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532347'
WHERE "Id" = 194;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532348'
WHERE "Id" = 195;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532349'
WHERE "Id" = 196;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353235'
WHERE "Id" = 197;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532351'
WHERE "Id" = 198;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532352'
WHERE "Id" = 199;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532354'
WHERE "Id" = 200;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532355'
WHERE "Id" = 201;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532356'
WHERE "Id" = 202;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532357'
WHERE "Id" = 203;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532358'
WHERE "Id" = 204;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532359'
WHERE "Id" = 205;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532361'
WHERE "Id" = 206;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532363'
WHERE "Id" = 207;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532364'
WHERE "Id" = 208;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532365'
WHERE "Id" = 209;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532366'
WHERE "Id" = 210;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532367'
WHERE "Id" = 211;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532368'
WHERE "Id" = 212;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353237'
WHERE "Id" = 213;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532371'
WHERE "Id" = 214;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532372'
WHERE "Id" = 215;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532373'
WHERE "Id" = 216;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532374'
WHERE "Id" = 217;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532375'
WHERE "Id" = 218;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532376'
WHERE "Id" = 219;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532377'
WHERE "Id" = 220;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532378'
WHERE "Id" = 221;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353238'
WHERE "Id" = 222;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532381'
WHERE "Id" = 223;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532382'
WHERE "Id" = 224;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532383'
WHERE "Id" = 225;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532384'
WHERE "Id" = 226;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353244'
WHERE "Id" = 227;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532442'
WHERE "Id" = 228;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532444'
WHERE "Id" = 229;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532445'
WHERE "Id" = 230;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532446'
WHERE "Id" = 231;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532447'
WHERE "Id" = 232;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532449'
WHERE "Id" = 233;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353245'
WHERE "Id" = 234;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532451'
WHERE "Id" = 235;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532452'
WHERE "Id" = 236;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532453'
WHERE "Id" = 237;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532454'
WHERE "Id" = 238;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532456'
WHERE "Id" = 239;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532457'
WHERE "Id" = 240;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532458'
WHERE "Id" = 241;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532459'
WHERE "Id" = 242;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353246'
WHERE "Id" = 243;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532461'
WHERE "Id" = 244;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532462'
WHERE "Id" = 245;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532464'
WHERE "Id" = 246;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532465'
WHERE "Id" = 247;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532466'
WHERE "Id" = 248;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532467'
WHERE "Id" = 249;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532468'
WHERE "Id" = 250;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532469'
WHERE "Id" = 251;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353247'
WHERE "Id" = 252;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532472'
WHERE "Id" = 253;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532473'
WHERE "Id" = 254;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532474'
WHERE "Id" = 255;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532475'
WHERE "Id" = 256;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532476'
WHERE "Id" = 257;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532477'
WHERE "Id" = 258;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532479'
WHERE "Id" = 259;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353248'
WHERE "Id" = 260;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532481'
WHERE "Id" = 261;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532482'
WHERE "Id" = 262;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532486'
WHERE "Id" = 263;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532487'
WHERE "Id" = 264;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532488'
WHERE "Id" = 265;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353249'
WHERE "Id" = 266;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532491'
WHERE "Id" = 267;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532492'
WHERE "Id" = 268;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532493'
WHERE "Id" = 269;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532544'
WHERE "Id" = 270;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532545'
WHERE "Id" = 271;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532547'
WHERE "Id" = 272;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532548'
WHERE "Id" = 273;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532549'
WHERE "Id" = 274;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353255'
WHERE "Id" = 275;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532552'
WHERE "Id" = 276;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532553'
WHERE "Id" = 277;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532554'
WHERE "Id" = 278;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532555'
WHERE "Id" = 279;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532556'
WHERE "Id" = 280;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532557'
WHERE "Id" = 281;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532558'
WHERE "Id" = 282;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353256'
WHERE "Id" = 283;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532561'
WHERE "Id" = 284;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532562'
WHERE "Id" = 285;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532563'
WHERE "Id" = 286;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532564'
WHERE "Id" = 287;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532566'
WHERE "Id" = 288;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532567'
WHERE "Id" = 289;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532568'
WHERE "Id" = 290;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532569'
WHERE "Id" = 291;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353257'
WHERE "Id" = 292;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532571'
WHERE "Id" = 293;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532573'
WHERE "Id" = 294;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532574'
WHERE "Id" = 295;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532575'
WHERE "Id" = 296;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532576'
WHERE "Id" = 297;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532577'
WHERE "Id" = 298;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532579'
WHERE "Id" = 299;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353258'
WHERE "Id" = 300;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532581'
WHERE "Id" = 301;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532582'
WHERE "Id" = 302;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532583'
WHERE "Id" = 303;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532584'
WHERE "Id" = 304;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532586'
WHERE "Id" = 305;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532587'
WHERE "Id" = 306;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532589'
WHERE "Id" = 307;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532591'
WHERE "Id" = 308;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532592'
WHERE "Id" = 309;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532593'
WHERE "Id" = 310;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532595'
WHERE "Id" = 311;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532596'
WHERE "Id" = 312;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532597'
WHERE "Id" = 313;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532598'
WHERE "Id" = 314;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.35326'
WHERE "Id" = 315;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532601'
WHERE "Id" = 316;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532602'
WHERE "Id" = 317;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532603'
WHERE "Id" = 318;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532604'
WHERE "Id" = 319;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532606'
WHERE "Id" = 320;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532607'
WHERE "Id" = 321;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532608'
WHERE "Id" = 322;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532609'
WHERE "Id" = 323;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353261'
WHERE "Id" = 324;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532612'
WHERE "Id" = 325;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532613'
WHERE "Id" = 326;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532614'
WHERE "Id" = 327;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532615'
WHERE "Id" = 328;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532617'
WHERE "Id" = 329;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532618'
WHERE "Id" = 330;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532619'
WHERE "Id" = 331;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353262'
WHERE "Id" = 332;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532621'
WHERE "Id" = 333;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532622'
WHERE "Id" = 334;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532623'
WHERE "Id" = 335;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532625'
WHERE "Id" = 336;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532626'
WHERE "Id" = 337;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532627'
WHERE "Id" = 338;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532628'
WHERE "Id" = 339;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532629'
WHERE "Id" = 340;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353263'
WHERE "Id" = 341;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532631'
WHERE "Id" = 342;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532633'
WHERE "Id" = 343;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532634'
WHERE "Id" = 344;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532635'
WHERE "Id" = 345;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532636'
WHERE "Id" = 346;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532637'
WHERE "Id" = 347;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532638'
WHERE "Id" = 348;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353264'
WHERE "Id" = 349;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532641'
WHERE "Id" = 350;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532642'
WHERE "Id" = 351;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532643'
WHERE "Id" = 352;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532645'
WHERE "Id" = 353;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532646'
WHERE "Id" = 354;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532696'
WHERE "Id" = 355;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532698'
WHERE "Id" = 356;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532699'
WHERE "Id" = 357;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532701'
WHERE "Id" = 358;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532702'
WHERE "Id" = 359;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532703'
WHERE "Id" = 360;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532704'
WHERE "Id" = 361;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532705'
WHERE "Id" = 362;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532706'
WHERE "Id" = 363;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532707'
WHERE "Id" = 364;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532708'
WHERE "Id" = 365;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532709'
WHERE "Id" = 366;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353271'
WHERE "Id" = 367;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532711'
WHERE "Id" = 368;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532713'
WHERE "Id" = 369;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532714'
WHERE "Id" = 370;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532715'
WHERE "Id" = 371;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532716'
WHERE "Id" = 372;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532717'
WHERE "Id" = 373;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532719'
WHERE "Id" = 374;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353272'
WHERE "Id" = 375;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532721'
WHERE "Id" = 376;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532722'
WHERE "Id" = 377;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532723'
WHERE "Id" = 378;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532725'
WHERE "Id" = 379;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532726'
WHERE "Id" = 380;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532727'
WHERE "Id" = 381;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532728'
WHERE "Id" = 382;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532729'
WHERE "Id" = 383;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353273'
WHERE "Id" = 384;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532732'
WHERE "Id" = 385;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532733'
WHERE "Id" = 386;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532734'
WHERE "Id" = 387;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532735'
WHERE "Id" = 388;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532737'
WHERE "Id" = 389;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532738'
WHERE "Id" = 390;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532739'
WHERE "Id" = 391;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532741'
WHERE "Id" = 392;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532742'
WHERE "Id" = 393;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532743'
WHERE "Id" = 394;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532744'
WHERE "Id" = 395;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532745'
WHERE "Id" = 396;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532746'
WHERE "Id" = 397;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532748'
WHERE "Id" = 398;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532749'
WHERE "Id" = 399;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.353275'
WHERE "Id" = 400;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532751'
WHERE "Id" = 401;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532752'
WHERE "Id" = 402;
SELECT changes();


UPDATE "StadiumSeats" SET "CreatedAt" = '2025-09-02 20:51:49.3532753'
WHERE "Id" = 403;
SELECT changes();


UPDATE "Tickets" SET "CreatedAt" = '2025-09-02 20:51:49.3531568', "PurchaseDate" = '2025-09-02 20:51:49.3531567', "QRCodeToken" = '9b2f1a62-013d-4f91-a997-9a9761c1c8ec'
WHERE "Id" = 1;
SELECT changes();


UPDATE "Tickets" SET "CreatedAt" = '2025-09-02 20:51:49.3531696', "PurchaseDate" = '2025-09-02 20:51:49.3531695', "QRCodeToken" = 'e6e5eec2-40a7-46e9-9ebf-d65932364b5f'
WHERE "Id" = 2;
SELECT changes();


UPDATE "Tickets" SET "CreatedAt" = '2025-09-02 20:51:49.3531711', "PurchaseDate" = '2025-09-02 20:51:49.353171', "QRCodeToken" = 'ff61fbe4-e322-4669-9b62-8dda9ca06091'
WHERE "Id" = 3;
SELECT changes();


UPDATE "Tickets" SET "CreatedAt" = '2025-09-02 20:51:49.3531722', "PurchaseDate" = '2025-09-02 20:51:49.3531721', "QRCodeToken" = 'f0beb48f-f3b4-46a8-bc92-83036303fc9e'
WHERE "Id" = 4;
SELECT changes();


UPDATE "Tickets" SET "CreatedAt" = '2025-09-02 20:51:49.3531738', "PurchaseDate" = '2025-09-02 20:51:49.3531738', "QRCodeToken" = 'f6f7d32b-d62b-441d-8224-75493f3d641d'
WHERE "Id" = 5;
SELECT changes();


UPDATE "Users" SET "CreatedAt" = '2025-09-02 20:51:49.3529924', "PasswordHash" = '$2a$11$74jsN7IXWe0mLZDsyA2AweFXzhJ4HEcdQ6ftx.Q.O4AlhZcU/o9r2'
WHERE "Id" = 1;
SELECT changes();


INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250902205150_CustomerTicketingSystem', '8.0.8');

COMMIT;

