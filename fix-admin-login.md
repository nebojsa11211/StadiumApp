# Admin Login Fix Guide

## Primary Issue: API Server Not Running

### **Immediate Solution**
1. **Start the API Server**:
   ```bash
   cd D:\AiApps\StadiumApp\StadiumApp\StadiumDrinkOrdering.API
   dotnet run --launch-profile https
   ```
   
   The API should start on `https://localhost:7010`

2. **Verify API is running**:
   ```bash
   curl -k https://localhost:7010/health
   ```

3. **Test login endpoint**:
   ```bash
   curl -k -X POST https://localhost:7010/api/auth/login \
     -H "Content-Type: application/json" \
     -d '{"email": "admin@stadium.com", "password": "admin123"}'
   ```

### **Expected Response**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "username": "admin",
    "email": "admin@stadium.com",
    "role": "Admin"
  },
  "expiresAt": "2024-01-02T12:00:00Z"
}
```

## Optional HTTPS Configuration Improvements

### 1. Simplify HTTP Client Handler (Program.cs)
Replace lines 73-91 with:
```csharp
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    
    // Development SSL bypass only
    if (builder.Environment.IsDevelopment())
    {
        handler.ServerCertificateCustomValidationCallback = 
            (sender, certificate, chain, sslPolicyErrors) => true;
    }
    
    // Use standard HTTP/1.1 to avoid HTTP/2 complications
    handler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
    
    return handler;
});
```

### 2. Enhanced Connection Error Detection (AdminApiService.cs)
Add after line 126:
```csharp
// Check if API server is reachable before attempting login
try
{
    using var testClient = new HttpClient(new HttpClientHandler 
    { 
        ServerCertificateCustomValidationCallback = (s, c, ch, e) => true 
    });
    testClient.Timeout = TimeSpan.FromSeconds(5);
    var healthResponse = await testClient.GetAsync($"{_httpClient.BaseAddress}health");
    Console.WriteLine($"API Health Check: {healthResponse.StatusCode}");
}
catch (Exception healthEx)
{
    Console.WriteLine($"⚠️ API Server Connectivity Issue: {healthEx.Message}");
    Console.WriteLine($"Ensure API is running on {_httpClient.BaseAddress}");
    throw new InvalidOperationException(
        $"Cannot connect to API server at {_httpClient.BaseAddress}. " +
        "Please ensure the API is running and accessible.", healthEx);
}
```

## Startup Sequence
1. **Terminal 1** - Start API:
   ```bash
   cd StadiumDrinkOrdering.API
   dotnet run --launch-profile https
   ```

2. **Terminal 2** - Start Admin:
   ```bash
   cd StadiumDrinkOrdering.Admin
   dotnet run --launch-profile https
   ```

3. **Navigate to**: https://localhost:7030/login
4. **Credentials**: admin@stadium.com / admin123

## Verification Steps
1. API responds at https://localhost:7010/health
2. Login endpoint returns JWT token
3. Admin app connects successfully
4. Login redirects to admin dashboard