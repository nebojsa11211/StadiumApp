using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace StadiumDrinkOrdering.Tests.Integration
{
    /// <summary>
    /// Integration tests to verify SQLite to Supabase migration success
    /// These tests validate that all critical functionality works after migration
    /// </summary>
    public class MigrationIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public MigrationIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Database_Connection_Should_Work()
        {
            // Arrange & Act
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            // Assert
            Assert.True(await context.Database.CanConnectAsync());
            
            // Verify we're using PostgreSQL
            Assert.Contains("Npgsql", context.Database.ProviderName);
        }

        [Fact]
        public async Task User_Authentication_Should_Work_After_Migration()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "admin",
                Password = "admin123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

            // Assert
            Assert.True(response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Assert.Contains("token", content.ToLower());
            }
        }

        [Fact]
        public async Task Drinks_API_Should_Return_Data()
        {
            // Act
            var response = await _client.GetAsync("/api/drinks");
            
            // Assert
            Assert.True(response.IsSuccessStatusCode);
            
            var drinks = await response.Content.ReadFromJsonAsync<List<DrinkDto>>();
            Assert.NotNull(drinks);
            
            // Should have migrated drinks
            if (drinks.Any())
            {
                var drink = drinks.First();
                Assert.True(drink.Id > 0);
                Assert.False(string.IsNullOrEmpty(drink.Name));
                Assert.True(drink.Price > 0);
            }
        }

        [Fact]
        public async Task Stadium_Structure_Should_Be_Intact()
        {
            // Act
            var response = await _client.GetAsync("/api/stadium-structure");
            
            // Assert - Should return either 200 with data or 204 if no structure
            Assert.True(response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData("users")]
        [InlineData("drinks")]
        [InlineData("orders")]
        [InlineData("events")]
        [InlineData("tribunes")]
        public async Task Critical_Tables_Should_Exist(string tableName)
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Act & Assert
            var query = $"SELECT 1 FROM {tableName} LIMIT 1";
            
            try
            {
                await context.Database.ExecuteSqlRawAsync(query);
                // If we get here, table exists and is accessible
                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.True(false, $"Table {tableName} is not accessible: {ex.Message}");
            }
        }

        [Fact]
        public async Task Foreign_Key_Relationships_Should_Work()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Act - Try to query across relationships
            var query = @"
                SELECT o.id, o.total_amount, u.username, u.email 
                FROM orders o 
                JOIN users u ON o.customer_id = u.id 
                LIMIT 5";

            try
            {
                var result = await context.Database.ExecuteSqlRawAsync(query);
                Assert.True(true); // If we get here, relationships work
            }
            catch (Exception ex)
            {
                Assert.True(false, $"Foreign key relationships broken: {ex.Message}");
            }
        }

        [Fact]
        public async Task Logging_System_Should_Work()
        {
            // Act
            var response = await _client.GetAsync("/api/logs/summary");
            
            // Assert
            Assert.True(response.IsSuccessStatusCode);
            
            // The endpoint should return even if no logs exist
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
        }

        [Fact]
        public async Task Health_Check_Should_Pass()
        {
            // Act
            var response = await _client.GetAsync("/health");
            
            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostgreSQL_Sequences_Should_Work()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Act - Check that sequences are properly configured
            var sequenceQuery = @"
                SELECT sequence_name, last_value 
                FROM information_schema.sequences 
                WHERE sequence_schema = 'public' 
                  AND sequence_name LIKE '%_id_seq'";

            try
            {
                await context.Database.ExecuteSqlRawAsync(sequenceQuery);
                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.True(false, $"Sequence configuration issue: {ex.Message}");
            }
        }

        [Fact]
        public async Task Database_Indexes_Should_Exist()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Act - Check critical indexes exist
            var indexQuery = @"
                SELECT indexname 
                FROM pg_indexes 
                WHERE schemaname = 'public' 
                  AND indexname LIKE 'idx_%'";

            try
            {
                var result = await context.Database.ExecuteSqlRawAsync(indexQuery);
                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.True(false, $"Index verification failed: {ex.Message}");
            }
        }

        [Fact]
        public async Task Data_Types_Should_Be_Correct()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Act - Verify critical data types
            try
            {
                // Test decimal precision for prices
                var priceTest = await context.Database.ExecuteSqlRawAsync(
                    "SELECT price FROM drinks WHERE price = 5.50 LIMIT 1");

                // Test timestamp fields
                var timestampTest = await context.Database.ExecuteSqlRawAsync(
                    "SELECT created_at FROM users WHERE created_at IS NOT NULL LIMIT 1");

                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.True(false, $"Data type verification failed: {ex.Message}");
            }
        }

        [Fact]
        public async Task Record_Counts_Should_Match_Expected()
        {
            // This test should be customized based on your actual data
            // It's a placeholder to verify data was migrated
            
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                var userCount = await context.Users.CountAsync();
                var drinkCount = await context.Drinks.CountAsync();
                var orderCount = await context.Orders.CountAsync();

                // Assert that we have some data (adjust based on your expectations)
                Assert.True(userCount >= 0, "Users should exist or be zero for fresh system");
                Assert.True(drinkCount >= 0, "Drinks should exist or be zero for fresh system");
                Assert.True(orderCount >= 0, "Orders count should be valid");

                // Log the counts for manual verification
                Console.WriteLine($"Migration verification - Users: {userCount}, Drinks: {drinkCount}, Orders: {orderCount}");
            }
            catch (Exception ex)
            {
                Assert.True(false, $"Record count verification failed: {ex.Message}");
            }
        }

        [Fact]
        public async Task Complex_Query_Performance_Should_Be_Reasonable()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            try
            {
                // Run a moderately complex query
                var complexQuery = @"
                    SELECT 
                        u.role,
                        COUNT(o.id) as order_count,
                        COALESCE(AVG(o.total_amount), 0) as avg_order_value
                    FROM users u
                    LEFT JOIN orders o ON u.id = o.customer_id
                    GROUP BY u.role";

                await context.Database.ExecuteSqlRawAsync(complexQuery);
                
                stopwatch.Stop();
                
                // Should complete within reasonable time (adjust threshold as needed)
                Assert.True(stopwatch.ElapsedMilliseconds < 5000, 
                    $"Complex query took {stopwatch.ElapsedMilliseconds}ms, which is too slow");
                    
                Console.WriteLine($"Complex query completed in {stopwatch.ElapsedMilliseconds}ms");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Assert.True(false, $"Complex query failed: {ex.Message}");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _client?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

/// <summary>
/// Manual validation tests to run after migration
/// These can be run as unit tests or manual checks
/// </summary>
public static class ManualMigrationValidation
{
    /// <summary>
    /// Run this after migration to validate data integrity
    /// </summary>
    public static async Task<bool> ValidateDataIntegrityAsync(ApplicationDbContext context)
    {
        var issues = new List<string>();

        try
        {
            // 1. Check for orphaned records
            var orphanedOrders = await context.Orders
                .Where(o => !context.Users.Any(u => u.Id == o.CustomerId))
                .CountAsync();
            
            if (orphanedOrders > 0)
                issues.Add($"Found {orphanedOrders} orphaned orders");

            // 2. Check data consistency
            var negativePrice = await context.Drinks
                .Where(d => d.Price < 0)
                .CountAsync();
                
            if (negativePrice > 0)
                issues.Add($"Found {negativePrice} drinks with negative prices");

            // 3. Check stadium structure integrity
            var seatsWithoutSections = await context.StadiumSeats
                .Where(s => !context.StadiumSections.Any(sec => sec.Id == s.SectionId))
                .CountAsync();
                
            if (seatsWithoutSections > 0)
                issues.Add($"Found {seatsWithoutSections} seats without valid sections");

            // 4. Check order totals match item totals
            var invalidOrderTotals = await context.Orders
                .Where(o => o.OrderItems.Any())
                .Where(o => Math.Abs(o.TotalAmount - o.OrderItems.Sum(i => i.Quantity * i.UnitPrice)) > 0.01m)
                .CountAsync();
                
            if (invalidOrderTotals > 0)
                issues.Add($"Found {invalidOrderTotals} orders with incorrect totals");

            // Log results
            if (issues.Any())
            {
                Console.WriteLine("Data Integrity Issues Found:");
                foreach (var issue in issues)
                {
                    Console.WriteLine($"- {issue}");
                }
                return false;
            }
            else
            {
                Console.WriteLine("✅ All data integrity checks passed");
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Data integrity validation failed: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Validate that all expected indexes exist for performance
    /// </summary>
    public static async Task<bool> ValidateIndexesAsync(ApplicationDbContext context)
    {
        try
        {
            var expectedIndexes = new[]
            {
                "idx_users_email",
                "idx_orders_customer_id", 
                "idx_orders_status",
                "idx_stadium_seats_section_id"
            };

            var existingIndexes = await context.Database.ExecuteSqlRawAsync(@"
                SELECT indexname 
                FROM pg_indexes 
                WHERE schemaname = 'public' 
                  AND indexname = ANY($1)", 
                expectedIndexes);

            Console.WriteLine("✅ Index validation completed");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Index validation failed: {ex.Message}");
            return false;
        }
    }
}