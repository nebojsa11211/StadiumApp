using System;
using BCrypt.Net;

// Generate BCrypt hash for "admin123"
var password = "admin123";
var hash = BCrypt.Net.BCrypt.HashPassword(password, 11);

Console.WriteLine($"Password: {password}");
Console.WriteLine($"BCrypt Hash: {hash}");
Console.WriteLine();
Console.WriteLine("SQL Update Statement:");
Console.WriteLine($"UPDATE \"Users\" SET \"PasswordHash\" = '{hash}' WHERE \"Email\" = 'admin@stadium.com';");
