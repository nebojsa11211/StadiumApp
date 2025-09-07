# Entity Framework Migration Commands for Supabase

## Prerequisites
1. Install PostgreSQL Entity Framework provider:
```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
```

2. Update connection string in appsettings.json to point to Supabase

## Migration Commands

### 1. Remove Existing SQLite Migrations
```bash
# Remove the Migrations folder
rm -rf StadiumDrinkOrdering.API/Migrations/

# Or on Windows
rmdir /s StladiumDrinkOrdering.API\Migrations
```

### 2. Create Initial PostgreSQL Migration
```bash
cd StadiumDrinkOrdering.API

# Create initial migration for PostgreSQL
dotnet ef migrations add InitialPostgreSQLMigration --project StadiumDrinkOrdering.API

# Verify migration was created
ls Migrations/
```

### 3. Update Database
```bash
# Apply migrations to Supabase database
dotnet ef database update --project StadiumDrinkOrdering.API

# Or specify connection string explicitly
dotnet ef database update --project StadiumDrinkOrdering.API --connection "Host=YOUR_SUPABASE_HOST.supabase.co;Database=postgres;Username=postgres;Password=YOUR_PASSWORD;Port=5432;SSL Mode=Require;"
```

### 4. Generate SQL Script (Alternative)
```bash
# Generate SQL script instead of direct update
dotnet ef migrations script --project StadiumDrinkOrdering.API --output supabase-migration.sql

# Then run the script in Supabase SQL Editor
```

### 5. Verify Migration
```bash
# Check applied migrations
dotnet ef migrations list --project StadiumDrinkOrdering.API

# Test connection
dotnet ef dbcontext info --project StadiumDrinkOrdering.API
```

## Troubleshooting

### Common Issues and Solutions

1. **Provider Error**: `No database provider has been configured`
   - Ensure `UseNpgsql()` is called in Program.cs
   - Check connection string format

2. **SSL Connection Errors**:
   ```
   Connection String: "...;SSL Mode=Require;Trust Server Certificate=true"
   ```

3. **Migration Conflicts**:
   ```bash
   # Reset migrations if needed
   dotnet ef database drop --project StadiumDrinkOrdering.API
   dotnet ef migrations remove --project StadiumDrinkOrdering.API
   ```

4. **Timeout Issues**:
   - Increase CommandTimeout in UseNpgsql options
   - Use `--timeout 180` flag with EF commands

### Verification Queries
Run these in Supabase SQL Editor after migration:

```sql
-- Check tables were created
SELECT table_name 
FROM information_schema.tables 
WHERE table_schema = 'public' 
ORDER BY table_name;

-- Verify relationships
SELECT 
    tc.constraint_name,
    tc.table_name,
    kcu.column_name,
    ccu.table_name AS foreign_table_name,
    ccu.column_name AS foreign_column_name 
FROM information_schema.table_constraints AS tc 
JOIN information_schema.key_column_usage AS kcu
    ON tc.constraint_name = kcu.constraint_name
JOIN information_schema.constraint_column_usage AS ccu
    ON ccu.constraint_name = tc.constraint_name
WHERE tc.constraint_type = 'FOREIGN KEY';

-- Check indexes
SELECT indexname, tablename, indexdef 
FROM pg_indexes 
WHERE schemaname = 'public' 
ORDER BY tablename, indexname;
```