-- Check actual row counts in your database
SELECT
    'LogEntries' as table_name,
    COUNT(*) as row_count,
    pg_size_pretty(pg_total_relation_size('"LogEntries"')) as table_size,
    MIN("Timestamp") as oldest_record,
    MAX("Timestamp") as newest_record
FROM "LogEntries"
UNION ALL
SELECT
    'IPBans',
    COUNT(*),
    pg_size_pretty(pg_total_relation_size('"IPBans"')),
    MIN("BanStart"::timestamp),
    MAX("BanEnd"::timestamp)
FROM "IPBans"
UNION ALL
SELECT
    'Users',
    COUNT(*),
    pg_size_pretty(pg_total_relation_size('"Users"')),
    MIN("CreatedAt"),
    MAX("LastLoginAt")
FROM "Users";
