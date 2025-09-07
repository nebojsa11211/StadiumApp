#!/usr/bin/env python3
"""
Stadium Drink Ordering System - SQLite to Supabase Data Migration Script
=========================================================================
This script handles the complete data migration from SQLite to PostgreSQL (Supabase)
with proper error handling, validation, and progress tracking.
"""

import sqlite3
import psycopg2
import psycopg2.extras
import csv
import os
import sys
from datetime import datetime
import json
import logging

# Configuration
SQLITE_DB_PATH = 'StadiumDrinkOrdering.db'
EXPORT_DIR = 'migration-export'
LOG_FILE = 'migration.log'

# Supabase connection details (fill in your actual values)
SUPABASE_CONFIG = {
    'host': 'YOUR_SUPABASE_HOST.supabase.co',
    'database': 'postgres', 
    'user': 'postgres',
    'password': 'YOUR_SUPABASE_PASSWORD',
    'port': '5432'
}

# Table migration order (respects foreign key dependencies)
MIGRATION_ORDER = [
    'users',
    'drinks', 
    'tribunes',
    'rings',
    'stadium_sections',
    'stadium_seats',
    'events',
    'orders',
    'order_items',
    'tickets',
    'payments',
    'shopping_carts',
    'cart_items',
    'seat_reservations',
    'event_staff_assignments',
    'order_sessions',
    'ticket_sessions',
    'log_entries'
]

# Column mappings for tables that need transformation
COLUMN_MAPPINGS = {
    'users': {
        'Id': 'id',
        'Username': 'username',
        'PasswordHash': 'password_hash',
        'Email': 'email',
        'PhoneNumber': 'phone_number', 
        'Role': 'role',
        'CreatedAt': 'created_at'
    },
    'drinks': {
        'Id': 'id',
        'Name': 'name',
        'Description': 'description',
        'Price': 'price',
        'ImageUrl': 'image_url',
        'Category': 'category',
        'IsAvailable': 'is_available',
        'AlcoholContent': 'alcohol_content',
        'VolumeML': 'volume_ml'
    },
    'orders': {
        'Id': 'id',
        'CustomerId': 'customer_id',
        'SeatNumber': 'seat_number',
        'TicketNumber': 'ticket_number',
        'Status': 'status',
        'OrderDate': 'order_date',
        'TotalAmount': 'total_amount',
        'SpecialInstructions': 'special_instructions',
        'EstimatedDeliveryTime': 'estimated_delivery_time',
        'ActualDeliveryTime': 'actual_delivery_time'
    }
}

def setup_logging():
    """Setup logging configuration"""
    logging.basicConfig(
        level=logging.INFO,
        format='%(asctime)s - %(levelname)s - %(message)s',
        handlers=[
            logging.FileHandler(LOG_FILE),
            logging.StreamHandler(sys.stdout)
        ]
    )

def connect_sqlite():
    """Connect to SQLite database"""
    try:
        conn = sqlite3.connect(SQLITE_DB_PATH)
        conn.row_factory = sqlite3.Row
        logging.info(f"Connected to SQLite database: {SQLITE_DB_PATH}")
        return conn
    except sqlite3.Error as e:
        logging.error(f"Error connecting to SQLite: {e}")
        sys.exit(1)

def connect_supabase():
    """Connect to Supabase PostgreSQL database"""
    try:
        conn = psycopg2.connect(**SUPABASE_CONFIG)
        conn.autocommit = False
        logging.info("Connected to Supabase PostgreSQL database")
        return conn
    except psycopg2.Error as e:
        logging.error(f"Error connecting to Supabase: {e}")
        sys.exit(1)

def get_table_structure(sqlite_conn, table_name):
    """Get table structure from SQLite"""
    cursor = sqlite_conn.cursor()
    cursor.execute(f"PRAGMA table_info({table_name})")
    columns = cursor.fetchall()
    return [(col[1], col[2]) for col in columns]  # (name, type)

def export_table_data(sqlite_conn, table_name):
    """Export data from SQLite table to CSV"""
    cursor = sqlite_conn.cursor()
    
    # Get all data from table
    cursor.execute(f"SELECT * FROM {table_name}")
    rows = cursor.fetchall()
    
    if not rows:
        logging.warning(f"Table {table_name} is empty, skipping export")
        return None
    
    # Create export directory if it doesn't exist
    os.makedirs(EXPORT_DIR, exist_ok=True)
    
    # Export to CSV
    csv_file = os.path.join(EXPORT_DIR, f"{table_name.lower()}.csv")
    with open(csv_file, 'w', newline='', encoding='utf-8') as f:
        writer = csv.writer(f)
        
        # Write headers
        headers = [description[0] for description in cursor.description]
        writer.writerow(headers)
        
        # Write data
        for row in rows:
            writer.writerow(row)
    
    logging.info(f"Exported {len(rows)} rows from {table_name} to {csv_file}")
    return csv_file

def transform_column_names(row_dict, table_name):
    """Transform SQLite column names to PostgreSQL column names"""
    if table_name in COLUMN_MAPPINGS:
        mapping = COLUMN_MAPPINGS[table_name]
        transformed = {}
        for old_name, value in row_dict.items():
            new_name = mapping.get(old_name, old_name.lower())
            transformed[new_name] = value
        return transformed
    else:
        # Default: convert to lowercase
        return {k.lower(): v for k, v in row_dict.items()}

def import_table_data(pg_conn, table_name, csv_file):
    """Import data from CSV to PostgreSQL table"""
    if not csv_file or not os.path.exists(csv_file):
        logging.warning(f"CSV file not found for {table_name}, skipping import")
        return 0
    
    cursor = pg_conn.cursor()
    imported_count = 0
    error_count = 0
    
    try:
        with open(csv_file, 'r', encoding='utf-8') as f:
            reader = csv.DictReader(f)
            
            for row_num, row in enumerate(reader, 1):
                try:
                    # Transform column names
                    transformed_row = transform_column_names(row, table_name)
                    
                    # Remove None/empty values and prepare for insertion
                    clean_row = {k: (v if v != '' else None) for k, v in transformed_row.items()}
                    
                    # Build INSERT query
                    columns = list(clean_row.keys())
                    placeholders = ['%s'] * len(columns)
                    values = list(clean_row.values())
                    
                    query = f"""
                    INSERT INTO {table_name.lower()} ({', '.join(columns)}) 
                    VALUES ({', '.join(placeholders)})
                    """
                    
                    cursor.execute(query, values)
                    imported_count += 1
                    
                except Exception as e:
                    error_count += 1
                    logging.error(f"Error importing row {row_num} into {table_name}: {e}")
                    logging.error(f"Row data: {row}")
                    
                    # Continue with next row instead of failing completely
                    continue
        
        # Commit the transaction for this table
        pg_conn.commit()
        logging.info(f"Successfully imported {imported_count} rows into {table_name}")
        
        if error_count > 0:
            logging.warning(f"Encountered {error_count} errors during {table_name} import")
            
    except Exception as e:
        pg_conn.rollback()
        logging.error(f"Fatal error importing {table_name}: {e}")
        return 0
    
    return imported_count

def reset_sequences(pg_conn, table_names):
    """Reset PostgreSQL sequences to match imported data"""
    cursor = pg_conn.cursor()
    
    for table_name in table_names:
        try:
            # Get current max ID from table
            cursor.execute(f"SELECT COALESCE(MAX(id), 0) + 1 FROM {table_name.lower()}")
            next_val = cursor.fetchone()[0]
            
            # Reset sequence
            sequence_name = f"{table_name.lower()}_id_seq"
            cursor.execute(f"ALTER SEQUENCE {sequence_name} RESTART WITH {next_val}")
            
            logging.info(f"Reset sequence {sequence_name} to {next_val}")
            
        except Exception as e:
            logging.warning(f"Could not reset sequence for {table_name}: {e}")
    
    pg_conn.commit()

def validate_migration(sqlite_conn, pg_conn):
    """Validate that migration was successful by comparing record counts"""
    logging.info("Validating migration...")
    
    sqlite_cursor = sqlite_conn.cursor()
    pg_cursor = pg_conn.cursor()
    
    validation_results = {}
    
    for table_name in MIGRATION_ORDER:
        try:
            # Get SQLite count
            sqlite_cursor.execute(f"SELECT COUNT(*) FROM {table_name}")
            sqlite_count = sqlite_cursor.fetchone()[0]
            
            # Get PostgreSQL count  
            pg_cursor.execute(f"SELECT COUNT(*) FROM {table_name.lower()}")
            pg_count = pg_cursor.fetchone()[0]
            
            validation_results[table_name] = {
                'sqlite_count': sqlite_count,
                'postgresql_count': pg_count,
                'match': sqlite_count == pg_count
            }
            
            status = "✅ MATCH" if sqlite_count == pg_count else "❌ MISMATCH"
            logging.info(f"{table_name}: SQLite={sqlite_count}, PostgreSQL={pg_count} {status}")
            
        except Exception as e:
            logging.error(f"Error validating {table_name}: {e}")
            validation_results[table_name] = {'error': str(e)}
    
    return validation_results

def create_migration_report(validation_results, start_time, end_time):
    """Create a detailed migration report"""
    report = {
        'migration_timestamp': start_time.isoformat(),
        'duration_seconds': (end_time - start_time).total_seconds(),
        'validation_results': validation_results,
        'total_tables': len(MIGRATION_ORDER),
        'successful_tables': sum(1 for result in validation_results.values() 
                                if result.get('match', False)),
        'failed_tables': sum(1 for result in validation_results.values() 
                           if not result.get('match', True)),
    }
    
    report_file = f"migration-report-{start_time.strftime('%Y%m%d-%H%M%S')}.json"
    with open(report_file, 'w') as f:
        json.dump(report, f, indent=2)
    
    logging.info(f"Migration report saved to {report_file}")
    return report

def main():
    """Main migration process"""
    setup_logging()
    start_time = datetime.now()
    
    logging.info("="*50)
    logging.info("Stadium Drink Ordering System - Database Migration")
    logging.info("="*50)
    
    # Connect to databases
    sqlite_conn = connect_sqlite()
    pg_conn = connect_supabase()
    
    try:
        total_imported = 0
        
        # Process each table in dependency order
        for table_name in MIGRATION_ORDER:
            logging.info(f"\n--- Processing table: {table_name} ---")
            
            # Export from SQLite
            csv_file = export_table_data(sqlite_conn, table_name)
            
            # Import to PostgreSQL
            if csv_file:
                imported = import_table_data(pg_conn, table_name, csv_file)
                total_imported += imported
        
        # Reset sequences to prevent ID conflicts
        logging.info("\n--- Resetting PostgreSQL sequences ---")
        reset_sequences(pg_conn, MIGRATION_ORDER)
        
        # Validate migration
        logging.info("\n--- Validating migration ---")
        validation_results = validate_migration(sqlite_conn, pg_conn)
        
        end_time = datetime.now()
        
        # Generate report
        report = create_migration_report(validation_results, start_time, end_time)
        
        # Summary
        logging.info("\n" + "="*50)
        logging.info("MIGRATION SUMMARY")
        logging.info("="*50)
        logging.info(f"Duration: {report['duration_seconds']:.2f} seconds")
        logging.info(f"Total records imported: {total_imported}")
        logging.info(f"Successful tables: {report['successful_tables']}/{report['total_tables']}")
        
        if report['failed_tables'] == 0:
            logging.info("🎉 Migration completed successfully!")
        else:
            logging.warning(f"⚠️ Migration completed with {report['failed_tables']} table(s) having issues")
            
    except Exception as e:
        logging.error(f"Migration failed: {e}")
        sys.exit(1)
    
    finally:
        sqlite_conn.close()
        pg_conn.close()
        logging.info("Database connections closed")

if __name__ == "__main__":
    main()