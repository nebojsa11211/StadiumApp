using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.Authentication.Utilities;

/// <summary>
/// Custom validation attribute for password strength validation
/// </summary>
public class StrongPasswordAttribute : ValidationAttribute
{
    private readonly PasswordPolicy _policy;

    public StrongPasswordAttribute()
    {
        _policy = new PasswordPolicy();
        ErrorMessage = "Password does not meet security requirements";
    }

    public StrongPasswordAttribute(int minLength, bool requireDigit = true, bool requireLowercase = true, bool requireUppercase = false)
    {
        _policy = new PasswordPolicy
        {
            MinLength = minLength,
            RequireDigit = requireDigit,
            RequireLowercase = requireLowercase,
            RequireUppercase = requireUppercase
        };
        ErrorMessage = "Password does not meet security requirements";
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string password)
        {
            return new ValidationResult("Password is required");
        }

        var result = PasswordValidator.ValidatePassword(password, _policy);
        if (result.IsValid)
        {
            return ValidationResult.Success!;
        }

        return new ValidationResult(result.GetErrorMessage());
    }
}

/// <summary>
/// Custom validation attribute for confirming password matches
/// </summary>
public class ConfirmPasswordAttribute : ValidationAttribute
{
    private readonly string _passwordPropertyName;

    public ConfirmPasswordAttribute(string passwordPropertyName)
    {
        _passwordPropertyName = passwordPropertyName;
        ErrorMessage = "Password and confirmation do not match";
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var confirmPassword = value as string;

        var passwordProperty = validationContext.ObjectType.GetProperty(_passwordPropertyName);
        if (passwordProperty == null)
        {
            return new ValidationResult($"Property {_passwordPropertyName} not found");
        }

        var password = passwordProperty.GetValue(validationContext.ObjectInstance) as string;

        if (!PasswordValidator.PasswordsMatch(password ?? string.Empty, confirmPassword ?? string.Empty))
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success!;
    }
}

/// <summary>
/// Custom validation attribute for JWT token format validation
/// </summary>
public class JwtTokenAttribute : ValidationAttribute
{
    public JwtTokenAttribute()
    {
        ErrorMessage = "Invalid JWT token format";
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success!; // Allow null values, use [Required] for mandatory
        }

        if (value is not string token)
        {
            return new ValidationResult("Token must be a string");
        }

        if (!JwtTokenValidator.IsValidTokenFormat(token))
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success!;
    }
}

/// <summary>
/// Custom validation attribute for email format validation with additional security checks
/// </summary>
public class SecureEmailAttribute : ValidationAttribute
{
    public bool AllowTemporaryEmails { get; set; } = true;
    public bool RequireVerifiedDomain { get; set; } = false;

    public SecureEmailAttribute()
    {
        ErrorMessage = "Invalid email address";
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success!; // Allow null values, use [Required] for mandatory
        }

        if (value is not string email)
        {
            return new ValidationResult("Email must be a string");
        }

        // Basic email format validation
        if (!new EmailAddressAttribute().IsValid(email))
        {
            return new ValidationResult("Invalid email format");
        }

        // Additional security checks
        if (!SecurityUtilities.IsSafeString(email))
        {
            return new ValidationResult("Email contains unsafe characters");
        }

        // Check for temporary email domains (if disabled)
        if (!AllowTemporaryEmails && IsTemporaryEmailDomain(email))
        {
            return new ValidationResult("Temporary email addresses are not allowed");
        }

        return ValidationResult.Success!;
    }

    private static bool IsTemporaryEmailDomain(string email)
    {
        var domain = email.Split('@').LastOrDefault()?.ToLower();
        if (string.IsNullOrEmpty(domain))
            return false;

        var temporaryDomains = new[]
        {
            "10minutemail.com", "tempmail.org", "guerrillamail.com",
            "mailinator.com", "throwaway.email", "temp-mail.org"
        };

        return temporaryDomains.Contains(domain);
    }
}

/// <summary>
/// Custom validation attribute for role validation
/// </summary>
public class ValidRoleAttribute : ValidationAttribute
{
    private readonly string[] _allowedRoles;

    public ValidRoleAttribute(params string[] allowedRoles)
    {
        _allowedRoles = allowedRoles;
        ErrorMessage = "Invalid role specified";
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success!; // Allow null values
        }

        if (value is not string role)
        {
            return new ValidationResult("Role must be a string");
        }

        if (_allowedRoles.Length > 0 && !_allowedRoles.Contains(role, StringComparer.OrdinalIgnoreCase))
        {
            return new ValidationResult($"Role must be one of: {string.Join(", ", _allowedRoles)}");
        }

        return ValidationResult.Success!;
    }
}

/// <summary>
/// Custom validation attribute for application context validation
/// </summary>
public class ValidApplicationContextAttribute : ValidationAttribute
{
    public ValidApplicationContextAttribute()
    {
        ErrorMessage = "Invalid application context";
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success!;
        }

        if (value is not string context)
        {
            return new ValidationResult("Application context must be a string");
        }

        var validContexts = new[]
        {
            Constants.AuthenticationConstants.ApplicationContexts.Admin,
            Constants.AuthenticationConstants.ApplicationContexts.Customer,
            Constants.AuthenticationConstants.ApplicationContexts.Staff
        };

        if (!validContexts.Contains(context, StringComparer.OrdinalIgnoreCase))
        {
            return new ValidationResult($"Application context must be one of: {string.Join(", ", validContexts)}");
        }

        return ValidationResult.Success!;
    }
}

/// <summary>
/// Custom validation attribute for session ID format validation
/// </summary>
public class SessionIdAttribute : ValidationAttribute
{
    public int MinLength { get; set; } = 16;
    public int MaxLength { get; set; } = 64;

    public SessionIdAttribute()
    {
        ErrorMessage = "Invalid session ID format";
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success!;
        }

        if (value is not string sessionId)
        {
            return new ValidationResult("Session ID must be a string");
        }

        if (sessionId.Length < MinLength || sessionId.Length > MaxLength)
        {
            return new ValidationResult($"Session ID must be between {MinLength} and {MaxLength} characters");
        }

        if (!sessionId.All(char.IsLetterOrDigit))
        {
            return new ValidationResult("Session ID must contain only letters and digits");
        }

        return ValidationResult.Success!;
    }
}

/// <summary>
/// Custom validation attribute for API key format validation
/// </summary>
public class ApiKeyAttribute : ValidationAttribute
{
    public int MinLength { get; set; } = 32;
    public int MaxLength { get; set; } = 64;

    public ApiKeyAttribute()
    {
        ErrorMessage = "Invalid API key format";
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success!;
        }

        if (value is not string apiKey)
        {
            return new ValidationResult("API key must be a string");
        }

        if (apiKey.Length < MinLength || apiKey.Length > MaxLength)
        {
            return new ValidationResult($"API key must be between {MinLength} and {MaxLength} characters");
        }

        if (!apiKey.All(char.IsLetterOrDigit))
        {
            return new ValidationResult("API key must contain only letters and digits");
        }

        return ValidationResult.Success!;
    }
}