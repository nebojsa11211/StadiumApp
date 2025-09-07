-- =============================================
-- Supabase PostgreSQL Schema Creation Script
-- Stadium Drink Ordering System Migration
-- =============================================

-- Enable necessary extensions
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-- =============================================
-- 1. USERS AND AUTHENTICATION
-- =============================================

CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    phone_number VARCHAR(20),
    role VARCHAR(20) NOT NULL DEFAULT 'Customer' CHECK (role IN ('Admin', 'Bartender', 'Waiter', 'Customer')),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- =============================================
-- 2. DRINKS CATALOG
-- =============================================

CREATE TABLE drinks (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    description TEXT,
    price NUMERIC(10,2) NOT NULL CHECK (price >= 0),
    image_url TEXT,
    category VARCHAR(50) DEFAULT 'Beverage',
    is_available BOOLEAN DEFAULT TRUE,
    alcohol_content NUMERIC(4,2) DEFAULT 0 CHECK (alcohol_content >= 0 AND alcohol_content <= 100),
    volume_ml INTEGER DEFAULT 0,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- =============================================
-- 3. STADIUM STRUCTURE (Hierarchical)
-- =============================================

-- Tribunes (Main sections: North, South, East, West)
CREATE TABLE tribunes (
    id SERIAL PRIMARY KEY,
    code VARCHAR(1) NOT NULL UNIQUE CHECK (code IN ('N', 'S', 'E', 'W')),
    name VARCHAR(100) NOT NULL,
    description TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Rings within tribunes (levels/tiers)
CREATE TABLE rings (
    id SERIAL PRIMARY KEY,
    tribune_id INTEGER NOT NULL REFERENCES tribunes(id) ON DELETE CASCADE,
    number INTEGER NOT NULL CHECK (number > 0),
    name VARCHAR(100) NOT NULL,
    description TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    UNIQUE(tribune_id, number)
);

-- Sections within rings
CREATE TABLE stadium_sections (
    id SERIAL PRIMARY KEY,
    ring_id INTEGER NOT NULL REFERENCES rings(id) ON DELETE CASCADE,
    code VARCHAR(20) NOT NULL,
    name VARCHAR(100) NOT NULL,
    description TEXT,
    max_capacity INTEGER NOT NULL DEFAULT 0 CHECK (max_capacity >= 0),
    base_price NUMERIC(10,2) DEFAULT 0,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    UNIQUE(ring_id, code)
);

-- Individual seats
CREATE TABLE stadium_seats (
    id SERIAL PRIMARY KEY,
    section_id INTEGER NOT NULL REFERENCES stadium_sections(id) ON DELETE CASCADE,
    row_number INTEGER NOT NULL CHECK (row_number > 0),
    seat_number INTEGER NOT NULL CHECK (seat_number > 0),
    seat_code VARCHAR(10) NOT NULL,
    is_accessible BOOLEAN DEFAULT FALSE,
    price_tier VARCHAR(20) DEFAULT 'Standard' CHECK (price_tier IN ('Premium', 'Standard', 'Economy')),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    UNIQUE(section_id, row_number, seat_number),
    UNIQUE(section_id, seat_code)
);

-- =============================================
-- 4. EVENTS AND TICKETS
-- =============================================

CREATE TABLE events (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200) NOT NULL,
    description TEXT,
    event_date TIMESTAMP WITH TIME ZONE NOT NULL,
    end_date TIMESTAMP WITH TIME ZONE,
    venue VARCHAR(200),
    category VARCHAR(50) DEFAULT 'Sports',
    base_price NUMERIC(10,2) NOT NULL DEFAULT 0,
    max_capacity INTEGER DEFAULT 0,
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

CREATE TABLE tickets (
    id SERIAL PRIMARY KEY,
    event_id INTEGER NOT NULL REFERENCES events(id) ON DELETE CASCADE,
    seat_id INTEGER NOT NULL REFERENCES stadium_seats(id),
    ticket_number VARCHAR(50) NOT NULL UNIQUE,
    price NUMERIC(10,2) NOT NULL CHECK (price >= 0),
    status VARCHAR(20) DEFAULT 'Valid' CHECK (status IN ('Valid', 'Used', 'Cancelled', 'Expired')),
    purchase_date TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    qr_code TEXT,
    customer_name VARCHAR(100),
    customer_email VARCHAR(255),
    customer_phone VARCHAR(20)
);

-- =============================================
-- 5. ORDERS AND PAYMENTS
-- =============================================

CREATE TABLE orders (
    id SERIAL PRIMARY KEY,
    customer_id INTEGER NOT NULL REFERENCES users(id),
    seat_number VARCHAR(20),
    ticket_number VARCHAR(50),
    status VARCHAR(20) NOT NULL DEFAULT 'Pending' 
        CHECK (status IN ('Pending', 'Accepted', 'In Preparation', 'Ready', 'Out for Delivery', 'Delivered', 'Cancelled')),
    order_date TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    total_amount NUMERIC(10,2) NOT NULL DEFAULT 0 CHECK (total_amount >= 0),
    special_instructions TEXT,
    estimated_delivery_time TIMESTAMP WITH TIME ZONE,
    actual_delivery_time TIMESTAMP WITH TIME ZONE,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

CREATE TABLE order_items (
    id SERIAL PRIMARY KEY,
    order_id INTEGER NOT NULL REFERENCES orders(id) ON DELETE CASCADE,
    drink_id INTEGER NOT NULL REFERENCES drinks(id),
    quantity INTEGER NOT NULL CHECK (quantity > 0),
    unit_price NUMERIC(10,2) NOT NULL CHECK (unit_price >= 0),
    subtotal NUMERIC(10,2) NOT NULL CHECK (subtotal >= 0),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

CREATE TABLE payments (
    id SERIAL PRIMARY KEY,
    order_id INTEGER REFERENCES orders(id),
    ticket_id INTEGER REFERENCES tickets(id),
    amount NUMERIC(10,2) NOT NULL CHECK (amount >= 0),
    payment_method VARCHAR(50) NOT NULL DEFAULT 'CreditCard',
    payment_status VARCHAR(20) DEFAULT 'Pending' 
        CHECK (payment_status IN ('Pending', 'Completed', 'Failed', 'Refunded')),
    transaction_id VARCHAR(100),
    payment_date TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    metadata_json TEXT
);

-- =============================================
-- 6. SHOPPING CART SYSTEM
-- =============================================

CREATE TABLE shopping_carts (
    id SERIAL PRIMARY KEY,
    session_id VARCHAR(50) NOT NULL UNIQUE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    expires_at TIMESTAMP WITH TIME ZONE DEFAULT (NOW() + INTERVAL '15 minutes')
);

CREATE TABLE cart_items (
    id SERIAL PRIMARY KEY,
    cart_id INTEGER NOT NULL REFERENCES shopping_carts(id) ON DELETE CASCADE,
    event_id INTEGER NOT NULL REFERENCES events(id),
    seat_id INTEGER NOT NULL REFERENCES stadium_seats(id),
    price NUMERIC(10,2) NOT NULL CHECK (price >= 0),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    UNIQUE(cart_id, seat_id)
);

-- Temporary seat reservations
CREATE TABLE seat_reservations (
    id SERIAL PRIMARY KEY,
    seat_id INTEGER NOT NULL REFERENCES stadium_seats(id),
    event_id INTEGER NOT NULL REFERENCES events(id),
    session_id VARCHAR(50) NOT NULL,
    reserved_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    expires_at TIMESTAMP WITH TIME ZONE DEFAULT (NOW() + INTERVAL '15 minutes'),
    UNIQUE(seat_id, event_id)
);

-- =============================================
-- 7. LOGGING SYSTEM
-- =============================================

CREATE TABLE log_entries (
    id SERIAL PRIMARY KEY,
    timestamp TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    level VARCHAR(20) NOT NULL DEFAULT 'Info' 
        CHECK (level IN ('Debug', 'Info', 'Warning', 'Error', 'Critical')),
    message TEXT NOT NULL,
    category VARCHAR(100),
    action VARCHAR(100),
    user_id INTEGER REFERENCES users(id),
    user_email VARCHAR(255),
    user_role VARCHAR(50),
    source VARCHAR(50) DEFAULT 'API',
    request_path TEXT,
    http_method VARCHAR(10),
    ip_address INET,
    user_agent TEXT,
    business_entity_type VARCHAR(50),
    business_entity_id VARCHAR(50),
    business_entity_name VARCHAR(200),
    monetary_amount NUMERIC(10,2),
    currency VARCHAR(3) DEFAULT 'USD',
    quantity INTEGER,
    status_before VARCHAR(50),
    status_after VARCHAR(50),
    details TEXT,
    metadata_json JSONB
);

-- =============================================
-- 8. EVENT STAFF ASSIGNMENTS
-- =============================================

CREATE TABLE event_staff_assignments (
    id SERIAL PRIMARY KEY,
    event_id INTEGER NOT NULL REFERENCES events(id) ON DELETE CASCADE,
    user_id INTEGER NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    role VARCHAR(50) NOT NULL,
    section_id INTEGER REFERENCES stadium_sections(id),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    UNIQUE(event_id, user_id, role)
);

-- =============================================
-- 9. ORDER SESSIONS
-- =============================================

CREATE TABLE order_sessions (
    id SERIAL PRIMARY KEY,
    session_id VARCHAR(50) NOT NULL UNIQUE,
    customer_email VARCHAR(255),
    customer_name VARCHAR(100),
    customer_phone VARCHAR(20),
    total_amount NUMERIC(10,2) DEFAULT 0,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    expires_at TIMESTAMP WITH TIME ZONE DEFAULT (NOW() + INTERVAL '30 minutes')
);

-- =============================================
-- 10. TICKET AUTHENTICATION SESSIONS
-- =============================================

CREATE TABLE ticket_sessions (
    id SERIAL PRIMARY KEY,
    ticket_number VARCHAR(50) NOT NULL,
    session_token VARCHAR(100) NOT NULL UNIQUE,
    authenticated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    expires_at TIMESTAMP WITH TIME ZONE DEFAULT (NOW() + INTERVAL '4 hours'),
    ip_address INET,
    user_agent TEXT
);

-- =============================================
-- INDEXES FOR PERFORMANCE
-- =============================================

-- User indexes
CREATE INDEX idx_users_email ON users(email);
CREATE INDEX idx_users_role ON users(role);
CREATE INDEX idx_users_created_at ON users(created_at);

-- Order indexes
CREATE INDEX idx_orders_customer_id ON orders(customer_id);
CREATE INDEX idx_orders_status ON orders(status);
CREATE INDEX idx_orders_order_date ON orders(order_date);
CREATE INDEX idx_orders_ticket_number ON orders(ticket_number);

-- Order items indexes
CREATE INDEX idx_order_items_order_id ON order_items(order_id);
CREATE INDEX idx_order_items_drink_id ON order_items(drink_id);

-- Stadium structure indexes
CREATE INDEX idx_rings_tribune_id ON rings(tribune_id);
CREATE INDEX idx_stadium_sections_ring_id ON stadium_sections(ring_id);
CREATE INDEX idx_stadium_seats_section_id ON stadium_seats(section_id);
CREATE INDEX idx_stadium_seats_section_row ON stadium_seats(section_id, row_number);

-- Event and ticket indexes
CREATE INDEX idx_events_event_date ON events(event_date);
CREATE INDEX idx_events_is_active ON events(is_active);
CREATE INDEX idx_tickets_event_id ON tickets(event_id);
CREATE INDEX idx_tickets_seat_id ON tickets(seat_id);
CREATE INDEX idx_tickets_ticket_number ON tickets(ticket_number);
CREATE INDEX idx_tickets_status ON tickets(status);

-- Cart and reservation indexes
CREATE INDEX idx_shopping_carts_session_id ON shopping_carts(session_id);
CREATE INDEX idx_cart_items_cart_id ON cart_items(cart_id);
CREATE INDEX idx_seat_reservations_expires_at ON seat_reservations(expires_at);
CREATE INDEX idx_seat_reservations_session_id ON seat_reservations(session_id);

-- Logging indexes
CREATE INDEX idx_log_entries_timestamp ON log_entries(timestamp);
CREATE INDEX idx_log_entries_level ON log_entries(level);
CREATE INDEX idx_log_entries_category ON log_entries(category);
CREATE INDEX idx_log_entries_user_id ON log_entries(user_id);
CREATE INDEX idx_log_entries_business_entity ON log_entries(business_entity_type, business_entity_id);

-- Payment indexes
CREATE INDEX idx_payments_order_id ON payments(order_id);
CREATE INDEX idx_payments_ticket_id ON payments(ticket_id);
CREATE INDEX idx_payments_payment_status ON payments(payment_status);
CREATE INDEX idx_payments_payment_date ON payments(payment_date);

-- =============================================
-- TRIGGERS FOR AUTO-UPDATE TIMESTAMPS
-- =============================================

CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = NOW();
    RETURN NEW;
END;
$$ language 'plpgsql';

-- Apply trigger to relevant tables
CREATE TRIGGER update_users_updated_at BEFORE UPDATE ON users 
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_orders_updated_at BEFORE UPDATE ON orders 
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_shopping_carts_updated_at BEFORE UPDATE ON shopping_carts 
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

-- =============================================
-- ROW LEVEL SECURITY (Optional)
-- =============================================

-- Enable RLS on sensitive tables
ALTER TABLE users ENABLE ROW LEVEL SECURITY;
ALTER TABLE orders ENABLE ROW LEVEL SECURITY;
ALTER TABLE tickets ENABLE ROW LEVEL SECURITY;

-- Example policies (customize based on your auth setup)
-- Users can only see their own records
CREATE POLICY users_own_records ON users 
    FOR ALL USING (id = current_setting('app.current_user_id')::INTEGER);

-- Customers can only see their own orders
CREATE POLICY orders_own_records ON orders 
    FOR SELECT USING (customer_id = current_setting('app.current_user_id')::INTEGER);

-- =============================================
-- INITIAL DATA SEEDING
-- =============================================

-- Insert default drinks
INSERT INTO drinks (name, description, price, category, volume_ml) VALUES
('Beer', 'Premium lager beer', 5.50, 'Alcoholic', 500),
('Coca Cola', 'Classic soft drink', 3.00, 'Soft Drink', 330),
('Water', 'Bottled spring water', 2.00, 'Water', 500),
('Coffee', 'Hot coffee', 3.50, 'Hot Beverage', 250),
('Wine', 'House red wine', 8.00, 'Alcoholic', 150),
('Orange Juice', 'Fresh orange juice', 4.00, 'Juice', 330);

-- Insert default stadium structure (basic example)
INSERT INTO tribunes (code, name, description) VALUES
('N', 'North Tribune', 'North side seating'),
('S', 'South Tribune', 'South side seating'),
('E', 'East Tribune', 'East side seating'),
('W', 'West Tribune', 'West side seating');

-- Insert rings for each tribune
INSERT INTO rings (tribune_id, number, name) 
SELECT t.id, 1, t.name || ' - Lower Ring' FROM tribunes t
UNION ALL
SELECT t.id, 2, t.name || ' - Upper Ring' FROM tribunes t;

-- Create admin user (password should be properly hashed in production)
INSERT INTO users (username, password_hash, email, role) VALUES
('admin', '$2a$11$...[properly_hashed_password]', 'admin@stadium.com', 'Admin');

COMMIT;