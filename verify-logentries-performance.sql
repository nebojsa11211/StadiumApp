-- =========================================================================
-- LOGENTRIES PERFORMANCE VERIFICATION SCRIPT
-- =========================================================================
-- Run this AFTER applying the optimization script to verify improvements
-- =========================================================================

\echo '========================================='
\echo 'LOGENTRIES PERFORMANCE VERIFICATION'
\echo '========================================='
\echo ''

-- 1. Check table size and row count
\echo '1. TABLE SIZE AND ROW COUNT:'
\echo '----------------------------'
SELECT
    pg_size_pretty(pg_total_relation_size('"LogEntries"')) as "Table Size",
    pg_size_pretty(pg_relation_size('"LogEntries"')) as "Data Size",
    pg_size_pretty(pg_total_relation_size('"LogEntries"') - pg_relation_size('"LogEntries"')) as "Index Size",
    COUNT(*) as "Total Rows",
    COUNT(*) FILTER (WHERE "Timestamp" >= NOW() - INTERVAL '7 days') as "Recent Rows (7d)",
    COUNT(*) FILTER (WHERE "Timestamp" < NOW() - INTERVAL '7 days') as "Old Rows (>7d)"
FROM "LogEntries";

\echo ''
\echo '2. INDEX VERIFICATION:'
\echo '----------------------'
-- 2. Verify all indexes exist
SELECT
    schemaname as "Schema",
    tablename as "Table",
    indexname as "Index Name",
    CASE
        WHEN indexdef LIKE '%gin%' THEN 'GIN (Full-Text)'
        WHEN indexdef LIKE '%btree%' THEN 'BTREE (Standard)'
        ELSE 'OTHER'
    END as "Index Type",
    pg_size_pretty(pg_relation_size(indexname::regclass)) as "Index Size"
FROM pg_indexes
WHERE tablename = 'LogEntries'
ORDER BY indexname;

\echo ''
\echo '3. INDEX USAGE STATISTICS:'
\echo '--------------------------'
-- 3. Check if indexes are being used
SELECT
    indexrelname as "Index Name",
    idx_scan as "Times Used",
    idx_tup_read as "Rows Read",
    idx_tup_fetch as "Rows Fetched",
    CASE
        WHEN idx_scan = 0 THEN 'NEVER USED'
        WHEN idx_scan < 10 THEN 'RARELY USED'
        WHEN idx_scan < 100 THEN 'SOMETIMES USED'
        ELSE 'FREQUENTLY USED'
    END as "Usage Status"
FROM pg_stat_user_indexes
WHERE schemaname = 'public' AND tablename = 'LogEntries'
ORDER BY idx_scan DESC;

\echo ''
\echo '4. TABLE BLOAT CHECK:'
\echo '---------------------'
-- 4. Check for table bloat (dead rows)
SELECT
    schemaname as "Schema",
    tablename as "Table",
    n_live_tup as "Live Rows",
    n_dead_tup as "Dead Rows",
    ROUND(100.0 * n_dead_tup / NULLIF(n_live_tup + n_dead_tup, 0), 2) as "Bloat %",
    last_vacuum as "Last VACUUM",
    last_autovacuum as "Last Auto-VACUUM",
    last_analyze as "Last ANALYZE",
    last_autoanalyze as "Last Auto-ANALYZE"
FROM pg_stat_user_tables
WHERE tablename = 'LogEntries';

\echo ''
\echo '5. PERFORMANCE TEST - LOG SUMMARY:'
\echo '-----------------------------------'
-- 5. Test query performance (should be fast with proper indexes)
\timing on
SELECT
    COUNT(*) as total_logs,
    COUNT(*) FILTER (WHERE "Level" = 'Error') as error_count,
    COUNT(*) FILTER (WHERE "Level" = 'Warning') as warning_count,
    COUNT(*) FILTER (WHERE "Level" = 'Info') as info_count,
    MAX("Timestamp") as last_log_time
FROM "LogEntries";
\timing off

\echo ''
\echo '6. PERFORMANCE TEST - TEXT SEARCH:'
\echo '-----------------------------------'
-- 6. Test full-text search performance
\timing on
SELECT COUNT(*)
FROM "LogEntries"
WHERE to_tsvector('english', COALESCE("Message", '')) @@ to_tsquery('english', 'error')
   OR to_tsvector('english', "Action") @@ to_tsquery('english', 'error');
\timing off

\echo ''
\echo '7. PERFORMANCE TEST - FILTERED QUERY:'
\echo '--------------------------------------'
-- 7. Test filtered query with ORDER BY
\timing on
SELECT "Id", "Timestamp", "Level", "Action", "Message"
FROM "LogEntries"
WHERE "Timestamp" >= NOW() - INTERVAL '24 hours'
  AND "Level" IN ('Error', 'Warning')
ORDER BY "Timestamp" DESC
LIMIT 100;
\timing off

\echo ''
\echo '8. EXPECTED PERFORMANCE TARGETS:'
\echo '---------------------------------'
\echo 'Query 5 (Log Summary):     Should complete in <100ms'
\echo 'Query 6 (Text Search):     Should complete in <500ms'
\echo 'Query 7 (Filtered Query):  Should complete in <200ms'
\echo ''
\echo 'If any query takes longer than these targets,'
\echo 'the optimization may not have been fully applied.'
\echo ''

\echo '========================================='
\echo 'VERIFICATION COMPLETE'
\echo '========================================='
