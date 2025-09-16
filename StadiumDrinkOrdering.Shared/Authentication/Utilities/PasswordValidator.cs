using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using StadiumDrinkOrdering.Shared.Authentication.Constants;

namespace StadiumDrinkOrdering.Shared.Authentication.Utilities;

/// <summary>
/// Utility class for password validation and strength checking
/// </summary>
public static class PasswordValidator
{
    /// <summary>
    /// Validates password against default requirements
    /// </summary>
    public static PasswordValidationResult ValidatePassword(string password)
    {
        return ValidatePassword(password, new PasswordPolicy());
    }

    /// <summary>
    /// Validates password against custom policy
    /// </summary>
    public static PasswordValidationResult ValidatePassword(string password, PasswordPolicy policy)
    {
        var result = new PasswordValidationResult { IsValid = true };

        if (string.IsNullOrEmpty(password))
        {
            result.IsValid = false;
            result.Errors.Add("Password is required");
            return result;
        }

        // Length validation
        if (password.Length < policy.MinLength)
        {
            result.IsValid = false;
            result.Errors.Add($"Password must be at least {policy.MinLength} characters long");
        }

        if (password.Length > policy.MaxLength)
        {
            result.IsValid = false;
            result.Errors.Add($"Password must not exceed {policy.MaxLength} characters");
        }

        // Character requirements
        if (policy.RequireDigit && !HasDigit(password))
        {
            result.IsValid = false;
            result.Errors.Add("Password must contain at least one digit (0-9)");
        }

        if (policy.RequireLowercase && !HasLowercase(password))
        {
            result.IsValid = false;
            result.Errors.Add("Password must contain at least one lowercase letter (a-z)");
        }

        if (policy.RequireUppercase && !HasUppercase(password))
        {
            result.IsValid = false;
            result.Errors.Add("Password must contain at least one uppercase letter (A-Z)");
        }

        if (policy.RequireNonAlphanumeric && !HasNonAlphanumeric(password))
        {
            result.IsValid = false;
            result.Errors.Add("Password must contain at least one special character (!@#$%^&* etc.)");
        }

        // Unique characters requirement
        if (password.Distinct().Count() < policy.UniqueChars)
        {
            result.IsValid = false;
            result.Errors.Add($"Password must contain at least {policy.UniqueChars} unique characters");
        }

        // Common password patterns
        if (policy.DisallowCommonPatterns && HasCommonPatterns(password))
        {
            result.IsValid = false;
            result.Errors.Add("Password contains common patterns that are not allowed");
        }

        // Sequential characters
        if (policy.DisallowSequentialChars && HasSequentialCharacters(password))
        {
            result.IsValid = false;
            result.Errors.Add("Password cannot contain sequential characters (abc, 123, etc.)");
        }

        // Repeated characters
        if (policy.MaxRepeatedChars > 0 && HasTooManyRepeatedChars(password, policy.MaxRepeatedChars))
        {
            result.IsValid = false;
            result.Errors.Add($"Password cannot have more than {policy.MaxRepeatedChars} repeated characters");
        }

        // Calculate strength
        result.Strength = CalculatePasswordStrength(password);

        return result;
    }

    /// <summary>
    /// Calculates password strength on a scale of 0-100
    /// </summary>
    public static int CalculatePasswordStrength(string password)
    {
        if (string.IsNullOrEmpty(password))
            return 0;

        int score = 0;

        // Length scoring
        score += Math.Min(password.Length * 4, 25);

        // Character variety scoring
        if (HasLowercase(password)) score += 5;
        if (HasUppercase(password)) score += 5;
        if (HasDigit(password)) score += 5;
        if (HasNonAlphanumeric(password)) score += 10;

        // Bonus for mixed character types
        int charTypes = 0;
        if (HasLowercase(password)) charTypes++;
        if (HasUppercase(password)) charTypes++;
        if (HasDigit(password)) charTypes++;
        if (HasNonAlphanumeric(password)) charTypes++;

        score += charTypes * 5;

        // Unique character bonus
        var uniqueChars = password.Distinct().Count();
        score += Math.Min(uniqueChars * 2, 20);

        // Length bonuses
        if (password.Length >= 12) score += 10;
        if (password.Length >= 16) score += 10;
        if (password.Length >= 20) score += 10;

        // Penalties
        if (HasCommonPatterns(password)) score -= 20;
        if (HasSequentialCharacters(password)) score -= 15;
        if (HasTooManyRepeatedChars(password, 3)) score -= 10;

        return Math.Max(0, Math.Min(100, score));
    }

    /// <summary>
    /// Gets password strength description
    /// </summary>
    public static string GetStrengthDescription(int strength)
    {
        return strength switch
        {
            <= 20 => "Very Weak",
            <= 40 => "Weak",
            <= 60 => "Fair",
            <= 80 => "Good",
            <= 95 => "Strong",
            _ => "Very Strong"
        };
    }

    /// <summary>
    /// Generates a secure random password
    /// </summary>
    public static string GenerateSecurePassword(int length = 12, bool includeSymbols = true)
    {
        const string lowercase = "abcdefghijklmnopqrstuvwxyz";
        const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string digits = "0123456789";
        const string symbols = "!@#$%^&*()_+-=[]{}|;:,.<>?";

        var chars = lowercase + uppercase + digits;
        if (includeSymbols)
            chars += symbols;

        var random = new Random();
        var password = new char[length];

        // Ensure at least one character from each required type
        password[0] = lowercase[random.Next(lowercase.Length)];
        password[1] = uppercase[random.Next(uppercase.Length)];
        password[2] = digits[random.Next(digits.Length)];

        int startIndex = 3;
        if (includeSymbols && length > 3)
        {
            password[3] = symbols[random.Next(symbols.Length)];
            startIndex = 4;
        }

        // Fill the rest randomly
        for (int i = startIndex; i < length; i++)
        {
            password[i] = chars[random.Next(chars.Length)];
        }

        // Shuffle the password
        for (int i = password.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (password[i], password[j]) = (password[j], password[i]);
        }

        return new string(password);
    }

    /// <summary>
    /// Checks if passwords match
    /// </summary>
    public static bool PasswordsMatch(string password, string confirmPassword)
    {
        return string.Equals(password, confirmPassword, StringComparison.Ordinal);
    }

    /// <summary>
    /// Validates password confirmation
    /// </summary>
    public static ValidationResult ValidatePasswordConfirmation(string password, string confirmPassword)
    {
        if (string.IsNullOrEmpty(confirmPassword))
        {
            return new ValidationResult("Password confirmation is required");
        }

        if (!PasswordsMatch(password, confirmPassword))
        {
            return new ValidationResult("Password and confirmation do not match");
        }

        return ValidationResult.Success!;
    }

    #region Private Helper Methods

    private static bool HasDigit(string password) => password.Any(char.IsDigit);

    private static bool HasLowercase(string password) => password.Any(char.IsLower);

    private static bool HasUppercase(string password) => password.Any(char.IsUpper);

    private static bool HasNonAlphanumeric(string password) => password.Any(c => !char.IsLetterOrDigit(c));

    private static bool HasCommonPatterns(string password)
    {
        var commonPasswords = new[]
        {
            "password", "123456", "qwerty", "abc123", "password123",
            "admin", "letmein", "welcome", "monkey", "dragon"
        };

        var lowerPassword = password.ToLower();
        return commonPasswords.Any(common => lowerPassword.Contains(common));
    }

    private static bool HasSequentialCharacters(string password)
    {
        for (int i = 0; i < password.Length - 2; i++)
        {
            if (IsSequential(password[i], password[i + 1], password[i + 2]))
                return true;
        }
        return false;
    }

    private static bool IsSequential(char a, char b, char c)
    {
        return (a + 1 == b && b + 1 == c) || (a - 1 == b && b - 1 == c);
    }

    private static bool HasTooManyRepeatedChars(string password, int maxRepeated)
    {
        return password.GroupBy(c => c).Any(g => g.Count() > maxRepeated);
    }

    #endregion
}

/// <summary>
/// Password policy configuration
/// </summary>
public class PasswordPolicy
{
    public int MinLength { get; set; } = AuthenticationConstants.PasswordRequirements.MinLength;
    public int MaxLength { get; set; } = AuthenticationConstants.PasswordRequirements.MaxLength;
    public bool RequireDigit { get; set; } = AuthenticationConstants.PasswordRequirements.RequireDigit;
    public bool RequireLowercase { get; set; } = AuthenticationConstants.PasswordRequirements.RequireLowercase;
    public bool RequireUppercase { get; set; } = AuthenticationConstants.PasswordRequirements.RequireUppercase;
    public bool RequireNonAlphanumeric { get; set; } = AuthenticationConstants.PasswordRequirements.RequireNonAlphanumeric;
    public int UniqueChars { get; set; } = AuthenticationConstants.PasswordRequirements.UniqueChars;
    public bool DisallowCommonPatterns { get; set; } = true;
    public bool DisallowSequentialChars { get; set; } = true;
    public int MaxRepeatedChars { get; set; } = 3;
}

/// <summary>
/// Result of password validation
/// </summary>
public class PasswordValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
    public int Strength { get; set; }
    public string StrengthDescription => PasswordValidator.GetStrengthDescription(Strength);

    public string GetErrorMessage() => string.Join(". ", Errors);
    public bool HasErrors => Errors.Count > 0;
}