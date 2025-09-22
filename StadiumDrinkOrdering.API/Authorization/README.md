# Stadium Drink Ordering - Authorization System

## Overview

This document outlines the comprehensive policy-based authorization system implemented for the Stadium Drink Ordering application. The system provides enterprise-grade security with role-based and resource-based authorization patterns.

## Architecture

### Authorization Policies (`AuthorizationPolicies.cs`)
Centralized policy definitions that support:
- **Role-based policies** (Admin, Staff, Customer access)
- **Operation-based policies** (Create, Read, Update, Delete permissions)
- **Resource-based policies** (Ownership validation)
- **Administrative policies** (System management, user management)
- **SignalR hub policies** (Real-time communication access)

### Custom Authorization Requirements
1. **`OrderOwnershipRequirement`** - Ensures users can only access their own orders
2. **`UserOwnershipRequirement`** - Protects user profile access
3. **`TicketOwnershipRequirement`** - Controls ticket access based on ownership

### Authorization Handlers
1. **`OrderOwnershipHandler`** - Validates order access permissions
2. **`UserOwnershipHandler`** - Validates user profile access
3. **`TicketOwnershipHandler`** - Validates ticket ownership and staff validation rights

### Authorization Service (`IStadiumAuthorizationService`)
Helper service providing:
- Role checking methods (`IsAdmin()`, `IsStaff()`, `IsCustomer()`)
- Permission validation (`CanManageUsers()`, `CanViewAnalytics()`)
- Resource ownership validation (`CanAccessOrder()`, `CanAccessUserProfile()`)
- User context methods (`GetCurrentUserId()`, `GetCurrentUserRole()`)

## Policy Definitions

### Core Role-Based Policies
- `RequireAdminRole` - Administrator access only
- `RequireStaffRole` - Staff access (Admin + Bartender + Waiter)
- `RequireCustomerRole` - Customer access only
- `RequireAuthenticatedUser` - Any authenticated user

### Operation-Based Policies
- `CanReadOrders` - View order information
- `CanCreateOrders` - Create new orders
- `CanUpdateOrders` - Modify existing orders
- `CanDeleteOrders` - Cancel/delete orders

### Resource-Based Policies
- `CanAccessOwnOrders` - Access only owned orders
- `CanAccessOwnProfile` - Access only own profile
- `CanAccessOwnTickets` - Access only owned tickets

### Administrative Policies
- `CanManageUsers` - User account management
- `CanManageDrinks` - Drink catalog management
- `CanManageEvents` - Event management
- `CanViewAnalytics` - Analytics and reporting
- `CanManageSystem` - System configuration
- `CanAccessLogs` - Centralized logging access
- `CanManageStadiumStructure` - Stadium structure management

### SignalR Hub Policies
- `CanAccessBartenderHub` - Staff real-time communication
- `CanAccessCustomerHub` - Customer notifications

## Controller Implementation

### Updated Controllers
All controllers have been updated to use policy-based authorization:

```csharp
[Authorize(Policy = AuthorizationPolicies.CanManageUsers)]
public class UsersController : ControllerBase

[Authorize(Policy = AuthorizationPolicies.RequireAuthenticatedUser)]
public class OrdersController : ControllerBase

[Authorize(Policy = AuthorizationPolicies.CanManageEvents)]
// Event management methods
```

### Method-Level Authorization
Individual actions use specific policies:

```csharp
[HttpGet("{id}")]
[Authorize(Policy = AuthorizationPolicies.CanAccessOwnOrders)]
public async Task<ActionResult<OrderDto>> GetOrder(int id)

[HttpPost]
[Authorize(Policy = AuthorizationPolicies.CanCreateOrders)]
public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderDto createOrderDto)

[HttpPut("{id}/status")]
[Authorize(Policy = AuthorizationPolicies.CanUpdateOrders)]
public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto updateDto)
```

## SignalR Hub Authorization

### BartenderHub
```csharp
[Authorize(Policy = AuthorizationPolicies.CanAccessBartenderHub)]
public class BartenderHub : Hub

[Authorize(Policy = AuthorizationPolicies.CanUpdateOrders)]
public async Task SendOrderUpdate(OrderDto order)

[Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
public async Task SendStaffAssignmentUpdate(int orderId, int staffId, string staffName, string role)
```

### CustomerHub
```csharp
[Authorize(Policy = AuthorizationPolicies.RequireAuthenticatedUser)]
public async Task JoinOrderGroup(int orderId)

[Authorize(Policy = AuthorizationPolicies.RequireStaffRole)]
public async Task SendOrderStatusUpdate(int orderId, string status)

[Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
public async Task SendBroadcastNotification(string message, string type)
```

## Frontend Authorization Components

### Authorization Helper Service (`IAuthorizationHelperService`)
Shared service for frontend applications providing:
- Role checking methods
- Permission validation
- User context information
- UI styling helpers

```csharp
// Role checks
bool IsAdmin();
bool IsStaff();
bool IsCustomer();

// Permission checks
bool CanManageUsers();
bool CanViewAnalytics();
bool CanAccessLogs();

// Resource ownership
bool CanAccessOrder(int? orderCustomerId);
bool CanAccessUserProfile(int? targetUserId);
```

### Authorization Component (`AuthorizeView.razor`)
Blazor component for conditional UI rendering:

```razor
<!-- Simple role check -->
<AuthorizeView RequireAdmin="true">
    <Authorized>Admin only content</Authorized>
    <NotAuthorized>Access denied</NotAuthorized>
</AuthorizeView>

<!-- Permission-based -->
<AuthorizeView RequireCanManageUsers="true">
    <Authorized>User management UI</Authorized>
</AuthorizeView>

<!-- Ownership validation -->
<AuthorizeView RequireCanAccessOrder="true" ResourceOwnerId="@order.CustomerId">
    <Authorized>Order details</Authorized>
    <NotAuthorized>You can only view your own orders</NotAuthorized>
</AuthorizeView>
```

## Security Features

### Resource-Based Authorization
- **Order Access**: Customers can only access their own orders; staff can access all
- **Profile Access**: Users can only access their own profiles; admins can access all
- **Ticket Validation**: Customers can only access their tickets; staff can validate any ticket

### Context-Aware Authorization
- **Route Parameters**: Handlers extract resource IDs from route data
- **Query Parameters**: Support for resource identification via query strings
- **Request Context**: Access to HTTP context for comprehensive validation

### Database Integration
- **Entity Framework Integration**: Handlers query database for ownership validation
- **Efficient Queries**: Optimized LINQ queries for authorization checks
- **Transaction Safety**: Authorization checks within database transaction boundaries

## Configuration

### Program.cs Registration
```csharp
// Configure authorization policies
builder.Services.AddAuthorization(options =>
{
    AuthorizationPolicies.ConfigurePolicies(options);
});

// Register custom authorization handlers
builder.Services.AddScoped<IAuthorizationHandler, OrderOwnershipHandler>();
builder.Services.AddScoped<IAuthorizationHandler, UserOwnershipHandler>();
builder.Services.AddScoped<IAuthorizationHandler, TicketOwnershipHandler>();

// Register authorization service
builder.Services.AddScoped<IStadiumAuthorizationService, StadiumAuthorizationService>();
```

### Frontend Service Registration
```csharp
// In Customer/Admin/Staff Program.cs
builder.Services.AddScoped<IAuthorizationHelperService, AuthorizationHelperService>();
```

## Testing Authorization

### Policy Testing
Use the authorization service to test permissions:

```csharp
// Test admin access
var canManageUsers = authService.CanManageUsers();

// Test resource ownership
var canAccessOrder = await authService.CanAccessOrderAsync(user, orderId);

// Test role membership
var isStaff = authService.IsStaff();
```

### API Testing
Test controllers with different user roles:

```bash
# Admin access
curl -H "Authorization: Bearer $ADMIN_TOKEN" https://localhost:9010/users

# Customer access to own order
curl -H "Authorization: Bearer $CUSTOMER_TOKEN" https://localhost:9010/orders/123

# Unauthorized access (should return 403)
curl -H "Authorization: Bearer $CUSTOMER_TOKEN" https://localhost:9010/users
```

## Security Best Practices Implemented

1. **Principle of Least Privilege**: Users only get permissions they need
2. **Defense in Depth**: Multiple layers of authorization checks
3. **Resource Ownership**: Strict validation of resource access rights
4. **Centralized Policies**: Single source of truth for authorization rules
5. **Comprehensive Logging**: All authorization decisions are logged
6. **Fail-Safe Defaults**: Default deny for undefined scenarios

## Troubleshooting

### Common Issues
1. **Missing Policies**: Ensure all policies are registered in `AuthorizationPolicies.ConfigurePolicies()`
2. **Handler Registration**: Verify custom handlers are registered as scoped services
3. **JWT Claims**: Ensure JWT tokens contain required claims (NameIdentifier, Role, Email)
4. **Database Context**: Verify ApplicationDbContext is available in authorization handlers

### Debug Authorization
Enable detailed authorization logging:

```csharp
builder.Logging.AddFilter("Microsoft.AspNetCore.Authorization", LogLevel.Debug);
```

## Future Enhancements

1. **Time-Based Policies**: Add support for time-restricted access
2. **Location-Based Policies**: Implement geo-location based authorization
3. **Dynamic Policies**: Support for runtime policy configuration
4. **Audit Trail**: Enhanced logging for compliance requirements
5. **Policy Composition**: Support for complex policy combinations

## Conclusion

This authorization system provides enterprise-grade security for the Stadium Drink Ordering application. It implements comprehensive role-based and resource-based authorization patterns while maintaining flexibility for future requirements.

The system ensures that:
- Only authorized users can access protected resources
- Users can only access resources they own (where applicable)
- Staff roles have appropriate administrative access
- All authorization decisions are logged and auditable
- Frontend applications can dynamically show/hide UI elements based on permissions

This implementation follows .NET security best practices and provides a solid foundation for a production-ready application.