using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;
using BCrypt.Net;

namespace StadiumDrinkOrdering.Tools
{
    class CreateAdminUser
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Creating/Updating Admin User...");
            
            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("StadiumDrinkOrdering.API/appsettings.json", optional: false)
                .AddJsonFile("StadiumDrinkOrdering.API/appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
            }

            Console.WriteLine($"Using connection: {connectionString?.Substring(0, 50)}...");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            using (var context = new ApplicationDbContext(optionsBuilder.Options))
            {
                try
                {
                    // Check if admin user exists
                    var adminUser = await context.Users
                        .FirstOrDefaultAsync(u => u.Email == "admin@stadium.com");

                    if (adminUser == null)
                    {
                        Console.WriteLine("Admin user not found. Creating new admin user...");
                        
                        // Create new admin user
                        adminUser = new User
                        {
                            Email = "admin@stadium.com",
                            Username = "admin",
                            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                            Role = UserRole.Admin,
                            CreatedAt = DateTime.UtcNow
                        };

                        context.Users.Add(adminUser);
                        await context.SaveChangesAsync();
                        
                        Console.WriteLine("✓ Admin user created successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Admin user found. Updating password...");
                        
                        // Update password
                        adminUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123");
                        adminUser.Role = UserRole.Admin;
                        
                        await context.SaveChangesAsync();
                        
                        Console.WriteLine("✓ Admin password updated successfully!");
                    }

                    Console.WriteLine("\nAdmin user details:");
                    Console.WriteLine($"  Email: {adminUser.Email}");
                    Console.WriteLine($"  Password: admin123");
                    Console.WriteLine($"  Role: {adminUser.Role}");
                    Console.WriteLine($"  Active: {adminUser.IsActive}");
                    Console.WriteLine($"  ID: {adminUser.Id}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner: {ex.InnerException.Message}");
                    }
                }
            }
        }
    }
}