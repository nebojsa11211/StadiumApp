-- ===============================================
-- SQLite to Supabase Migration Validation Scripts
-- ===============================================

-- ===============================================
-- PRE-MIGRATION VALIDATION (Run on SQLite)
-- ===============================================

-- 1. Record count baseline
SELECT 'PRE_MIGRATION_COUNTS' as phase;

CREATE TEMPORARY TABLE migration_baseline AS
SELECT 
    'Users' as table_name, COUNT(*) as record_count FROM Users
UNION ALL SELECT 'Drinks', COUNT(*) FROM Drinks
UNION ALL SELECT 'Orders', COUNT(*) FROM Orders  
UNION ALL SELECT 'OrderItems', COUNT(*) FROM OrderItems
UNION ALL SELECT 'Payments', COUNT(*) FROM Payments
UNION ALL SELECT 'Tribunes', COUNT(*) FROM Tribunes
UNION ALL SELECT 'Rings', COUNT(*) FROM Rings
UNION ALL SELECT 'StadiumSections', COUNT(*) FROM StadiumSections
UNION ALL SELECT 'StadiumSeats', COUNT(*) FROM StadiumSeats
UNION ALL SELECT 'Events', COUNT(*) FROM Events
UNION ALL SELECT 'Tickets', COUNT(*) FROM Tickets
UNION ALL SELECT 'ShoppingCarts', COUNT(*) FROM ShoppingCarts
UNION ALL SELECT 'CartItems', COUNT(*) FROM CartItems
UNION ALL SELECT 'SeatReservations', COUNT(*) FROM SeatReservations
UNION ALL SELECT 'LogEntries', COUNT(*) FROM LogEntries
UNION ALL SELECT 'EventStaffAssignments', COUNT(*) FROM EventStaffAssignments
UNION ALL SELECT 'OrderSessions', COUNT(*) FROM OrderSessions
UNION ALL SELECT 'TicketSessions', COUNT(*) FROM TicketSessions;

SELECT * FROM migration_baseline ORDER BY table_name;

-- 2. Data integrity checks
SELECT 'DATA_INTEGRITY_CHECKS' as phase;

-- Check for orphaned records
SELECT 'Orphaned Orders (no customer)' as check_name, COUNT(*) as count
FROM Orders o 
WHERE NOT EXISTS (SELECT 1 FROM Users u WHERE u.Id = o.CustomerId);

SELECT 'Orphaned OrderItems (no order)' as check_name, COUNT(*) as count
FROM OrderItems oi 
WHERE NOT EXISTS (SELECT 1 FROM Orders o WHERE o.Id = oi.OrderId);

SELECT 'Orphaned Tickets (no event)' as check_name, COUNT(*) as count  
FROM Tickets t
WHERE NOT EXISTS (SELECT 1 FROM Events e WHERE e.Id = t.EventId);

-- Check data ranges and consistency
SELECT 'Negative Prices in Drinks' as check_name, COUNT(*) as count
FROM Drinks WHERE Price < 0;

SELECT 'Invalid Order Statuses' as check_name, COUNT(*) as count
FROM Orders WHERE Status NOT IN ('Pending', 'Accepted', 'In Preparation', 'Ready', 'Out for Delivery', 'Delivered', 'Cancelled');

SELECT 'Future Order Dates' as check_name, COUNT(*) as count
FROM Orders WHERE OrderDate > datetime('now');

-- 3. Sample data for comparison
SELECT 'SAMPLE_DATA_SNAPSHOT' as phase;

-- Sample users (mask sensitive data)
SELECT Id, Username, SUBSTR(Email, 1, 3) || '***@' || SUBSTR(Email, INSTR(Email, '@')+1) as MaskedEmail, Role, CreatedAt
FROM Users 
ORDER BY Id 
LIMIT 5;

-- Sample orders with totals
SELECT Id, CustomerId, Status, OrderDate, TotalAmount, TicketNumber
FROM Orders 
ORDER BY OrderDate DESC 
LIMIT 5;

-- Stadium structure summary
SELECT 
    t.Code as Tribune,
    COUNT(DISTINCT r.Id) as RingCount,
    COUNT(DISTINCT ss.Id) as SectionCount,
    COUNT(st.Id) as SeatCount
FROM Tribunes t
LEFT JOIN Rings r ON t.Id = r.TribuneId
LEFT JOIN StadiumSections ss ON r.Id = ss.RingId  
LEFT JOIN StadiumSeats st ON ss.Id = st.SectionId
GROUP BY t.Code, t.Name
ORDER BY t.Code;

-- ===============================================
-- POST-MIGRATION VALIDATION (Run on PostgreSQL/Supabase)
-- ===============================================

-- 1. Record count verification
SELECT 'POST_MIGRATION_COUNTS' as phase;

SELECT 
    'users' as table_name, COUNT(*) as record_count FROM users
UNION ALL SELECT 'drinks', COUNT(*) FROM drinks
UNION ALL SELECT 'orders', COUNT(*) FROM orders  
UNION ALL SELECT 'order_items', COUNT(*) FROM order_items
UNION ALL SELECT 'payments', COUNT(*) FROM payments
UNION ALL SELECT 'tribunes', COUNT(*) FROM tribunes
UNION ALL SELECT 'rings', COUNT(*) FROM rings
UNION ALL SELECT 'stadium_sections', COUNT(*) FROM stadium_sections
UNION ALL SELECT 'stadium_seats', COUNT(*) FROM stadium_seats
UNION ALL SELECT 'events', COUNT(*) FROM events
UNION ALL SELECT 'tickets', COUNT(*) FROM tickets
UNION ALL SELECT 'shopping_carts', COUNT(*) FROM shopping_carts
UNION ALL SELECT 'cart_items', COUNT(*) FROM cart_items
UNION ALL SELECT 'seat_reservations', COUNT(*) FROM seat_reservations
UNION ALL SELECT 'log_entries', COUNT(*) FROM log_entries
UNION ALL SELECT 'event_staff_assignments', COUNT(*) FROM event_staff_assignments
UNION ALL SELECT 'order_sessions', COUNT(*) FROM order_sessions
UNION ALL SELECT 'ticket_sessions', COUNT(*) FROM ticket_sessions
ORDER BY table_name;

-- 2. Data type validation
SELECT 'DATA_TYPE_VALIDATION' as phase;

-- Check numeric ranges
SELECT 'Negative Prices in drinks' as check_name, COUNT(*) as count
FROM drinks WHERE price < 0;

SELECT 'Invalid timestamps' as check_name, COUNT(*) as count  
FROM orders WHERE order_date > NOW();

SELECT 'Invalid seat numbers' as check_name, COUNT(*) as count
FROM stadium_seats WHERE row_number <= 0 OR seat_number <= 0;

-- Check foreign key relationships
SELECT 'Orphaned orders (no customer)' as check_name, COUNT(*) as count
FROM orders o 
WHERE NOT EXISTS (SELECT 1 FROM users u WHERE u.id = o.customer_id);

SELECT 'Orphaned order_items' as check_name, COUNT(*) as count  
FROM order_items oi
WHERE NOT EXISTS (SELECT 1 FROM orders o WHERE o.id = oi.order_id);

-- 3. Compare sample data (match with pre-migration)
SELECT 'SAMPLE_DATA_COMPARISON' as phase;

-- Sample users (should match SQLite data)
SELECT id, username, SUBSTR(email, 1, 3) || '***@' || SUBSTR(email, POSITION('@' in email)+1) as masked_email, role, created_at
FROM users 
ORDER BY id 
LIMIT 5;

-- Sample orders (should match SQLite data)  
SELECT id, customer_id, status, order_date, total_amount, ticket_number
FROM orders 
ORDER BY order_date DESC 
LIMIT 5;

-- Stadium structure comparison
SELECT 
    t.code as tribune,
    COUNT(DISTINCT r.id) as ring_count,
    COUNT(DISTINCT ss.id) as section_count, 
    COUNT(st.id) as seat_count
FROM tribunes t
LEFT JOIN rings r ON t.id = r.tribune_id
LEFT JOIN stadium_sections ss ON r.id = ss.ring_id
LEFT JOIN stadium_seats st ON ss.id = st.section_id
GROUP BY t.code, t.name
ORDER BY t.code;

-- 4. PostgreSQL specific validations
SELECT 'POSTGRESQL_SPECIFIC_CHECKS' as phase;

-- Check sequences are properly set
SELECT 
    schemaname,
    sequencename,
    last_value,
    start_value,
    increment_by
FROM pg_sequences 
WHERE schemaname = 'public'
ORDER BY sequencename;

-- Verify indexes exist
SELECT 
    indexname,
    tablename,
    indexdef
FROM pg_indexes 
WHERE schemaname = 'public' 
  AND indexname LIKE 'idx_%'
ORDER BY tablename, indexname;

-- Check constraints
SELECT 
    tc.constraint_name,
    tc.table_name,
    tc.constraint_type,
    cc.check_clause
FROM information_schema.table_constraints tc
LEFT JOIN information_schema.check_constraints cc 
    ON tc.constraint_name = cc.constraint_name
WHERE tc.table_schema = 'public' 
  AND tc.constraint_type IN ('CHECK', 'FOREIGN KEY', 'UNIQUE')
ORDER BY tc.table_name, tc.constraint_type;

-- ===============================================  
-- FUNCTIONAL TESTING QUERIES
-- ===============================================

-- Test critical business operations
SELECT 'FUNCTIONAL_TESTS' as phase;

-- 1. User authentication test data
SELECT 'Test user authentication' as test_name;
SELECT id, username, role 
FROM users 
WHERE role = 'Admin' 
LIMIT 1;

-- 2. Order creation workflow
SELECT 'Order workflow test' as test_name;
SELECT 
    o.id,
    o.status,
    COUNT(oi.id) as item_count,
    SUM(oi.quantity * oi.unit_price) as calculated_total,
    o.total_amount as stored_total,
    CASE 
        WHEN ABS(SUM(oi.quantity * oi.unit_price) - o.total_amount) < 0.01 THEN 'PASS'
        ELSE 'FAIL'
    END as total_validation
FROM orders o
LEFT JOIN order_items oi ON o.id = oi.order_id
GROUP BY o.id, o.status, o.total_amount
HAVING COUNT(oi.id) > 0
LIMIT 5;

-- 3. Stadium structure integrity
SELECT 'Stadium structure test' as test_name;
SELECT 
    'Tribune-Ring-Section-Seat hierarchy' as component,
    COUNT(*) as valid_seats
FROM stadium_seats st
JOIN stadium_sections ss ON st.section_id = ss.id
JOIN rings r ON ss.ring_id = r.id  
JOIN tribunes t ON r.tribune_id = t.id
WHERE t.code IN ('N', 'S', 'E', 'W');

-- 4. Event ticketing system
SELECT 'Event ticketing test' as test_name;
SELECT 
    e.name as event_name,
    COUNT(t.id) as ticket_count,
    COUNT(CASE WHEN t.status = 'Valid' THEN 1 END) as valid_tickets,
    COUNT(CASE WHEN t.status = 'Used' THEN 1 END) as used_tickets
FROM events e
LEFT JOIN tickets t ON e.id = t.event_id
GROUP BY e.id, e.name
LIMIT 3;

-- ===============================================
-- PERFORMANCE BASELINE TESTS  
-- ===============================================

SELECT 'PERFORMANCE_BASELINE' as phase;

-- Index usage verification (PostgreSQL specific)
EXPLAIN (ANALYZE, BUFFERS) 
SELECT * FROM users WHERE email = 'admin@stadium.com';

EXPLAIN (ANALYZE, BUFFERS)
SELECT o.*, u.username 
FROM orders o 
JOIN users u ON o.customer_id = u.id 
WHERE o.status = 'Pending'
LIMIT 10;

EXPLAIN (ANALYZE, BUFFERS)
SELECT ss.*, r.name as ring_name, t.name as tribune_name
FROM stadium_seats ss
JOIN stadium_sections sec ON ss.section_id = sec.id
JOIN rings r ON sec.ring_id = r.id
JOIN tribunes t ON r.tribune_id = t.id
WHERE t.code = 'N' AND r.number = 1
LIMIT 20;

-- Query performance comparison baseline
SELECT 'Query timing tests' as test_name;

-- Time complex aggregation query
\timing on
SELECT 
    u.role,
    COUNT(o.id) as order_count,
    AVG(o.total_amount) as avg_order_value,
    SUM(o.total_amount) as total_revenue
FROM users u
LEFT JOIN orders o ON u.id = o.customer_id
GROUP BY u.role;
\timing off