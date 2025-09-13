# Stadium Drink Ordering System - Comprehensive Test Plan

## Overview
This document outlines all necessary tests for the Stadium Drink Ordering System, covering API functionality, UI/UX testing, integration tests, and end-to-end user workflows across all three applications: Customer, Admin, and Staff.

## Test Categories

### 🔧 Test Types Legend
- **API**: Direct API endpoint testing (curl/Postman)
- **PW**: Playwright automated UI testing
- **UNIT**: Unit testing (.NET Test Framework)
- **INT**: Integration testing
- **E2E**: End-to-end workflow testing
- **MAN**: Manual testing required

---

## 1. API Backend Tests (StadiumDrinkOrdering.API)

### 1.1 Authentication & Authorization
| Test Case | Type | Description | Expected Result |
|-----------|------|-------------|----------------|
| AUTH-001 | API | POST /api/auth/login with valid admin credentials | Returns JWT token, 200 OK |
| AUTH-002 | API | POST /api/auth/login with invalid credentials | Returns 401 Unauthorized |
| AUTH-003 | API | POST /api/auth/register with valid data | Creates user, returns 201 |
| AUTH-004 | API | Access protected endpoint without token | Returns 401 Unauthorized |
| AUTH-005 | API | Access admin endpoint with staff token | Returns 403 Forbidden |
| AUTH-006 | API | Token expiration handling | Returns 401 after token expires |

### 1.2 Events Management API
| Test Case | Type | Description | Expected Result |
|-----------|------|-------------|----------------|
| EVENT-001 | API | GET /api/event - List all events | Returns events array, 200 OK |
| EVENT-002 | API | GET /api/event/{id} - Get specific event | Returns event details, 200 OK |
| EVENT-003 | API | POST /api/event - Create new event | Creates event, returns 201 |
| EVENT-004 | API | PUT /api/event/{id} - Update event | Updates event, returns 200 OK |
| EVENT-005 | API | DELETE /api/event/{id} - Delete event | Deletes event, returns 204 |
| EVENT-006 | API | POST /api/event/{id}/activate - Activate event | Event becomes active, 200 OK |
| EVENT-007 | API | POST /api/event/{id}/deactivate - Deactivate event | Event becomes inactive, 200 OK |

### 1.3 Demo Data Generation API
| Test Case | Type | Description | Expected Result |
|-----------|------|-------------|----------------|
| DEMO-001 | API | POST /api/demo-data/generate-comprehensive | Generates full demo dataset, 200 OK |
| DEMO-002 | API | POST /api/demo-data/generate-event - With event data | Creates event with tickets, 200 OK |
| DEMO-003 | API | POST /api/demo-data/generate/{eventId} - For specific event | Generates data for event, 200 OK |
| DEMO-004 | API | POST /api/demo-data/generate-orders - Generate test orders | Creates test orders, 200 OK |
| DEMO-005 | API | POST /api/demo-data/generate-staff-assignments | Creates staff assignments, 200 OK |
| DEMO-006 | API | DELETE /api/demo-data/clear | Clears all demo data, 200 OK |
| DEMO-007 | API | POST /api/demo-data/generate-seat-mappings | Generates seat mappings, 200 OK |

### 1.4 Customer Ticketing API
| Test Case | Type | Description | Expected Result |
|-----------|------|-------------|----------------|
| TICKET-001 | API | GET /api/customer/ticketing/events | Returns active events list |
| TICKET-002 | API | GET /api/customer/ticketing/events/{eventId} | Returns event details with pricing |
| TICKET-003 | API | GET /api/customer/ticketing/events/{eventId}/sections/{sectionId}/availability | Returns seat availability |

### 1.5 Shopping Cart API
| Test Case | Type | Description | Expected Result |
|-----------|------|-------------|----------------|
| CART-001 | API | GET /api/customer/cart?sessionId={id} | Returns cart contents |
| CART-002 | API | POST /api/customer/cart/add - Add seat to cart | Adds seat, reserves it for 15 min |
| CART-003 | API | DELETE /api/customer/cart/remove - Remove seat | Removes seat, releases reservation |
| CART-004 | API | DELETE /api/customer/cart/clear?sessionId={id} | Clears entire cart |
| CART-005 | API | GET /api/customer/cart/summary?sessionId={id} | Returns cart summary with totals |
| CART-006 | API | GET /api/customer/cart/seat-availability | Checks individual seat availability |

### 1.6 Order Processing API
| Test Case | Type | Description | Expected Result |
|-----------|------|-------------|----------------|
| ORDER-001 | API | POST /api/customer/orders/create | Processes ticket order, creates tickets |
| ORDER-002 | API | GET /api/customer/orders/{orderId}/confirmation | Returns order confirmation details |
| ORDER-003 | API | GET /api/customer/orders/my-orders?email={email} | Returns customer order history |

### 1.7 Stadium Structure API
| Test Case | Type | Description | Expected Result |
|-----------|------|-------------|----------------|
| STADIUM-001 | API | GET /api/stadium-structure | Returns stadium structure |
| STADIUM-002 | API | POST /api/stadium-structure/import - With JSON file | Imports structure, 200 OK |
| STADIUM-003 | API | GET /api/stadium-structure/export | Exports structure as JSON |

### 1.8 DataGrid API
| Test Case | Type | Description | Expected Result |
|-----------|------|-------------|----------------|
| DATAGRID-001 | API | GET /api/datagrid/tables | Returns list of database tables with metadata |
| DATAGRID-002 | API | GET /api/datagrid/table-data/{tableName} | Returns paginated table data |
| DATAGRID-003 | API | GET /api/datagrid/table-data/{tableName}?page=2 | Returns second page of data |
| DATAGRID-004 | API | GET /api/datagrid/table-data/{tableName}?sortColumn=Id&sortDirection=desc | Returns sorted data |
| DATAGRID-005 | API | GET /api/datagrid/table-data/{tableName}?filters={"Name":"test"} | Returns filtered data |
| DATAGRID-006 | API | GET /api/datagrid/export/{tableName} | Returns CSV export of table data |
| DATAGRID-007 | API | GET /api/datagrid/tables without auth token | Returns 401 Unauthorized |
| DATAGRID-008 | API | GET /api/datagrid/table-data/NonExistentTable | Returns appropriate error |

### 1.9 Centralized Logging API
| Test Case | Type | Description | Expected Result |
|-----------|------|-------------|----------------|
| LOG-001 | API | POST /api/logs/log-action with valid data | Logs action successfully, returns 200 OK |
| LOG-002 | API | POST /api/logs/log-action without auth token | Returns 200 OK (allows anonymous logging) |
| LOG-003 | API | POST /api/logs/log-batch with multiple entries | Processes batch successfully, returns success count |
| LOG-004 | API | POST /api/logs/log-batch with empty array | Returns 400 Bad Request |
| LOG-005 | API | POST /api/logs/log-batch with 150 entries | Processes only first 100, returns batch info |
| LOG-006 | API | POST /api/logs/search with admin token | Returns paginated logs, 200 OK |
| LOG-007 | API | POST /api/logs/search with customer token | Returns 403 Forbidden |
| LOG-008 | API | GET /api/logs/summary with admin token | Returns log statistics, 200 OK |
| LOG-009 | API | DELETE /api/logs/clear-old with admin token | Clears old logs, returns success message |
| LOG-010 | API | DELETE /api/logs/clear-old with staff token | Returns 403 Forbidden |
| LOG-011 | API | POST /api/logs/search with filters | Returns filtered results |
| LOG-012 | API | POST /api/logs/search with date range | Returns logs within specified dates |
| LOG-013 | API | POST /api/logs/search with pagination | Returns correct page of results |
| LOG-014 | API | Trigger exception in any endpoint | Automatic log entry created via middleware |
| LOG-015 | API | High-volume logging (1000+ requests/min) | Batch processing handles load efficiently |

---

## 2. Customer Application Tests (StadiumDrinkOrdering.Customer)

### 2.1 Navigation & Layout
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| CUST-NAV-001 | PW | Main navigation menu visibility | Navigate to homepage | All menu items visible and clickable |
| CUST-NAV-002 | PW | Logo navigation to home | Click Stadium logo | Redirects to homepage |
| CUST-NAV-003 | PW | Mobile responsive navigation | Resize to mobile view | Menu collapses to hamburger |
| CUST-NAV-004 | PW | Authentication state in nav | Login/logout actions | Shows appropriate auth buttons |

### 2.2 Authentication Pages
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| CUST-AUTH-001 | PW | Login page layout | Navigate to /login | Form displays with email/password fields |
| CUST-AUTH-002 | PW | Valid login process | Enter valid credentials, click Login | Redirects to dashboard/home |
| CUST-AUTH-003 | PW | Invalid login handling | Enter invalid credentials | Shows error message |
| CUST-AUTH-004 | PW | Registration page layout | Navigate to /register | Form displays with all required fields |
| CUST-AUTH-005 | PW | Valid registration | Fill form with valid data, submit | Creates account, logs in user |
| CUST-AUTH-006 | PW | Registration validation | Submit form with invalid data | Shows validation errors |
| CUST-AUTH-007 | PW | Logout functionality | Click logout button | Clears session, redirects to home |

### 2.3 Events Page
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| CUST-EVENT-001 | PW | Events list display | Navigate to /events | Shows available events with details |
| CUST-EVENT-002 | PW | Event filtering by date | Use date range filter | Shows events in selected range |
| CUST-EVENT-003 | PW | Event filtering by type | Select event type filter | Shows only events of selected type |
| CUST-EVENT-004 | PW | Event search functionality | Enter search term | Shows matching events |
| CUST-EVENT-005 | PW | Empty events state | Clear all events from DB | Shows "No events" message |
| CUST-EVENT-006 | PW | Event card interactions | Click on event card | Navigates to event details |

### 2.4 Event Details & Seat Selection
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| CUST-SEAT-001 | PW | Event details page display | Navigate to /event-details/{id} | Shows complete event information |
| CUST-SEAT-002 | PW | Stadium sections display | View event details | Shows stadium section overview |
| CUST-SEAT-003 | PW | Seat selection modal | Click "Select Seats" on section | Opens seat selection modal |
| CUST-SEAT-004 | PW | Individual seat selection | Click on available seat | Seat becomes selected |
| CUST-SEAT-005 | PW | Multiple seat selection | Select multiple seats | All seats become selected |
| CUST-SEAT-006 | PW | Seat deselection | Click selected seat | Seat becomes deselected |
| CUST-SEAT-007 | PW | Unavailable seat handling | Click on taken seat | No selection, shows unavailable state |
| CUST-SEAT-008 | PW | Price calculation | Select seats | Shows correct total price |
| CUST-SEAT-009 | PW | Add to cart functionality | Select seats, click "Add to Cart" | Seats added to cart |
| CUST-SEAT-010 | PW | Cart quantity update | Add seats to cart | Cart icon shows correct count |

### 2.5 Shopping Cart
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| CUST-CART-001 | PW | Cart page display | Navigate to cart | Shows cart items with details |
| CUST-CART-002 | PW | Empty cart state | Clear all items | Shows "Cart is empty" message |
| CUST-CART-003 | PW | Item removal | Click remove button | Item removed from cart |
| CUST-CART-004 | PW | Quantity modification | Change item quantity | Updates price totals |
| CUST-CART-005 | PW | Price calculations | Add multiple items | Shows correct subtotal, fees, total |
| CUST-CART-006 | PW | Seat reservation timeout | Wait 15+ minutes | Shows expiration warning |
| CUST-CART-007 | PW | Proceed to checkout | Click "Checkout" button | Navigates to checkout page |

### 2.6 Checkout Process
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| CUST-CHECK-001 | PW | Checkout page layout | Navigate to /checkout | Shows customer info and payment forms |
| CUST-CHECK-002 | PW | Customer information form | Fill customer details | Form accepts and validates data |
| CUST-CHECK-003 | PW | Payment information form | Fill payment details | Form accepts and validates data |
| CUST-CHECK-004 | PW | Order summary display | View checkout page | Shows correct items and pricing |
| CUST-CHECK-005 | PW | Form validation | Submit with invalid data | Shows validation errors |
| CUST-CHECK-006 | PW | Successful order placement | Complete valid checkout | Creates order, navigates to confirmation |
| CUST-CHECK-007 | PW | Payment processing error | Simulate payment failure | Shows error message |
| CUST-CHECK-008 | PW | Terms acceptance | Try to submit without accepting terms | Shows validation error |

### 2.7 Order Confirmation
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| CUST-CONF-001 | PW | Confirmation page display | Complete order | Shows order confirmation details |
| CUST-CONF-002 | PW | Ticket details display | View confirmation | Shows individual tickets with QR codes |
| CUST-CONF-003 | PW | Order summary accuracy | Check confirmation details | All details match order placed |
| CUST-CONF-004 | PW | Print functionality | Click print button | Opens print dialog |
| CUST-CONF-005 | PW | Download functionality | Click download button | Downloads ticket PDF |
| CUST-CONF-006 | PW | Email functionality | Click email button | Sends confirmation email |

### 2.8 Order History
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| CUST-HIST-001 | PW | Order history access | Navigate to order history | Shows past orders |
| CUST-HIST-002 | PW | Order details view | Click on order | Shows detailed order information |
| CUST-HIST-003 | PW | Empty history state | No previous orders | Shows "No orders" message |
| CUST-HIST-004 | PW | Order search/filter | Search for specific order | Shows matching results |

---

## 3. Admin Application Tests (StadiumDrinkOrdering.Admin)

### 3.1 Authentication & Navigation
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| ADMIN-AUTH-001 | PW | Admin login page | Navigate to /login | Shows login form with demo credentials |
| ADMIN-AUTH-002 | PW | Valid admin login | Login with admin@stadium.com/admin123 | Redirects to dashboard |
| ADMIN-AUTH-003 | PW | Navigation menu visibility | After login | Shows all admin navigation items |
| ADMIN-AUTH-004 | PW | Role-based access | Login as non-admin | Restricted access to admin features |

### 3.2 Dashboard
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| ADMIN-DASH-001 | PW | Dashboard layout | Navigate to dashboard | Shows overview cards and quick actions |
| ADMIN-DASH-002 | PW | Statistics display | View dashboard | Shows current order/revenue stats |
| ADMIN-DASH-003 | PW | Quick action buttons | Click dashboard buttons | Navigate to respective sections |
| ADMIN-DASH-004 | PW | SignalR connection status | Check connection indicator | Shows "Connected" status |

### 3.3 Event Management
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| ADMIN-EVENT-001 | PW | Events page layout | Navigate to /events | Shows event management interface |
| ADMIN-EVENT-002 | PW | Create new event button | Click "Create New Event" | Opens event creation modal |
| ADMIN-EVENT-003 | PW | Event creation form | Fill and submit form | Creates new event |
| ADMIN-EVENT-004 | PW | Form validation | Submit invalid data | Shows validation errors |
| ADMIN-EVENT-005 | PW | Event editing | Click edit on existing event | Opens edit modal with populated data |
| ADMIN-EVENT-006 | PW | Event update | Modify and save event | Updates event successfully |
| ADMIN-EVENT-007 | PW | Event activation | Click activate button | Event becomes active |
| ADMIN-EVENT-008 | PW | Event deactivation | Click deactivate button | Event becomes inactive |
| ADMIN-EVENT-009 | PW | Event details modal | Click "View Details" | Shows comprehensive event info |
| ADMIN-EVENT-010 | PW | Event filtering | Use type/status filters | Shows filtered results |
| ADMIN-EVENT-011 | PW | Event search | Search by name | Shows matching events |
| ADMIN-EVENT-012 | PW | Grid view toggle | Click "Grid" button | Shows events in grid layout |
| ADMIN-EVENT-013 | PW | Calendar view toggle | Click "Calendar" button | Shows events in calendar layout |
| ADMIN-EVENT-014 | PW | Empty events state | No events in system | Shows "No Events Found" message |
| ADMIN-EVENT-015 | PW | Demo data generation | Click "Generate Demo Data" | **FIXED: Shows toast when no events** |
| ADMIN-EVENT-016 | PW | Demo data generation (with events) | Click "Generate Demo Data" with events | Generates demo data successfully |
| ADMIN-EVENT-017 | PW | Events refresh | Click "Refresh" button | Reloads events list |
| ADMIN-EVENT-018 | PW | Event statistics display | View event cards | Shows correct ticket/revenue stats |

### 3.4 Order Management
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| ADMIN-ORDER-001 | PW | Orders page layout | Navigate to /orders | Shows order management interface |
| ADMIN-ORDER-002 | PW | Orders list display | View orders page | Shows all orders with details |
| ADMIN-ORDER-003 | PW | Order status filtering | Filter by status | Shows orders of selected status |
| ADMIN-ORDER-004 | PW | Order search | Search by order ID/customer | Shows matching orders |
| ADMIN-ORDER-005 | PW | Order details view | Click on order | Shows detailed order information |
| ADMIN-ORDER-006 | PW | Order status update | Change order status | Updates status successfully |
| ADMIN-ORDER-007 | PW | Order assignment | Assign order to staff | Updates assigned staff |

### 3.5 Drinks Management
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| ADMIN-DRINK-001 | PW | Drinks page layout | Navigate to /drinks | Shows drink catalog management |
| ADMIN-DRINK-002 | PW | Add new drink | Click "Add Drink" button | Opens drink creation form |
| ADMIN-DRINK-003 | PW | Drink creation | Fill and submit form | Creates new drink |
| ADMIN-DRINK-004 | PW | Drink editing | Click edit on existing drink | Opens edit form |
| ADMIN-DRINK-005 | PW | Drink update | Modify and save drink | Updates drink successfully |
| ADMIN-DRINK-006 | PW | Drink deletion | Click delete button | Removes drink after confirmation |
| ADMIN-DRINK-007 | PW | Stock level management | Update stock quantity | Updates inventory |
| ADMIN-DRINK-008 | PW | Price modification | Change drink price | Updates pricing |

### 3.6 User Management
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| ADMIN-USER-001 | PW | Users page layout | Navigate to /users | Shows user management interface |
| ADMIN-USER-002 | PW | Users list display | View users page | Shows all users with roles |
| ADMIN-USER-003 | PW | Create new user | Click "Create User" | Opens user creation form |
| ADMIN-USER-004 | PW | User creation | Fill and submit form | Creates new user |
| ADMIN-USER-005 | PW | User editing | Click edit on user | Opens edit form |
| ADMIN-USER-006 | PW | User role modification | Change user role | Updates user role |
| ADMIN-USER-007 | PW | User deactivation | Deactivate user | User becomes inactive |
| ADMIN-USER-008 | PW | Password reset | Reset user password | Updates user password |

### 3.7 Stadium Structure Management
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| ADMIN-STAD-001 | PW | Structure page layout | Navigate to structure management | Shows stadium structure interface |
| ADMIN-STAD-002 | PW | Current structure display | View structure page | Shows existing stadium layout |
| ADMIN-STAD-003 | PW | JSON import functionality | Upload stadium JSON file | Imports structure successfully |
| ADMIN-STAD-004 | PW | Import validation | Upload invalid JSON | Shows validation errors |
| ADMIN-STAD-005 | PW | Structure export | Click export button | Downloads structure as JSON |
| ADMIN-STAD-006 | PW | Structure visualization | View imported structure | Shows visual representation |

### 3.8 Database Explorer (DataGrid)
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| ADMIN-DATAGRID-001 | PW | DataGrid page access | Navigate to /datagrid | Shows Database Explorer interface |
| ADMIN-DATAGRID-002 | PW | Table selection | Select table from dropdown | Shows table columns and data |
| ADMIN-DATAGRID-003 | PW | Data display | View table data | Shows paginated rows with proper formatting |
| ADMIN-DATAGRID-004 | PW | Column sorting | Click column header | Sorts data by selected column |
| ADMIN-DATAGRID-005 | PW | Data filtering | Enter filter text | Filters rows based on column values |
| ADMIN-DATAGRID-006 | PW | Pagination | Navigate between pages | Shows different data pages |
| ADMIN-DATAGRID-007 | PW | CSV export | Click Export CSV button | Downloads table data as CSV |
| ADMIN-DATAGRID-008 | PW | Authentication required | Access without login | Returns 401 Unauthorized |
| ADMIN-DATAGRID-009 | PW | Empty table display | Select empty table | Shows "No records found" |
| ADMIN-DATAGRID-010 | PW | Large dataset handling | Select table with many rows | Proper pagination and performance |

### 3.9 Analytics Dashboard
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| ADMIN-ANALYTICS-001 | PW | Analytics page layout | Navigate to /analytics | Shows analytics dashboard |
| ADMIN-ANALYTICS-002 | PW | Revenue charts display | View analytics | Shows revenue trends and charts |
| ADMIN-ANALYTICS-003 | PW | Popular items display | Check analytics | Shows most ordered items |
| ADMIN-ANALYTICS-004 | PW | Time period filtering | Change date range | Updates analytics data |
| ADMIN-ANALYTICS-005 | PW | Export functionality | Click export button | Exports analytics report |

### 3.10 Centralized Logging Management
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| ADMIN-LOGGING-001 | PW | Logs page access | Navigate to /logs | Shows centralized logs interface |
| ADMIN-LOGGING-002 | PW | Log entries display | View logs page | Shows log entries with all required columns |
| ADMIN-LOGGING-003 | PW | Log level filtering | Select Error level filter | Shows only error log entries |
| ADMIN-LOGGING-004 | PW | Category filtering | Select Security category | Shows only security-related logs |
| ADMIN-LOGGING-005 | PW | Date range filtering | Set date range filter | Shows logs within specified dates |
| ADMIN-LOGGING-006 | PW | User-specific filtering | Filter by user ID/email | Shows logs for specific user |
| ADMIN-LOGGING-007 | PW | Full-text search | Search in log messages | Shows matching log entries |
| ADMIN-LOGGING-008 | PW | Log pagination | Navigate through pages | Shows different log pages correctly |
| ADMIN-LOGGING-009 | PW | Log details modal | Click on log entry | Shows expanded log details |
| ADMIN-LOGGING-010 | PW | Log export functionality | Click export button | Downloads filtered logs as CSV/JSON |
| ADMIN-LOGGING-011 | PW | Real-time log updates | Generate new logs | New entries appear without refresh |
| ADMIN-LOGGING-012 | PW | Log summary statistics | View logs dashboard | Shows error counts, trends, statistics |
| ADMIN-LOGGING-013 | PW | Clear old logs button | Click "Clear Old Logs" | Prompts confirmation, clears logs |
| ADMIN-LOGGING-014 | PW | Log retention settings | Access retention configuration | Shows/allows retention period settings |
| ADMIN-LOGGING-015 | PW | Error rate monitoring | View analytics section | Shows error rate trends and alerts |
| ADMIN-LOGGING-016 | PW | Log source filtering | Filter by application source | Shows logs from specific apps (Customer/Admin/API) |
| ADMIN-LOGGING-017 | PW | Security event highlights | View security logs | Critical security events highlighted |
| ADMIN-LOGGING-018 | PW | Performance log analysis | View performance category | Shows response times, slow queries |
| ADMIN-LOGGING-019 | PW | Batch log processing info | Check batch statistics | Shows batch processing performance metrics |
| ADMIN-LOGGING-020 | PW | Log search performance | Search large log dataset | Search completes within reasonable time |

---

## 4. Staff Application Tests (StadiumDrinkOrdering.Staff)

### 4.1 Authentication & Navigation
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| STAFF-AUTH-001 | PW | Staff login page | Navigate to staff login | Shows login form |
| STAFF-AUTH-002 | PW | Valid staff login | Login with staff credentials | Redirects to staff dashboard |
| STAFF-AUTH-003 | PW | Staff navigation menu | After login | Shows staff-specific navigation |

### 4.2 Order Processing
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| STAFF-ORDER-001 | PW | Orders queue display | View staff dashboard | Shows pending orders |
| STAFF-ORDER-002 | PW | Order acceptance | Click "Accept" on order | Order status changes to accepted |
| STAFF-ORDER-003 | PW | Order preparation | Mark order as "In Preparation" | Status updates accordingly |
| STAFF-ORDER-004 | PW | Order completion | Mark order as "Ready" | Status updates, notifies customer |
| STAFF-ORDER-005 | PW | Order delivery | Mark order as "Delivered" | Completes order workflow |

### 4.3 Bartender Dashboard
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| STAFF-BAR-001 | PW | Bartender dashboard layout | Navigate to bartender dashboard | Shows real-time order queue |
| STAFF-BAR-002 | PW | Real-time updates | Place new order from customer app | Order appears in real-time |
| STAFF-BAR-003 | PW | Order prioritization | Multiple orders in queue | Shows proper order priority |

---

## 5. Integration Tests

### 5.1 Cross-Application Workflows
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| INT-001 | E2E | Complete ticket purchase flow | Customer: Browse → Select → Checkout → Confirm | Full ticket purchase completed |
| INT-002 | E2E | Admin creates event, customer purchases | Admin creates event → Customer buys ticket | End-to-end event-to-purchase flow |
| INT-003 | E2E | Order processing workflow | Customer orders → Staff processes → Delivery | Complete order lifecycle |
| INT-004 | E2E | Real-time updates test | Staff updates order → Customer sees update | SignalR real-time communication works |

### 5.2 Database Consistency
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| INT-DB-001 | INT | Seat reservation consistency | Multiple users select same seat simultaneously | Only one succeeds |
| INT-DB-002 | INT | Cart timeout cleanup | Cart items expire after 15 minutes | Database cleanup occurs |
| INT-DB-003 | INT | Order data integrity | Complete order process | All related data consistent |

### 5.3 SignalR Real-time Communication
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| INT-SIGNALR-001 | PW | Connection establishment | Load any page | SignalR connection shows "Connected" |
| INT-SIGNALR-002 | PW | Order status updates | Staff changes order status | Customer sees real-time update |
| INT-SIGNALR-003 | PW | New order notifications | Customer places order | Staff sees real-time notification |

### 5.4 Centralized Logging Integration
| Test Case | Type | Description | Steps | Expected Result |
|-----------|------|-------------|-------|----------------|
| INT-LOG-001 | E2E | Cross-application log aggregation | Generate logs from Customer, Admin, Staff apps | All logs appear in Admin logs interface |
| INT-LOG-002 | E2E | Automatic exception logging | Trigger exceptions in API endpoints | Exceptions automatically logged with full context |
| INT-LOG-003 | E2E | User action tracking | Perform actions across all applications | User actions logged with proper attribution |
| INT-LOG-004 | E2E | Batch processing verification | Generate high-volume logs | Logs processed in batches efficiently |
| INT-LOG-005 | E2E | Log retention cleanup | Set short retention period, wait | Old logs cleaned up automatically |
| INT-LOG-006 | E2E | Security event logging | Attempt unauthorized access | Security events logged immediately |
| INT-LOG-007 | E2E | Performance logging | Trigger slow operations | Performance metrics captured in logs |
| INT-LOG-008 | INT | Database consistency | Concurrent logging operations | No log entries lost or corrupted |
| INT-LOG-009 | INT | Background service health | Monitor LogRetentionBackgroundService | Service runs daily cleanup successfully |
| INT-LOG-010 | INT | GlobalExceptionMiddleware integration | All API endpoints error handling | Middleware captures all unhandled exceptions |
| INT-LOG-011 | E2E | Log search accuracy | Generate specific logs, search for them | Search returns correct results |
| INT-LOG-012 | E2E | Real-time log monitoring | Generate logs while Admin views logs page | New logs appear in real-time |
| INT-LOG-013 | INT | Queue processing under load | Generate 1000+ log entries rapidly | All entries processed without loss |
| INT-LOG-014 | E2E | Log export integrity | Export large dataset | Exported data matches database content |
| INT-LOG-015 | INT | Service startup validation | Start API service | CentralizedLoggingClient and background services start correctly |

---

## 6. Performance Tests

### 6.1 Load Testing
| Test Case | Type | Description | Expected Result |
|-----------|------|-------------|----------------|
| PERF-001 | API | High concurrent user load | System handles 100+ concurrent users |
| PERF-002 | API | Database query performance | All queries respond within 2 seconds |
| PERF-003 | PW | Page load times | All pages load within 3 seconds |

### 6.2 Stress Testing
| Test Case | Type | Description | Expected Result |
|-----------|------|-------------|----------------|
| STRESS-001 | API | Cart reservation stress test | System handles seat reservation conflicts |
| STRESS-002 | API | Order processing under load | Order processing remains stable |

### 6.3 Logging Performance Tests
| Test Case | Type | Description | Expected Result |
|-----------|------|-------------|----------------|
| PERF-LOG-001 | API | High-volume log ingestion | 10,000 log entries/minute processed successfully |
| PERF-LOG-002 | API | Batch processing efficiency | Batch operations complete within 1 second |
| PERF-LOG-003 | API | Log search performance | Search 100,000+ logs returns results within 3 seconds |
| PERF-LOG-004 | API | Concurrent logging stress | 100 concurrent log requests handled without blocking |
| PERF-LOG-005 | INT | Memory usage under load | Logging client memory remains stable under high load |
| PERF-LOG-006 | INT | Database performance | Log writes don't impact application database performance |
| PERF-LOG-007 | API | Log export performance | Export 50,000+ logs completes within 30 seconds |
| PERF-LOG-008 | INT | Background service efficiency | Log retention cleanup completes within 5 minutes |
| PERF-LOG-009 | API | Queue processing latency | Queued logs processed within 5-second interval |
| PERF-LOG-010 | INT | Log aggregation performance | Multiple source logs aggregated efficiently |

---

## 7. Security Tests

### 7.1 Authentication Security
| Test Case | Type | Description | Expected Result |
|-----------|------|-------------|----------------|
| SEC-001 | API | JWT token validation | Invalid tokens rejected |
| SEC-002 | API | Role-based access control | Unauthorized access blocked |
| SEC-003 | API | Password security | Passwords properly hashed |

### 7.2 Input Validation
| Test Case | Type | Description | Expected Result |
|-----------|------|-------------|----------------|
| SEC-004 | API | SQL injection attempts | Malicious input sanitized |
| SEC-005 | PW | XSS prevention | Script injection prevented |
| SEC-006 | API | Input length validation | Oversized inputs rejected |

### 7.3 Logging Security Tests
| Test Case | Type | Description | Expected Result |
|-----------|------|-------------|----------------|
| SEC-LOG-001 | API | Sensitive data filtering | Passwords/tokens not logged in plaintext |
| SEC-LOG-002 | API | Log injection prevention | Malicious log content sanitized |
| SEC-LOG-003 | API | Unauthorized log access | Non-admin users can't access log endpoints |
| SEC-LOG-004 | API | Log tampering prevention | Log entries cannot be modified after creation |
| SEC-LOG-005 | API | PII data protection | Personal information properly anonymized in logs |
| SEC-LOG-006 | API | Log export authorization | Only authorized users can export logs |
| SEC-LOG-007 | API | Rate limiting on log endpoints | Excessive logging requests are throttled |
| SEC-LOG-008 | API | Log deletion authorization | Only admins can delete/clear logs |
| SEC-LOG-009 | INT | Audit trail integrity | All log management actions are audited |
| SEC-LOG-010 | API | Cross-application log isolation | Apps can only read their own logs unless authorized |
| SEC-LOG-011 | API | Encryption in transit | Log transmission uses HTTPS in production |
| SEC-LOG-012 | INT | Log storage security | Log database access properly restricted |
| SEC-LOG-013 | API | Authentication bypass prevention | Critical log operations require valid authentication |
| SEC-LOG-014 | API | Log flooding protection | System handles malicious high-volume logging |
| SEC-LOG-015 | INT | Security event alerting | Security violations trigger immediate alerts |

---

## 8. Browser Compatibility Tests

### 8.1 Cross-Browser Testing
| Test Case | Type | Browser | Description |
|-----------|------|---------|-------------|
| BROWSER-001 | PW | Chrome | Full functionality test |
| BROWSER-002 | PW | Firefox | Full functionality test |
| BROWSER-003 | PW | Safari | Full functionality test |
| BROWSER-004 | PW | Edge | Full functionality test |

### 8.2 Mobile Responsiveness
| Test Case | Type | Device | Description |
|-----------|------|--------|-------------|
| MOBILE-001 | PW | iPhone | Mobile layout and functionality |
| MOBILE-002 | PW | Android | Mobile layout and functionality |
| MOBILE-003 | PW | Tablet | Tablet layout and functionality |

---

## 9. Error Handling Tests

### 9.1 Network Error Scenarios
| Test Case | Type | Description | Expected Result |
|-----------|------|-------------|----------------|
| ERROR-001 | PW | Network disconnection | Graceful error handling |
| ERROR-002 | PW | API server down | Appropriate error messages |
| ERROR-003 | PW | Timeout scenarios | Timeout handling works |

### 9.2 Data Validation Errors
| Test Case | Type | Description | Expected Result |
|-----------|------|-------------|----------------|
| ERROR-004 | PW | Form validation errors | Clear error messages shown |
| ERROR-005 | PW | Server-side validation | Server errors properly displayed |

---

## 10. Accessibility Tests

### 10.1 WCAG Compliance
| Test Case | Type | Description | Expected Result |
|-----------|------|-------------|----------------|
| A11Y-001 | PW | Keyboard navigation | All features accessible via keyboard |
| A11Y-002 | PW | Screen reader compatibility | Proper ARIA labels and descriptions |
| A11Y-003 | PW | Color contrast | Meets WCAG AA standards |
| A11Y-004 | PW | Focus management | Proper focus indicators |

---

## Test Execution Guidelines

### Playwright Test Implementation
```typescript
// Example Playwright test structure
import { test, expect } from '@playwright/test';

test.describe('Admin Event Management', () => {
  test.beforeEach(async ({ page }) => {
    // Login as admin
    await page.goto('https://localhost:7030/login');
    await page.fill('[data-testid="email"]', 'admin@stadium.com');
    await page.fill('[data-testid="password"]', 'admin123');
    await page.click('[data-testid="login-button"]');
  });

  test('ADMIN-EVENT-015: Demo data generation shows toast when no events', async ({ page }) => {
    await page.goto('https://localhost:7030/events');
    await page.click('#admin-events-demo-data-btn');
    await expect(page.locator('.toast.error')).toContainText('No events available for demo data generation');
  });
});

// Centralized Logging Playwright Tests
test.describe('Admin Centralized Logging', () => {
  test.beforeEach(async ({ page }) => {
    // Login as admin
    await page.goto('https://localhost:7030/login');
    await page.fill('[data-testid="email"]', 'admin@stadium.com');
    await page.fill('[data-testid="password"]', 'admin123');
    await page.click('[data-testid="login-button"]');
  });

  test('ADMIN-LOGGING-001: Logs page displays correctly', async ({ page }) => {
    await page.goto('https://localhost:7030/logs');
    await expect(page.locator('h1')).toContainText('System Logs');
    await expect(page.locator('[data-testid="log-search-form"]')).toBeVisible();
    await expect(page.locator('[data-testid="logs-table"]')).toBeVisible();
  });

  test('ADMIN-LOGGING-003: Log level filtering works', async ({ page }) => {
    await page.goto('https://localhost:7030/logs');
    
    // Select Error level filter
    await page.selectOption('[data-testid="log-level-filter"]', 'Error');
    await page.click('[data-testid="search-logs-btn"]');
    
    // Verify only error logs are shown
    const logRows = page.locator('[data-testid="log-row"]');
    const count = await logRows.count();
    
    for (let i = 0; i < count; i++) {
      const levelCell = logRows.nth(i).locator('[data-testid="log-level"]');
      await expect(levelCell).toContainText('Error');
    }
  });

  test('ADMIN-LOGGING-007: Full-text search functionality', async ({ page }) => {
    await page.goto('https://localhost:7030/logs');
    
    // Search for specific text
    await page.fill('[data-testid="log-search-text"]', 'Login');
    await page.click('[data-testid="search-logs-btn"]');
    
    // Verify results contain search term
    const logRows = page.locator('[data-testid="log-row"]');
    const count = await logRows.count();
    expect(count).toBeGreaterThan(0);
    
    // Check first row contains search term
    const firstRow = logRows.first();
    const content = await firstRow.textContent();
    expect(content).toContain('Login');
  });

  test('ADMIN-LOGGING-009: Log details modal opens', async ({ page }) => {
    await page.goto('https://localhost:7030/logs');
    
    // Click on first log entry
    const firstLogRow = page.locator('[data-testid="log-row"]').first();
    await firstLogRow.click();
    
    // Verify modal opens with details
    const modal = page.locator('[data-testid="log-details-modal"]');
    await expect(modal).toBeVisible();
    await expect(modal.locator('[data-testid="log-action"]')).toBeVisible();
    await expect(modal.locator('[data-testid="log-category"]')).toBeVisible();
    await expect(modal.locator('[data-testid="log-timestamp"]')).toBeVisible();
  });

  test('ADMIN-LOGGING-013: Clear old logs with confirmation', async ({ page }) => {
    await page.goto('https://localhost:7030/logs');
    
    // Click clear old logs button
    await page.click('[data-testid="clear-old-logs-btn"]');
    
    // Verify confirmation dialog
    const confirmDialog = page.locator('[data-testid="confirmation-dialog"]');
    await expect(confirmDialog).toBeVisible();
    await expect(confirmDialog).toContainText('Are you sure you want to clear old logs?');
    
    // Confirm action
    await page.click('[data-testid="confirm-clear-logs"]');
    
    // Verify success message
    await expect(page.locator('.toast.success')).toContainText('Old logs cleared successfully');
  });
});

// Integration test for cross-application logging
test.describe('Centralized Logging Integration', () => {
  test('INT-LOG-001: Cross-application log aggregation', async ({ browser }) => {
    // Create separate contexts for Admin and Customer
    const adminContext = await browser.newContext();
    const customerContext = await browser.newContext();
    
    const adminPage = await adminContext.newPage();
    const customerPage = await customerContext.newPage();
    
    // Login as admin
    await adminPage.goto('https://localhost:7030/login');
    await adminPage.fill('[data-testid="email"]', 'admin@stadium.com');
    await adminPage.fill('[data-testid="password"]', 'admin123');
    await adminPage.click('[data-testid="login-button"]');
    
    // Perform customer action (generates log)
    await customerPage.goto('https://localhost:7020/events');
    
    // Check admin logs for customer action
    await adminPage.goto('https://localhost:7030/logs');
    await adminPage.selectOption('[data-testid="source-filter"]', 'Customer');
    await adminPage.click('[data-testid="search-logs-btn"]');
    
    // Verify customer logs appear in admin interface
    const logRows = adminPage.locator('[data-testid="log-row"]');
    await expect(logRows.first()).toBeVisible();
    
    await adminContext.close();
    await customerContext.close();
  });
});
```

### API Test Implementation
```bash
# Example API test using curl
curl -X POST https://localhost:7010/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@stadium.com","password":"admin123"}' \
  | jq '.token'

# Centralized Logging API Tests
# Test single log entry
curl -X POST https://localhost:7010/api/logs/log-action \
  -H "Content-Type: application/json" \
  -d '{
    "action": "TestAction",
    "category": "UserAction",
    "userId": "test-user",
    "details": "API test log entry",
    "source": "TestSuite"
  }'

# Test batch logging
curl -X POST https://localhost:7010/api/logs/log-batch \
  -H "Content-Type: application/json" \
  -d '[
    {
      "action": "BatchTest1",
      "category": "SystemTest",
      "details": "First batch entry"
    },
    {
      "action": "BatchTest2", 
      "category": "SystemTest",
      "details": "Second batch entry"
    }
  ]'

# Test log search with admin token
ADMIN_TOKEN=$(curl -X POST https://localhost:7010/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@stadium.com","password":"admin123"}' | jq -r '.token')

curl -X POST https://localhost:7010/api/logs/search \
  -H "Authorization: Bearer $ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "page": 1,
    "pageSize": 10,
    "level": "Error",
    "category": "SystemError",
    "dateFrom": "2024-01-01T00:00:00Z",
    "dateTo": "2024-12-31T23:59:59Z"
  }'

# Test log statistics
curl -X GET https://localhost:7010/api/logs/summary \
  -H "Authorization: Bearer $ADMIN_TOKEN"
```

### Test Data Setup
- Use demo data generation endpoints for consistent test data
- Clear database between test runs
- Use test-specific event/user data

### Test Environment
- **Development**: Use SQLite database
- **CI/CD**: Use containerized environment
- **Local Testing**: Docker Compose setup

---

## Test Reporting
- Generate HTML reports with screenshots for failed tests
- Track test coverage metrics
- Monitor test execution times
- Document known issues and workarounds

---

## Maintenance
- Update tests when new features are added
- Review and update test data regularly
- Monitor test reliability and flakiness
- Keep browser drivers updated