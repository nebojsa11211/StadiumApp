using System.Security.Cryptography;
using System.Text;

namespace StadiumDrinkOrdering.Shared.Authentication.Utilities;

/// <summary>
/// Utility class for security-related operations
/// </summary>
public static class SecurityUtilities
{
    private const string DefaultCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private const string AlphaNumericCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private const string NumericCharacters = "0123456789";
    private const string AlphaCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private const string HexCharacters = "0123456789ABCDEF";

    /// <summary>
    /// Generates a cryptographically secure random string
    /// </summary>
    public static string GenerateSecureRandomString(int length, string? allowedCharacters = null)
    {
        if (length <= 0)
            throw new ArgumentException("Length must be greater than 0", nameof(length));

        allowedCharacters ??= DefaultCharacters;

        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length * 4]; // Over-allocate to ensure we have enough random data
        rng.GetBytes(bytes);

        var result = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            var randomIndex = BitConverter.ToUInt32(bytes, i * 4) % (uint)allowedCharacters.Length;
            result.Append(allowedCharacters[(int)randomIndex]);
        }

        return result.ToString();
    }

    /// <summary>
    /// Generates a secure random token suitable for authentication
    /// </summary>
    public static string GenerateAuthToken(int length = 32)
    {
        return GenerateSecureRandomString(length, AlphaNumericCharacters);
    }

    /// <summary>
    /// Generates a secure random session ID
    /// </summary>
    public static string GenerateSessionId(int length = 24)
    {
        return GenerateSecureRandomString(length, AlphaNumericCharacters);
    }

    /// <summary>
    /// Generates a secure random API key
    /// </summary>
    public static string GenerateApiKey(int length = 40)
    {
        return GenerateSecureRandomString(length, AlphaNumericCharacters);
    }

    /// <summary>
    /// Generates a secure random verification code (numeric)
    /// </summary>
    public static string GenerateVerificationCode(int length = 6)
    {
        return GenerateSecureRandomString(length, NumericCharacters);
    }

    /// <summary>
    /// Generates a secure random hex string
    /// </summary>
    public static string GenerateHexString(int length = 16)
    {
        return GenerateSecureRandomString(length, HexCharacters);
    }

    /// <summary>
    /// Generates a secure random GUID-like string
    /// </summary>
    public static string GenerateSecureGuid()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[16];
        rng.GetBytes(bytes);
        return new Guid(bytes).ToString();
    }

    /// <summary>
    /// Hashes a password using PBKDF2 with salt
    /// </summary>
    public static (string Hash, string Salt) HashPassword(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[32];
        rng.GetBytes(salt);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32);

        return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
    }

    /// <summary>
    /// Verifies a password against its hash
    /// </summary>
    public static bool VerifyPassword(string password, string hash, string salt)
    {
        try
        {
            var saltBytes = Convert.FromBase64String(salt);
            var hashBytes = Convert.FromBase64String(hash);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100000, HashAlgorithmName.SHA256);
            var testHash = pbkdf2.GetBytes(32);

            return CryptographicOperations.FixedTimeEquals(hashBytes, testHash);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Generates a secure hash of data using SHA256
    /// </summary>
    public static string GenerateHash(string data)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(data);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Generates a secure hash with salt
    /// </summary>
    public static (string Hash, string Salt) GenerateHashWithSalt(string data)
    {
        var salt = GenerateSecureRandomString(16);
        var combined = data + salt;
        var hash = GenerateHash(combined);
        return (hash, salt);
    }

    /// <summary>
    /// Verifies data against hash with salt
    /// </summary>
    public static bool VerifyHashWithSalt(string data, string hash, string salt)
    {
        var combined = data + salt;
        var testHash = GenerateHash(combined);
        return string.Equals(hash, testHash, StringComparison.Ordinal);
    }

    /// <summary>
    /// Generates a time-based one-time password (TOTP) code
    /// </summary>
    public static string GenerateTotpCode(string secret, DateTime? timestamp = null)
    {
        timestamp ??= DateTime.UtcNow;
        var unixTimestamp = ((DateTimeOffset)timestamp.Value).ToUnixTimeSeconds();
        var timeStep = unixTimestamp / 30; // 30-second time step

        var secretBytes = Convert.FromBase64String(secret);
        var timeBytes = BitConverter.GetBytes(timeStep);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(timeBytes);

        using var hmac = new HMACSHA1(secretBytes);
        var hash = hmac.ComputeHash(timeBytes);

        var offset = hash[hash.Length - 1] & 0x0F;
        var code = ((hash[offset] & 0x7F) << 24) |
                   ((hash[offset + 1] & 0xFF) << 16) |
                   ((hash[offset + 2] & 0xFF) << 8) |
                   (hash[offset + 3] & 0xFF);

        return (code % 1000000).ToString("D6");
    }

    /// <summary>
    /// Validates a TOTP code
    /// </summary>
    public static bool ValidateTotpCode(string secret, string code, int windowSize = 1)
    {
        var now = DateTime.UtcNow;

        // Check current time and surrounding windows
        for (int i = -windowSize; i <= windowSize; i++)
        {
            var testTime = now.AddSeconds(i * 30);
            var testCode = GenerateTotpCode(secret, testTime);
            if (string.Equals(code, testCode, StringComparison.Ordinal))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Generates a cryptographically secure random number
    /// </summary>
    public static int GenerateSecureRandomNumber(int minValue = 0, int maxValue = int.MaxValue)
    {
        if (minValue >= maxValue)
            throw new ArgumentException("minValue must be less than maxValue");

        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[4];
        rng.GetBytes(bytes);
        var randomValue = BitConverter.ToUInt32(bytes, 0);

        var range = (long)maxValue - minValue;
        return (int)(minValue + (randomValue % range));
    }

    /// <summary>
    /// Generates a secure random byte array
    /// </summary>
    public static byte[] GenerateSecureRandomBytes(int length)
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);
        return bytes;
    }

    /// <summary>
    /// Constant-time string comparison to prevent timing attacks
    /// </summary>
    public static bool SecureStringEquals(string? a, string? b)
    {
        if (a == null && b == null) return true;
        if (a == null || b == null) return false;

        var aBytes = Encoding.UTF8.GetBytes(a);
        var bBytes = Encoding.UTF8.GetBytes(b);

        return CryptographicOperations.FixedTimeEquals(aBytes, bBytes);
    }

    /// <summary>
    /// Obfuscates sensitive data for logging (shows only first and last characters)
    /// </summary>
    public static string ObfuscateForLogging(string? sensitiveData, int visibleChars = 2)
    {
        if (string.IsNullOrEmpty(sensitiveData))
            return string.Empty;

        if (sensitiveData.Length <= visibleChars * 2)
            return new string('*', sensitiveData.Length);

        var start = sensitiveData.Substring(0, visibleChars);
        var end = sensitiveData.Substring(sensitiveData.Length - visibleChars);
        var middle = new string('*', sensitiveData.Length - visibleChars * 2);

        return start + middle + end;
    }

    /// <summary>
    /// Validates that a string contains only safe characters (no control characters)
    /// </summary>
    public static bool IsSafeString(string input)
    {
        if (string.IsNullOrEmpty(input))
            return true;

        return !input.Any(c => char.IsControl(c) && c != '\r' && c != '\n' && c != '\t');
    }

    /// <summary>
    /// Sanitizes user input by removing or escaping dangerous characters
    /// </summary>
    public static string SanitizeUserInput(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        // Remove control characters except common whitespace
        var sanitized = new string(input.Where(c => !char.IsControl(c) || c == '\r' || c == '\n' || c == '\t').ToArray());

        // Trim whitespace
        return sanitized.Trim();
    }

    /// <summary>
    /// Generates a secure backup code (for account recovery)
    /// </summary>
    public static string GenerateBackupCode()
    {
        // Generate 8 groups of 4 characters separated by hyphens
        var groups = new string[8];
        for (int i = 0; i < 8; i++)
        {
            groups[i] = GenerateSecureRandomString(4, AlphaNumericCharacters.ToUpper());
        }
        return string.Join("-", groups);
    }

    /// <summary>
    /// Validates the format of a backup code
    /// </summary>
    public static bool IsValidBackupCodeFormat(string code)
    {
        if (string.IsNullOrEmpty(code))
            return false;

        var parts = code.Split('-');
        return parts.Length == 8 && parts.All(part => part.Length == 4 && part.All(char.IsLetterOrDigit));
    }
}