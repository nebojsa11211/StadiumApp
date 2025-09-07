using System.ComponentModel.DataAnnotations;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Tests.Models;

public class UserValidationTests
{
    [Fact]
    public void User_WithValidData_PassesValidation()
    {
        // Arrange
        var user = new User
        {
            Username = "testuser",
            Email = "test@stadium.com",
            PasswordHash = "hashed_password",
            Role = UserRole.Customer
        };

        // Act
        var validationResults = ValidateModel(user);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void User_WithEmptyUsername_FailsValidation()
    {
        // Arrange
        var user = new User
        {
            Username = "",
            Email = "test@stadium.com",
            PasswordHash = "hashed_password",
            Role = UserRole.Customer
        };

        // Act
        var validationResults = ValidateModel(user);

        // Assert
        Assert.Single(validationResults);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Username"));
    }

    [Fact]
    public void User_WithInvalidEmail_FailsValidation()
    {
        // Arrange
        var user = new User
        {
            Username = "testuser",
            Email = "invalid-email",
            PasswordHash = "hashed_password",
            Role = UserRole.Customer
        };

        // Act
        var validationResults = ValidateModel(user);

        // Assert
        Assert.Single(validationResults);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Email"));
    }

    [Fact]
    public void User_WithTooLongUsername_FailsValidation()
    {
        // Arrange
        var user = new User
        {
            Username = new string('a', 101), // 101 characters, exceeds 100 limit
            Email = "test@stadium.com",
            PasswordHash = "hashed_password",
            Role = UserRole.Customer
        };

        // Act
        var validationResults = ValidateModel(user);

        // Assert
        Assert.Single(validationResults);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Username"));
    }

    private static IList<System.ComponentModel.DataAnnotations.ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
        var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(model, null, null);
        System.ComponentModel.DataAnnotations.Validator.TryValidateObject(model, validationContext, validationResults, true);
        return validationResults;
    }
}

public class LoginDtoValidationTests
{
    [Fact]
    public void LoginDto_WithValidData_PassesValidation()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "test@stadium.com",
            Password = "password123"
        };

        // Act
        var validationResults = ValidateModel(loginDto);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void LoginDto_WithInvalidEmail_FailsValidation()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "invalid-email",
            Password = "password123"
        };

        // Act
        var validationResults = ValidateModel(loginDto);

        // Assert
        Assert.Single(validationResults);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Email"));
    }

    [Fact]
    public void LoginDto_WithShortPassword_FailsValidation()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "test@stadium.com",
            Password = "12345" // Less than 6 characters
        };

        // Act
        var validationResults = ValidateModel(loginDto);

        // Assert
        Assert.Single(validationResults);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Password"));
    }

    [Fact]
    public void LoginDto_WithEmptyEmail_FailsValidation()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "",
            Password = "password123"
        };

        // Act
        var validationResults = ValidateModel(loginDto);

        // Assert
        Assert.Single(validationResults);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Email"));
    }

    private static IList<System.ComponentModel.DataAnnotations.ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
        var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(model, null, null);
        System.ComponentModel.DataAnnotations.Validator.TryValidateObject(model, validationContext, validationResults, true);
        return validationResults;
    }
}

public class RegisterDtoValidationTests
{
    [Fact]
    public void RegisterDto_WithValidData_PassesValidation()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Username = "testuser",
            Email = "test@stadium.com",
            Password = "password123",
            ConfirmPassword = "password123"
        };

        // Act
        var validationResults = ValidateModel(registerDto);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void RegisterDto_WithMismatchedPasswords_FailsValidation()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Username = "testuser",
            Email = "test@stadium.com",
            Password = "password123",
            ConfirmPassword = "differentpassword"
        };

        // Act
        var validationResults = ValidateModel(registerDto);

        // Assert
        Assert.Single(validationResults);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains("ConfirmPassword"));
    }

    [Fact]
    public void RegisterDto_WithTooLongUsername_FailsValidation()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Username = new string('a', 101), // Exceeds 100 character limit
            Email = "test@stadium.com",
            Password = "password123",
            ConfirmPassword = "password123"
        };

        // Act
        var validationResults = ValidateModel(registerDto);

        // Assert
        Assert.Single(validationResults);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Username"));
    }

    private static IList<System.ComponentModel.DataAnnotations.ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
        var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(model, null, null);
        System.ComponentModel.DataAnnotations.Validator.TryValidateObject(model, validationContext, validationResults, true);
        return validationResults;
    }
}