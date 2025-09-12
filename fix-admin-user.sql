-- Check if admin user exists
SELECT 
    "Id", 
    "Email", 
    "FullName", 
    "Role",
    "CreatedAt"
FROM "Users" 
WHERE "Email" = 'admin@stadium.com';

-- If the user doesn't exist, this will create it
-- Password: admin123 (hashed with BCrypt)
INSERT INTO "Users" (
    "Email", 
    "PasswordHash", 
    "FullName", 
    "Role", 
    "PhoneNumber",
    "CreatedAt",
    "IsActive",
    "RefreshToken",
    "RefreshTokenExpiryTime"
)
SELECT 
    'admin@stadium.com',
    '$2a$11$rBvbLcyP6UZRwS6jNfzZOO.qOSvY0XhNABH.X8wlFEOdqNBRFDIbC', -- BCrypt hash of 'admin123'
    'Stadium Administrator',
    'Admin',
    '555-0100',
    CURRENT_TIMESTAMP,
    true,
    NULL,
    NULL
WHERE NOT EXISTS (
    SELECT 1 FROM "Users" WHERE "Email" = 'admin@stadium.com'
);

-- If user exists but password needs reset, update it
UPDATE "Users" 
SET "PasswordHash" = '$2a$11$rBvbLcyP6UZRwS6jNfzZOO.qOSvY0XhNABH.X8wlFEOdqNBRFDIbC'
WHERE "Email" = 'admin@stadium.com';

-- Verify the update
SELECT 
    "Id", 
    "Email", 
    "FullName", 
    "Role",
    "IsActive"
FROM "Users" 
WHERE "Email" = 'admin@stadium.com';